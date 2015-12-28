using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using System.IO;
using System.Xml;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes;
using System.ComponentModel;
using MARC.HI.EHRS.SVC.Auditing.Data;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{
    /// <summary>
    /// The patient resource
    /// </summary>
    [XmlType("Patient", Namespace = "http://hl7.org/fhir")]
    [XmlRoot("Patient", Namespace = "http://hl7.org/fhir")] 
    [ParticipantObjectMap(IdType = AuditableObjectIdType.PatientNumber, Role = AuditableObjectRole.Patient, Type = AuditableObjectType.Person, OidName = "CR_CID")]
    [Profile(ProfileId = "svccore", Name = "MARC-HI ServiceCore FHIR Profile")]
    [ResourceProfile(Name = "ServiceCore Resource - Patient")]
    public class Patient : ResourceBase
    {
        /// <summary>
        /// Namespace Declarations
        /// </summary>
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Namespaces { get { return this.m_namespaces; } }

        /// <summary>
        /// Patient constructor
        /// </summary>
        public Patient()
        {
            this.Link = new List<Resource<Patient>>();
            this.Identifier = new List<DataTypes.Identifier>();
            this.Active = new FhirBoolean(true);
            this.Name = new List<HumanName>();
            this.Telecom = new List<Telecom>();
            this.Address = new List<Address>();
            this.Language = new List<CodeableConcept>();
            this.Photo = new List<Attachment>();
            this.Contact = new List<Contact>();
        }

        /// <summary>
        /// Gets or sets a list of identifiers
        /// </summary>
        [XmlElement("identifier")]
        [Description("An identifier for the person as this patient")]
        [ElementProfile(MaxOccurs = -1)]
        public List<DataTypes.Identifier> Identifier { get; set; }

        /// <summary>
        /// The name of the individual
        /// </summary>
        [XmlElement("name")]
        [Description("A name associated with the individual")]
        public List<HumanName> Name { get; set; }

        /// <summary>
        /// The telecommunications addresses for the individual
        /// </summary>
        [XmlElement("telecom")]
        [Description("A contact detail for the individual")]
        public List<Telecom> Telecom { get; set; }

        /// <summary>
        /// The gender of the individual
        /// </summary>
        [XmlElement("gender")]
        [Description("Gender for administrative purposes")]
        [ElementProfile(RemoteBinding = "http://hl7.org/fhir/vs/administrative-gender")]
        public CodeableConcept Gender { get; set; }

        /// <summary>
        /// The birth date of the individual
        /// </summary>
        [XmlElement("birthDate")]
        [Description("The date and time of birth for the individual")]
        public Date BirthDate { get; set; }

        /// <summary>
        /// True if the individual is deceased
        /// </summary>
        [XmlElement("deceasedDateTime", typeof(Date))]
        [XmlElement("deceasedBoolean", typeof(FhirBoolean))]
        [Description("Indicates if the individual is deceased or not")]
        public Object Deceased { get; set; }

        /// <summary>
        /// Gets or sets the addresses of the user
        /// </summary>
        [XmlElement("address")]
        [Description("Addresses for the individual")]
        public List<Address> Address { get; set; }

        /// <summary>
        /// Gets or sets the marital status of the user
        /// </summary>
        [XmlElement("maritalStatus")]
        [Description("Marital (civil) status of a person")]
        [ElementProfile(RemoteBinding = "http://hl7.org/fhir/vs/marital-status")]
        public CodeableConcept MaritalStatus { get; set; }

        /// <summary>
        /// The multiple birth indicator
        /// </summary>
        [XmlElement("multipleBirthInteger", typeof(FhirInt))]
        [XmlElement("multipleBirthBoolean", typeof(FhirBoolean))]
        [Description("Whether patient is part of a multiple birth")]
        public Shareable MultipleBirth { get; set; }

        /// <summary>
        /// Gets or sets the photograph of the user
        /// </summary>
        [XmlElement("photo")]
        [Description("Image of the person")]
        public List<Attachment> Photo { get; set; }

        /// <summary>
        /// Contact details
        /// </summary>
        [Description("A contact party (e.g. guardian, partner, friend) for the patient")]
        [XmlElement("contact")]
        [ElementProfile(MaxOccurs = -1)]
        public List<Contact> Contact { get; set; }

        /// <summary>
        /// Animal reference
        /// </summary>
        [XmlElement("animal")]
        [Description("If this patient is a non-human")]
        public Animal Animal { get; set; }

        /// <summary>
        /// Gets or sets the language of the user
        /// </summary>
        [Description("Person's proficiancy level of a language")]
        [XmlElement("communication")]
        public List<CodeableConcept> Language { get; set; }

        /// <summary>
        /// Provider of the patient resource
        /// </summary>
        [XmlElement("careProvider")]
        [Description("Provider managing this patient")]
        public Resource Provider { get; set; }

        /// <summary>
        /// Provider of the patient resource
        /// </summary>
        [XmlElement("managingOrganization")]
        [Description("Organization managing this patient")]
        public Resource<Organization> ManagingOrganization { get; set; }

        /// <summary>
        /// Link between this patient and others
        /// </summary>
        [XmlElement("link")]
        [Description("Other patient resources linked to this patient resource")]
        public List<Resource<Patient>> Link { get; set; }

        /// <summary>
        /// True when the patient is active
        /// </summary>
        [XmlElement("active")]
        [Description("Whether this patient's record is in active use")]
        public FhirBoolean Active { get; set; }

        /// <summary>
        /// Generate the narrative
        /// </summary>
        internal override void WriteText(XmlWriter xw)
        {
            xw.WriteStartElement("div");
            xw.WriteAttributeString("class", "h1");
            xw.WriteString(String.Format("Patient {0}", this.Id));
            xw.WriteEndElement(); // div

            // Now output
            xw.WriteStartElement("table", NS_XHTML);
            xw.WriteAttributeString("border", "1");
            xw.WriteElementString("caption", NS_XHTML, "Identifiers");
            xw.WriteStartElement("tbody", NS_XHTML);
            base.WriteTableRows(xw, "Identifiers", this.Identifier.ToArray());
            xw.WriteEndElement(); // tbody
            xw.WriteEndElement(); // table

            // Now output demographics
            xw.WriteStartElement("table", NS_XHTML);
            xw.WriteAttributeString("border", "1");
            xw.WriteElementString("caption", NS_XHTML, "Demographic Information");
            xw.WriteStartElement("tbody", NS_XHTML);
            base.WriteTableRows(xw, "Name", this.Name.ToArray());
            base.WriteTableRows(xw, "DOB", this.BirthDate);
            base.WriteTableRows(xw, "Gender", this.Gender);
            base.WriteTableRows(xw, "Address", this.Address.ToArray());
            base.WriteTableRows(xw, "Telecom", this.Telecom.ToArray());
            // Contacts
            if (this.Contact != null)
                base.WriteTableRows(xw, "Contacts", this.Contact.ToArray());
            // Extended Attributes
            base.WriteTableRows(xw, "Extended Attributes", this.Extension.ToArray());

            if (this.Contained != null)
                base.WriteTableRows(xw, "Contained Resources", this.Contained.ToArray());

            xw.WriteEndElement(); // tbody
            xw.WriteEndElement(); // table

            
        }

        /// <summary>
        /// Represent as a string
        /// </summary>
        public override string ToString()
        {
            return String.Format("[Patient] {0}", this.Name.FirstOrDefault());
        }
    }
}
