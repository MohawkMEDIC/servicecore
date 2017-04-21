



using Newtonsoft.Json;
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
* Date: 23-8-2012
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Auditing.Data
{
    /// <summary>
    /// Identifies an object that adds context to the audit
    /// </summary>
    [XmlType(nameof(AuditableObject), Namespace = "http://marc-hi.ca/svc/audit")]
    [JsonObject(nameof(AuditableObject))]
    public class AuditableObject
    {

        /// <summary>
        /// New object data
        /// </summary>
        public AuditableObject()
        {
            this.ObjectData = new List<ObjectDataExtension>();
        }
        /// <summary>
        /// Identifies the object in the event
        /// </summary>
        [XmlElement("id"), JsonProperty("id")]
        public string ObjectId { get; set; }

        /// <summary>
        /// Identifies the type of object being expressed
        /// </summary>
        [XmlElement("type"), JsonProperty("type")]
        public AuditableObjectType Type { get; set; }

        /// <summary>
        /// Identifies the role type of the object
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("role"), JsonProperty("role")]
        public AuditableObjectRole RoleXml { get { return this.Role.GetValueOrDefault(); } set { this.Role = value; } }


        /// <summary>
        /// Gets whether ID type code is specified
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public bool RoleXmlSpecified { get { return this.Role.HasValue; } }

        /// <summary>
        /// Identifies the role type of the object
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public AuditableObjectRole? Role { get; set; }

        /// <summary>
        /// Identifies where in the lifecycle of the object this object is currently within
        /// </summary>
        [XmlElement("lifecycle"), JsonProperty("lifecycle")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public AuditableObjectLifecycle LifecycleTypeXml { get { return this.LifecycleType.GetValueOrDefault(); } set { this.LifecycleType = value; } }

        /// <summary>
        /// Gets whether ID type code is specified
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public bool LifecycleTypeXmlSpecified { get { return this.LifecycleType.HasValue; } }

        /// <summary>
        /// Lifecycle type
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public AuditableObjectLifecycle? LifecycleType { get; set; }

        /// <summary>
        /// Identifies the type of identifier supplied
        /// </summary>
        [XmlElement("idType"), JsonProperty("idType")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public AuditableObjectIdType IDTypeCodeXml { get { return this.IDTypeCode.GetValueOrDefault(); } set { this.IDTypeCode = value; } }

        /// <summary>
        /// Gets whether ID type code is specified
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public bool IDTypeCodeXmlSpecified { get { return this.IDTypeCode.HasValue; } }

        /// <summary>
        /// Gets or sets the id type code
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public AuditableObjectIdType? IDTypeCode { get; set; }
        
        /// <summary>
        /// Custom id type code
        /// </summary>
        [XmlElement("customCode"), JsonProperty("customCode")]
        public AuditCode CustomIdTypeCode { get; set; }
        
        /// <summary>
        /// Data associated with the object
        /// </summary>
        [XmlElement("queryData"), JsonProperty("queryData")]
        public string QueryData { get; set; }
        
        /// <summary>
        /// Data associated with the object
        /// </summary>
        [XmlElement("name"), JsonProperty("name")]
        public string NameData { get; set; }

        /// <summary>
        /// Additional object data
        /// </summary>
        [XmlElement("dictionary"), JsonProperty("dictionary")]
        public List<ObjectDataExtension> ObjectData { get; set; }
        
    }

    /// <summary>
    /// Represents object data extension
    /// </summary>
    [XmlType(nameof(ObjectDataExtension), Namespace = "http://marc-hi.ca/svc/audit")]
    public class ObjectDataExtension
    {

        public ObjectDataExtension()
        {

        }

        /// <summary>
        /// Object data extension
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public ObjectDataExtension(String key, byte[] value)
        {

        }
        /// <summary>
        /// Key of the extension
        /// </summary>
        [XmlAttribute("key"), JsonProperty("key")]
        public String Key { get; set; }

        /// <summary>
        /// Value of the extension
        /// </summary>
        [XmlAttribute("value"), JsonProperty("value")]
        public Byte[] Value { get; set; }

    }
}
