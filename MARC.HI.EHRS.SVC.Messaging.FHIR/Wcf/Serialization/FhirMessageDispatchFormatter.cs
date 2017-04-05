/*
 * Copyright 2016-2016 Mohawk College of Applied Arts and Technology
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you 
 * may not use this file except in compliance with the License. You may 
 * obtain a copy of the License at 
 * 
 * http://www.apache.org/licenses/LICENSE-2.0 
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations under 
 * the License.
 * 
 * User: fyfej
 * Date: 2016-1-26
 */
using Newtonsoft.Json;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Wcf.BodyWriters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.ServiceModel.Web;
using System.Xml.Schema;
using System.Diagnostics;
using Newtonsoft.Json.Converters;
using Hl7.Fhir.Serialization;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Resources;
using System.Collections.Specialized;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Wcf.Serialization
{
    /// <summary>
    /// Represents a dispatch message formatter which uses the JSON.NET serialization
    /// </summary>
    public class FhirMessageDispatchFormatter : IDispatchMessageFormatter
    {
        // The operation description
        private OperationDescription m_operationDescription;

        // Trace source
        private TraceSource m_traceSource = new TraceSource("MARC.HI.EHRS.SVC.Messaging.FHIR");
        // Known types
        private static Type[] s_knownTypes = typeof(IFhirServiceContract).GetCustomAttributes<ServiceKnownTypeAttribute>().Select(o=>o.Type).ToArray();

        // Serializers
        private static Dictionary<Type, XmlSerializer> s_serializers = new Dictionary<Type, XmlSerializer>();

        // Static ctor
        static FhirMessageDispatchFormatter()
        {
            foreach (var s in s_knownTypes)
                s_serializers.Add(s, new XmlSerializer(s,  s.GetCustomAttributes<XmlIncludeAttribute>().Select(o => o.Type).ToArray()));
        }

        public FhirMessageDispatchFormatter()
        {

        }

        /// <summary>
        /// Creates a new json dispatch message formatter
        /// </summary>
        public FhirMessageDispatchFormatter(OperationDescription operationDescription)
        {
            this.m_operationDescription = operationDescription;
        }

        /// <summary>
        /// Deserialize the request
        /// </summary>
        public void DeserializeRequest(Message request, object[] parameters)
        {

            try
            {
                HttpRequestMessageProperty httpRequest = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];
                string contentType = httpRequest.Headers[HttpRequestHeader.ContentType];


                UriTemplateMatch templateMatch = (UriTemplateMatch)request.Properties.SingleOrDefault(o => o.Value is UriTemplateMatch).Value;
                // Not found
                if (templateMatch == null)
                {
                    throw new WebFaultException(HttpStatusCode.NotFound);
                }

                for (int pNumber = 0; pNumber < parameters.Length; pNumber++)
                {
                    var parm = this.m_operationDescription.Messages[0].Body.Parts[pNumber];

                    // Simple parameter
                    if (templateMatch.BoundVariables.AllKeys.Any(o=>o.ToString().ToLower() == parm.Name.ToLower()))
                    {
                        var rawData = templateMatch.BoundVariables[parm.Name];
                        parameters[pNumber] = Convert.ChangeType(rawData, parm.Type);
                    }
                    // Use XML Serializer
                    else if (contentType?.StartsWith("application/xml+fhir") == true)
                    {
                        var messageFormatProperty = (WebBodyFormatMessageProperty)request.Properties[WebBodyFormatMessageProperty.Name];
                        XmlDictionaryReader rawReader = request.GetReaderAtBodyContents();

                        switch (messageFormatProperty.Format)
                        {
                            case WebContentFormat.Raw:
                                {
                                    rawReader.ReadStartElement("Binary");
                                    byte[] rawBody = rawReader.ReadContentAsBase64();

                                    using (MemoryStream ms = new MemoryStream(rawBody))
                                    {
                                        using (XmlReader bodyReader = XmlReader.Create(ms))
                                        {
                                            while (bodyReader.NodeType != XmlNodeType.Element)
                                                bodyReader.Read();

                                            Type eType = s_knownTypes.FirstOrDefault(o => o.GetCustomAttribute<XmlRootAttribute>()?.ElementName == bodyReader.LocalName &&
                                            o.GetCustomAttribute<XmlRootAttribute>()?.Namespace == bodyReader.NamespaceURI);
                                            XmlSerializer xsz = s_serializers[eType];
                                            parameters[pNumber] = xsz.Deserialize(bodyReader);
                                        }
                                    }
                                }

                                break;
                            case WebContentFormat.Xml:
                                {
                                    using (rawReader)
                                    {
                                        rawReader.MoveToStartElement();
                                        Type eType = s_knownTypes.FirstOrDefault(o => o.GetCustomAttribute<XmlRootAttribute>()?.ElementName == rawReader.LocalName && o.GetCustomAttribute<XmlRootAttribute>()?.Namespace == rawReader.NamespaceURI);

                                        this.m_traceSource.TraceEvent(TraceEventType.Information, 0, "Contract: {0}", typeof(IFhirServiceContract).Name);
                                        this.m_traceSource.TraceEvent(TraceEventType.Information, 0, "Attempting to deserialize type: {0}", eType?.Name);

                                        XmlSerializer xsz = s_serializers[eType];
                                        parameters[pNumber] = xsz.Deserialize(rawReader);
                                    }
                                }
                                break;
                        }

                    }
                    // Use JSON Serializer
                    else if (contentType?.StartsWith("application/json+fhir") == true)
                    {
                        // Read the binary contents form the WCF pipeline
                        XmlDictionaryReader bodyReader = request.GetReaderAtBodyContents();
                        bodyReader.ReadStartElement("Binary");
                        byte[] rawBody = bodyReader.ReadContentAsBase64();

                        // Now read the JSON data
                        Object fhirObject = null;
                        using (MemoryStream mstream = new MemoryStream(rawBody))
                        using (StreamReader sr = new StreamReader(mstream))
                        {
                            string fhirContent = sr.ReadToEnd();
                            fhirObject = FhirParser.ParseFromJson(fhirContent);
                        }

                        // Now we want to serialize the FHIR MODEL object and re-parse as our own API bundle object
                        MemoryStream ms = new MemoryStream(FhirSerializer.SerializeResourceToXmlBytes(fhirObject as Hl7.Fhir.Model.Resource));

                        XmlSerializer xsz = s_serializers[fhirObject?.GetType()];
                        parameters[0] = xsz.Deserialize(ms);
                    }
                    else if (contentType != null)// TODO: Binaries
                        throw new InvalidOperationException("Invalid request format");
                }
            }
            catch (Exception e)
            {
                this.m_traceSource.TraceEvent(TraceEventType.Error, e.HResult, e.ToString());
                throw;
            }

        }

        /// <summary>
        /// Serialize the reply
        /// </summary>
        public Message SerializeReply(MessageVersion messageVersion, object[] parameters, object result)
        {
            try
            {
                // Outbound control
                var format = WebContentFormat.Raw;
                Message request = OperationContext.Current.RequestContext.RequestMessage;
                HttpRequestMessageProperty httpRequest = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];
                string accepts = httpRequest.Headers[HttpRequestHeader.Accept],
                    contentType = httpRequest.Headers[HttpRequestHeader.ContentType],
                    formatParm = httpRequest.QueryString;

                Message reply = null;

                // Result is serializable
                if (result?.GetType().GetCustomAttribute<XmlTypeAttribute>() != null ||
                    result?.GetType().GetCustomAttribute<JsonObjectAttribute>() != null)
                {
                    XmlSerializer xsz = s_serializers[result.GetType()];
                    MemoryStream ms = new MemoryStream();
                    xsz.Serialize(ms, result);
                    format = WebContentFormat.Xml;
                    contentType = "application/xml+fhir";
                    ms.Seek(0, SeekOrigin.Begin);

                    // The request was in JSON or the accept is JSON
                    if (accepts?.StartsWith("application/json+fhir") == true ||
                        contentType?.StartsWith("application/json+fhir") == true ||
                        formatParm.Contains("_format=json"))
                    {
                        // Parse XML object
                        Object fhirObject = null;
                        using (StreamReader sr = new StreamReader(ms))
                        {
                            String fhirContent = sr.ReadToEnd();
                            fhirObject = FhirParser.ParseFromXml(fhirContent);
                        }

                        // Now we serialize to JSON
                        byte[] body = FhirSerializer.SerializeResourceToJsonBytes(fhirObject as Hl7.Fhir.Model.Resource);
                        
                        // Prepare reply for the WCF pipeline
                        format = WebContentFormat.Raw;
                        contentType = "application/json+fhir";
                        reply = Message.CreateMessage(messageVersion, this.m_operationDescription?.Messages[1]?.Action, new RawBodyWriter(body));

                    }
                    // The request was in XML and/or the accept is JSON
                    else
                    {
                        reply = Message.CreateMessage(messageVersion, this.m_operationDescription?.Messages[1]?.Action, XmlDictionaryReader.Create(ms));
                    }
                }
                else if (result is XmlSchema)
                {
                    MemoryStream ms = new MemoryStream();
                    (result as XmlSchema).Write(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    format = WebContentFormat.Xml;
                    contentType = "text/xml";
                    reply = Message.CreateMessage(messageVersion, this.m_operationDescription.Messages[1].Action, XmlDictionaryReader.Create(ms));
                }
                else if (result is Stream) // TODO: This is messy, clean it up
                {
                    reply = Message.CreateMessage(messageVersion, this.m_operationDescription.Messages[1].Action, new RawBodyWriter(result as Stream));
                }

                reply.Properties.Add(WebBodyFormatMessageProperty.Name, new WebBodyFormatMessageProperty(format));
                WebOperationContext.Current.OutgoingResponse.ContentType = contentType;
                WebOperationContext.Current.OutgoingResponse.Headers.Add("X-PoweredBy", String.Format("{0} v{1} ({2})", Assembly.GetEntryAssembly().GetName().Name, Assembly.GetEntryAssembly().GetName().Version, Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion));
                WebOperationContext.Current.OutgoingResponse.Headers.Add("X-GeneratedOn", DateTime.Now.ToString("o"));

                return reply;
            }
            catch (Exception e)
            {
                this.m_traceSource.TraceEvent(TraceEventType.Error, e.HResult, e.ToString());
                throw;
            }
        }
    }
}
