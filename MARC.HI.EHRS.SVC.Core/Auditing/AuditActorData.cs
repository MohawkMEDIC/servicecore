/* 
 * Copyright 2008-2011 Mohawk College of Applied Arts and Technology
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
 * User: Justin Fyfe
 * Date: 08-24-2011
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MARC.HI.EHRS.SVC.Core.DataTypes
{
    /// <summary>
    /// Data related to actors that participate in the event
    /// </summary>
    public class AuditActorData
    {
        /// <summary>
        /// The unique identifier for the user in the system
        /// </summary>
        public string UserIdentifier { get; set; }
        /// <summary>
        /// The name of the user in the system
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// True if the user is the primary requestor
        /// </summary>
        public bool UserIsRequestor { get; set; }
        /// <summary>
        /// Identifies the network access point from which the user accessed the system
        /// </summary>
        public string NetworkAccessPointId { get; set; }
        /// <summary>
        /// Identifies the type of network access point
        /// </summary>
        public NetworkAccessPointType NetworkAccessPointType { get; set; }
        /// <summary>
        /// Identifies the role(s) that the actor has played
        /// </summary>
        public List<String> ActorRoleCode { get; set; }
        /// <summary>
        /// Default ctor
        /// </summary>
        public AuditActorData() { ActorRoleCode = new List<string>(); }
    }
}
