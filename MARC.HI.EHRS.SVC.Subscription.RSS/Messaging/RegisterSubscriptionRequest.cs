using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Core.DataTypes;
using MARC.HI.EHRS.SVC.Core.ComponentModel.Components;
using MARC.HI.EHRS.SVC.Subscription.Core;

namespace MARC.HI.EHRS.SVC.Subscription.Data.Messaging
{
    /// <summary>
    /// Subscription request
    /// </summary>
    [XmlType("RegisterSubscriptionRequest", Namespace = "urn:marc-hi:ehrs:subscription")]
    [XmlRoot("subscribe", Namespace = "urn:marc-hi:ehrs:subscription")]
    public class RegisterSubscriptionRequest : FilterExpressionBase
    {

        /// <summary>
        /// Identifies the author of the expression tree
        /// </summary>
        [XmlElement("author")]
        public HealthcareParticipant ResponsiblePerson { get; set; }

        /// <summary>
        /// Gets the pin
        /// </summary>
        [XmlElement("pin")]
        public string Pin { get; set; }
    }
}
