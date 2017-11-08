using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace MARC.HI.EHRS.SVC.Core.Wcf
{
    /// <summary>
    /// Log messaging endpoint behavior
    /// </summary>
    public class WcfLoggingEndpointBehavior : IEndpointBehavior
    {
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        /// <summary>
        /// Apply dispatch behavior
        /// </summary>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new LogMessageInspector());
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}
