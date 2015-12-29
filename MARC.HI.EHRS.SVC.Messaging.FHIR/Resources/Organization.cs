using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes;
using System.ComponentModel;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{

    /// <summary>
    /// Identifies an organization
    /// </summary>
    [XmlRoot("Organization",Namespace = "http://hl7.org/fhir")] 
    [XmlType("Organization", Namespace = "http://hl7.org/fhir")]
    [Profile(ProfileId = "svccore")]
    [ResourceProfile(Name = "ServiceCore Resource - Organization")]
    public class Organization : ResourceBase
    {

        /// <summary>
        /// Gets or sets the unique identifiers for the organization
        /// </summary>
        [XmlElement("identifier")]
        [Description("Identifier for the organization")]
        public List<Identifier> Identifier { get; set; }

        /// <summary>
        /// Gets or sets the name of the organization
        /// </summary>
        [XmlElement("name")]
        [Description("Name used for the organization")]
        public FhirString Name { get; set; }

        /// <summary>
        /// Gets or sets the type of organization
        /// </summary>
        [XmlElement("type")]
        [Description("Kind of organization")]
        public FhirCodeableConcept Type { get; set; }

        /// <summary>
        /// Gets or sets the telecommunications addresses
        /// </summary>
        [XmlElement("telecom")]
        [Description("A contact detail for the organization")]
        public List<FhirTelecom> Telecom { get; set; }

        /// <summary>
        /// Gets or sets the addresses of the 
        /// </summary>
        [XmlElement("address")]
        [Description("An address for the organization")]
        public List<FhirAddress> Address { get; set; }

        /// <summary>
        /// Part of
        /// </summary>
        [XmlElement("partOf")]
        [Description("The organization of which this organization forms a part")]
        public Reference<Organization> PartOf { get; set; }

        /// <summary>
        /// Gets or sets the contact entities
        /// </summary>
        [XmlElement("contactEntity")]
        [Description("Contact information for the organization")]
        public List<ContactEntity> ContactEntity { get; set; }

        /// <summary>
        /// Gets or sets the active flag for the item
        /// </summary>
        [XmlElement("active")]
        [Description("Whether the organization's record is still in active use")]
        public FhirBoolean Active { get; set; }

    }
}
