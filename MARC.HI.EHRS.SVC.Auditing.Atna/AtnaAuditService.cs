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
using MARC.HI.EHRS.SVC.Auditing.Atna.Configuration;
using System.Configuration;
using MARC.HI.EHRS.SVC.Core.Services;
using MARC.HI.EHRS.SVC.Auditing.Atna.Format;
using System.Threading;
using System.ComponentModel;
using System.Net;
using System.Diagnostics;
using MARC.Everest.Threading;

namespace MARC.HI.EHRS.SVC.Auditing.Atna
{
    /// <summary>
    /// Represents an audit service that communicates Audits via
    /// RFC3881 (ATNA style) audits
    /// </summary>
    [Description("RFC3881 Audit Service")]
    public class AtnaAuditService : IAuditorService, IDisposable
    {
    
        // Configuration
        protected AuditConfiguration m_configuration;

        // Wait 
        private WaitThreadPool m_waitThreadPool = new WaitThreadPool();

        /// <summary>
        /// Creates a new instance of the ATNA audit service
        /// </summary>
        public AtnaAuditService()
        {
            this.m_configuration = ConfigurationManager.GetSection("marc.hi.ehrs.svc.auditing.atna") as AuditConfiguration;
        }

        #region IAuditorService Members

        /// <summary>
        /// Queue the sending of an audit
        /// </summary>
        /// <param name="state"></param>
        private void SendAuditAsync(object state)
        {

            try
            {
                var ad = state as MARC.HI.EHRS.SVC.Core.DataTypes.AuditData;
                ISystemConfigurationService sysConfigSvc = this.Context.GetService(typeof(ISystemConfigurationService)) as ISystemConfigurationService;

                // Create the audit basic
                AuditMessage am = new AuditMessage(
                    ad.Timestamp, (ActionType)Enum.Parse(typeof(ActionType), ad.ActionCode.Value.ToString()),
                    (OutcomeIndicator)Enum.Parse(typeof(OutcomeIndicator), ad.Outcome.Value.ToString()),
                    (EventIdentifierType)Enum.Parse(typeof(EventIdentifierType), ad.EventIdentifier.ToString()),
                    null
                );
                if (ad.EventTypeCode != null)
                    am.EventIdentification.EventType.Add(new CodeValue<String>(ad.EventTypeCode.Code, ad.EventTypeCode.CodeSystem));

                am.SourceIdentification.Add(new AuditSourceIdentificationType()
                {
                    AuditEnterpriseSiteID = String.Format("{1}^^^&{0}&ISO", sysConfigSvc.DeviceIdentifier, sysConfigSvc.DeviceName),
                    AuditSourceID = Dns.GetHostName()
                });
                
                // Add additional data like the participant
                bool thisFound = false;
                string dnsName = Dns.GetHostName();
                foreach (var adActor in ad.Actors)
                {
                    thisFound |= (adActor.NetworkAccessPointId == Environment.MachineName || adActor.NetworkAccessPointId == dnsName) &&
                        adActor.NetworkAccessPointType == MARC.HI.EHRS.SVC.Core.DataTypes.NetworkAccessPointType.MachineName;
                    var act = new AuditActorData()
                    {
                        NetworkAccessPointId = adActor.NetworkAccessPointId,
                        NetworkAccessPointType = (NetworkAccessPointType)Enum.Parse(typeof(NetworkAccessPointType), adActor.NetworkAccessPointType.ToString()),
                        NetworkAccessPointTypeSpecified = adActor.NetworkAccessPointType != 0,
                        UserIdentifier = adActor.UserIdentifier,
                        UserIsRequestor = adActor.UserIsRequestor,
                        UserName = adActor.UserName
                    };
                    foreach (var rol in adActor.ActorRoleCode)
                        act.ActorRoleCode.Add(new CodeValue<string>(rol.Code, rol.CodeSystem)
                            {
                                DisplayName = rol.DisplayName
                            });
                    am.Actors.Add(act);
                }

                
                foreach (var aoPtctpt in ad.AuditableObjects)
                {
                    am.AuditableObjects.Add(new AuditableObject()
                    {
                        IDTypeCode = aoPtctpt.IDTypeCode.HasValue ?
                            aoPtctpt.IDTypeCode.Value != Core.DataTypes.AuditableObjectIdType.Custom ?
                                new CodeValue<AuditableObjectIdType>((AuditableObjectIdType)Enum.Parse(typeof(AuditableObjectIdType), aoPtctpt.IDTypeCode.ToString())) :
                                new CodeValue<AuditableObjectIdType>((AuditableObjectIdType)Enum.Parse(typeof(AuditableObjectIdType), aoPtctpt.CustomIdTypeCode.Code)) :
                            null,
                        LifecycleType = aoPtctpt.LifecycleType.HasValue ? (AuditableObjectLifecycle)Enum.Parse(typeof(AuditableObjectLifecycle), aoPtctpt.LifecycleType.ToString()) : 0,
                        LifecycleTypeSpecified = aoPtctpt.LifecycleType.HasValue,
                        ObjectId = aoPtctpt.ObjectId,
                        Role = (AuditableObjectRole)Enum.Parse(typeof(AuditableObjectRole), aoPtctpt.Role.ToString()),
                        RoleSpecified = aoPtctpt.Role != 0,
                        Type = (AuditableObjectType)Enum.Parse(typeof(AuditableObjectType), aoPtctpt.Type.ToString()),
                        TypeSpecified = aoPtctpt.Type != 0,
                        ObjectSpec = aoPtctpt.QueryData ?? aoPtctpt.NameData,
                        ObjectSpecChoice = aoPtctpt.QueryData == null ? ObjectDataChoiceType.ParticipantObjectName : ObjectDataChoiceType.ParticipantObjectQuery
                    });
                    // TODO: Object Data
                }

                // Was a record of this service found?
                if (!thisFound)
                    am.Actors.Add(new AuditActorData()
                    {
                        NetworkAccessPointId = Environment.MachineName,
                        NetworkAccessPointType = NetworkAccessPointType.MachineName,
                        UserIdentifier = String.Format("{1}^^^&{0}&ISO", sysConfigSvc.DeviceIdentifier, sysConfigSvc.DeviceName)
                    });


                // Send the message
                this.m_configuration.MessagePublisher.SendMessage(am);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
            }
        }

        /// <summary>
        /// Send an audit to the endpoint
        /// </summary>
        public bool SendAudit(MARC.HI.EHRS.SVC.Core.DataTypes.AuditData ad)
        {

            this.m_waitThreadPool.QueueUserWorkItem(SendAuditAsync, ad);
            return true;
        }

        #endregion

        #region IUsesHostContext Members

        /// <summary>
        /// Gets or sets the context of the ATNA message
        /// </summary>
        public IServiceProvider Context { get; set; }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            this.m_waitThreadPool.Dispose();
        }

        #endregion
    }
}
