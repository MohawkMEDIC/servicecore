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
