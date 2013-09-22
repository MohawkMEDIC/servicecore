using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes
{
    /// <summary>
    /// Represents a codeable concept
    /// </summary>
    [XmlType("CodeableConcept", Namespace = "http://hl7.org/fhir")]
    public class CodeableConcept : Shareable
    {
        /// <summary>
        /// Codeable concept default ctor
        /// </summary>
        public CodeableConcept()
        {
            this.Coding = new List<Coding>();
        }

        /// <summary>
        /// Codable concept ctor
        /// </summary>
        public CodeableConcept(Uri system, String code)
        {
            this.Coding = new List<Coding>() { 
                new Coding(system, code)
            };
            this.Primary = this.Coding[0].MakeIdRef();
        }

        /// <summary>
        /// Coding
        /// </summary>
        [XmlElement("coding")]
        public List<Coding> Coding { get; set; }

        /// <summary>
        /// Gets or sets the text
        /// </summary>
        [XmlElement("text")]
        public FhirString Text { get; set; }

        /// <summary>
        /// Gets or sets the primary
        /// </summary>
        [XmlElement("primary")]
        public IdRef Primary { get; set; }

        /// <summary>
        /// Get the primary code
        /// </summary>
        public Coding GetPrimaryCode()
        {
            if (this.Primary == null && this.Coding.Count > 0)
                return Coding[0];
            else if (this.Primary == null)
                return null;
            return this.Primary.ResolveReference(this) as Coding;
        }

        /// <summary>
        /// Write text
        /// </summary>
        internal override void WriteText(System.Xml.XmlWriter w)
        {

            if (this.Coding.Count > 1)
            {
                w.WriteStartElement("table", NS_XHTML);
                w.WriteStartElement("tbody", NS_XHTML);

                foreach (var cd in this.Coding)
                {
                    w.WriteStartElement("tr", NS_XHTML);
                    w.WriteStartElement("td", NS_XHTML);
                    if (this.Primary != null && this.Primary.Value != null &&
                        this.Primary.Value == cd.XmlId)
                        w.WriteAttributeString("style", "font-weight:bold");
                    cd.WriteText(w);
                    w.WriteEndElement(); // td
                    w.WriteEndElement();

                }

                w.WriteEndElement(); //tbody
                w.WriteEndElement(); // table
            }
            else if(this.Coding.Count == 1)
            {
                this.GetPrimaryCode().WriteText(w);
            }
        }
    }
}
