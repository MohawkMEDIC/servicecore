using MARC.HI.EHRS.SVC.Messaging.FHIR.Wcf.Serialization;
using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Wcf.Behavior
{
    /// <summary>
    /// FHIR Serializer operation behavior
    /// </summary>
    internal class FHIRSerializerOperationBehavior : IOperationBehavior
    {
        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
        }

        /// <summary>
        /// Apply the dispatch behavior
        /// </summary>
        /// <param name="operationDescription"></param>
        /// <param name="dispatchOperation"></param>
        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            dispatchOperation.Formatter = new FhirMessageDispatchFormatter(operationDescription);
        }

        public void Validate(OperationDescription operationDescription)
        {
        }
    }
}