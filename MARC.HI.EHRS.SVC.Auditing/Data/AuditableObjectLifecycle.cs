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

namespace MARC.HI.EHRS.SVC.Auditing.Data
{
    /// <summary>
    /// Auditable object lifecycle
    /// </summary>
    public enum AuditableObjectLifecycle
    {
        Creation = 0x01,
        Import = 0x02,
        Amendment = 0x03,
        Verification = 0x04,
        Translation = 0x05,
        Access = 0x06,
        Deidentification = 0x07,
        Aggregation = 0x08,
        Report = 0x09,
        Export = 0x0a,
        Disclosure = 0x0b,
        ReceiptOfDisclosure = 0x0c,
        Archiving = 0x0d,
        LogicalDeletion = 0x0e,
        PermanentErasure = 0x0f
    }
}
