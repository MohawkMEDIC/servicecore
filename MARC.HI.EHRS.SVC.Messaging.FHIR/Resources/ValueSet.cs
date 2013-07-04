using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{
    /// <summary>
    /// Value set definition
    /// </summary>
    [XmlType("ValueSet", Namespace = "http://hl7.org/fhir")]
    [XmlRoot("ValueSet", Namespace = "http://hl7.org/fhir")]
    [Profile(ProfileId = "svccore")]
    [ResourceProfile(Name = "valueset")]
    public class ValueSet : ResourceBase
    {
        /// <summary>
        /// Identifier of the value set
        /// </summary>
        [XmlElement("identifier")]
        public FhirString Identifier { get; set; }

        /// <summary>
        /// Version of the value set
        /// </summary>
        [XmlElement("version")]
        public FhirString Version { get; set; }

        /// <summary>
        /// Name of the value set
        /// </summary>
        [XmlElement("name")]
        [ElementProfile(MinOccurs = 1)]
        public FhirString Name { get; set; }

        /// <summary>
        /// Publisher of the value set
        /// </summary>
        [XmlElement("publisher")]
        public FhirString Publisher { get; set; }

        /// <summary>
        /// Description of the value set
        /// </summary>
        [XmlElement("description")]
        public FhirString Description { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [XmlElement("status")]
        public PrimitiveCode<String> Status { get; set; }

        /// <summary>
        /// The date of publication
        /// </summary>
        [XmlElement("date")]
        public DateOnly Date { get; set; }

        /// <summary>
        /// Gets or sets the definition
        /// </summary>
        [XmlElement("define")]
        public ValueSetDefinition Define { get; set; }

        /// <summary>
        /// Compse a definitoin
        /// </summary>
        [XmlElement("compose")]
        public ComposeDefinition Compose { get; set; }

        /// <summary>
        /// Write text
        /// </summary>
        internal override void WriteText(System.Xml.XmlWriter w)
        {

            w.WriteStartElement("table");
            w.WriteAttributeString("border", "1");
            w.WriteStartElement("caption");
            this.Name.WriteText(w);
            w.WriteString("(");

            if(this.Identifier != null)
                this.Identifier.WriteText(w);
            w.WriteString(") - ");

            if (this.Define != null)
                w.WriteString("Defines");
            else
                w.WriteString("Composed of");

            w.WriteEndElement();// caption
            w.WriteStartElement("tbody");


            // Write headers
            if (this.Define != null)
            {
                w.WriteStartElement("tr");
                this.WriteTableHeader(w, (FhirString)"Mnemonic");
                this.WriteTableHeader(w, (FhirString)"System");
                this.WriteTableHeader(w, (FhirString)"Display Name");
                w.WriteEndElement(); // tr

                foreach (var itm in this.Define.Concept)
                {
                    w.WriteStartElement("tr");
                    this.WriteTableCell(w, itm);
                    this.WriteTableCell(w, this.Define.System);
                    this.WriteTableCell(w, itm.Display);
                    w.WriteEndElement(); // tr
                }
            }
            else
            {
                w.WriteStartElement("tr");
                this.WriteTableHeader(w, (FhirString)"System");
                this.WriteTableHeader(w, (FhirString)"Mnemonic");
                w.WriteEndElement(); // tr

                foreach (var itm in this.Compose.Include)
                    foreach (var code in itm.Code)
                    {
                        w.WriteStartElement("tr");

                        if(code == itm.Code.FirstOrDefault())
                            this.WriteTableCell(w, itm.System, 1, itm.Code.Count);
                        this.WriteTableCell(w, code);
                        w.WriteEndElement(); // tr
                    }
            }
            w.WriteEndElement();
            w.WriteEndElement(); // table

        }
    }
}
