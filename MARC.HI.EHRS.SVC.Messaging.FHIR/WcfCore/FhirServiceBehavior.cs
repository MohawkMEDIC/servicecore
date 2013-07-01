using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.Services;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Syndication;
using System.Diagnostics;
using MARC.HI.EHRS.SVC.Core.DataTypes;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Util;
using System.ComponentModel;
using MARC.Everest.Connectors;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Handlers;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Resources;
using System.Data;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.WcfCore
{
    /// <summary>
    /// FHIR service behavior
    /// </summary>
    public class FhirServiceBehavior : IFhirServiceContract
    {

        #region IFhirServiceContract Members

        public ResourceBase ReadResource(string resourceType, string id, string mimeType)
        {
            throw new NotImplementedException();
        }

        public ResourceBase VReadResource(string resourceType, string id, string vid, string mimeType)
        {
            throw new NotImplementedException();
        }

        public ResourceBase UpdateResource(string resourceType, string id, string mimeType, ResourceBase target)
        {
            throw new NotImplementedException();
        }

        public ResourceBase DeleteResource(string resourceType, string id, string mimeType)
        {
            throw new NotImplementedException();
        }

        public ResourceBase CreateResource(string resourceType, string mimeType, ResourceBase target)
        {
            throw new NotImplementedException();
        }

        public OperationOutcome ValidateResource(string resourceType, string id, ResourceBase target)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Searches a resource from the client registry datastore 
        /// </summary>
        public System.ServiceModel.Syndication.Atom10FeedFormatter SearchResource(string resourceType)
        {
            // Get the services from the service registry
            var auditService = ApplicationContext.CurrentContext.GetService(typeof(IAuditorService)) as IAuditorService;

            // Stuff for auditing and exception handling
            AuditData audit = null;
            List<IResultDetail> details = new List<IResultDetail>();
            FhirQueryResult result = null;

            try
            {

                // Get query parameters
                var queryParameters = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters;
                var resourceProcessor = FhirResourceHandlerUtil.GetResourceHandler(resourceType);

                // Setup outgoing content
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/atom+xml";
                if (resourceProcessor == null) // Unsupported resource
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.NotFound;
                    return null;
                }

                // Process incoming request
                result = resourceProcessor.Query(WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters);

                if (result.Outcome == ResultCode.Rejected)
                    throw new InvalidOperationException("Message was rejected");
                else if (result.Outcome != ResultCode.Accepted)
                    throw new DataException("Query failed");

                audit = AuditUtil.CreateAuditData(result.Results);
                // Create the Atom feed
                return new Atom10FeedFormatter(MessageUtil.CreateFeed(result));

            }
            catch (InvalidOperationException e)
            {
                Trace.TraceError(e.ToString());
                details.Add(new ResultDetail(ResultDetailType.Error, e.Message, e));
                audit = AuditUtil.CreateAuditData(null);
                audit.Outcome = OutcomeIndicator.SeriousFail;
                throw new WebFaultException<Atom10FeedFormatter>(new Atom10FeedFormatter(MessageUtil.CreateFeed(result)), (System.Net.HttpStatusCode)422);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                details.Add(new ResultDetail(ResultDetailType.Error, e.Message, e)); 
                audit = AuditUtil.CreateAuditData(null);
                audit.Outcome = OutcomeIndicator.EpicFail;
                throw new WebFaultException<Atom10FeedFormatter>(new Atom10FeedFormatter(MessageUtil.CreateFeed(result)), System.Net.HttpStatusCode.InternalServerError);
            }
            finally
            {
                if (auditService != null)
                    auditService.SendAudit(audit);
            }
            return null;

        }

        public System.Xml.XmlElement GetOptions()
        {
            throw new NotImplementedException();
        }

        public System.ServiceModel.Syndication.Atom10FeedFormatter PostTransaction(System.ServiceModel.Syndication.Atom10FeedFormatter feed)
        {
            throw new NotImplementedException();
        }

        public System.ServiceModel.Syndication.Atom10FeedFormatter GetResourceInstanceHistory(string resourceType, string id, string mimeType)
        {
            throw new NotImplementedException();
        }

        public System.ServiceModel.Syndication.Atom10FeedFormatter GetResourceHistory(string resourceType, string mimeType)
        {
            throw new NotImplementedException();
        }

        public System.ServiceModel.Syndication.Atom10FeedFormatter GetHistory(string mimeType)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
