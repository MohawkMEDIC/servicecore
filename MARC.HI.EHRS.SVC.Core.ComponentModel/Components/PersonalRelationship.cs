using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Core.ComponentModel.Components
{
    /// <summary>
    /// Identifies a personal relationship component. A personal relationship
    /// is a client relation with another client
    /// </summary>
    [Serializable][XmlType("PersonalRelationship")]
    public class PersonalRelationship : Client
    {
        /// <summary>
        /// Identifies the kind of relationship
        /// </summary>
        [XmlAttribute("kind")]
        public string RelationshipKind { get; set; }

    }
}
