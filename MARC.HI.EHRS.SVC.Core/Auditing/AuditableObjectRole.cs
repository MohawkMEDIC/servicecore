/* 
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

namespace MARC.HI.EHRS.SVC.Core.DataTypes
{
    /// <summary>
    /// Identifies roles of objects in the audit event
    /// </summary>
    public enum AuditableObjectRole
    {
        /// <remarks>Use with object type Person</remarks>
        Patient = 0x01,
        /// <remarks>Use with object type Organization</remarks>
        Location = 0x02,
        /// <remarks>Use with object type SysObject</remarks>
        Report = 0x03,
        /// <remarks>Use with object type Person or Organization</remarks>
        Resource = 0x04,
        /// <remarks>Use with object type SysObject</remarks>
        MasterFile = 0x05,
        /// <remarks>Use with object type Person, SysObject</remarks>
        User = 0x06,
        /// <remarks>Use with object type SysObject</remarks>
        List = 0x07,
        /// <remarks>Use with object type Person</remarks>
        Doctor = 0x08,
        /// <remarks>Use with object type Organization</remarks>
        Subscriber = 0x09,
        /// <remarks>Use with object type Person, Organization</remarks>
        Guarantor = 0x0a,
        /// <remarks>Use with object type SyOBject</remarks>
        SecurityUser = 0x0b,
        /// <remarks>Use with object type SysObject</remarks>
        SecurityGroup = 0x0c,
        /// <remarks>Use with object type SysObject</remarks>
        SecurityResource = 0x0d,
        /// <remarks>Use with object type SysObject</remarks>
        SecurityGranularityDefinition = 0x0e,
        /// <remarks>Use with object type Person or Organization</remarks>
        Provider = 0x0f,
        /// <remarks>Use with object type SysObject</remarks>
        DataDestination = 0x10,
        /// <remarks>Use with object type SysObject</remarks>
        DataRepository = 0x11,
        /// <remarks>Use with object type SysObject</remarks>
        Schedule = 0x12,
        /// <remarks>Use with object type Person</remarks>
        Customer = 0x13,
        /// <remarks>Use with object type SysObject</remarks>
        Job = 0x14,
        /// <remarks>Use with object type SysObject</remarks>
        JobStream = 0x15,
        /// <remarks>Use with object type SysObject</remarks>
        Table = 0x16,
        /// <remarks>Use with object type SysObject</remarks>
        RoutingCriteria = 0x17,
        /// <remarks>Use with object type SysObject</remarks>
        Query = 0x18
    }
}
