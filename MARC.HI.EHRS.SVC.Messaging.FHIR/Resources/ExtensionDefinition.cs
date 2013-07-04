using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes;
using System.ComponentModel;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{
    /// <summary>
    /// Represents a definition of an extension 
    /// </summary>
    [XmlType("ExtensionDefn", Namespace = "http://hl7.org/fhir")]
    public class ExtensionDefinition : Shareable
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ExtensionDefinition()
        {
            this.Context = new List<FhirString>();
        }

        /// <summary>
        /// The unique identifier for the extension
        /// </summary>
        [XmlElement("code")]
        [Description("A unique code within the profile used to identify the extension")]
        public PrimitiveCode<String> Code { get; set; }

        /// <summary>
        /// Context types
        /// </summary>
        [XmlElement("contextType")]
        [Description("Identifies the type of context to which the extension applies")]
        public PrimitiveCode<String> ContextType { get; set; }

        /// <summary>
        /// Identifies one or more contexts the extension applies to
        /// </summary>
        [XmlElement("context")]
        [ElementProfile(MinOccurs = 1)]
        [Description("Identifies the types of resource or data type elements to which the extension can be applied")]
        public List<FhirString> Context { get; set; }

        /// <summary>
        /// The definition of the extension
        /// </summary>
        [Description("The definition of the extension")]
        [XmlElement("definition")]
        [ElementProfile(MinOccurs = 1)]
        public ElementDefinition Definition { get; set; }

        /// <summary>
        /// Write element extension to the output
        /// </summary>
        internal override void WriteText(System.Xml.XmlWriter w)
        {
            w.WriteStartElement("table");
            w.WriteAttributeString("border", "1");
            w.WriteElementString("caption", String.Format("Extension - {0}", this.Code));
            w.WriteStartElement("tbody");
            
            // Output the 
            base.WriteTableRows(w, "Applies To", this.Context.ToArray());

            base.WriteTableRows(w, "Type", this.Definition.Type.ToArray());

            if(this.Definition.Binding != null)
            {
                w.WriteStartElement("tr");
                
                base.WriteTableCell(w, (FhirString)"Binding");
                w.WriteStartElement("td");
                w.WriteStartElement("a");
                w.WriteAttributeString("href", String.Format("#{0}", this.Definition.Binding));
                w.WriteString(this.Definition.Binding);
                w.WriteEndElement(); // a
                w.WriteEndElement(); //td
                w.WriteEndElement(); //tr

            }
            base.WriteTableRows(w, "Comments", this.Definition.Comments ?? this.Definition.FormalDefinition ?? this.Definition.ShortDefinition);
            w.WriteEndElement(); // tbody
            w.WriteEndElement(); // table        }
        }
    }
}
