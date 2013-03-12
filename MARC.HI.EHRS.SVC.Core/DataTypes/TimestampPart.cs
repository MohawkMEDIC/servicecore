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
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Core.DataTypes
{
    /// <summary>
    /// Represents a component of a timestamp set
    /// </summary>
    [Serializable][XmlType("TimestampPart")]
    public class TimestampPart : Datatype, ICloneable
    {
        /// <summary>
        /// Identifies the timestamp part type
        /// </summary>
        public enum TimestampPartType
        {
            LowBound,
            HighBound,
            Width,
            Standlone
        }

        /// <summary>
        /// Create a new timestamp part
        /// </summary>
        public TimestampPart()
        {}

        /// <summary>
        /// Create a new instance of the timestamp part type
        /// </summary>
        public TimestampPart(TimestampPartType partType, DateTime value, string precision)
        {
            this.PartType = partType;
            this.Value = value;
            this.Precision = precision;
        }

        /// <summary>
        /// Gets or sets the part type of the timestamp part
        /// </summary>
        [XmlAttribute("type")]
        public TimestampPartType PartType { get; set; }

        /// <summary>
        /// Represents the value of the timestamp part
        /// </summary>
        [XmlElement("value")]
        public DateTime Value { get; set; }

        /// <summary>
        /// Identifies the precision of the timestamp
        /// </summary>
        [XmlAttribute("precision")]
        public string Precision { get; set; }

        /// <summary>
        /// Clone the item
        /// </summary>
        public Object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
