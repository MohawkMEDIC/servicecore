using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.Services;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Syndication;
using System.Diagnostics;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Util;
using System.ComponentModel;
using MARC.Everest.Connectors;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Handlers;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Resources;
using System.Data;
using System.IO;
using System.Xml;
using System.Net;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Configuration;
using System.Configuration;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Reflection;
using System.IO.Compression;
using MARC.HI.EHRS.SVC.Auditing.Data;
using MARC.HI.EHRS.SVC.Auditing.Services;
using MARC.HI.EHRS.SVC.Core;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.WcfCore
{
    /// <summary>
    /// FHIR service behavior
    /// </summary>
    public class FhirServiceBehavior : IFhirServiceContract
    {

        private const string FHIR_TYPE = "application/fhir+xml; charset=utf-8";
        private const string ATOM_TYPE = "application/atom+xml; charset=utf-8";

        #region IFhirServiceContract Members

        /// <summary>
        /// Get schema
        /// </summary>
        public XmlSchema GetSchema(int schemaId)
        {
            XmlSchemas schemaCollection = new XmlSchemas();
            
            XmlReflectionImporter importer = new XmlReflectionImporter("http://hl7.org/fhir");
            XmlSchemaExporter exporter = new XmlSchemaExporter(schemaCollection);
            
            foreach(var cls in typeof(FhirServiceBehavior).Assembly.GetTypes().Where(o=>o.GetCustomAttribute<XmlRootAttribute>() != null && !o.IsGenericTypeDefinition))
                exporter.ExportTypeMapping(importer.ImportTypeMapping(cls, "http://hl7.org/fhir"));

            return schemaCollection[schemaId];
        }

        /// <summary>
        /// Get the index
        /// </summary>
        public Stream Index()
        {
            try
            {
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
                WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-Disposition", "filename=\"index.html\"");
                WebOperationContext.Current.OutgoingResponse.LastModified = DateTime.UtcNow;
                FhirServiceConfiguration config = ConfigurationManager.GetSection("marc.hi.ehrs.svc.messaging.fhir") as FhirServiceConfiguration;
                if (!String.IsNullOrEmpty(config.LandingPage))
                {
                    using (var fs = File.OpenRead(config.LandingPage))
                    {
                        MemoryStream ms = new MemoryStream();
                        int br = 1024;
                        byte[] buffer = new byte[1024];
                        while (br == 1024)
                        {
                            br = fs.Read(buffer, 0, 1024);
                            ms.Write(buffer, 0, br);
                        }
                        ms.Seek(0, SeekOrigin.Begin);
                        return ms;
                    }
                }
                else
                    return typeof(FhirServiceBehavior).Assembly.GetManifestResourceStream("MARC.HI.EHRS.SVC.Messaging.FHIR.index.htm");
            }
            catch (IOException)
            {
                throw new WebFaultException(HttpStatusCode.NotFound);
            }
        }

        /// <summary>
        /// Read a reasource
        /// </summary>
        public ResourceBase ReadResource(string resourceType, string id, string mimeType)
        {
            FhirOperationResult result = null;
            try
            {
               
                // Setup outgoing content
                WebOperationContext.Current.OutgoingResponse.ContentType = FHIR_TYPE;
                result = this.PerformRead(resourceType, id, null);
                String baseUri = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.AbsoluteUri;
                WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-Location", String.Format("{0}/_history/{1}", baseUri, result.Results[0].VersionId));
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
                WebOperationContext.Current.OutgoingResponse.ContentType = FHIR_TYPE;
                result = this.PerformRead(resourceType, id, vid);
                return result.Results[0];
            }
            catch (Exception e)
            {
                return this.ErrorHelper(e, result, false) as ResourceBase;
            }
        }

        /// <summary>
        /// Update a resource
        /// </summary>
        public ResourceBase UpdateResource(string resourceType, string id, string mimeType, ResourceBase target)
        {
            FhirOperationResult result = null;
            AuditData audit = null;
            IAuditorService auditService = ApplicationContext.Current.GetService(typeof(IAuditorService)) as IAuditorService;

            try
            {

                // Setup outgoing content/
                WebOperationContext.Current.OutgoingResponse.ContentType = FHIR_TYPE;

                // Create or update?
                var handler = FhirResourceHandlerUtil.GetResourceHandler(resourceType);
                if (handler == null)
                    throw new FileNotFoundException(); // endpoint not found!

                result = handler.Update(id, target, DataPersistenceMode.Production);
                if (result == null || result.Results.Count == 0) // Create
                    throw new NotSupportedException("Update is not supported on non-existant resource");

                if (result == null || result.Outcome == ResultCode.Rejected)
                    throw new InvalidDataException("Resource structure is not valid");
                else if (result.Outcome == ResultCode.AcceptedNonConformant)
                    throw new ConstraintException("Resource not conformant");
                else if (result.Outcome == ResultCode.TypeNotAvailable)
                    throw new FileNotFoundException(String.Format("Resource {0} not found", WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri));
                else if (result.Outcome != ResultCode.Accepted)
                    throw new DataException("Update failed");

                audit = AuditUtil.CreateAuditData(result.Results);

                String baseUri = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri.AbsoluteUri;
                WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-Location", String.Format("{0}{1}/{2}/_history/{3}", baseUri, resourceType, result.Results[0].Id, result.Results[0].VersionId));
                WebOperationContext.Current.OutgoingResponse.LastModified = result.Results[0].Timestamp;
                WebOperationContext.Current.OutgoingResponse.ETag = result.Results[0].VersionId;

                return result.Results[0];

            }
            catch (Exception e)
            {
                audit = AuditUtil.CreateAuditData(null);
                audit.Outcome = OutcomeIndicator.EpicFail;
                return this.ErrorHelper(e, result, false) as ResourceBase;
            }
            finally
            {
                if (auditService != null)
                    auditService.SendAudit(audit);
            }
        }

        /// <summary>
        /// Delete a resource
        /// </summary>
        public ResourceBase DeleteResource(string resourceType, string id, string mimeType)
        {
            FhirOperationResult result = null;
            AuditData audit = null;
            IAuditorService auditService = ApplicationContext.Current.GetService(typeof(IAuditorService)) as IAuditorService;

            try
            {

                // Setup outgoing content/
                WebOperationContext.Current.OutgoingResponse.ContentType = FHIR_TYPE;
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NoContent;

                // Create or update?
                var handler = FhirResourceHandlerUtil.GetResourceHandler(resourceType);
                if (handler == null)
                    throw new FileNotFoundException(); // endpoint not found!

                result = handler.Delete(id, DataPersistenceMode.Production);

                if (result == null || result.Outcome == ResultCode.Rejected)
                    throw new NotSupportedException();
                else if (result.Outcome == ResultCode.TypeNotAvailable)
                    throw new FileNotFoundException(String.Format("Resource {0} not found", WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri));
                else if (result.Outcome != ResultCode.Accepted)
                    throw new DataException("Delete failed");

                audit = AuditUtil.CreateAuditData(result.Results);

                return null;

            }
            catch (Exception e)
            {
               
                audit = AuditUtil.CreateAuditData(null);
                audit.Outcome = OutcomeIndicator.EpicFail;
                return this.ErrorHelper(e, result, false) as ResourceBase;
            }
            finally
            {
                if (auditService != null)
                    auditService.SendAudit(audit);
            }
        }

        /// <summary>
        /// Create a resource
        /// </summary>
        public ResourceBase CreateResource(string resourceType, string mimeType, ResourceBase target)
        {
            FhirOperationResult result = null;

            AuditData audit = null;
            IAuditorService auditService = ApplicationContext.Current.GetService(typeof(IAuditorService)) as IAuditorService;

            try
            {

                // Setup outgoing content
                WebOperationContext.Current.OutgoingResponse.ContentType = FHIR_TYPE;

                // Create or update?
                var handler = FhirResourceHandlerUtil.GetResourceHandler(resourceType);
                if (handler == null)
                    throw new FileNotFoundException(); // endpoint not found!

                result = handler.Create(target, DataPersistenceMode.Production);
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Created;

                if (result == null || result.Outcome == ResultCode.Rejected)
                    throw new InvalidDataException("Resource structure is not valid");
                else if (result.Outcome == ResultCode.AcceptedNonConformant)
                    throw new ConstraintException("Resource not conformant");
                else if (result.Outcome == ResultCode.TypeNotAvailable)
                    throw new FileNotFoundException(String.Format("Resource {0} not found", WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri));
                else if (result.Outcome != ResultCode.Accepted)
                    throw new DataException("Create failed");

                audit = AuditUtil.CreateAuditData(result.Results);

                String baseUri = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri.AbsoluteUri;
                WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-Location", String.Format("{0}{1}/{2}/_history/{3}", baseUri, resourceType, result.Results[0].Id, result.Results[0].VersionId));
                WebOperationContext.Current.OutgoingResponse.LastModified = result.Results[0].Timestamp;
                WebOperationContext.Current.OutgoingResponse.ETag = result.Results[0].VersionId;


                return result.Results[0];

            }
            catch (Exception e)
            {
                audit = AuditUtil.CreateAuditData(null);
                audit.Outcome = OutcomeIndicator.EpicFail;
                return this.ErrorHelper(e, result, false) as ResourceBase;
            }
            finally
            {
                if (auditService != null)
                    auditService.SendAudit(audit);
            }
        }

        /// <summary>
        /// Validate a resource (really an update with debugging / non comit)
        /// </summary>
        public OperationOutcome ValidateResource(string resourceType, string id, ResourceBase target)
        {
            FhirOperationResult result = null;
            try
            {

                // Setup outgoing content
                WebOperationContext.Current.OutgoingResponse.ContentType = FHIR_TYPE;

                // Create or update?
                var handler = FhirResourceHandlerUtil.GetResourceHandler(resourceType);
                if (handler == null)
                    throw new FileNotFoundException(); // endpoint not found!
                
                result = handler.Update(id, target, DataPersistenceMode.Debugging);
                if (result == null || result.Results.Count == 0) // Create
                {
                    result = handler.Create(target, DataPersistenceMode.Debugging);
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Created;
                }

                if (result == null || result.Outcome == ResultCode.Rejected)
                    throw new InvalidDataException("Resource structure is not valid");
                else if (result.Outcome == ResultCode.AcceptedNonConformant)
                    throw new ConstraintException("Resource not conformant");
                else if (result.Outcome == ResultCode.TypeNotAvailable)
                    throw new FileNotFoundException(String.Format("Resource {0} not found", WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri));
                else if (result.Outcome != ResultCode.Accepted)
                    throw new DataException("Validate failed");
                                
                // Return constraint
                return MessageUtil.CreateOutcomeResource(result);

            }
            catch (Exception e)
            {
                return this.ErrorHelper(e, result, false) as OperationOutcome;
            }
        }

        /// <summary>
        /// Searches a resource from the client registry datastore 
        /// </summary>
        public Object SearchResource(string resourceType)
        {
            // Get the services from the service registry
            var auditService = ApplicationContext.Current.GetService(typeof(IAuditorService)) as IAuditorService;

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
                WebOperationContext.Current.OutgoingResponse.ContentType = ATOM_TYPE;
                WebOperationContext.Current.OutgoingRequest.Headers.Add("Last-Modified", DateTime.Now.ToString("ddd, dd MMM yyyy HH:mm:ss zzz"));
                
                if (resourceProcessor == null) // Unsupported resource
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.NotFound;
                    return null;
                }
                
                // TODO: Appropriately format response
                // Process incoming request
                result = resourceProcessor.Query(WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters);

                if (result == null || result.Outcome == ResultCode.Rejected)
                    throw new InvalidDataException("Message was rejected");
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
                return this.ErrorHelper(e, result, false) ;
            }
            finally
            {
                if (auditService != null)
                    auditService.SendAudit(audit);
            }

        }

        /// <summary>
        /// Get conformance
        /// </summary>
        public Conformance GetOptions()
        {
            var retVal = new Conformance(); // ConformanceUtil.GetConformanceStatement();
            WebOperationContext.Current.OutgoingResponse.ContentType = FHIR_TYPE;
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-Location", String.Format("{0}Conformance/{1}/_history/{2}", WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri, retVal.Id, retVal.VersionId));
            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
            WebOperationContext.Current.OutgoingResponse.Headers.Remove("Content-Disposition");
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-Disposition", "filename=\"conformance.xml\"");
            return retVal;
        }

        /// <summary>
        /// Posting transaction is not supported
        /// </summary>
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

                WebOperationContext.Current.OutgoingResponse.ContentType = ATOM_TYPE;
                readResult = this.PerformRead(resourceType, id, String.Empty);
                WebOperationContext.Current.OutgoingResponse.Headers.Remove("Content-Disposition");
                return new Atom10FeedFormatter(MessageUtil.CreateFeed(readResult));
            }
            catch (Exception e)
            {
                return this.ErrorHelper(e, readResult, true) as Atom10FeedFormatter;
            }
        }

        /// <summary>
        /// Not implemented result
        /// </summary>
        public System.ServiceModel.Syndication.Atom10FeedFormatter GetResourceHistory(string resourceType, string mimeType)
        {
            
            var result = new FhirOperationResult()
            {
                Outcome = ResultCode.Rejected,
                Details = new List<IResultDetail>() { 
                    new ResultDetail(ResultDetailType.Error, "For security reasons resource history is not supported", null, null)
                }
            };
            return this.ErrorHelper(new NotImplementedException(), result, true) as Atom10FeedFormatter;
            
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public System.ServiceModel.Syndication.Atom10FeedFormatter GetHistory(string mimeType)
        {
            var result = new FhirOperationResult()
            {
                Outcome = ResultCode.Rejected,
                Details = new List<IResultDetail>() { 
                    new ResultDetail(ResultDetailType.Error, "For security reasons system history is not supported", null, null)
                }
            };
            return this.ErrorHelper(new NotImplementedException(), result, true) as Atom10FeedFormatter;
        }

        /// <summary>
        /// Throw an appropriate exception based on the caught exception
        /// </summary>
        private object ErrorHelper(Exception e, FhirOperationResult result, bool returnBundle)
        {

            if (result == null && returnBundle)
                result = new FhirQueryResult() { Details = new List<IResultDetail>(), Query = new FhirQuery() { Start = 0, Quantity = 0 } };
            else if (result == null)
                result = new FhirOperationResult() { Details = new List<IResultDetail>() { new ResultDetail(ResultDetailType.Error, "No information available", e) } };


            Trace.TraceError(e.ToString());
            result.Details.Add(new ResultDetail(ResultDetailType.Error, e.Message, e));

            HttpStatusCode retCode = HttpStatusCode.OK;

            if (e is NotSupportedException)
                retCode = System.Net.HttpStatusCode.MethodNotAllowed;
            else if (e is NotImplementedException)
                retCode = System.Net.HttpStatusCode.NotImplemented;
            else if (e is InvalidDataException)
                retCode = HttpStatusCode.BadRequest;
            else if (e is FileLoadException)
                retCode = System.Net.HttpStatusCode.Gone;
            else if (e is FileNotFoundException || e is ArgumentException)
                retCode = System.Net.HttpStatusCode.NotFound;
            else if (e is ConstraintException)
                retCode = (HttpStatusCode)422;
            else
                retCode = System.Net.HttpStatusCode.InternalServerError;

            WebOperationContext.Current.OutgoingResponse.StatusCode = retCode;
            WebOperationContext.Current.OutgoingResponse.Format = WebMessageFormat.Xml;

            if (returnBundle)
            {
                WebOperationContext.Current.OutgoingResponse.ContentType = ATOM_TYPE;
                throw new WebFaultException<Atom10FeedFormatter>(new Atom10FeedFormatter(MessageUtil.CreateFeed(result)), retCode);
            }
            else
            {
                WebOperationContext.Current.OutgoingResponse.ContentType = FHIR_TYPE;
                WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-Disposition", "filename=\"error.xml\"");
                throw new WebFaultException<OperationOutcome>(MessageUtil.CreateOutcomeResource(result), retCode);
            }
                //return MessageUtil.CreateOutcomeResource(result);

        }

        
        /// <summary>
        /// Perform a read against the underlying IFhirResourceHandler
        /// </summary>
        private FhirOperationResult PerformRead(string resourceType, string id, string vid)
        {
            // Get the services from the service registry
            var auditService = ApplicationContext.Current.GetService(typeof(IAuditorService)) as IAuditorService;

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
                
                    throw new FileNotFoundException("Specified resource type is not found");
                

                // TODO: Appropriately format response
                // Process incoming request
                result = resourceProcessor.Read(id, vid);

                if (result.Outcome == ResultCode.Rejected)
                    throw new InvalidDataException("Message was rejected");
                else if (result.Outcome == (ResultCode.NotAvailable | ResultCode.Rejected))
                    throw new FileLoadException(String.Format("Resource {0} is no longer available", WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri));
                else if (result.Outcome == ResultCode.TypeNotAvailable ||
                    result.Results == null || result.Results.Count == 0)
                    throw new FileNotFoundException(String.Format("Resource {0} not found", WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri));
                else if (result.Outcome != ResultCode.Accepted)
                    throw new DataException("Read failed");
                audit = AuditUtil.CreateAuditData(result.Results);

                // Create the result
                if (result.Results != null && result.Results.Count > 0 && WebOperationContext.Current.OutgoingResponse.ContentType == FHIR_TYPE)
                {
                    WebOperationContext.Current.OutgoingResponse.LastModified = result.Results[0].Timestamp;
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

        /// <summary>
        /// Get meta-data
        /// </summary>
        public Conformance GetMetaData()
        {
            return this.GetOptions();
        }

        #endregion

        #region IFhirServiceContract Members

        /// <summary>
        /// Get the current time
        /// </summary>
        public DateTime Time()
        {
            return DateTime.Now;
        }

        #endregion

        /// <summary>
        /// Create or update
        /// </summary>
        public ResourceBase CreateUpdateResource(string resourceType, string id, string mimeType, ResourceBase target)
        {
            return this.UpdateResource(resourceType, id, mimeType, target);
        }

        /// <summary>
        /// Alternate search
        /// </summary>
        public Object SearchResourceAlt(string resourceType)
        {
            return this.SearchResource(resourceType);
        }
    }
}
