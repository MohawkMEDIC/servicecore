using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Core.DataTypes;

namespace MARC.HI.EHRS.SVC.Subscription.Core
{
    /// <summary>
    /// Subscription 
    /// </summary>
    [XmlType("Subscription", Namespace = "urn:marc-hi:ehrs:subscription")]
    [XmlRoot("subscription", Namespace = "urn:marc-hi:ehrs:subscription")]
    public class Subscription : FilterExpressionBase
    {

        /// <summary>
        /// Represents the subscription id
        /// </summary>
        [XmlAttribute("id")]
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Represents the participant local identifier
        /// </summary>
        [XmlIgnore]
        public DomainIdentifier ParticipantId { get; set; }

        /// <summary>
        /// Gets the last updated time
        /// </summary>
        [XmlIgnore]
        public DateTime LastUpdate { get; set; }
    }

    [XmlType("Subscription", Namespace = "urn:marc-hi:ehrs:subscription")]
    [XmlRoot("subscriptionInformation", Namespace = "urn:marc-hi:ehrs:subscription")]
    public class SubscriptionDisclosure
    {
        [XmlElement("participantId")]
        public DomainIdentifier ParticipantId { get; set; }
    }
}
