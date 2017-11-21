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
    /// Extension context value set
    /// </summary>
    [XmlType("ExtensionContext", Namespace = "http://hl7.org/fhir")]
    [FhirValueSet(Uri = "http://hl7.org/fhir/ValueSet/extension-context")]
    public enum ExtensionContext
    {
        [XmlEnum("resource")]
        Resource,
        [XmlEnum("datatype")]
        Datatype,
        [XmlEnum("mapping")]
        Mapping,
        [XmlEnum("extension")]
        Extension
    }

    /// <summary>
    /// Structure definition kind
    /// </summary>
    [XmlType("StructureDefinitionKind", Namespace = "http://hl7.org/fhir")]
    [FhirValueSet(Uri = "http://hl7.org/fhir/ValueSet/structure-definition-kind")]
    public enum StructureDefinitionKind
    {
        [XmlEnum("datatype")]
        Datatype,
        [XmlEnum("resource")]
        Resource,
        [XmlEnum("logical")]
        Logical
    }

    /// <summary>
    /// Represents a profile
    /// </summary>
    [XmlType("StructureDefinition", Namespace = "http://hl7.org/fhir")]
    [XmlRoot("StructureDefinition", Namespace = "http://hl7.org/fhir")]
    public class StructureDefinition : DomainResourceBase
    {
        /// <summary>
        /// Structure definition
        /// </summary>
        public StructureDefinition()
        {
            this.Identifier = new List<FhirIdentifier>();
            this.Contact = new List<ContactDetail>();
        }

        /// <summary>
        /// Gets or sets the absolute URL to access the resource
        /// </summary>
        [XmlElement("url")]
        [Description("Absolute URL used to reference this StructureDefinition")]
        [FhirElement(MinOccurs = 1)]
        public FhirUri Url { get; set; }
        /// <summary>
        /// Gets or sets other identifiers for the definition
        /// </summary>
        [XmlElement("identifier")]
        [Description("Other identifier for the StructureDefinition")]
        public List<FhirIdentifier> Identifier { get; set; }
        /// <summary>
        /// Gets or sets the logical identifier for the version of the structure definition
        /// </summary>
        [XmlElement("version")]
        [Description("Logical identifier for the version of the structure")]
        public FhirString Version { get; set; }
        /// <summary>
        /// Gets or sets the informal name for the structure definition
        /// </summary>
        [XmlElement("name")]
        [Description("Informal name for this structure definition")]
        public FhirString Name { get; set; }
        /// <summary>
        /// Gets or sets the display name of the structure
        /// </summary>
        [XmlElement("display")]
        [Description("Use this name when displaying the value")]
        public FhirString Display { get; set; }
        /// <summary>
        /// Gets or sets the status of the structure definition
        /// </summary>
        [XmlElement("status")]
        [Description("The status of the structure definition")]
        public FhirCode<PublicationStatus> Status { get; set; }
        /// <summary>
        /// Gets or sets whether this structure definition is experimental
        /// </summary>
        [XmlElement("experimental")]
        [Description("If for testing purposes, not real usage")]
        public FhirBoolean Experimental { get; set; }
        /// <summary>
        /// Gets or sets the publisher name
        /// </summary>
        [XmlElement("publisher")]
        [Description("Name of the publisher")]
        public FhirString Publisher { get; set; }
        /// <summary>
        /// Gets or sets the contact information for the publisher
        /// </summary>
        [XmlElement("contact")]
        [Description("Contact details of the publisher")]
        public List<ContactDetail> Contact { get; set; }
        /// <summary>
        /// Gets or sets the date for this version of the structure definition
        /// </summary>
        [XmlElement("date")]
        [Description("Date for this version of the structure definition")]
        public FhirDateTime Date { get; set; }
        /// <summary>
        /// Gets or sets natural language description
        /// </summary>
        [XmlElement("description")]
        [Description("Natural language description of the structure definition")]
        public FhirString Description { get; set; }
        /// <summary>
        /// Gets or sets the context 
        /// </summary>
        [XmlElement("useContext")]
        [Description("Content intends to support these contexts")]
        public List<FhirCodeableConcept> UseContext { get; set; }
        /// <summary>
        /// Gets or sets the reason why the structure definition is needed
        /// </summary>
        [XmlElement("requirements")]
        [Description("Scope and usage for this structure definition")]
        public FhirString Requirements { get; set; }
        /// <summary>
        /// Gets or sets use and/or publishing restrictions
        /// </summary>
        [XmlElement("copyright")]
        [Description("Use and/or publishing restrictions")]
        public FhirString Copyright { get; set; }
        /// <summary>
        /// Assists with indexing and finding
        /// </summary>
        [XmlElement("code")]
        [Description("Assists with indexing and finding")]
        [FhirElement(MaxOccurs = 0)]
        public List<FhirCodeableConcept> Code { get; set; }
        /// <summary>
        /// Gets or sets the version of FHIR
        /// </summary>
        [XmlElement("fhirVersion")]
        [Description("FHIR Version this StructureDefinition targets")]
        public FhirString FhirVersion { get; set; }
        /// <summary>
        /// Gets or sets the kind of structure definition
        /// </summary>
        [XmlElement("kind")]
        [Description("Identifies the kind of structure definition")]
        [FhirElement(MinOccurs = 1)]
        public FhirCode<StructureDefinitionKind> Kind { get; set; }
        /// <summary>
        /// Gets or sets the datatype or resource type
        /// </summary>
        [XmlElement("constrainedType")]
        [Description("Anyh datatype or resource, including abstract ones")]
        public FhirCode<String> ConstrainedType { get; set; }
        /// <summary>
        /// Whether the structure is abstract
        /// </summary>
        [XmlElement("abstract")]
        [Description("Whether the structure is abstract")]
        [FhirElement(MinOccurs = 1)]
        public FhirBoolean Abstract { get; set; }
        /// <summary>
        /// Gets or sets the context where an extension can be used
        /// </summary>
        [XmlElement("context")]
        [Description("Where the extension can be used in instances")]
        public List<FhirCode<ExtensionContext>> Context { get; set; }
        /// <summary>
        /// Structure that this structure definition extends
        /// </summary>
        [XmlElement("base")]
        [Description("Structure that this set of constraints applies to")]
        public FhirUri Base { get; set; }
        ///// <summary>
        ///// Snapshot view of the structure 
        ///// </summary>
        [XmlElement("snapshot")]
        [Description("Snapshot view of the structure")]
        public StructureDefinitionContent Snapshot { get; set; }
    }
}
