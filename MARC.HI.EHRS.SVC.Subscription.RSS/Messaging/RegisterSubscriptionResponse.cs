using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Core.DataTypes;
using MARC.HI.EHRS.SVC.Core.Issues;

namespace MARC.HI.EHRS.SVC.Subscription.Data.Messaging
{
    /// <summary>
    /// Subscription 
    /// </summary>
    [XmlType("RegisterSubscriptionResposne", Namespace = "urn:marc-hi:ehrs:subscription")]
    [XmlRoot("subscription", Namespace = "urn:marc-hi:ehrs:subscription")]
    public class RegisterSubscriptionResponse
    {

        /// <summary>
        /// Represents the subscription id
        /// </summary>
        [XmlElement("id")]
        public List<DomainIdentifier> SubscriptionId { get; set; }

        /// <summary>
        /// Detected issues with the subscription
        /// </summary>
        [XmlElement("issue")]
        public List<DetectedIssue> Issues { get; set; }


    }
}
