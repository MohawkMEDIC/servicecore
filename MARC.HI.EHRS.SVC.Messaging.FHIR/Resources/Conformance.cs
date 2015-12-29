using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using System.ComponentModel;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{
    /// <summary>
    /// Conformance resource
    /// </summary>
    [XmlType("Conformance", Namespace = "http://hl7.org/fhir")]
    [XmlRoot("Conformance", Namespace = "http://hl7.org/fhir")]
    public class Conformance : ResourceBase
    {

        public Conformance()
        {
            this.Telecom = new List<FhirTelecom>();
            this.Rest = new List<RestDefinition>();
            this.Format = new List<FhirCode<string>>();
        }
        /// <summary>
        /// Logical identifier to this resource
        /// </summary>
        [Description("Logical id to refernece this statement")]
        [XmlElement("identifier")]
        public FhirString Identifier { get; set; }
        /// <summary>
        /// Logical id for this version of the statement
        /// </summary>
        [Description("Logical id for the version of this statement")]
        [XmlElement("version")]
        public FhirString Version { get; set; }
        /// <summary>
        /// Gets or sets the name of the statement
        /// </summary>
        [XmlElement("name")]
        [Description("Informal name for this statement")]
        public FhirString Name { get; set; }
        /// <summary>
        /// The publishing organization
        /// </summary>
        [Description("Publishing organization")]
        [XmlElement("publisher")]
        public FhirString Publisher { get; set; }
        /// <summary>
        /// Contacts for the organization
        /// </summary>
        [Description("Contacts for the organization")]
        [XmlElement("telecom")]
        public List<FhirTelecom> Telecom { get; set; }
        /// <summary>
        /// Description of the conformance statement
        /// </summary>
        [Description("Human description of the conformance statement")]
        [XmlElement("description")]
        public FhirString Description { get; set; }
        /// <summary>
        /// The status of the conformance statement
        /// </summary>
        [Description("Status of the conformance statement")]
        [XmlElement("status")]
        public FhirCode<String> Status { get; set; }
        /// <summary>
        /// True if the conformance statement is experimental
        /// </summary>
        [Description("If for testing purposes, not real useage")]
        [XmlElement("experimental")]
        public FhirBoolean Experimental { get; set; }
        /// <summary>
        /// Date the spec was published
        /// </summary>
        [Description("Date of publication")]
        [XmlElement("date")]
        public FhirDateTime Date { get; set; }
        /// <summary>
        /// Describes the software that is covered by this conformance statement
        /// </summary>
        [XmlElement("software")]
        [Description("Describes the software that is covered by this conformance statement")]
        public SoftwareDefinition Software { get; set; }
        /// <summary>
        /// Describes the specified instance
        /// </summary>
        [Description("Describes the specific instnace")]
        [XmlElement("implementation")]
        public ImplementationDefinition Implementation { get; set; }
        /// <summary>
        /// Gets or sets the FHIR version
        /// </summary>
        [Description("The FHIR version")]
        [XmlElement("fhirVersion")]
        [ElementProfile(MinOccurs = 1)]
        public FhirString FhirVersion { get; set; }
        /// <summary>
        /// True if application accepts unknown elements
        /// </summary>
        [Description("True if application accepts unknown elements")]
        [XmlElement("acceptUnknown")]
        [ElementProfile(MinOccurs = 1)]
        public FhirBoolean AcceptUnknown { get; set; }
        /// <summary>
        /// Formats supported
        /// </summary>
        [Description("Formats supported")]
        [XmlElement("format")]
        public List<FhirCode<String>> Format { get; set; }
        /// <summary>
        /// Endpoint if restful
        /// </summary>
        [XmlElement("rest")]
        [Description("Endpoint if restful")]
        public List<RestDefinition> Rest { get; set; }

        /// <summary>
        /// Write text for the resource
        /// </summary>
        internal override void WriteText(System.Xml.XmlWriter w)
        {
            w.WriteStartElement("div");
            w.WriteAttributeString("class", "h1");
            w.WriteString(String.Format("Conformance Statement {0}", this.Id));
            w.WriteEndElement(); // div

            w.WriteStartElement("table");
            w.WriteAttributeString("border", "1");
            w.WriteStartElement("caption");
            this.Name.WriteText(w);
            w.WriteEndElement(); // caption

            // Elements
            w.WriteStartElement("tbody");
            base.WriteTableRows(w, "Publisher", this.Publisher);
            base.WriteTableRows(w, "Published On", this.Date); 
            base.WriteTableRows(w, "Description", this.Description);
            base.WriteTableRows(w, "Status", this.Status);
            base.WriteTableRows(w, "Software Name", this.Software.Name);
            base.WriteTableRows(w, "Software Version", this.Software.Version);
            base.WriteTableRows(w, "Software Release Date", this.Software.ReleaseDate);
            base.WriteTableRows(w, "Base URI", this.Implementation.Url);
            base.WriteTableRows(w, "FHIR Version", this.FhirVersion);
            base.WriteTableRows(w, "Accepted Formats", this.Format.ToArray());
            base.WriteTableRows(w, "RESTful Implementations", this.Rest.ToArray());
            w.WriteEndElement();
            w.WriteEndElement(); // table

        }
    }
}
