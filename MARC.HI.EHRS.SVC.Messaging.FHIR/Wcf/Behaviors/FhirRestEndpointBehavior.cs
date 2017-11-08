﻿/*
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
 * Date: 2016-1-24
 */
using MARC.HI.EHRS.SVC.Core.Wcf;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Wcf.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Wcf.Behavior
{
    /// <summary>
    /// FHIR REST Endpoint Behavior
    /// </summary>
    public class FhirRestEndpointBehavior : IEndpointBehavior
    {
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        /// <summary>
        /// Apply a dispatcher
        /// </summary>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {

            // Add the inspector for the messaging
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new FhirMessageInspector());
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new LogMessageInspector());

            // Apply to each operation the FHIR formatter
            foreach (var op in endpoint.Contract.Operations)
            {
                //    foreach (var ob in op.OperationBehaviors.Where(o => o.GetType().Name.StartsWith("DataContract") || o.GetType().Name.StartsWith("XmlSerializer")).ToArray())
                //        op.OperationBehaviors.Remove(ob);// HACK: This is total Hax


                op.OperationBehaviors.Add(new FHIRSerializerOperationBehavior());
                //op.Formatter = new FHIRMessageDispatchFormatter(new OperationDescription(op.Name, endpoint.Contract));
                //}
            }
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            //base.Validate(endpoint);
            BindingElementCollection bindingElements = endpoint.Binding.CreateBindingElements();
            WebMessageEncodingBindingElement webEncoder = bindingElements.Find<WebMessageEncodingBindingElement>();
            if (webEncoder == null)
                throw new InvalidOperationException("FHIR Must be bound to type webHttpBinding");
        }

        ///// <summary>
        ///// Request dispatch formatter
        ///// </summary>
        //protected override IDispatchMessageFormatter GetRequestDispatchFormatter(OperationDescription operationDescription, ServiceEndpoint endpoint)
        //{
        //    return new FHIRMessageDispatchFormatter(operationDescription);
        //}

        ///// <summary>
        ///// Reply dispatch formatter
        ///// </summary>
        //protected override IDispatchMessageFormatter GetReplyDispatchFormatter(OperationDescription operationDescription, ServiceEndpoint endpoint)
        //{
        //    return new FHIRMessageDispatchFormatter(operationDescription);
        //}

        
    }
}
