using MARC.HI.EHRS.SVC.Messaging.FHIR.Resources;
using SwaggerWcf.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Wcf
{
    /// <summary>
    /// FHIR Service Contract
    /// </summary>
    [ServiceContract]
    [ServiceKnownType(typeof(Patient))]
    [ServiceKnownType(typeof(Organization))]
    [ServiceKnownType(typeof(Picture))]
    [ServiceKnownType(typeof(Practitioner))]
    [ServiceKnownType(typeof(OperationOutcome))]
    [ServiceKnownType(typeof(ValueSet))]
    [ServiceKnownType(typeof(StructureDefinition))]
    [ServiceKnownType(typeof(Bundle))]
    [ServiceKnownType(typeof(Immunization))]
    [ServiceKnownType(typeof(ImmunizationRecommendation))]
    [ServiceKnownType(typeof(Conformance))]
    [ServiceKnownType(typeof(RelatedPerson))]
    [ServiceKnownType(typeof(Encounter))]
    [ServiceKnownType(typeof(Condition))]
    [ServiceKnownType(typeof(AdverseEvent))]
    [ServiceKnownType(typeof(MedicationAdministration))]
    [ServiceKnownType(typeof(Location))]
    [ServiceKnownType(typeof(AllergyIntolerance))]
    [ServiceKnownType(typeof(System.ServiceModel.Syndication.Atom10FeedFormatter))]
    [XmlSerializerFormat(SupportFaults = true)]
    public interface IFhirServiceContract
    {

        /// <summary>
        /// Get index page
        /// </summary>
        [WebGet(UriTemplate = "/")]
        Stream Index();

        /// <summary>
        /// Get the schema
        /// </summary>
        [WebGet(UriTemplate = "/?xsd={schemaId}")]
        XmlSchema GetSchema(int schemaId);

        /// <summary>
        /// Gets the current time on the service
        /// </summary>
        /// <returns></returns>
        [WebGet(UriTemplate = "/time")]
        [SwaggerWcfPath("Get Server Time", "Gets the current server time so clients can have consistent time with this service")]
        DateTime Time();

        /// <summary>
        /// Read a resource
        /// </summary>
        [WebGet(UriTemplate = "/{resourceType}/{id}?_format={mimeType}", BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract(Name = "read")]
        [SwaggerWcfPath("Read Resource", "Fetches the most current version of the identified resource", ExternalDocsUrl = "http://hl7.org/fhir", ExternalDocsDescription = "HL7 FHIR STU3 Documentation")]
        DomainResourceBase ReadResource(string resourceType, string id, string mimeType);

        /// <summary>
        /// Version read a resource
        /// </summary>
        [WebGet(UriTemplate = "/{resourceType}/{id}/_history/{vid}?_format={mimeType}", BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract(Name = "vread")]
        [SwaggerWcfPath("Version Read Resource", "Fetches a specific version of the identified resource", ExternalDocsUrl = "http://hl7.org/fhir", ExternalDocsDescription = "HL7 FHIR STU3 Documentation")]
        DomainResourceBase VReadResource(string resourceType, string id, string vid, string mimeType);

        /// <summary>
        /// Update a resource
        /// </summary>
        [WebInvoke(UriTemplate = "/{resourceType}/{id}?_format={mimeType}", Method = "PUT", BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract(Name = "update")]
        [SwaggerWcfPath("Update Resource", "Updates the provided resource with the provided resource", ExternalDocsUrl = "http://hl7.org/fhir", ExternalDocsDescription = "HL7 FHIR STU3 Documentation")]
        DomainResourceBase UpdateResource(string resourceType, string id, string mimeType, DomainResourceBase target);

        /// <summary>
        /// Delete a resource
        /// </summary>
        [WebInvoke(UriTemplate = "/{resourceType}/{id}?_format={mimeType}", Method = "DELETE", BodyStyle = WebMessageBodyStyle.Bare)]
        [SwaggerWcfPath("Delete (Obsolete) Resource", "Performs a logical deletion on the identified resource", ExternalDocsUrl = "http://hl7.org/fhir", ExternalDocsDescription = "HL7 FHIR STU3 Documentation")]
        [OperationContract(Name = "delete")]
        DomainResourceBase DeleteResource(string resourceType, string id, string mimeType);

        /// <summary>
        /// Create a resource
        /// </summary>
        [WebInvoke(UriTemplate = "/{resourceType}?_format={mimeType}", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare)]
        [SwaggerWcfPath("Create Resource", "Creates a new instance of the provided resource", ExternalDocsUrl = "http://hl7.org/fhir", ExternalDocsDescription = "HL7 FHIR STU3 Documentation")]
        [OperationContract(Name = "create")]
        DomainResourceBase CreateResource(string resourceType, string mimeType, DomainResourceBase target);

        /// <summary>
        /// Create a resource
        /// </summary>
        [WebInvoke(UriTemplate = "/{resourceType}/{id}?_format={mimeType}", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare)]
        [SwaggerWcfPath("Create or Update Resource", "Creates a new instance of the provided resource or updates it if it already exists", ExternalDocsUrl = "http://hl7.org/fhir", ExternalDocsDescription = "HL7 FHIR STU3 Documentation")]
        [OperationContract]
        DomainResourceBase CreateUpdateResource(string resourceType, string id, string mimeType, DomainResourceBase target);

        /// <summary>
        /// Validate a resource
        /// </summary>
        [WebInvoke(UriTemplate = "/{resourceType}/_validate/{id}", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare)]
        [SwaggerWcfPath("Validate Resource", "Validates that the provided resource can be persisted without actually persisting the resource", ExternalDocsUrl = "http://hl7.org/fhir", ExternalDocsDescription = "HL7 FHIR STU3 Documentation")]
        [OperationContract(Name = "validate")]
        OperationOutcome ValidateResource(string resourceType, string id, DomainResourceBase target);

        /// <summary>
        /// Version read a resource
        /// </summary>
        [SwaggerWcfPath("Search Resource", "Performs a search on the specified resource", ExternalDocsUrl = "http://hl7.org/fhir", ExternalDocsDescription = "HL7 FHIR STU3 Documentation")]
        [WebGet(UriTemplate = "/{resourceType}", BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract(Name = "search-type")]
        Bundle SearchResource(string resourceType);


        /// <summary>
        /// Version read a resource
        /// </summary>
        [WebGet(UriTemplate = "/{resourceType}/_search", BodyStyle = WebMessageBodyStyle.Bare)]
        [SwaggerWcfPath("Search Resource", "Executes a search against the specified resouce type", ExternalDocsUrl = "http://hl7.org/fhir", ExternalDocsDescription = "HL7 FHIR STU3 Documentation")]
        [OperationContract()]
        Bundle SearchResourceAlt(string resourceType);

        /// <summary>
        /// Options for this service
        /// </summary>
        [SwaggerWcfPath("Get Conformance", "Retrieves the conformance statement for this particular service", ExternalDocsUrl = "http://hl7.org/fhir", ExternalDocsDescription = "HL7 FHIR STU3 Documentation")]
        [WebInvoke(UriTemplate = "/", Method = "OPTIONS", ResponseFormat = WebMessageFormat.Xml, RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare)]
        Conformance GetOptions();

        /// <summary>
        /// Options for this service
        /// </summary>
        [SwaggerWcfPath("Get Conformance", "Get the conformance statement for this particular service (GET instead of OPTIONS)", ExternalDocsUrl = "http://hl7.org/fhir", ExternalDocsDescription = "HL7 FHIR STU3 Documentation")]
        [WebInvoke(UriTemplate = "/metadata", Method = "GET", ResponseFormat = WebMessageFormat.Xml, RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare)]
        Conformance GetMetaData();

        /// <summary>
        /// Post a transaction
        /// </summary>
        [SwaggerWcfPath("Execute Transaction", "Posts the specified transaction to the service whereby all entries must successfully be created or none", ExternalDocsUrl = "http://hl7.org/fhir", ExternalDocsDescription = "HL7 FHIR STU3 Documentation")]
        [WebInvoke(UriTemplate = "/", Method = "POST", ResponseFormat = WebMessageFormat.Xml, RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare)]
        Bundle PostTransaction(Bundle feed);

        /// <summary>
        /// Get history
        /// </summary>
        [WebGet(UriTemplate = "/{resourceType}/{id}/_history?_format={mimeType}", ResponseFormat = WebMessageFormat.Xml, RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare)]
        [SwaggerWcfPath("Resource Instance History", "Gets a complete history of changes for a particular identified resource", ExternalDocsUrl = "http://hl7.org/fhir", ExternalDocsDescription = "HL7 FHIR STU3 Documentation")]
        [OperationContract(Name = "history-instance")]
        Bundle GetResourceInstanceHistory(string resourceType, string id, string mimeType);

        /// <summary>
        /// Get history
        /// </summary>
        [SwaggerWcfPath("Get History", "Gets a complete history of changes for the type of resource", ExternalDocsUrl = "http://hl7.org/fhir", ExternalDocsDescription = "HL7 FHIR STU3 Documentation")]
        [WebGet(UriTemplate = "/{resourceType}/_history?_format={mimeType}", ResponseFormat = WebMessageFormat.Xml, RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare)]
        Bundle GetResourceHistory(string resourceType, string mimeType);

        /// <summary>
        /// Get history for all
        /// </summary>
        [SwaggerWcfPath("Server History", "Gets a complete history of all changes on this server", ExternalDocsUrl = "http://hl7.org/fhir", ExternalDocsDescription = "HL7 FHIR STU3 Documentation")]
        [WebGet(UriTemplate = "/_history?_format={mimeType}", ResponseFormat = WebMessageFormat.Xml, RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare)]
        Bundle GetHistory(string mimeType);

    }

}
