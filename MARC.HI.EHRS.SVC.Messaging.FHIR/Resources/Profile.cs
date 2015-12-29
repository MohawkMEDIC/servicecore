using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using System.ComponentModel;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Backbone;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{
    /// <summary>
    /// Represents a profile
    /// </summary>
    [XmlType("Profile", Namespace = "http://hl7.org/fhir")]
    [XmlRoot("Profile", Namespace = "http://hl7.org/fhir")]
    [Profile(ProfileId = "svccore")]
    [ResourceProfile(Name = "profile")]
    public class Profile : ResourceBase
    {

        /// <summary>
        /// Status structure
        /// </summary>
        [XmlType("Status", Namespace = "http://hl7.org/fhir")]
        public class StatusType
        {
            /// <summary>
            /// The status of the object
            /// </summary>
            [XmlElement("code")]
            [Description("draft | testing | review | production | withdrawn | superseded")]
            public FhirCode<String> Code { get; set; }
            /// <summary>
            /// A comment containing supplemental status info
            /// </summary>
            [XmlElement("comment")]
            [Description("Supplemental status info")]
            public FhirString Comment { get; set; }
        }

        /// <summary>
        /// Creates a new instance of the profile resource
        /// </summary>
        public Profile()
        {
            this.Structure = new List<StructureDefinition>();
            this.ExtensionDefinition = new List<ExtensionDefinition>();
            this.Telecom = new List<FhirTelecom>();
            this.Code = new List<FhirCoding>();

        }

        /// <summary>
        /// Gets or sets the identifier for the profile
        /// </summary>
        [XmlElement("identifier")]
        [Description("Logical id to reference this profile")]
        public FhirString Identifier { get; set; }

        /// <summary>
        /// Version of the profile
        /// </summary>
        [Description("Logical id for this version of the profile")]
        [XmlElement("version")]
        public FhirString Version { get; set; }

        /// <summary>
        /// Informal name for the profile
        /// </summary>
        [XmlElement("name")]
        [Description("Informal name of the profile")]
        [ElementProfile(MinOccurs = 1)]
        public FhirString Name { get; set; }

        /// <summary>
        /// Gets or sets the publisher
        /// </summary>
        [Description("Name of the publisher")]
        [XmlElement("publisher")]
        public FhirString Publisher { get; set; }

        /// <summary>
        /// Gets or sets the telecom
        /// </summary>
        [Description("Contact information of the publisher")]
        [XmlElement("telecom")]
        public List<FhirTelecom> Telecom { get; set; }

        /// <summary>
        /// Description of the profile
        /// </summary>
        [Description("Natural language description of the profile")]
        [XmlElement("description")]
        public FhirString Description { get; set; }

        /// <summary>
        /// Gets or sets codes used for indexing
        /// </summary>
        [Description("A code to assist with indexing and finding")]
        [XmlElement("code")]
        public List<FhirCoding> Code { get; set; }

        /// <summary>
        /// Gets or sets the status
        /// </summary>
        [Description("The current status")]
        [XmlElement("status")]
        [ElementProfile(MinOccurs = 1)]
        public StatusType Status { get; set; }

        /// <summary>
        /// The resource or data type constraint
        /// </summary>
        [Description("A constraint on a resource or datatype")]
        [XmlElement("structure")]
        public List<StructureDefinition> Structure { get; set; }

        /// <summary>
        /// Gets or sets a list of extension definitions
        /// </summary>
        [Description("Definition of an extension")]
        [XmlElement("extensionDefn")]
        public List<ExtensionDefinition> ExtensionDefinition { get; set; }

        /// <summary>
        /// Write a text representation of the profile
        /// </summary>
        /// <param name="w"></param>
        internal override void WriteText(System.Xml.XmlWriter w)
        {
            w.WriteStartElement("div");
            w.WriteAttributeString("class", "h1");
            this.Name.WriteText(w);
            w.WriteEndElement(); // div

            w.WriteStartElement("em");
            this.Publisher.WriteText(w);
            w.WriteEndElement(); // em

            // Emit description
            if (this.Description != null)
            {
                w.WriteStartElement("p");
                this.Description.WriteText(w);
                w.WriteEndElement(); //p
            }

            // Resource content
            foreach (var res in this.Structure)
                res.WriteText(w);

            // Extensions
            foreach (var ext in this.ExtensionDefinition)
                ext.WriteText(w);
        }

        /// <summary>
        /// Represent the profile as a string
        /// </summary>
        public override string ToString()
        {
            return String.Format("[Profile] {0}", this.Name);
        }
    }
}
