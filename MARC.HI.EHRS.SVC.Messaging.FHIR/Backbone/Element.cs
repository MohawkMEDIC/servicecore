using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using System.ComponentModel;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes;
using System.Reflection;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Backbone
{
    /// <summary>
    /// Describes an element
    /// </summary>
    [XmlType("Element", Namespace = "http://hl7.org/fhir")]
    public class Element : BackboneElement
    {

        /// <summary>
        /// Default ctor
        /// </summary>
        public Element()
        {

        }

         /// <summary>
        /// Root type
        /// </summary>
        public Element(Type rootType)
            : this()
        {
            var profile = rootType.GetCustomAttribute<ResourceProfileAttribute>();
            var traversal = rootType.GetCustomAttribute<XmlRootAttribute>();

            // Now populate!
            // First, populate what we can
            if (profile != null)
                this.Name = profile.Name;

            // The traversal
            if (traversal != null)
                this.Path = traversal.ElementName;

            this.Definition = new ElementDefinition(rootType);
        }

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
        /// Gets or sets the definition
        /// </summary>
        [Description("Definition of the content of the element to provide a more specific definition than that contained for the element in the base resource")]
        [XmlElement("definition")]
        [ElementProfile(MinOccurs = 1)]
        public ElementDefinition Definition { get; set; }

        /// <summary>
        /// Gets or sets whether the element can be bundled
        /// </summary>
        [XmlElement("bundled")]
        [Description("Whether the Resource that is the value for this element is included in the bundle, if the profile is specifying a bundle")]
        public FhirBoolean Bundled { get; set; }

        /// <summary>
        /// Write a textual representation of this 
        /// </summary>
        internal override void WriteText(System.Xml.XmlWriter w)
        {
            
            w.WriteStartElement("tr");
            base.WriteTableCell(w, this.Path, 1, 1, this.Definition.MaxOccurs == "0" ? "text-decoration:line-through" : null);
            this.Definition.WriteText(w);
            w.WriteEndElement(); // tr

        }
    }
}
