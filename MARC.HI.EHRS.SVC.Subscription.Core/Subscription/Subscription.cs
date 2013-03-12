/**
 * Copyright 2012-2013 Mohawk College of Applied Arts and Technology
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you 
 * may not use this file except in compliance with the License. You may 
 * obtain a copy of the License at 
 * 
 * http://www.apache.org/licenses/LICENSE-2.0 
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations under 
 * the License.
 * 
 * User: fyfej
 * Date: 7-5-2012
 */

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
