/*
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
using MARC.HI.EHRS.SVC.Core.Services;
using System.Diagnostics;
using MARC.HI.EHRS.SVC.Auditing.Data;
using System.Runtime.InteropServices;
using System.ComponentModel;
using MARC.HI.EHRS.SVC.Auditing.Services;
using MARC.HI.EHRS.SVC.Core;

namespace MARC.HI.EHRS.SVC.Auditing
{
    /// <summary>
    /// Dummy Audit Service
    /// </summary>
    [Description("Dummy Auditor Service")]
    public class DummyAuditService : IAuditorService
    {


        #region IAuditorService Members

        /// <summary>
        /// Send an audit
        /// </summary>
        public bool SendAudit(AuditData ad)
        {

            StringBuilder auditSb = new StringBuilder();
            auditSb.AppendFormat("AUDIT {0} : {1} {2}\r\n", ad.ActionCode, ad.Outcome, ad.EventTypeCode == null ? "" : ad.EventTypeCode.Code);
            auditSb.AppendFormat("SITE: {0}\r\n", ApplicationContext.Current.Configuration.DeviceIdentifier);
            foreach (AuditableObject ao in ad.AuditableObjects ?? new List<AuditableObject>())
                auditSb.AppendFormat("AO-> ID({0}:{1}) (LC-{2}, ROL-{3})\r\n", ao.IDTypeCode, ao.ObjectId, ao.LifecycleType, ao.Role);
            foreach (AuditActorData aad in ad.Actors ?? new List<AuditActorData>())
                auditSb.AppendFormat("ACT-> {0} ID-{1} ISRQ-{2} NAP-{3}\r\n", aad.UserName, aad.UserIdentifier, aad.UserIsRequestor, aad.NetworkAccessPointId);

            
            Console.WriteLine(auditSb.ToString());
            return true;
        }

        #endregion

    }
}
