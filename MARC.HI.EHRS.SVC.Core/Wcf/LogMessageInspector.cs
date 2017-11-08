﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace MARC.HI.EHRS.SVC.Core.Wcf
{
    /// <summary>
    /// A log inspector that just logs requests
    /// </summary>
    public class LogMessageInspector : IDispatchMessageInspector
    {

        private TraceSource m_httpSource = new TraceSource("MARC.HI.EHRS.SVC.Core.HTTP");

        /// <summary>
        /// After request is received
        /// </summary>
        /// <param name="request"></param>
        /// <param name="channel"></param>
        /// <param name="instanceContext"></param>
        /// <returns></returns>
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            Guid httpCorrelator = Guid.NewGuid();
            
            this.m_httpSource.TraceEvent(TraceEventType.Verbose, 0, "HTTP RQO {0} : {1} {2} ({3}) - {4}", 
                (OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty)?.Address.ToString(),
                WebOperationContext.Current.IncomingRequest.Method,
                WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri,
                WebOperationContext.Current.IncomingRequest.UserAgent,
                httpCorrelator);
            return new KeyValuePair<Guid, DateTime>(httpCorrelator, DateTime.Now);
        }

        /// <summary>
        /// Fire before sending reply
        /// </summary>
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            if (correlationState is KeyValuePair<Guid, DateTime>)
            {
                var httpCorrelation = (KeyValuePair<Guid, DateTime>)correlationState;
                var processingTime = DateTime.Now.Subtract(httpCorrelation.Value);

                this.m_httpSource.TraceEvent(TraceEventType.Verbose, 0, "HTTP RSP {0} : {1} ({2} ms)",
                    httpCorrelation.Key,
                    WebOperationContext.Current.OutgoingResponse.StatusCode,
                    processingTime.TotalMilliseconds);
            }
        }
    }
}
