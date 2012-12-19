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
 * Date: 19-7-2012
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Core.DataTypes
{
    /// <summary>
    /// Represents codified data
    /// </summary>
    [Serializable][XmlType("CodeValue")]
    public class CodeValue : Datatype
    {
        /// <summary>
        /// The OID of the code system from which the code is pulled
        /// </summary>
        [XmlAttribute("codeSys")]
        public string CodeSystem { get; set; }
        /// <summary>
        /// The codified data
        /// </summary>
        [XmlAttribute("code")]
        public string Code { get; set; }
        /// <summary>
        /// The english display name for the code
        /// </summary>
        [XmlAttribute("display")]
        public string DisplayName { get; set; }
        /// <summary>
        /// Identifies the version of the code system from which the code is derived
        /// </summary>
        [XmlAttribute("version")]
        public string CodeSystemVersion { get; set; }
        /// <summary>
        /// Identifies the code or concept in the original system 
        /// </summary>
        [XmlElement("originalText")]
        public string OriginalText { get; set; }
        /// <summary>
        /// Gets or sets the name of the code system
        /// </summary>
        [XmlElement("codeSystemName")]
        public string CodeSystemName { get; set; }
        /// <summary>
        /// Identifies the qualifiers for the code
        /// </summary>
        [XmlIgnore]
        public Dictionary<CodeValue, CodeValue> Qualifies { get; set; }

        /// <summary>
        /// Default ctor
        /// </summary>
        public CodeValue() { }

        /// <summary>
        /// Create a new instance of the audit code with the specified parameters
        /// </summary>
        /// <param name="code">The code</param>
        public CodeValue(string code)
            : this()
        { this.Code = code; }

        /// <summary>
        /// Create a new instance of the audit code with the specified parameters
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="codeSystem">The OID of the system from wich the code was drawn</param>
        public CodeValue(string code, string codeSystem)
            : this(code)
        { this.CodeSystem = codeSystem; }

    }
}
