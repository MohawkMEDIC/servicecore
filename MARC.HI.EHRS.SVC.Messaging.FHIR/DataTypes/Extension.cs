using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Resources;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes
{
    /// <summary>
    /// Represents an extension
    /// </summary>
    [XmlType("Extension", Namespace = "http://hl7.org/fhir")]
    public class Extension : Shareable
    {
        /// <summary>
        /// URL of the extension definition
        /// </summary>
        [XmlAttribute("url")]
        public String Url { get; set; }

        /// <summary>
        /// True if is modifier
        /// </summary>
        [XmlElement("isModifier")]
        public FhirBoolean IsModifier { get; set; }

        /// <summary>
        /// Value choice
        /// </summary>
        [XmlElement("valueInteger", typeof(FhirInt))]
        [XmlElement("valueDecimal", typeof(FhirDecimal))]
        [XmlElement("valueDateTime", typeof(Date))]
        [XmlElement("valueDate", typeof(DateOnly))]
        [XmlElement("valueInstant", typeof(Primitive<DateTime>))]
        [XmlElement("valueString", typeof(FhirString))]
        [XmlElement("valueUri", typeof(FhirUri))]
        [XmlElement("valueBoolean", typeof(FhirBoolean))]
        [XmlElement("valueCode", typeof(PrimitiveCode<String>))]
        [XmlElement("valueBase64Binary", typeof(FhirBinary))]
        [XmlElement("valueCoding", typeof(Coding))]
        [XmlElement("valueCodeableConcept", typeof(CodeableConcept))]
        [XmlElement("valueAttachment", typeof(Attachment))]
        [XmlElement("valueIdentifier", typeof(Identifier))]
        [XmlElement("valueQuantity", typeof(Quantity))]
        [XmlElement("valueChoice", typeof(Choice))]
        [XmlElement("valueRange", typeof(Range))]
        [XmlElement("valuePeriod", typeof(Period))]
        [XmlElement("valueRatio", typeof(Ratio))]
        [XmlElement("valueHumanName", typeof(HumanName))]
        [XmlElement("valueAddress", typeof(Address))]
        [XmlElement("valueContact" ,typeof(Telecom))]
        [XmlElement("valueSchedule", typeof(Schedule))]
        [XmlElement("valueResource", typeof(Resource))]
        public Shareable Value { get; set; }


        /// <summary>
        /// Write extension information
        /// </summary>
        internal override void WriteText(System.Xml.XmlWriter w)
        {
            if(this.Value != null)
                this.Value.WriteText(w);
            w.WriteString(" - Profile: ");
            w.WriteStartElement("a");
            w.WriteAttributeString("href", this.Url);
            w.WriteString(this.Url);
            w.WriteEndElement(); //a
        }

    }
}
