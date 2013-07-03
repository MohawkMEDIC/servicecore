using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using System.IO;
using System.Xml;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes;
using MARC.HI.EHRS.SVC.Core.DataTypes;
using System.ComponentModel;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{
    /// <summary>
    /// The patient resource
    /// </summary>
    [XmlType("Patient", Namespace = "http://hl7.org/fhir")]
    [XmlRoot("Patient", Namespace = "http://hl7.org/fhir")] 
    [ParticipantObjectMap(IdType = AuditableObjectIdType.PatientNumber, Role = AuditableObjectRole.Patient, Type = AuditableObjectType.Person, OidName = "CR_CID")]
    [Profile(ProfileId = "svccore", Name = "MARC-HI ServiceCore FHIR Profile", Import = "http://hl7.org/implement/standards/fhir/profiles-resources.xml")]
    [ResourceProfile(Name = "patient")]
    public class Patient : ResourceBase
    {

        /// <summary>
        /// Patient constructor
        /// </summary>
        public Patient()
        {
            this.Link = new List<Resource<Patient>>();
            this.Identifier = new List<Identifier>();
            this.Active = new FhirBoolean(true);
        }

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
        /// Gets or sets a list of identifiers
        /// </summary>
        [XmlElement("identifier")]
        [Description("An identifier for the person as this patient")]
        [ElementProfile(MaxOccurs = -1)]
        public List<Identifier> Identifier { get; set; }

        /// <summary>
        /// Patient demographics 
        /// </summary>
        [XmlElement("details")]
        [Description("Patient demographics")]
        public Demographics Details { get; set; }

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
        /// Provider of the patient resource
        /// </summary>
        [XmlElement("provider")]
        [Description("Organization managing this patient")]
        public Resource<Organization> Provider { get; set; }

        /// <summary>
        /// The multiple birth indicator
        /// </summary>
        [XmlElement("multipleBirthInteger", typeof(FhirInt))]
        [XmlElement("multipleBirthBoolean", typeof(FhirBoolean))]
        [Description("Whether patient is part of a multiple birth")]
        public Shareable MultipleBirth { get; set; }

        /// <summary>
        /// The deceased date of the resource
        /// </summary>
        [XmlElement("deceasedDate")]
        [Description("Date of death of the patient")]
        public Date DeceasedDate { get; set; }

        /// <summary>
        /// Generate the narrative
        /// </summary>
        internal override void WriteText(XmlWriter xw)
        {

            // Now output
            xw.WriteStartElement("table", NS_XHTML);
            xw.WriteElementString("caption", NS_XHTML, "Identifiers");
            xw.WriteStartElement("tbody", NS_XHTML);
            base.WriteTableRows(xw, "Identifiers", this.Identifier.ToArray());
            xw.WriteEndElement(); // tbody
            xw.WriteEndElement(); // table

            if (this.Details != null)
                this.Details.WriteText(xw);

        }

    }
}
