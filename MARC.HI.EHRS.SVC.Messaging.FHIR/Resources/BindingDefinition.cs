using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using System.ComponentModel;
using MARC.Everest.Attributes;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{
    /// <summary>
    /// Represents a binding definition
    /// </summary>
    [XmlType("Binding", Namespace = "http://hl7.org/fhir")]
    public class BindingDefinition : Shareable
    {

        /// <summary>
        /// Default CTOR
        /// </summary>
        public BindingDefinition()
        {

        }

        /// <summary>
        /// Creates a new binding definition
        /// </summary>
        public BindingDefinition(FHIR.Attributes.ElementProfileAttribute profile)
        {
            // Binding ? Get the name and set er up
            string bindingName = null;
            if (profile.Binding != null)
            {
                var structAtt = profile.Binding.GetCustomAttribute<StructureAttribute>();
                if (structAtt != null)
                    bindingName = structAtt.Name;
                else
                {
                    var xtypeName = profile.Binding.GetCustomAttribute<XmlTypeAttribute>();
                    if (xtypeName != null)
                        bindingName = xtypeName.TypeName;
                    else
                        bindingName = profile.Binding.Name;
                }
            }
            else if (profile.RemoteBinding != null)
                bindingName = new Uri(profile.RemoteBinding).Segments.Last();

            // Create the binding?
            if (bindingName != null)
            {
                this.Name = bindingName;
                this.IsExtensible = profile.Binding == null;
                this.Conformance = new PrimitiveCode<string>("preferred");
                this.Reference = profile.RemoteBinding == null ?
                            (Shareable)new Resource<ValueSet>()
                            {
                                Reference = profile.Binding.GetValueSetDefinition().ToString()
                            } : (FhirUri)new Uri(profile.RemoteBinding);
            }
        }

        /// <summary>
        /// Binding definition
        /// </summary>
        /// <param name="ext"></param>
        public BindingDefinition(FHIR.Attributes.ExtensionDefinitionAttribute ext) : this(new ElementProfileAttribute()
            {
                Binding = ext.Binding,
                RemoteBinding = ext.RemoteBinding
            })
        {
            
        }

        /// <summary>
        /// The name of the binding
        /// </summary>
        [XmlElement("name")]
        [Description("Defines a linkage between a vocabulary binding name used in the profile (or expected to be used in profile importing this one) and a value set or code list")]
        public FhirString Name { get; set; }

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
        /// The definition (description) of the binding
        /// </summary>
        [XmlElement("definition")]
        [Description("Describes the intended use of this particular set of codes")]
        public FhirString Definition { get; set; }
        /// <summary>
        /// Identifies the referenced value set
        /// </summary>
        [XmlElement("referenceUri", typeof(FhirUri))]
        [XmlElement("referenceResource", typeof(Resource<ValueSet>))]
        [Description("Points to the value set or external definition that identifies the set of codes to be used")]
        public Shareable Reference { get; set; }

        /// <summary>
        /// Write text
        /// </summary>
        internal override void WriteText(System.Xml.XmlWriter w)
        {

            w.WriteStartElement("a");
            w.WriteAttributeString("name", this.Name);
            w.WriteEndElement();
            w.WriteStartElement("table");
            w.WriteAttributeString("border", "1");
            w.WriteElementString("caption", String.Format("ValueSet Binding - {0}", this.Name));
            w.WriteStartElement("tbody");

            // Output the 
            base.WriteTableRows(w, "Extensible", this.IsExtensible);
            base.WriteTableRows(w, "Conformance", this.Conformance);
            base.WriteTableRows(w, "Reference", this.Reference);
            w.WriteEndElement(); // tbody
            w.WriteEndElement(); // table      
        }
    }
}
