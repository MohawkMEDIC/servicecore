using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Core.ComponentModel.Components
{
    /// <summary>
    /// Identifies an annotation component
    /// </summary>
    [Serializable][XmlType("Annotation")]
    public class Annotation : HealthServiceRecordContainer
    {
        /// <summary>
        /// Gets or sets the text of the note component
        /// </summary>
        [XmlElement("text")]
        public string Text { get; set; }

        /// <summary>
        /// The language of the annotation (ISO639-1)
        /// </summary>
        [XmlAttribute("lang")]
        public string Language { get; set; }
    }
}
