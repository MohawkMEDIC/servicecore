using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;
using System.Runtime.Serialization.Json;
using Hl7.Fhir.Serialization;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.WcfCore
{
    /// <summary>
    /// Represents a message inspector for FHIR messages that translates FHIR objects to/from JSON using the FHIR API
    /// </summary>
    public class FhirMessageInspector : IDispatchMessageInspector
    {
        /// <summary>
        /// Get the message's classified format
        /// </summary>
        private WebContentFormat GetContentFormat(Message message)
        {
            WebContentFormat retVal = WebContentFormat.Default;
            if (message.Properties.ContainsKey(WebBodyFormatMessageProperty.Name))
            {
                WebBodyFormatMessageProperty propertyValue = message.Properties[WebBodyFormatMessageProperty.Name] as WebBodyFormatMessageProperty;
                retVal = propertyValue.Format;
            }
            return retVal;
        }

        /// <summary>
        /// After a request is received. If the request is JSON we need to translate it to XML
        /// </summary>
        public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
        {
            var format = this.GetContentFormat(request);
            if (WebOperationContext.Current.IncomingRequest.UriTemplateMatch != null && 
                (WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters["_format"] == "json" ||
                WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters["_format"] == "application/json+fhir"))
                format = WebContentFormat.Json;

            String messageContent = String.Empty, retVal = null;
            MessageProperties properties = OperationContext.Current.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string remoteEndpoint = "http://anonymous";
            if (endpoint != null)
                remoteEndpoint = endpoint.Address;

            // If the requested response or the incoming request is not XML we need to translate to XML for the WCF contract 
            // behavior
            switch (format)
            {
                case WebContentFormat.Json:
                {
                    // Load the inbound message
                    if (!request.IsEmpty)
                    {
                        // Message content - We read the JSON content
                        var ms = new MemoryStream();
                        using (var jw = JsonReaderWriterFactory.CreateJsonWriter(ms, Encoding.UTF8, false))
                            request.WriteBodyContents(jw);
                        
                        // Read the memory stream
                        messageContent = Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Length);

                        // Then we serialize to XML
                        ms = new MemoryStream();
                        // Use the FHIR serializer to read the JSON
                        using(XmlWriter xw = XmlWriter.Create(ms, new XmlWriterSettings()))
                            if (messageContent.Contains("resourceType\":\"Bundle"))
                            {
                                var bundle = FhirParser.ParseBundleFromJson(messageContent);
                                FhirSerializer.SerializeBundle(bundle, xw);
                            }
                            else
                            {
                                var resource = FhirParser.ParseResourceFromJson(messageContent);
                                FhirSerializer.SerializeResource(resource, xw);
                            }

                        // Seek to begin
                        ms.Seek(0, SeekOrigin.Begin);

                        XmlDictionaryReader xdr = XmlDictionaryReader.CreateTextReader(ms, XmlDictionaryReaderQuotas.Max);
                        Message xmlMessage = Message.CreateMessage(request.Version, request.Headers.Action, xdr);
                        xmlMessage.Properties.CopyProperties(request.Properties);
                        xmlMessage.Headers.CopyHeadersFrom(request.Headers);
                        xmlMessage.Properties.Remove(WebBodyFormatMessageProperty.Name);
                        xmlMessage.Properties.Add(WebBodyFormatMessageProperty.Name, new WebBodyFormatMessageProperty(WebContentFormat.Xml));
                        request = xmlMessage;
                    }

                    retVal = "application/json+fhir";
                    break;
                }
                case WebContentFormat.Xml:
                {
                    if (!request.IsEmpty)
                    {
                        // Dump
                        using (var ms = new MemoryStream())
                        {
                            using (var xdr = XmlDictionaryWriter.CreateTextWriter(ms, Encoding.UTF8, false))
                            {
                                request.WriteBodyContents(xdr);
                                xdr.Flush();
                                ms.Flush();
                            }
                            messageContent = Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Length);

                            var outMs = new MemoryStream(Encoding.UTF8.GetBytes(messageContent));
                            var rdr = XmlDictionaryReader.CreateTextReader(outMs, XmlDictionaryReaderQuotas.Max);
                            Message xmlMessage = Message.CreateMessage(rdr, int.MaxValue, request.Version);
                            xmlMessage.Properties.CopyProperties(request.Properties);
                            xmlMessage.Headers.CopyHeadersFrom(request.Headers);
                            request = xmlMessage;
                            retVal = null;
                        }
                    }
                    break;
                }
            }

            Trace.TraceInformation("FHIR IN (FROM {0} ON {1}): {2}", remoteEndpoint, request.Properties.Via, messageContent);

            return retVal;
        }

        /// <summary>
        /// If the reply is expected in JSON then send
        /// </summary>
        /// <param name="reply"></param>
        /// <param name="correlationState"></param>
        public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            
            string encodings = WebOperationContext.Current.IncomingRequest.Headers.Get("Accept-Encoding");

            if (!string.IsNullOrEmpty(encodings) && this.GetContentFormat(reply) != WebContentFormat.Raw)
            {
                encodings = encodings.ToLowerInvariant();

                if (encodings.Contains("gzip"))
                {
                    //context.Response.Filter = new GZipStream(context.Response.Filter, CompressionMode.Compress);
                    WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-Encoding", "gzip");
                    WebOperationContext.Current.OutgoingResponse.Headers.Add("X-CompressResponseStream", "gzip");
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.Headers.Add("X-CompressResponseStream", "no-known-accept");
                }
            }

            String messageContent = String.Empty;
            MessageProperties properties = OperationContext.Current.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string remoteEndpoint = "http://anonymous";
            if (endpoint != null)
                remoteEndpoint = endpoint.Address;

            // Need to translate
            if (correlationState != null && correlationState.ToString() == "application/json+fhir")
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    // Write out the XML
                    using (var xdr = XmlDictionaryWriter.CreateTextWriter(ms, Encoding.UTF8, false))
                        reply.WriteBodyContents(xdr);

                    // Read in the JSON
                    messageContent = Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Length);

                    if (WebOperationContext.Current.OutgoingResponse.ContentType.StartsWith("application/atom+xml")) // bundle
                    {
                        var bundle = FhirParser.ParseBundleFromXml(messageContent);
                        messageContent = FhirSerializer.SerializeBundleToJson(bundle);
                    }
                    else
                    {
                        var resource = FhirParser.ParseResourceFromXml(messageContent);
                        messageContent = FhirSerializer.SerializeResourceToJson(resource);
                    }

                    //Trace.TraceInformation("OUT TX: {0}", messageContent);
                    // Correct the headers
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json+fhir; charset=UTF-8";
                    if (WebOperationContext.Current.OutgoingResponse.Headers["Content-Disposition"] != null)
                        WebOperationContext.Current.OutgoingResponse.Headers["Content-Disposition"] = WebOperationContext.Current.OutgoingResponse.Headers["Content-Disposition"].Replace(".xml", ".js");

                    // Output the message
                    var outMs = new MemoryStream(Encoding.UTF8.GetBytes(messageContent));
                    var jdr = JsonReaderWriterFactory.CreateJsonReader(outMs, XmlDictionaryReaderQuotas.Max);
                    Message xmlMessage = Message.CreateMessage(reply.Version, reply.Headers.Action, jdr);
                    xmlMessage.Properties.CopyProperties(reply.Properties);
                    xmlMessage.Headers.CopyHeadersFrom(reply.Headers);
                    xmlMessage.Properties.Remove(WebBodyFormatMessageProperty.Name);
                    xmlMessage.Properties.Add(WebBodyFormatMessageProperty.Name, new WebBodyFormatMessageProperty(WebContentFormat.Json));
                    reply = xmlMessage;
                }
            }
            else
            {
                
                using(var ms = new MemoryStream())
                {
                    using (var xdr = XmlDictionaryWriter.CreateTextWriter(ms, Encoding.UTF8, false))
                    {
                        reply.WriteBodyContents(xdr);
                        xdr.Flush();
                        ms.Flush();
                    }
                    messageContent = Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Length);

                    var outMs = new MemoryStream(Encoding.UTF8.GetBytes(messageContent));
                    var rdr = XmlDictionaryReader.CreateTextReader(outMs, XmlDictionaryReaderQuotas.Max);
                    Message xmlMessage = Message.CreateMessage(rdr, int.MaxValue, reply.Version);
                    xmlMessage.Properties.CopyProperties(reply.Properties);
                    xmlMessage.Headers.CopyHeadersFrom(reply.Headers);
                    reply = xmlMessage;

                }

            }


            // TODO: Add a configuration option to disable this
            Dictionary<String, String> requiredHeaders = new Dictionary<string, string>() {
                {"Access-Control-Allow-Origin", "*"},
                {"Access-Control-Request-Method", "GET,OPTIONS"},
                {"Access-Control-Allow-Headers", "X-Requested-With,Content-Type"}
            };
            foreach (var kv in requiredHeaders)
                if (!WebOperationContext.Current.OutgoingResponse.Headers.AllKeys.Contains(kv.Key))
                    WebOperationContext.Current.OutgoingResponse.Headers.Add(kv.Key, kv.Value);

            // Output message
            Trace.TraceInformation("FHIR OUT (TO:{1}): {0}", messageContent, remoteEndpoint);

            // Compress
            if (WebOperationContext.Current.OutgoingResponse.Headers["Content-Encoding"] == "gzip")
            {
                try
                {
                    Message compressedMessage = Message.CreateMessage(reply.Version, reply.Headers.Action, new GZipWriter(System.Text.Encoding.UTF8.GetBytes(messageContent)));
                    compressedMessage.Properties.CopyProperties(reply.Properties);
                    compressedMessage.Properties.Remove(WebBodyFormatMessageProperty.Name);
                    compressedMessage.Properties.Add(WebBodyFormatMessageProperty.Name, new WebBodyFormatMessageProperty(WebContentFormat.Raw));
                    reply = compressedMessage;
                }
                catch (Exception e)
                {
                    Trace.TraceError(e.ToString());
                }
            }
        }
    }
}
