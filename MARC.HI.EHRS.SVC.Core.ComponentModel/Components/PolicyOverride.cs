using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Core.DataTypes;

namespace MARC.HI.EHRS.SVC.Core.ComponentModel.Components
{

    /// <summary>
    /// Policy override reason
    /// </summary>
    public enum PolicyOverrideReason
    {
        Emergency,
        ProfessionalJudgement
    }

    /// <summary>
    /// Consent policy override!
    /// </summary>
    [Serializable][XmlType("PolicyOverride")]
    public class PolicyOverride : HealthServiceRecordContainer
    {

        /// <summary>
        /// Identifies the form where the policy override is active
        /// </summary>
        [XmlElement("formId")]
        public DomainIdentifier FormId { get; set; }

        /// <summary>
        /// Identifies the reason for the policy override
        /// </summary>
        [XmlAttribute("reason")]
        public PolicyOverrideReason Reason { get; set; }

        /// <summary>
        /// Identifies the time that the policy is effective for
        /// </summary>
        [XmlElement("effectiveTime")]
        public TimestampSet EffectiveTime { get; set; }

    }

}
