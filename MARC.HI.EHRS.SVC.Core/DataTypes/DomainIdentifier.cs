/**
 * Copyright 2012-2012 Mohawk College of Applied Arts and Technology
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
 * Date: 17-8-2012
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Core.DataTypes
{
    /// <summary>
    /// Identifies something within a particular domain
    /// </summary>
    [Serializable][XmlType("DomainIdentifier")]
    public class DomainIdentifier : Datatype
    {

        /// <summary>
        /// Gets or sets the domain to which the identifier belongs
        /// </summary>
        [XmlAttribute("domain")]
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets the identifier within the domain
        /// </summary>
        [XmlAttribute("uid")]
        public string Identifier { get; set; }

        /// <summary>
        /// private identifier (used for matching only)
        /// </summary>
        [XmlAttribute("priv")]
        public bool IsPrivate { get; set; }

        /// <summary>
        /// If true, the domain identifier is issue by a licensing authority
        /// </summary>
        [XmlAttribute("licenseAuthority")]
        public bool IsLicenseAuthority { get; set; }

        /// <summary>
        /// Assigning authority
        /// </summary>
        [XmlAttribute("assigningAuth")]
        public string AssigningAuthority { get; set; }

        /// <summary>
        /// Compare this object to antoher
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is DomainIdentifier)
            {
                DomainIdentifier dObj = obj as DomainIdentifier;
                return dObj.Domain == Domain && dObj.IsLicenseAuthority.Equals(IsLicenseAuthority)
                    && dObj.Identifier == Identifier;
            }
            else
                return base.Equals(obj);
        }
    }
}
