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
    /// Describes an element
    /// </summary>
    [XmlType("Element", Namespace = "http://hl7.org/fhir")]
    public class Element
    {
        /// <summary>
        /// Gets or sets the path upon which the element is bound
        /// </summary>
        [Description("The path identifies the element and is expressed as a \".\"-separated list of ancestor elements, beginning with the name of the resource")]
        [XmlElement("path")]
        [ElementProfile(MinOccurs = 1)]
        public FhirString Path { get; set; }
        /// <summary>
        /// Gets or sets the name of the restriction
        /// </summary>
        [Description("A unique name referring to a specific set of constraints applied to this element")]
        [XmlElement("name")]
        public FhirString Name { get; set; }
        /// <summary>
        /// Gets or sets whether the element can be bundled
        /// </summary>
        [XmlElement("bundled")]
        [Description("Whether the Resource that is the value for this element is included in the bundle, if the profile is specifying a bundle")]
        public FhirBoolean Bundled { get; set; }
        /// <summary>
        /// Gets or sets the definition
        /// </summary>
        [Description("Definition of the content of the element to provide a more specific definition than that contained for the element in the base resource")]
        [XmlElement("definition")]
        [ElementProfile(MinOccurs = 1)]
        public ElementDefinition Definition { get; set; }
    }
}
