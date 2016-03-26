using MARC.HI.EHRS.SVC.Core.Exceptions;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Resources;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Authentication;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Wcf.Serialization
{
    /// <summary>
    /// Error handler
    /// </summary>
    public class FhirErrorHandler : IErrorHandler
    {
        /// <summary>
        /// Handle error
        /// </summary>
        public bool HandleError(Exception error)
        {
            return true;
        }

        /// <summary>
        /// Provide fault
        /// </summary>
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            // Formulate appropriate response
            if (error is PolicyViolationException || error is SecurityException || (error as FaultException)?.Code.SubCode?.Name == "FailedAuthentication")
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Forbidden;
            else if (error is SecurityTokenException)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                WebOperationContext.Current.OutgoingResponse.Headers.Add("WWW-Authenticate", "Bearer");
            }
            else if (error is WebFaultException)
                WebOperationContext.Current.OutgoingResponse.StatusCode = (error as WebFaultException).StatusCode;
            else if (error is Newtonsoft.Json.JsonException ||
                error is System.Xml.XmlException)
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
            else
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;

            // Construct an error result
            var errorResult = new OperationOutcome()
            { 
                Issue = new List<Issue>()
                {
                    new Issue() { Diagnostics  = error.Message, Severity = IssueSeverity.Error }
                }
            };

            // Cascade inner exceptions
            var ie = error.InnerException;
            while (ie != null)
                errorResult.Issue.Add(new Issue() { Diagnostics = String.Format("Caused by {0}", error.Message), Severity = IssueSeverity.Error });

            // Return error in XML only at this point
            fault = new FhirMessageDispatchFormatter().SerializeReply(version, null, errorResult);

            
        }
    }
}
