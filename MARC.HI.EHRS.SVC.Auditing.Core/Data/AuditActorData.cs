
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
* Date: 7-8-2012
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Auditing.Data
{
    /// <summary>
    /// Data related to actors that participate in the event
    /// </summary>
    [XmlType(nameof(AuditActorData), Namespace = "http://marc-hi.ca/svc/audit")]
    public class AuditActorData
    {
        /// <summary>
        /// The unique identifier for the user in the system
        /// </summary>
        [XmlAttribute("uid")]
        public string UserIdentifier { get; set; }
        /// <summary>
        /// The name of the user in the system
        /// </summary>
        [XmlAttribute("uname")]
        public string UserName { get; set; }
        /// <summary>
        /// True if the user is the primary requestor
        /// </summary>
        [XmlAttribute("isReq")]
        public bool UserIsRequestor { get; set; }
        /// <summary>
        /// Identifies the network access point from which the user accessed the system
        /// </summary>
        [XmlAttribute("apId")]
        public string NetworkAccessPointId { get; set; }
        /// <summary>
        /// Identifies the type of network access point
        /// </summary>
        [XmlAttribute("apType")]
        public NetworkAccessPointType NetworkAccessPointType { get; set; }
        /// <summary>
        /// Identifies the role(s) that the actor has played
        /// </summary>
        [XmlElement("role")]
        public List<AuditCode> ActorRoleCode { get; set; }
        /// <summary>
        /// Default ctor
        /// </summary>
        public AuditActorData() { ActorRoleCode = new List<AuditCode>(); }

        /// <summary>
        /// Alternative user identifier
        /// </summary>
        [XmlAttribute("altUid")]
        public string AlternativeUserId { get; set; }
    }
}
