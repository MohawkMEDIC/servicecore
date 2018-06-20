using MARC.Everest.Connectors;
using MARC.HI.EHRS.SVC.Auditing.Data;
using MARC.HI.EHRS.SVC.Auditing.Services;
using MARC.HI.EHRS.SVC.Core;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Handlers;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Reflection;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Configuration;
using MARC.HI.EHRS.SVC.Core.Services;
using System.Diagnostics;
using System.Net;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Resources;
using MARC.HI.EHRS.SVC.Core.Exceptions;
using System.ServiceModel;
using SwaggerWcf.Attributes;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Wcf
{
    /// <summary>
    /// FHIR service behavior
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    [SwaggerWcf("/fhir")]
    public class FhirServiceBehavior : IFhirServiceContract
    {

        
        private TraceSource m_tracer = new TraceSource("MARC.HI.EHRS.SVC.Messaging.FHIR");

        #region IFhirServiceContract Members

        /// <summary>
        /// Get schema
        /// </summary>
        [SwaggerWcfHidden]
        public XmlSchema GetSchema(int schemaId)
        {
            this.ThrowIfNotReady();

            XmlSchemas schemaCollection = new XmlSchemas();

            XmlReflectionImporter importer = new XmlReflectionImporter("http://hl7.org/fhir");
            XmlSchemaExporter exporter = new XmlSchemaExporter(schemaCollection);

            foreach (var cls in typeof(FhirServiceBehavior).Assembly.GetTypes().Where(o => o.GetCustomAttribute<XmlRootAttribute>() != null && !o.IsGenericTypeDefinition))
                exporter.ExportTypeMapping(importer.ImportTypeMapping(cls, "http://hl7.org/fhir"));

            return schemaCollection[schemaId];
        }

        /// <summary>
        /// Get the index
        /// </summary>
        [SwaggerWcfHidden]
        public Stream Index()
        {
            this.ThrowIfNotReady();

            try
            {
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
                WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-Disposition", "filename=\"index.html\"");
                WebOperationContext.Current.OutgoingResponse.LastModified = DateTime.UtcNow;
                FhirServiceConfiguration config = ApplicationContext.Current.GetService<IConfigurationManager>().GetSection("marc.hi.ehrs.svc.messaging.fhir") as FhirServiceConfiguration;
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
        [SwaggerWcfContentTypes(ConsumeTypes = new String[] { "application/xml+fhir", "application/json+fhir" }, ProduceTypes = new String[] { "application/xml+fhir", "application/json+fhir" })]
        [SwaggerWcfTag("HL7 Fast Health Interoperability Resources (FHIR)")]
        [SwaggerWcfResponse(400, "The client sent a request in a format which is not understood by this server")]
        [SwaggerWcfResponse(401, "The client attempted to perform an operation requires authentication (this can also happen when the client is required to elevate to another user credential)")]
        [SwaggerWcfResponse(403, "The client attempted to perform an operation which it is not permitted to perform")]
        [SwaggerWcfResponse(404, "The client requested access to a resource which is not available")]
        [SwaggerWcfResponse(422, "The client requested an operation which violated business rules or some other formal constraint")]
        [SwaggerWcfResponse(500, "The server encountered an issue processing your request")]
        [SwaggerWcfResponse(503, "This service is not in a state where it can service your request")]
        [SwaggerWcfSecurity("OAUTH2")]
        public DomainResourceBase ReadResource(string resourceType, string id, string mimeType)
        {
            this.ThrowIfNotReady();

            FhirOperationResult result = null;
            try
            {

                // Setup outgoing content
                result = this.PerformRead(resourceType, id, null);
                String baseUri = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.AbsoluteUri;
                WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-Location", String.Format("{0}/_history/{1}", baseUri, result.Results[0].VersionId));
                return result.Results[0];
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// Read resource with version
        /// </summary>
        [SwaggerWcfContentTypes(ConsumeTypes = new String[] { "application/xml+fhir", "application/json+fhir" }, ProduceTypes = new String[] { "application/xml+fhir", "application/json+fhir" })]
        [SwaggerWcfTag("HL7 Fast Health Interoperability Resources (FHIR)")]
        [SwaggerWcfResponse(400, "The client sent a request in a format which is not understood by this server")]
        [SwaggerWcfResponse(401, "The client attempted to perform an operation requires authentication (this can also happen when the client is required to elevate to another user credential)")]
        [SwaggerWcfResponse(403, "The client attempted to perform an operation which it is not permitted to perform")]
        [SwaggerWcfResponse(404, "The client requested access to a resource which is not available")]
        [SwaggerWcfResponse(422, "The client requested an operation which violated business rules or some other formal constraint")]
        [SwaggerWcfResponse(500, "The server encountered an issue processing your request")]
        [SwaggerWcfResponse(503, "This service is not in a state where it can service your request")]
        [SwaggerWcfSecurity("OAUTH2")]
        public DomainResourceBase VReadResource(string resourceType, string id, string vid, string mimeType)
        {
            this.ThrowIfNotReady();

            FhirOperationResult result = null;
            try
            {
                // Setup outgoing content
                result = this.PerformRead(resourceType, id, vid);
                return result.Results[0];
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// Update a resource
        /// </summary>
        [SwaggerWcfContentTypes(ConsumeTypes = new String[] { "application/xml+fhir", "application/json+fhir" }, ProduceTypes = new String[] { "application/xml+fhir", "application/json+fhir" })]
        [SwaggerWcfTag("HL7 Fast Health Interoperability Resources (FHIR)")]
        [SwaggerWcfResponse(400, "The client sent a request in a format which is not understood by this server")]
        [SwaggerWcfResponse(401, "The client attempted to perform an operation requires authentication (this can also happen when the client is required to elevate to another user credential)")]
        [SwaggerWcfResponse(403, "The client attempted to perform an operation which it is not permitted to perform")]
        [SwaggerWcfResponse(404, "The client requested access to a resource which is not available")]
        [SwaggerWcfResponse(422, "The client requested an operation which violated business rules or some other formal constraint")]
        [SwaggerWcfResponse(500, "The server encountered an issue processing your request")]
        [SwaggerWcfResponse(503, "This service is not in a state where it can service your request")]
        [SwaggerWcfSecurity("OAUTH2")]
        public DomainResourceBase UpdateResource(string resourceType, string id, string mimeType, DomainResourceBase target)
        {
            this.ThrowIfNotReady();

            FhirOperationResult result = null;
            AuditData audit = null;
            IAuditorService auditService = ApplicationContext.Current.GetService(typeof(IAuditorService)) as IAuditorService;

            try
            {

                // Setup outgoing content/

                // Create or update?
                var handler = FhirResourceHandlerUtil.GetResourceHandler(resourceType);
                if (handler == null)
                    throw new FileNotFoundException(); // endpoint not found!

                result = handler.Update(id, target, TransactionMode.Commit);
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
                throw;
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
        [SwaggerWcfContentTypes(ConsumeTypes = new String[] { "application/xml+fhir", "application/json+fhir" }, ProduceTypes = new String[] { "application/xml+fhir", "application/json+fhir" })]
        [SwaggerWcfTag("HL7 Fast Health Interoperability Resources (FHIR)")]
        [SwaggerWcfResponse(400, "The client sent a request in a format which is not understood by this server")]
        [SwaggerWcfResponse(401, "The client attempted to perform an operation requires authentication (this can also happen when the client is required to elevate to another user credential)")]
        [SwaggerWcfResponse(403, "The client attempted to perform an operation which it is not permitted to perform")]
        [SwaggerWcfResponse(404, "The client requested access to a resource which is not available")]
        [SwaggerWcfResponse(422, "The client requested an operation which violated business rules or some other formal constraint")]
        [SwaggerWcfResponse(500, "The server encountered an issue processing your request")]
        [SwaggerWcfResponse(503, "This service is not in a state where it can service your request")]
        [SwaggerWcfSecurity("OAUTH2")]
        public DomainResourceBase DeleteResource(string resourceType, string id, string mimeType)
        {
            this.ThrowIfNotReady();

            FhirOperationResult result = null;
            AuditData audit = null;
            IAuditorService auditService = ApplicationContext.Current.GetService(typeof(IAuditorService)) as IAuditorService;

            try
            {

                // Setup outgoing content/
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NoContent;

                // Create or update?
                var handler = FhirResourceHandlerUtil.GetResourceHandler(resourceType);
                if (handler == null)
                    throw new FileNotFoundException(); // endpoint not found!

                result = handler.Delete(id, TransactionMode.Commit);

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
                throw;
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
        [SwaggerWcfContentTypes(ConsumeTypes = new String[] { "application/xml+fhir", "application/json+fhir" }, ProduceTypes = new String[] { "application/xml+fhir", "application/json+fhir" })]
        [SwaggerWcfTag("HL7 Fast Health Interoperability Resources (FHIR)")]
        [SwaggerWcfResponse(400, "The client sent a request in a format which is not understood by this server")]
        [SwaggerWcfResponse(401, "The client attempted to perform an operation requires authentication (this can also happen when the client is required to elevate to another user credential)")]
        [SwaggerWcfResponse(403, "The client attempted to perform an operation which it is not permitted to perform")]
        [SwaggerWcfResponse(404, "The client requested access to a resource which is not available")]
        [SwaggerWcfResponse(422, "The client requested an operation which violated business rules or some other formal constraint")]
        [SwaggerWcfResponse(500, "The server encountered an issue processing your request")]
        [SwaggerWcfResponse(503, "This service is not in a state where it can service your request")]
        [SwaggerWcfSecurity("OAUTH2")]
        public DomainResourceBase CreateResource(string resourceType, string mimeType, DomainResourceBase target)
        {
            this.ThrowIfNotReady();

            FhirOperationResult result = null;

            AuditData audit = null;
            IAuditorService auditService = ApplicationContext.Current.GetService(typeof(IAuditorService)) as IAuditorService;

            try
            {

                // Setup outgoing content

                // Create or update?
                var handler = FhirResourceHandlerUtil.GetResourceHandler(resourceType);
                if (handler == null)
                    throw new FileNotFoundException(); // endpoint not found!

                result = handler.Create(target, TransactionMode.Commit);
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
                throw;
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
        [SwaggerWcfContentTypes(ConsumeTypes = new String[] { "application/xml+fhir", "application/json+fhir" }, ProduceTypes = new String[] { "application/xml+fhir", "application/json+fhir" })]
        [SwaggerWcfTag("HL7 Fast Health Interoperability Resources (FHIR)")]
        [SwaggerWcfResponse(400, "The client sent a request in a format which is not understood by this server")]
        [SwaggerWcfResponse(401, "The client attempted to perform an operation requires authentication (this can also happen when the client is required to elevate to another user credential)")]
        [SwaggerWcfResponse(403, "The client attempted to perform an operation which it is not permitted to perform")]
        [SwaggerWcfResponse(404, "The client requested access to a resource which is not available")]
        [SwaggerWcfResponse(422, "The client requested an operation which violated business rules or some other formal constraint")]
        [SwaggerWcfResponse(500, "The server encountered an issue processing your request")]
        [SwaggerWcfResponse(503, "This service is not in a state where it can service your request")]
        [SwaggerWcfSecurity("OAUTH2")]
        public OperationOutcome ValidateResource(string resourceType, string id, DomainResourceBase target)
        {
            this.ThrowIfNotReady();

            FhirOperationResult result = null;
            try
            {

                // Setup outgoing content

                // Create or update?
                var handler = FhirResourceHandlerUtil.GetResourceHandler(resourceType);
                if (handler == null)
                    throw new FileNotFoundException(); // endpoint not found!

                result = handler.Update(id, target, TransactionMode.Rollback);
                if (result == null || result.Results.Count == 0) // Create
                {
                    result = handler.Create(target, TransactionMode.Rollback);
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
                throw;
            }
        }

        /// <summary>
        /// Searches a resource from the client registry datastore 
        /// </summary>
        [SwaggerWcfContentTypes(ConsumeTypes = new String[] { "application/xml+fhir", "application/json+fhir" }, ProduceTypes = new String[] { "application/xml+fhir", "application/json+fhir" })]
        [SwaggerWcfTag("HL7 Fast Health Interoperability Resources (FHIR)")]
        [SwaggerWcfResponse(400, "The client sent a request in a format which is not understood by this server")]
        [SwaggerWcfResponse(401, "The client attempted to perform an operation requires authentication (this can also happen when the client is required to elevate to another user credential)")]
        [SwaggerWcfResponse(403, "The client attempted to perform an operation which it is not permitted to perform")]
        [SwaggerWcfResponse(404, "The client requested access to a resource which is not available")]
        [SwaggerWcfResponse(422, "The client requested an operation which violated business rules or some other formal constraint")]
        [SwaggerWcfResponse(500, "The server encountered an issue processing your request")]
        [SwaggerWcfResponse(503, "This service is not in a state where it can service your request")]
        [SwaggerWcfSecurity("OAUTH2")]
        public Bundle SearchResource(string resourceType)
        {
            this.ThrowIfNotReady();

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
                WebOperationContext.Current.OutgoingRequest.Headers.Add("Last-Modified", DateTime.Now.ToString("ddd, dd MMM yyyy HH:mm:ss zzz"));

                if (resourceProcessor == null) // Unsupported resource
                    throw new FileNotFoundException();

                // TODO: Appropriately format response
                // Process incoming request
                result = resourceProcessor.Query(WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters);

                if (result == null || result.Outcome == ResultCode.Rejected)
                    throw new InvalidDataException("Message was rejected");
                else if (result.Outcome != ResultCode.Accepted)
                    throw new DataException("Query failed");

                audit = AuditUtil.CreateAuditData(result.Results);
                // Create the Atom feed
                return MessageUtil.CreateBundle(result);

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
        /// Get conformance
        /// </summary>
        [SwaggerWcfContentTypes(ConsumeTypes = new String[] { "application/xml+fhir", "application/json+fhir" }, ProduceTypes = new String[] { "application/xml+fhir", "application/json+fhir" })]
        [SwaggerWcfTag("HL7 Fast Health Interoperability Resources (FHIR)")]
        [SwaggerWcfResponse(400, "The client sent a request in a format which is not understood by this server")]
        [SwaggerWcfResponse(401, "The client attempted to perform an operation requires authentication (this can also happen when the client is required to elevate to another user credential)")]
        [SwaggerWcfResponse(403, "The client attempted to perform an operation which it is not permitted to perform")]
        [SwaggerWcfResponse(404, "The client requested access to a resource which is not available")]
        [SwaggerWcfResponse(422, "The client requested an operation which violated business rules or some other formal constraint")]
        [SwaggerWcfResponse(500, "The server encountered an issue processing your request")]
        [SwaggerWcfResponse(503, "This service is not in a state where it can service your request")]
        [SwaggerWcfSecurity("OAUTH2")]
        public Conformance GetOptions()
        {
            this.ThrowIfNotReady();

            var retVal = ConformanceUtil.GetConformanceStatement();
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-Location", String.Format("{0}Conformance/{1}/_history/{2}", WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri, retVal.Id, retVal.VersionId));
            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
            WebOperationContext.Current.OutgoingResponse.Headers.Remove("Content-Disposition");
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-Disposition", "filename=\"conformance.xml\"");
            return retVal;
        }

        /// <summary>
        /// Posting transaction is not supported
        /// </summary>
        [SwaggerWcfContentTypes(ConsumeTypes = new String[] { "application/xml+fhir", "application/json+fhir" }, ProduceTypes = new String[] { "application/xml+fhir", "application/json+fhir" })]
        [SwaggerWcfTag("HL7 Fast Health Interoperability Resources (FHIR)")]
        [SwaggerWcfResponse(400, "The client sent a request in a format which is not understood by this server")]
        [SwaggerWcfResponse(401, "The client attempted to perform an operation requires authentication (this can also happen when the client is required to elevate to another user credential)")]
        [SwaggerWcfResponse(403, "The client attempted to perform an operation which it is not permitted to perform")]
        [SwaggerWcfResponse(404, "The client requested access to a resource which is not available")]
        [SwaggerWcfResponse(422, "The client requested an operation which violated business rules or some other formal constraint")]
        [SwaggerWcfResponse(500, "The server encountered an issue processing your request")]
        [SwaggerWcfResponse(503, "This service is not in a state where it can service your request")]
        [SwaggerWcfSecurity("OAUTH2")]
        public Bundle PostTransaction(Bundle feed)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a resource's history
        /// </summary>
        [SwaggerWcfContentTypes(ConsumeTypes = new String[] { "application/xml+fhir", "application/json+fhir" }, ProduceTypes = new String[] { "application/xml+fhir", "application/json+fhir" })]
        [SwaggerWcfTag("HL7 Fast Health Interoperability Resources (FHIR)")]
        [SwaggerWcfResponse(400, "The client sent a request in a format which is not understood by this server")]
        [SwaggerWcfResponse(401, "The client attempted to perform an operation requires authentication (this can also happen when the client is required to elevate to another user credential)")]
        [SwaggerWcfResponse(403, "The client attempted to perform an operation which it is not permitted to perform")]
        [SwaggerWcfResponse(404, "The client requested access to a resource which is not available")]
        [SwaggerWcfResponse(422, "The client requested an operation which violated business rules or some other formal constraint")]
        [SwaggerWcfResponse(500, "The server encountered an issue processing your request")]
        [SwaggerWcfResponse(503, "This service is not in a state where it can service your request")]
        [SwaggerWcfSecurity("OAUTH2")]
        public Bundle GetResourceInstanceHistory(string resourceType, string id, string mimeType)
        {
            this.ThrowIfNotReady();

            FhirOperationResult readResult = null;
            try
            {

                readResult = this.PerformRead(resourceType, id, String.Empty);
                WebOperationContext.Current.OutgoingResponse.Headers.Remove("Content-Disposition");
                return MessageUtil.CreateBundle(readResult);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// Not implemented result
        /// </summary>
        [SwaggerWcfContentTypes(ConsumeTypes = new String[] { "application/xml+fhir", "application/json+fhir" }, ProduceTypes = new String[] { "application/xml+fhir", "application/json+fhir" })]
        [SwaggerWcfTag("HL7 Fast Health Interoperability Resources (FHIR)")]
        [SwaggerWcfResponse(400, "The client sent a request in a format which is not understood by this server")]
        [SwaggerWcfResponse(401, "The client attempted to perform an operation requires authentication (this can also happen when the client is required to elevate to another user credential)")]
        [SwaggerWcfResponse(403, "The client attempted to perform an operation which it is not permitted to perform")]
        [SwaggerWcfResponse(404, "The client requested access to a resource which is not available")]
        [SwaggerWcfResponse(422, "The client requested an operation which violated business rules or some other formal constraint")]
        [SwaggerWcfResponse(500, "The server encountered an issue processing your request")]
        [SwaggerWcfResponse(503, "This service is not in a state where it can service your request")]
        [SwaggerWcfSecurity("OAUTH2")]
        public Bundle GetResourceHistory(string resourceType, string mimeType)
        {
            this.ThrowIfNotReady();

            throw new NotSupportedException("For security reasons resource history is not supported");

        }

        /// <summary>
        /// Not implemented
        /// </summary>
        [SwaggerWcfContentTypes(ConsumeTypes = new String[] { "application/xml+fhir", "application/json+fhir" }, ProduceTypes = new String[] { "application/xml+fhir", "application/json+fhir" })]
        [SwaggerWcfTag("HL7 Fast Health Interoperability Resources (FHIR)")]
        [SwaggerWcfResponse(400, "The client sent a request in a format which is not understood by this server")]
        [SwaggerWcfResponse(401, "The client attempted to perform an operation requires authentication (this can also happen when the client is required to elevate to another user credential)")]
        [SwaggerWcfResponse(403, "The client attempted to perform an operation which it is not permitted to perform")]
        [SwaggerWcfResponse(404, "The client requested access to a resource which is not available")]
        [SwaggerWcfResponse(422, "The client requested an operation which violated business rules or some other formal constraint")]
        [SwaggerWcfResponse(500, "The server encountered an issue processing your request")]
        [SwaggerWcfResponse(503, "This service is not in a state where it can service your request")]
        [SwaggerWcfSecurity("OAUTH2")]
        public Bundle GetHistory(string mimeType)
        {
            this.ThrowIfNotReady();

            throw new NotSupportedException("For security reasons system history is not supported");
        }


        /// <summary>
        /// Perform a read against the underlying IFhirResourceHandler
        /// </summary>
        [SwaggerWcfContentTypes(ConsumeTypes = new String[] { "application/xml+fhir", "application/json+fhir" }, ProduceTypes = new String[] { "application/xml+fhir", "application/json+fhir" })]
        [SwaggerWcfTag("HL7 Fast Health Interoperability Resources (FHIR)")]
        [SwaggerWcfResponse(400, "The client sent a request in a format which is not understood by this server")]
        [SwaggerWcfResponse(401, "The client attempted to perform an operation requires authentication (this can also happen when the client is required to elevate to another user credential)")]
        [SwaggerWcfResponse(403, "The client attempted to perform an operation which it is not permitted to perform")]
        [SwaggerWcfResponse(404, "The client requested access to a resource which is not available")]
        [SwaggerWcfResponse(422, "The client requested an operation which violated business rules or some other formal constraint")]
        [SwaggerWcfResponse(500, "The server encountered an issue processing your request")]
        [SwaggerWcfResponse(503, "This service is not in a state where it can service your request")]
        [SwaggerWcfSecurity("OAUTH2")]
        private FhirOperationResult PerformRead(string resourceType, string id, string vid)
        {
            this.ThrowIfNotReady();
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
                if (result.Results != null && result.Results.Count > 0 )
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
        [SwaggerWcfContentTypes(ConsumeTypes = new String[] { "application/xml+fhir", "application/json+fhir" }, ProduceTypes = new String[] { "application/xml+fhir", "application/json+fhir" })]
        [SwaggerWcfTag("HL7 Fast Health Interoperability Resources (FHIR)")]
        [SwaggerWcfResponse(400, "The client sent a request in a format which is not understood by this server")]
        [SwaggerWcfResponse(401, "The client attempted to perform an operation requires authentication (this can also happen when the client is required to elevate to another user credential)")]
        [SwaggerWcfResponse(403, "The client attempted to perform an operation which it is not permitted to perform")]
        [SwaggerWcfResponse(404, "The client requested access to a resource which is not available")]
        [SwaggerWcfResponse(422, "The client requested an operation which violated business rules or some other formal constraint")]
        [SwaggerWcfResponse(500, "The server encountered an issue processing your request")]
        [SwaggerWcfResponse(503, "This service is not in a state where it can service your request")]
        [SwaggerWcfSecurity("OAUTH2")]
        public Conformance GetMetaData()
        {
            return this.GetOptions();
        }

        #endregion

        #region IFhirServiceContract Members

        /// <summary>
        /// Get the current time
        /// </summary>
        [SwaggerWcfContentTypes(ConsumeTypes = new String[] { "application/xml+fhir", "application/json+fhir" }, ProduceTypes = new String[] { "application/xml+fhir", "application/json+fhir" })]
        [SwaggerWcfTag("HL7 Fast Health Interoperability Resources (FHIR)")]
        [SwaggerWcfResponse(400, "The client sent a request in a format which is not understood by this server")]
        [SwaggerWcfResponse(401, "The client attempted to perform an operation requires authentication (this can also happen when the client is required to elevate to another user credential)")]
        [SwaggerWcfResponse(403, "The client attempted to perform an operation which it is not permitted to perform")]
        [SwaggerWcfResponse(404, "The client requested access to a resource which is not available")]
        [SwaggerWcfResponse(422, "The client requested an operation which violated business rules or some other formal constraint")]
        [SwaggerWcfResponse(500, "The server encountered an issue processing your request")]
        [SwaggerWcfResponse(503, "This service is not in a state where it can service your request")]
        [SwaggerWcfSecurity("OAUTH2")]
        public DateTime Time()
        {
            return DateTime.Now;
        }

        #endregion

        /// <summary>
        /// Create or update
        /// </summary>
        [SwaggerWcfContentTypes(ConsumeTypes = new String[] { "application/xml+fhir", "application/json+fhir" }, ProduceTypes = new String[] { "application/xml+fhir", "application/json+fhir" })]
        [SwaggerWcfTag("HL7 Fast Health Interoperability Resources (FHIR)")]
        [SwaggerWcfResponse(400, "The client sent a request in a format which is not understood by this server")]
        [SwaggerWcfResponse(401, "The client attempted to perform an operation requires authentication (this can also happen when the client is required to elevate to another user credential)")]
        [SwaggerWcfResponse(403, "The client attempted to perform an operation which it is not permitted to perform")]
        [SwaggerWcfResponse(404, "The client requested access to a resource which is not available")]
        [SwaggerWcfResponse(422, "The client requested an operation which violated business rules or some other formal constraint")]
        [SwaggerWcfResponse(500, "The server encountered an issue processing your request")]
        [SwaggerWcfResponse(503, "This service is not in a state where it can service your request")]
        [SwaggerWcfSecurity("OAUTH2")]
        public DomainResourceBase CreateUpdateResource(string resourceType, string id, string mimeType, DomainResourceBase target)
        {
            return this.UpdateResource(resourceType, id, mimeType, target);
        }

        /// <summary>
        /// Alternate search
        /// </summary>
        [SwaggerWcfContentTypes(ConsumeTypes = new String[] { "application/xml+fhir", "application/json+fhir" }, ProduceTypes = new String[] { "application/xml+fhir", "application/json+fhir" })]
        [SwaggerWcfTag("HL7 Fast Health Interoperability Resources (FHIR)")]
        [SwaggerWcfResponse(400, "The client sent a request in a format which is not understood by this server")]
        [SwaggerWcfResponse(401, "The client attempted to perform an operation requires authentication (this can also happen when the client is required to elevate to another user credential)")]
        [SwaggerWcfResponse(403, "The client attempted to perform an operation which it is not permitted to perform")]
        [SwaggerWcfResponse(404, "The client requested access to a resource which is not available")]
        [SwaggerWcfResponse(422, "The client requested an operation which violated business rules or some other formal constraint")]
        [SwaggerWcfResponse(500, "The server encountered an issue processing your request")]
        [SwaggerWcfResponse(503, "This service is not in a state where it can service your request")]
        [SwaggerWcfSecurity("OAUTH2")]
        public Bundle SearchResourceAlt(string resourceType)
        {
            return this.SearchResource(resourceType);
        }

        /// <summary>
        /// Throws an exception if the service is not yet ready
        /// </summary>
        private void ThrowIfNotReady()
        {
            if (!ApplicationContext.Current.IsRunning)
                throw new DomainStateException();

        }
    }

}
