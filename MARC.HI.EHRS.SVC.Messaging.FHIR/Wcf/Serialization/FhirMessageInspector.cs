using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.IO;
using System.Xml;
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Reflection;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Wcf.BodyWriters;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Configuration;
using MARC.HI.EHRS.SVC.Core;
using MARC.HI.EHRS.SVC.Core.Services;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Wcf.Serialization
{
    /// <summary>
    /// Represents an IMSI message inspector which can inspect messages and perform tertiary functions
    /// not included in WCF (such as compression)
    /// </summary>
    public class FhirMessageInspector : IDispatchMessageInspector
    {

        // Service config
        private FhirServiceConfiguration m_configuration = ApplicationContext.Current.GetService<IConfigurationManager>().GetSection("marc.hi.ehrs.svc.messaging.fhir") as FhirServiceConfiguration;

        // Trace source
        private TraceSource m_traceSource = new TraceSource("MARC.HI.EHRS.SVC.Messaging.FHIR");

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
        /// After receiving a request, do any message stuff here
        /// </summary>
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            try
            {
                // Handle compressed requests
                var compressionScheme = CompressionUtil.GetCompressionScheme(WebOperationContext.Current.IncomingRequest.Headers[System.Net.HttpRequestHeader.ContentEncoding]);
                if (compressionScheme != null)
                    CompressionUtil.DeCompressMessage(ref request, compressionScheme, this.GetContentFormat(request));

                
                return null;
            }
            catch(Exception e)
            {
                this.m_traceSource.TraceEvent(TraceEventType.Error, e.HResult, e.ToString());
                return null;
            }
        }

        /// <summary>
        /// Before sending a reply
        /// </summary>
        public void BeforeSendReply(ref Message reply, object correlationState)
        {

            try
            {
                string encodings = WebOperationContext.Current.IncomingRequest.Headers.Get("Accept-Encoding");
                string compressionScheme = String.Empty;

                if (!string.IsNullOrEmpty(encodings))
                {
                    encodings = encodings.ToLowerInvariant();

                    if (encodings.Contains("gzip"))
                        compressionScheme = "gzip";
                    else if (encodings.Contains("deflate"))
                        compressionScheme = "deflate";
                    else
                        WebOperationContext.Current.OutgoingResponse.Headers.Add("X-CompressResponseStream", "no-known-accept");
                }


                // TODO: Add a configuration option to disable this
                FhirCorsConfiguration config = null;
                String resourcePath = "*";
                Dictionary<String, String> requiredHeaders = new Dictionary<string, string>();

                // CORS?
                if (this.m_configuration.CorsConfiguration.TryGetValue("*", out config))
                    requiredHeaders = new Dictionary<string, string>() {
                        {"Access-Control-Allow-Origin", config.Domain},
                        {"Access-Control-Request-Method", config.Actions},
                        {"Access-Control-Allow-Headers", config.Headers}
                    };

                foreach (var kv in requiredHeaders)
                    if (!WebOperationContext.Current.OutgoingResponse.Headers.AllKeys.Contains(kv.Key))
                        WebOperationContext.Current.OutgoingResponse.Headers.Add(kv.Key, kv.Value);

                // Finally compress
                // Compress
                if (!String.IsNullOrEmpty(compressionScheme))
                {
                    try
                    {
                        WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-Encoding", compressionScheme);
                        WebOperationContext.Current.OutgoingResponse.Headers.Add("X-CompressResponseStream", compressionScheme);
                        byte[] messageContent = null;

                        // Read binary contents of the message
                        switch(this.GetContentFormat(reply))
                        {
                            case WebContentFormat.Default:
                            case WebContentFormat.Xml:
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    // Write out the XML
                                    using (var xdr = XmlDictionaryWriter.CreateTextWriter(ms, Encoding.UTF8, false))
                                        reply.WriteBodyContents(xdr);
                                    messageContent = ms.ToArray(); // original message content
                                }
                                break;
                            case WebContentFormat.Raw:
                                {
                                    var xdr = reply.GetReaderAtBodyContents();
                                    xdr.ReadStartElement("Binary");
                                    messageContent = xdr.ReadContentAsBase64();
                                    break;
                                }
                        }

                        Message compressedMessage = Message.CreateMessage(reply.Version, reply.Headers.Action, new CompressionBodyWriter(messageContent, CompressionUtil.GetCompressionScheme(compressionScheme)));
                        compressedMessage.Properties.CopyProperties(reply.Properties);
                        compressedMessage.Properties.Remove(WebBodyFormatMessageProperty.Name);
                        compressedMessage.Properties.Add(WebBodyFormatMessageProperty.Name, new WebBodyFormatMessageProperty(WebContentFormat.Raw));
                        reply = compressedMessage;

                    }
                    catch (Exception e)
                    {
                        this.m_traceSource.TraceEvent(TraceEventType.Error, e.HResult, e.ToString());
                    }
                }
            }
            catch(Exception e)
            {
                this.m_traceSource.TraceEvent(TraceEventType.Error, e.HResult, e.ToString());
            }
        }
    }
}