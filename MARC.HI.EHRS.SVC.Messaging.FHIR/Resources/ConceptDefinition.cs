using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{
    /// <summary>
    /// Identifies a concept definition
    /// </summary>
    [XmlType("Concept", Namespace = "http://hl7.org/fhir")]
    public class ConceptDefinition : Shareable
    {

        /// <summary>
        /// Gets or sets the code
        /// </summary>
        [XmlElement("code")]
        [ElementProfile(MinOccurs = 1)]
        public PrimitiveCode<String> Code { get; set; }
        
        /// <summary>
        /// Gets or sets the abstract modifier
        /// </summary>
        [XmlElement("abstract")]
        public FhirBoolean Abstract { get; set; }

        /// <summary>
        /// Gets or sets the display name for the code
        /// </summary>
        [XmlElement("display")]
        public FhirString Display { get; set; }

        /// <summary>
        /// Gets or sets the definition
        /// </summary>
        [XmlElement("definition")]
        public FhirString Definition { get; set; }

        /// <summary>
        /// Write text
        /// </summary>
        internal override void WriteText(System.Xml.XmlWriter w)
        {
            w.WriteStartElement("a");
            w.WriteAttributeString("href", String.Format("#{0}", this.Code));
            this.Code.WriteText(w);
            w.WriteEndElement();
        }
    }
}
