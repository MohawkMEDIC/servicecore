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
using System.Linq;
using System.Text;

namespace MARC.HI.EHRS.SVC.Core.DataTypes
{
    /// <summary>
    /// Identifies an object that adds context to the audit
    /// </summary>
    public class AuditableObject
    {
        /// <summary>
        /// New object data
        /// </summary>
        public AuditableObject()
        {
            this.ObjectData = new Dictionary<string, byte[]>();
        }
        /// <summary>
        /// Identifies the object in the event
        /// </summary>
        public string ObjectId { get; set; }
        /// <summary>
        /// Identifies the type of object being expressed
        /// </summary>
        public AuditableObjectType? Type { get; set; }
        /// <summary>
        /// Identifies the role type of the object
        /// </summary>
        public AuditableObjectRole? Role { get; set; }
        /// <summary>
        /// Identifies where in the lifecycle of the object this object is currently within
        /// </summary>
        public AuditableObjectLifecycle? LifecycleType { get; set; }
        /// <summary>
        /// Identifies the type of identifier supplied
        /// </summary>
        public AuditableObjectIdType? IDTypeCode { get; set; }
        /// <summary>
        /// Custom id type code
        /// </summary>
        public CodeValue CustomIdTypeCode { get; set; }
        /// <summary>
        /// Data associated with the object
        /// </summary>
        public string QueryData { get; set; }
        /// <summary>
        /// Data associated with the object
        /// </summary>
        public string NameData { get; set; }

        /// <summary>
        /// Additional object data
        /// </summary>
        public Dictionary<String, byte[]> ObjectData { get; set; }
    }
}
