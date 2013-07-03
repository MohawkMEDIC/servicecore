using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using System.ComponentModel;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{
    /// <summary>
    /// Element definition
    /// </summary>
    [XmlType("ElementDefinition", Namespace = "http://hl7.org/fhir")]
    public class ElementDefinition
    {

        /// <summary>
        /// Short definition of the extension
        /// </summary>
        [XmlElement("short")]
        [Description("A concise definition that is shown in the concise XML format that summarized profiles")]
        public FhirString ShortDefinition { get; set; }
        /// <summary>
        /// Formal definition of the extension
        /// </summary>
        [XmlElement("formal")]
        [Description("The definition must be consistent with the base definition but convey the meaning of the element")]
        public FhirString FormalDefinition { get; set; }
        /// <summary>
        /// Minimum occurance
        /// </summary>
        [XmlElement("min")]
        [Description("The minimum number of times this element must appear in the instance")]
        public FhirInt MinOccurs { get; set; }
        /// <summary>
        /// Maximum occurance
        /// </summary>
        [XmlElement("max")]
        [Description("The maximum number of times this element can appear in the instance")]
        public FhirString MaxOccurs { get; set; }
        /// <summary>
        /// True if the object must be supported
        /// </summary>
        [XmlElement("mustSupport")]
        [Description("If true, conformant resource authors must be capable of providing a value for the element and resource consumers must be capable of extracting and doing something useful with the data element. If false, the element may be ignored and not supported")]
        public FhirBoolean MustSupport { get; set; }
        /// <summary>
        /// True if the object must be understood
        /// </summary>
        [Description("If true, the element cannot be ignored by systems unless they recognize the element and a pre-determination has been made that it is not relevant to their particular system")]
        [XmlElement("mustUnderstand")]
        public FhirBoolean MustUnderstand { get; set; }
        /// <summary>
        /// The external binding if applicable
        /// </summary>
        [Description("Identifies the set of codes that applies to this element if a data type supporting codes is used")]
        [XmlElement("binding")]
        public FhirString Binding { get; set; }

    }
}
