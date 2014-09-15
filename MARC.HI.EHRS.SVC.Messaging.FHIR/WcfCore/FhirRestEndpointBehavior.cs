using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.WcfCore
{
    /// <summary>
    /// Represents an endpoint behavior
    /// </summary>
    public class FhirRestEndpointBehavior : IEndpointBehavior
    {
        /// <summary>
        /// Add binding parameters
        /// </summary>
        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            
        }

        /// <summary>
        /// Apply a client behavior
        /// </summary>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
        }

        /// <summary>
        /// Apply a dispatcher behavior
        /// </summary>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new FhirMessageInspector());
        }

        /// <summary>
        /// Validate endpoint
        /// </summary>
        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}
