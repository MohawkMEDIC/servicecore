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
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Core.DataTypes;

namespace MARC.HI.EHRS.SVC.Core.ComponentModel.Components
{

    /// <summary>
    /// Policy override reason
    /// </summary>
    [XmlType("PolicyOverrideReason", Namespace="urn:marc-hi:svc:componentModel")]
    public enum PolicyOverrideReason
    {
        Emergency,
        ProfessionalJudgement
    }

    /// <summary>
    /// Consent policy override!
    /// </summary>
    [Serializable]
    [XmlType("PolicyOverride", Namespace = "urn:marc-hi:svc:componentModel")]
    public class PolicyOverride : HealthServiceRecordContainer
    {

        /// <summary>
        /// Identifies the form where the policy override is active
        /// </summary>
        [XmlElement("formId")]
        public Identifier FormId { get; set; }

        /// <summary>
        /// Identifies the reason for the policy override
        /// </summary>
        [XmlAttribute("reason")]
        public PolicyOverrideReason Reason { get; set; }

        /// <summary>
        /// Identifies the time that the policy is effective for
        /// </summary>
        [XmlElement("effectiveTime")]
        public TimestampSet EffectiveTime { get; set; }

    }

}
