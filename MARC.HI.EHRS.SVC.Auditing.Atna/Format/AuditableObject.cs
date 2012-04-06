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
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Auditing.Atna.Format
{
    /// <summary>
    /// Identifies an object that adds context to the audit
    /// </summary>
    public class AuditableObject
    {
        /// <summary>
        /// Identifies the object in the event
        /// </summary>
        [XmlAttribute("ParticipantObjectID")]
        public string ObjectId { get; set; }
        /// <summary>
        /// Identifies the type of object being expressed
        /// </summary>
        [XmlAttribute("ParticipantObjectTypeCode")]
        public AuditableObjectType Type { get; set; }
        [XmlIgnore]
        public bool TypeSpecified { get; set; }
        /// <summary>
        /// Identifies the role type of the object
        /// </summary>
        [XmlAttribute("ParticipantObjectTypeCodeRole")]
        public AuditableObjectRole Role { get; set; }
        [XmlIgnore]
        public bool RoleSpecified { get; set; }
        /// <summary>
        /// Identifies where in the lifecycle of the object this object is currently within
        /// </summary>
        [XmlAttribute("ParticipantObjectDataLifeCycle")]
        public AuditableObjectLifecycle LifecycleType { get; set; }
        [XmlIgnore]
        public bool LifecycleTypeSpecified { get; set; }
        /// <summary>
        /// Identifies the type of identifier supplied
        /// </summary>
        [XmlElement("ParticipantObjectIDTypeCode")]
        public CodeValue<AuditableObjectIdType> IDTypeCode { get; set; }
    }
}
