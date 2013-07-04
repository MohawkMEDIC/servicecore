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
using System.IO;
using System.Xml;
using System.Net;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.WcfCore
{
    /// <summary>
    /// FHIR service behavior
    /// </summary>
    public class FhirServiceBehavior : IFhirServiceContract
    {

        #region IFhirServiceContract Members

        /// <summary>
        /// Read a reasource
        /// </summary>
        public ResourceBase ReadResource(string resourceType, string id, string mimeType)
        {
            FhirOperationResult result = null;
            try
            {
                // Setup outgoing content
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/fhir+xml";
                result = this.PerformRead(resourceType, id, null);
                return result.Results[0];
            }
            catch (Exception e)
            {
                return this.ErrorHelper(e, result, false) as ResourceBase;
            }
        }

        /// <summary>
        /// Read resource with version
        /// </summary>
        public ResourceBase VReadResource(string resourceType, string id, string vid, string mimeType)
        {
            FhirOperationResult result = null;
            try
            {
                // Setup outgoing content
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/fhir+xml";
                result = this.PerformRead(resourceType, id, vid);
                return result.Results[0];
            }
            catch (Exception e)
            {
                return this.ErrorHelper(e, result, false) as ResourceBase;
            }
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
                WebOperationContext.Current.OutgoingRequest.Headers.Add("Last-Modified", DateTime.Now.ToString("ddd, dd MMM yyyy HH:mm:ss zzz"));
                
                if (resourceProcessor == null) // Unsupported resource
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.NotFound;
                    return null;
                }
                
                // TODO: Appropriately format response
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
            catch (Exception e)
            {
                audit = AuditUtil.CreateAuditData(null);
                audit.Outcome = OutcomeIndicator.EpicFail; 
                return this.ErrorHelper(e, result, true) as Atom10FeedFormatter;
            }
            finally
            {
                if (auditService != null)
                    auditService.SendAudit(audit);
            }

        }

       public System.Xml.XmlElement GetOptions()
        {
            throw new NotImplementedException();
        }

        public System.ServiceModel.Syndication.Atom10FeedFormatter PostTransaction(System.ServiceModel.Syndication.Atom10FeedFormatter feed)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a resource's history
        /// </summary>
        public System.ServiceModel.Syndication.Atom10FeedFormatter GetResourceInstanceHistory(string resourceType, string id, string mimeType)
        {
            FhirOperationResult readResult = null;
            try
            {
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/atom+xml";
                readResult = this.PerformRead(resourceType, id, String.Empty);
                WebOperationContext.Current.OutgoingResponse.Headers.Remove("Content-Disposition");
                return new Atom10FeedFormatter(MessageUtil.CreateFeed(readResult));
            }
            catch (Exception e)
            {
                return this.ErrorHelper(e, readResult, true) as Atom10FeedFormatter;
            }
        }

        public System.ServiceModel.Syndication.Atom10FeedFormatter GetResourceHistory(string resourceType, string mimeType)
        {
            throw new NotImplementedException();
        }

        public System.ServiceModel.Syndication.Atom10FeedFormatter GetHistory(string mimeType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Throw an appropriate exception based on the caught exception
        /// </summary>
        private object ErrorHelper(Exception e, FhirOperationResult result, bool returnBundle)
        {

            if (result == null && returnBundle)
            {
                result = new FhirQueryResult() { Details = new List<IResultDetail>(), Query = new FhirQuery() { Start = 0, Quantity = 0 } };
            }

            Trace.TraceError(e.ToString());
            result.Details.Add(new ResultDetail(ResultDetailType.Error, e.Message, e));

            HttpStatusCode retCode = HttpStatusCode.OK;
            
            if (e is NotSupportedException)
                retCode = System.Net.HttpStatusCode.MethodNotAllowed;
            else if (e is NotImplementedException)
                retCode = System.Net.HttpStatusCode.NotImplemented;
            else if (e is InvalidOperationException)
                retCode = HttpStatusCode.BadRequest;
            else if (e is FileLoadException)
                retCode = System.Net.HttpStatusCode.Gone;
            else if (e is FileNotFoundException)
                retCode = System.Net.HttpStatusCode.NotFound;
            else
                retCode = System.Net.HttpStatusCode.InternalServerError;

            WebOperationContext.Current.OutgoingResponse.StatusCode = retCode;
            WebOperationContext.Current.OutgoingResponse.Format = WebMessageFormat.Xml;
            
            if (returnBundle)
                throw new WebFaultException<Atom10FeedFormatter>(new Atom10FeedFormatter(MessageUtil.CreateFeed(result)), retCode);
            else
                throw new WebFaultException<OperationOutcome>(MessageUtil.CreateOutcomeResource(result), retCode);

                //return MessageUtil.CreateOutcomeResource(result);

        }


        /// <summary>
        /// Perform a read against the underlying IFhirResourceHandler
        /// </summary>
        private FhirOperationResult PerformRead(string resourceType, string id, string vid)
        {
            // Get the services from the service registry
            var auditService = ApplicationContext.CurrentContext.GetService(typeof(IAuditorService)) as IAuditorService;

            // Stuff for auditing and exception handling
            AuditData audit = null;
            List<IResultDetail> details = new List<IResultDetail>();
            FhirOperationResult result = null;

            try
            {

                // Get query parameters
                var queryParameters = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters;
                var resourceProcessor = FhirResourceHandlerUtil.GetResourceHandler(resourceType);

                if (resourceProcessor == null) // Unsupported resource
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.NotFound;
                    return null;
                }

                // TODO: Appropriately format response
                // Process incoming request
                result = resourceProcessor.Read(id, vid);

                if (result.Outcome == ResultCode.Rejected)
                    throw new InvalidOperationException("Message was rejected");
                else if (result.Outcome == (ResultCode.NotAvailable | ResultCode.Rejected))
                    throw new FileLoadException(String.Format("Resource {0} is no longer available", WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri));
                else if (result.Outcome == ResultCode.TypeNotAvailable ||
                    result.Results == null || result.Results.Count == 0)
                    throw new FileNotFoundException(String.Format("Resource {0} not found", WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri));
                else if (result.Outcome != ResultCode.Accepted)
                    throw new DataException("Read failed");
                audit = AuditUtil.CreateAuditData(result.Results);

                WebOperationContext.Current.OutgoingResponse.Headers.Add("Last-Modified", result.Results[0].Timestamp.ToString("ddd, dd MMM yyyy HH:mm:ss zzz"));
                // Create the result
                if (result.Results != null && result.Results.Count == 1)
                {
                    WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-Disposition", String.Format("filename=\"{0}-{1}-{2}.xml\"", resourceType, result.Results[0].Id, result.Results[0].VersionId));
                    WebOperationContext.Current.OutgoingResponse.ETag = result.Results[0].VersionId;
                }
                return result;

            }
            catch (Exception e)
            {
                audit = AuditUtil.CreateAuditData(null);
                audit.Outcome = OutcomeIndicator.EpicFail;
                throw;
            }
            finally
            {
                if (auditService != null)
                    auditService.SendAudit(audit);
            }
        }
        #endregion
    }
}
