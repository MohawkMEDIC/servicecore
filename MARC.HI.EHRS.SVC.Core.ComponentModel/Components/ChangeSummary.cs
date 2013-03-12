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
 * Date: 7-5-2012
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.DataTypes;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Core.ComponentModel.Components
{
    /// <summary>
    /// Change summary component
    /// </summary>
    [Serializable][XmlType("ChangeSummary")]
    public class ChangeSummary : HealthServiceRecordContainer
    {

        /// <summary>
        /// Alternate identifier for the versioned domain identifier
        /// </summary>
        [XmlElement("altId")]
        public VersionedDomainIdentifier AlternateIdentifier { get; set; }

        /// <summary>
        /// Language code
        /// </summary>
        [XmlAttribute("lang")]
        public string LanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the version identifier
        /// </summary>
        [XmlAttribute("version")]
        public decimal VersionIdentifier { get; set; }

        /// <summary>
        /// Identifies the type of change
        /// </summary>
        [XmlElement("changeType")]
        public CodeValue ChangeType { get; set; }

        /// <summary>
        /// Gets or sets the status
        /// </summary>
        [XmlAttribute("status")]
        public StatusType Status { get; set; }

        /// <summary>
        /// Gets or sets the effective time
        /// </summary>
        [XmlElement("effectiveTime")]
        public TimestampSet EffectiveTime { get; set; }
    }
}
