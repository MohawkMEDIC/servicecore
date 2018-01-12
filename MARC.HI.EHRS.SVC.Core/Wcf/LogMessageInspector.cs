/*
 * Copyright 2010-2018 Mohawk College of Applied Arts and Technology
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
 * Date: 21-11-2017
 */

using System;
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

            // Windows we get CPU usage
            float usage = 0.0f;
            if(Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                using (PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", true))
                    usage = cpuCounter.NextValue();
                
            }
            this.m_httpSource.TraceEvent(TraceEventType.Verbose, 0, "HTTP RQO {0} : {1} {2} ({3}) - {4} (CPU {5}%)", 
                (OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty)?.Address.ToString(),
                WebOperationContext.Current.IncomingRequest.Method,
                WebOperationContext.Current.IncomingRequest.UriTemplateMatch?.RequestUri,
                WebOperationContext.Current.IncomingRequest.UserAgent,
                httpCorrelator, 
                usage);

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

                float usage = 0.0f;
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    using (PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", true))
                        usage = cpuCounter.NextValue();
                }

                this.m_httpSource.TraceEvent(TraceEventType.Verbose, 0, "HTTP RSP {0} : {1} ({2} ms - CPU {3}%)",
                    httpCorrelation.Key,
                    WebOperationContext.Current.OutgoingResponse.StatusCode,
                    processingTime.TotalMilliseconds,
                    usage);
            }
        }
    }
}
