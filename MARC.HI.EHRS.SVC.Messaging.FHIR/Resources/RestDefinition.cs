using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using System.ComponentModel;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{
    /// <summary>
    /// RESTFul definition
    /// </summary>
    [XmlType("Rest", Namespace = "http://hl7.org/fhir")]
    public class RestDefinition : Shareable
    {

        /// <summary>
        /// Creates a new instance of the rest definition
        /// </summary>
        public RestDefinition()
        {
            this.Resource = new List<ResourceDefinition>();
        }

        /// <summary>
        /// Gets or sets the mode (client or server)
        /// </summary>
        [XmlElement("mode")]
        [Description("Describes the mode of REST implementation")]
        public PrimitiveCode<String> Mode { get; set; }
        /// <summary>
        /// Gets or sets the documentation about the REST 
        /// </summary>
        [XmlElement("documentation")]
        [Description("General description of the implementation")]
        public FhirString Documentation { get; set; }
        // TODO: Security?

        /// <summary>
        /// Resource supported by the rest interface
        /// </summary>
        [XmlElement("resource")]
        [Description("Resource served on the rest interface")]
        [ElementProfile(MinOccurs = 1)]
        public List<ResourceDefinition> Resource { get; set; }

        /// <summary>
        /// RESTful information
        /// </summary>
        internal override void WriteText(System.Xml.XmlWriter w)
        {
            w.WriteStartElement("table");
            w.WriteAttributeString("border", "1");
            w.WriteElementString("caption", String.Format("{0} Operations", this.Mode));
            w.WriteStartElement("tbody");

            // Setup table headers
            List<PrimitiveCode<String>> headers = new List<PrimitiveCode<String>>();
            headers.Add(new PrimitiveCode<string>("Resource"));
            foreach (var res in this.Resource)
                foreach (var op in res.Operation)
                    if (!headers.Exists(o=>o.Value == op.Type.Value))
                        headers.Add(op.Type);
            w.WriteStartElement("tr");
            foreach (var hdr in headers)
                base.WriteTableHeader(w, hdr);
            w.WriteEndElement();

            // Now create resource options
            foreach (var res in this.Resource)
            {
                w.WriteStartElement("tr");
                base.WriteTableCell(w, res);
                bool[] supported = new bool[headers.Count - 1];
                foreach (var op in res.Operation)
                    supported[headers.FindIndex(o => o.Value == op.Type.Value) - 1] = true;
                for (int i = 0; i < supported.Length; i++)
                    base.WriteTableCell(w, new FhirString(supported[i] ? "X" : " "));
                w.WriteEndElement(); // tr
            }

            w.WriteEndElement(); // tbody
            w.WriteEndElement(); // table

           

        }

    }
}
