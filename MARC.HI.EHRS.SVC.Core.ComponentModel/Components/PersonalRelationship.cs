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
 * Date: 1-8-2012
 */

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
    [Serializable]
    [XmlType("PersonalRelationship", Namespace = "urn:marc-hi:svc:componentModel")]
    public class PersonalRelationship : Client
    {
        /// <summary>
        /// Identifies the kind of relationship
        /// </summary>
        [XmlAttribute("kind")]
        public string RelationshipKind { get; set; }

        /// <summary>
        /// Gets or sets the status
        /// </summary>
        [XmlAttribute("status")]
        public StatusType Status { get; set; }
    }
}
