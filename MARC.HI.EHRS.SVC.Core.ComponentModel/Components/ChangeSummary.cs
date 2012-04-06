using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.DataTypes;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Core.ComponentModel.Components
{
    /// <summary>
    /// Change summary component
    /// </summary>
    [Serializable][XmlType("ChangeSummary")]
    public class ChangeSummary : HealthServiceRecordContainer
    {

        /// <summary>
        /// Alternate identifier for the versioned domain identifier
        /// </summary>
        [XmlElement("altId")]
        public VersionedDomainIdentifier AlternateIdentifier { get; set; }

        /// <summary>
        /// Language code
        /// </summary>
        [XmlAttribute("lang")]
        public string LanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the version identifier
        /// </summary>
        [XmlAttribute("version")]
        public decimal VersionIdentifier { get; set; }

        /// <summary>
        /// Identifies the type of change
        /// </summary>
        [XmlElement("changeType")]
        public CodeValue ChangeType { get; set; }

        /// <summary>
        /// Gets or sets the status
        /// </summary>
        [XmlAttribute("status")]
        public StatusType Status { get; set; }

        /// <summary>
        /// Gets or sets the effective time
        /// </summary>
        [XmlElement("effectiveTime")]
        public TimestampSet EffectiveTime { get; set; }
    }
}
