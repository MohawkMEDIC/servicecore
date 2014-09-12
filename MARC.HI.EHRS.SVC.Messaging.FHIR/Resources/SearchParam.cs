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
    /// Search parameter
    /// </summary>
    [XmlType("SearchParam", Namespace = "http://hl7.org/fhir")]
    public class SearchParam : Shareable
    {

        /// <summary>
        /// Gets or sets the name of the search parameter
        /// </summary>
        [XmlElement("name")]
        [Description("The name of the search parameter")]
        [ElementProfile(MinOccurs = 1)]
        public FhirString Name { get; set; }

        /// <summary>
        /// Gets or sets the source of the search parameter definition
        /// </summary>
        [XmlElement("definition")]
        [Description("The source of the search parameter definition")]
        public FhirUri Definition { get; set; }

        /// <summary>
        /// Gets or sets the type of the parameter
        /// </summary>
        [XmlElement("type")]
        [Description("The type of the search parameter")]
        [ElementProfile(MinOccurs = 1)]
        public PrimitiveCode<String> Type { get; set; }

        /// <summary>
        /// Gets or sets the documentation related to the parameter
        /// </summary>
        [XmlElement("documentation")]
        [Description("Contents and meaning of the parameter")]
        [ElementProfile (MinOccurs = 1)]
        public FhirString Documentation { get; set; }

        /// <summary>
        /// Write textual output of the search parameter
        /// </summary>
        internal override void WriteText(System.Xml.XmlWriter w)
        {
            w.WriteStartElement("tr");
            base.WriteTableCell(w, this.Name);
            base.WriteTableCell(w, this.Type);
            base.WriteTableCell(w, this.Documentation);
            w.WriteEndElement(); // tr
        }

    }
}
