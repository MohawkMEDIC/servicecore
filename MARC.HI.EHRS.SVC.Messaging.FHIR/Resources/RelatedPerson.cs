﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using System.ComponentModel;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{
    /// <summary>
    /// Represents a related person
    /// </summary>
    [XmlType("RelatedPerson", Namespace = "http://hl7.org/fhir")]
    [XmlRoot("RelatedPerson", Namespace = "http://hl7.org/fhir")]
    [Profile(ProfileId = "svccore", Name = "MARC-HI ServiceCore FHIR Profile")]
    [ResourceProfile(Name = "ServiceCore Resource - RelatedPerson")]
    public class RelatedPerson : ResourceBase
    {
        /// <summary>
        /// Related person
        /// </summary>
        public RelatedPerson()
        {
            this.Identifier = new List<Identifier>();
            this.Telecom = new List<FhirTelecom>();
            this.Photo = new List<Attachment>();
        }

        /// <summary>
        /// Gets or sets the identifier for the relationship
        /// </summary>
        [Description("An identifier for the person as a relationship")]
        [ElementProfile(MaxOccurs = -1)]
        [XmlElement("identifier")]
        public List<Identifier> Identifier { get; set; }

        /// <summary>
        /// Gets or sets the patient to which this person is related
        /// </summary>
        [Description("The person to which this person is related")]
        [ElementProfile(MaxOccurs = 1, MinOccurs = 1)]
        [XmlElement("patient")]
        public Reference<Patient> Patient { get; set; }

        /// <summary>
        /// Gets or sets the relationship type
        /// </summary>
        [Description("The relationship this person has with the patient")]
        [ElementProfile(MaxOccurs = 0, MinOccurs = 1, RemoteBinding = "http://hl7.org/fhir/vs/relatedperson-relationshiptype")]
        [XmlElement("relationship")]
        public FhirCodeableConcept Relationship { get; set; }

        /// <summary>
        /// Gets or sets the person's name
        /// </summary>
        [Description("The name of the related person")]
        [ElementProfile(MaxOccurs = 1)]
        [XmlElement("name")]
        public FhirHumanName Name { get; set; }

        /// <summary>
        /// Gets or sets the person's telecom addresses
        /// </summary>
        [Description("Telecommunications addresses")]
        [ElementProfile(MaxOccurs = -1)]
        [XmlElement("telecom")]
        public List<FhirTelecom> Telecom { get; set; }

        /// <summary>
        /// Gets or sets the gender of the patient
        /// </summary>
        [XmlElement("gender")]
        [Description("Gender for administrative purposes")]
        [ElementProfile(RemoteBinding = "http://hl7.org/fhir/vs/administrative-gender")]
        public FhirCodeableConcept Gender { get; set; }

        /// <summary>
        /// Gets or sets the address of the related person
        /// </summary>
        [XmlElement("address")]
        [Description("Address of the related person")]
        [ElementProfile(MaxOccurs = 1)]
        public FhirAddress Address { get; set; }

        /// <summary>
        /// Gets or sets the photograph of the person
        /// </summary>
        [XmlElement("photo")]
        [Description("Photograph of the related person")]
        [ElementProfile(MaxOccurs = -1)]
        public List<Attachment> Photo { get; set; }

        /// <summary>
        /// Write textual content
        /// </summary>
        internal override void WriteText(System.Xml.XmlWriter xw)
        {
            xw.WriteStartElement("div");
            xw.WriteAttributeString("class", "h1");
            xw.WriteString(String.Format("Related Person {0}", this.Id));
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
            base.WriteTableRows(xw, "Name", this.Name);
            base.WriteTableRows(xw, "Gender", this.Gender);
            base.WriteTableRows(xw, "Address", this.Address);
            base.WriteTableRows(xw, "Telecom", this.Telecom.ToArray());
            // Related to
            base.WriteTableRows(xw, "Relation", this.Relationship);
            base.WriteTableRows(xw, "To Patient", this.Patient);
            // Extended Attributes
            base.WriteTableRows(xw, "Extended Attributes", this.Extension.ToArray());
            xw.WriteEndElement(); // tbody
            xw.WriteEndElement(); // table
        }

        /// <summary>
        /// Represent as a string
        /// </summary>
        public override string ToString()
        {
            return String.Format("Patient: {0}", this.Name);
        }

    }
}
