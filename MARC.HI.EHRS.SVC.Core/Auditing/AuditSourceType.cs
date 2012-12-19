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
 * Date: 7-5-2012
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MARC.HI.EHRS.SVC.Core.DataTypes
{
    /// <summary>
    /// Audit source type
    /// </summary>
    public enum AuditSourceType
    {
        EndUserInterface = 1,
        DeviceOrInstrument = 2,
        WebServerProcess = 3,
        ApplicationServerProcess = 4,
        DatabaseServerProcess = 5,
        SecurityServerProcess = 6,
        ISOLevel1or3Component = 7,
        ISOLevel4or6Software = 8,
        Other = 9
    }
}
