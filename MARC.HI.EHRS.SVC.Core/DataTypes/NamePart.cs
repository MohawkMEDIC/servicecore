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
 * Date: 13-8-2012
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Core.DataTypes
{
    /// <summary>
    /// Identifies a single component of a more complex name
    /// </summary>
    [Serializable][XmlType("NamePart")]
    public class NamePart
    {
        /// <summary>
        /// Identifies the role that the name part plays
        /// </summary>
        public enum NamePartType
        {
            None = 0x0,
            Given = 0x01,
            Prefix = 0x02,
            Delimeter = 0x03,
            Suffix = 0x04,
            Family = 0x05
        }

        /// <summary>
        /// Gets or sets the type of name component represnted
        /// </summary>
        [XmlAttribute("type")]
        public NamePartType Type { get; set; }

        /// <summary>
        /// Gets or sets the value of the name part
        /// </summary>
        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}
