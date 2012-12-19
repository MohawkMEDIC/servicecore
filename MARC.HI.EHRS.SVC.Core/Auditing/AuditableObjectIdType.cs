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
 * Date: 23-8-2012
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MARC.HI.EHRS.SVC.Core.DataTypes
{
    /// <summary>
    /// Classifies the type of identifier that a auditable object may have
    /// </summary>
    public enum AuditableObjectIdType
    {
        /// <remarks>Use with object type code Person</remarks>
        MedicalRecord = 0x01,
        /// <remarks>Use with object type code Person</remarks>
        PatientNumber = 0x02,
        /// <remarks>Use with object type code Person</remarks>
        EncounterNumber = 0x03,
        /// <remarks>Use with object type code Person</remarks>
        EnrolleeNumber = 0x04,
        /// <remarks>Use with object type code Person</remarks>
        SocialSecurityNumber = 0x05,
        /// <remarks>Use with object type code Person</remarks>
        AccountNumber = 0x06,
        /// <remarks>Use with object type code Person or Organization</remarks>
        GuarantorNumber = 0x07,
        /// <remarks>Use with object type code SystemObject</remarks>
        ReportName = 0x08,
        /// <remarks>Use with object type code SystemObject</remarks>
        ReportNumber = 0x09,
        /// <remarks>Use with object type code SystemObject</remarks>
        SearchCritereon = 0x0a,
        /// <remarks>Use with object type code Person or SystemObject</remarks>
        UserIdentifier = 0x0b,
        /// <remarks>Use with object type code SystemObject</remarks>
        Uri = 0x0c,
        /// <summary>
        /// Custom code
        /// </summary>
        Custom = 0x0d
    }
}
