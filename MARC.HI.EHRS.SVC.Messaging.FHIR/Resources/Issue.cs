using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using System.Xml.Serialization;
using System.ComponentModel;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{
    /// <summary>
    /// Represents an issue detail
    /// </summary>
    [XmlType("Issue", Namespace = "http://hl7.org/fhir")]
    public class Issue : FhirElement
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public Issue()
        {
            this.Location = new List<FhirString>();
        }

        /// <summary>
        /// Gets or sets the severity
        /// </summary>
        [XmlElement("severity")]
        [Description("Identifies the severity of operation")]
        [ElementProfile(MinOccurs = 1, RemoteBinding = "http://hl7.org/fhir/issue-severity")]
        public FhirCode<String> Severity { get; set; }

        /// <summary>
        /// Gets or sets the type of error
        /// </summary>
        [XmlElement("type")]
        [Description("Identifies the type of issue detected")]
        [ElementProfile(RemoteBinding = "http://hl7.org/fhir/issue-type")]
        public FhirCoding Type { get; set; }

        /// <summary>
        /// Gets or sets the details of the issue
        /// </summary>
        [XmlElement("details")]
        [Description("Additional description of the issue")]
        public FhirString Details { get; set; }

        /// <summary>
        /// Gets or sets the location
        /// </summary>
        [XmlElement("location")]
        [Description("XPath of the element(s) related to the issue")]
        public List<FhirString> Location { get; set; }
    }
}
