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
    /// Update mode of the data
    /// </summary>
    public enum UpdateModeType
    {
        /// <summary>
        /// Add the data no matter what
        /// </summary>
        Add,
        /// <summary>
        /// Add the data or update if it is already present
        /// </summary>
        AddOrUpdate,
        /// <summary>
        /// Perform an update only
        /// </summary>
        Update,
        /// <summary>
        /// Remove the data
        /// </summary>
        Remove,
        /// <summary>
        /// Ignore the data serves as a key only
        /// </summary>
        Ignore
    }

    /// <summary>
    /// Represents the base of all data types
    /// </summary>
    [Serializable][XmlType("Datatype")]
    public abstract class Datatype
    {

        /// <summary>
        /// Represents the key for the items
        /// </summary>
        [XmlAttribute("key")]
        public decimal Key { get; set; }

        /// <summary>
        /// Update mode of the data
        /// </summary>
        [XmlAttribute("updateMode")]
        public UpdateModeType UpdateMode { get; set; }

    }
}
