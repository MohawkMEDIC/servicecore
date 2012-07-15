﻿/* 
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

namespace MARC.HI.EHRS.SVC.Core.DataTypes
{
    /// <summary>
    /// Identifies an address within  a telecommunications context
    /// </summary>
    [Serializable][XmlType("TelecomAddress")]
    public class TelecommunicationsAddress
    {
        /// <summary>
        /// Identifies the use of the telecom address
        /// </summary>
        [XmlAttribute("use")]
        public string Use { get; set; }
        /// <summary>
        /// Identifies the value of the telecom address
        /// </summary>
        [XmlAttribute("value")]
        public string Value { get; set; }
        /// <summary>
        /// Usable period
        /// </summary>
        [XmlElement("usable")]
        public TimestampSet UsablePeriod { get; set; }
    }
}
