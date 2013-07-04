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
    /// Represents a binding definition
    /// </summary>
    [XmlType("Binding", Namespace = "http://hl7.org/fhir")]
    public class BindingDefinition : Shareable
    {
        /// <summary>
        /// The name of the binding
        /// </summary>
        [XmlElement("name")]
        [Description("Defines a linkage between a vocabulary binding name used in the profile (or expected to be used in profile importing this one) and a value set or code list")]
        public FhirString String { get; set; }
        /// <summary>
        /// The definition (description) of the binding
        /// </summary>
        [XmlElement("definition")]
        [Description("Describes the intended use of this particular set of codes")]
        public FhirString Definition { get; set; }
        /// <summary>
        /// The type of binding
        /// </summary>
        [XmlElement("type")]
        [Description("Identifies how the set of codes for this binding is being defined")]
        public PrimitiveCode<String> Type { get; set; }
        /// <summary>
        /// True if codes can be added to the binding
        /// </summary>
        [XmlElement("isExtensible")]
        [Description("If true, then conformant systems may use additional codes or (where the data type permits) text alone to convey concepts not covered by the set of codes identified in the binding. If false, then conformant systems are constrained to the provided codes alone")]
        public FhirBoolean IsExtensible { get; set; }
        /// <summary>
        /// Specifies the level of conformance
        /// </summary>
        [XmlElement("conformance")]
        [Description("Indicates the degree of conformance expectations associated with this binding")]
        public PrimitiveCode<String> Conformance { get; set; }
        /// <summary>
        /// Identifies the referenced value set
        /// </summary>
        [XmlElement("referenceValueSet")]
        [Description("Points to the value set or external definition that identifies the set of codes to be used")]
        public Resource<ValueSet> Reference { get; set; }
    }
}
