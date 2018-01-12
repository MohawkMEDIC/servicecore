using MARC.HI.EHRS.SVC.Messaging.FHIR.Resources;
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
        DateTime Time();

        /// <summary>
        /// Read a resource
        /// </summary>
        [WebGet(UriTemplate = "/{resourceType}/{id}?_format={mimeType}", ResponseFormat = WebMessageFormat.Xml, RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract(Name = "read")]
        DomainResourceBase ReadResource(string resourceType, string id, string mimeType);

        /// <summary>
        /// Version read a resource
        /// </summary>
        [WebGet(UriTemplate = "/{resourceType}/{id}/_history/{vid}?_format={mimeType}", ResponseFormat = WebMessageFormat.Xml, RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract(Name = "vread")]
        DomainResourceBase VReadResource(string resourceType, string id, string vid, string mimeType);

        /// <summary>
        /// Update a resource
        /// </summary>
        [WebInvoke(UriTemplate = "/{resourceType}/{id}?_format={mimeType}", Method = "PUT", ResponseFormat = WebMessageFormat.Xml, RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract(Name = "update")]
        DomainResourceBase UpdateResource(string resourceType, string id, string mimeType, DomainResourceBase target);

        /// <summary>
        /// Delete a resource
        /// </summary>
        [WebInvoke(UriTemplate = "/{resourceType}/{id}?_format={mimeType}", Method = "DELETE", ResponseFormat = WebMessageFormat.Xml, RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract(Name = "delete")]
        DomainResourceBase DeleteResource(string resourceType, string id, string mimeType);

        /// <summary>
        /// Create a resource
        /// </summary>
        [WebInvoke(UriTemplate = "/{resourceType}?_format={mimeType}", Method = "POST", ResponseFormat = WebMessageFormat.Xml, RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract(Name = "create")]
        DomainResourceBase CreateResource(string resourceType, string mimeType, DomainResourceBase target);

        /// <summary>
        /// Create a resource
        /// </summary>
        [WebInvoke(UriTemplate = "/{resourceType}/{id}?_format={mimeType}", Method = "POST", ResponseFormat = WebMessageFormat.Xml, RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract]
        DomainResourceBase CreateUpdateResource(string resourceType, string id, string mimeType, DomainResourceBase target);

        /// <summary>
        /// Validate a resource
        /// </summary>
        [WebInvoke(UriTemplate = "/{resourceType}/_validate/{id}", Method = "POST", ResponseFormat = WebMessageFormat.Xml, RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract(Name = "validate")]
        OperationOutcome ValidateResource(string resourceType, string id, DomainResourceBase target);

        /// <summary>
        /// Version read a resource
        /// </summary>
        [WebGet(UriTemplate = "/{resourceType}", ResponseFormat = WebMessageFormat.Xml, RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract(Name = "search-type")]
        Bundle SearchResource(string resourceType);


        /// <summary>
        /// Version read a resource
        /// </summary>
        [WebGet(UriTemplate = "/{resourceType}/_search", ResponseFormat = WebMessageFormat.Xml, RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract()]
        Bundle SearchResourceAlt(string resourceType);

        /// <summary>
        /// Options for this service
        /// </summary>
        [WebInvoke(UriTemplate = "/", Method = "OPTIONS", ResponseFormat = WebMessageFormat.Xml, RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare)]
        Conformance GetOptions();

        /// <summary>
        /// Options for this service
        /// </summary>
        [WebInvoke(UriTemplate = "/metadata", Method = "GET", ResponseFormat = WebMessageFormat.Xml, RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare)]
        Conformance GetMetaData();

        /// <summary>
        /// Post a transaction
        /// </summary>
        [WebInvoke(UriTemplate = "/", Method = "POST", ResponseFormat = WebMessageFormat.Xml, RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare)]
        Bundle PostTransaction(Bundle feed);

        /// <summary>
        /// Get history
        /// </summary>
        [WebGet(UriTemplate = "/{resourceType}/{id}/_history?_format={mimeType}", ResponseFormat = WebMessageFormat.Xml, RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract(Name = "history-instance")]
        Bundle GetResourceInstanceHistory(string resourceType, string id, string mimeType);

        /// <summary>
        /// Get history
        /// </summary>
        [WebGet(UriTemplate = "/{resourceType}/_history?_format={mimeType}", ResponseFormat = WebMessageFormat.Xml, RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare)]
        Bundle GetResourceHistory(string resourceType, string mimeType);

        /// <summary>
        /// Get history for all
        /// </summary>
        [WebGet(UriTemplate = "/_history?_format={mimeType}", ResponseFormat = WebMessageFormat.Xml, RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare)]
        Bundle GetHistory(string mimeType);

    }

}
