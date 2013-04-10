/**
 * Copyright 2013-2013 Mohawk College of Applied Arts and Technology
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
 * Date: 12-3-2013
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using MARC.HI.EHRS.SVC.Core.Services;
using System.Reflection;
using MARC.HI.EHRS.SVC.Core.DataTypes;
using System.ComponentModel;

namespace MARC.HI.EHRS.SVC.Core
{
    /// <summary>
    /// Utility for starting / stopping hosted services
    /// </summary>
    public static class ServiceUtil
    {


        // Msg handler service
        private static IMessageHandlerService s_messageHandlerService;
        // timer service
        private static ITimerService s_timerService;
        // Audit service
        private static IAuditorService s_auditorService;

        /// <summary>
        /// Helper function to start the services
        /// </summary>
        public static int Start(Guid activityId)
        {
            Trace.CorrelationManager.ActivityId = activityId;
            Trace.TraceInformation("Starting host context on Console Presentation System at {0}", DateTime.Now);

            // Detect platform
            if (System.Environment.OSVersion.Platform != PlatformID.Win32NT)
                Trace.TraceWarning("Not running on WindowsNT, some features may not function correctly");

            // Do this because loading stuff is tricky ;)
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

            try
            {
                // Initialize 
                HostContext context = new HostContext();

                Trace.TraceInformation("Getting default message handler service.");
                s_messageHandlerService = context.GetService(typeof(IMessageHandlerService)) as IMessageHandlerService;
                s_timerService = context.GetService(typeof(ITimerService)) as ITimerService;
                s_auditorService = context.GetService(typeof(IAuditorService)) as IAuditorService;

                if (s_messageHandlerService == null)
                {
                    Trace.TraceError("PANIC! Can't find a default message handler service: {0}", "No IMessageHandlerService classes are registered with this host context");
                    return 1911;
                }
                else
                {
                    Trace.TraceInformation("Starting message handler service {0}", s_messageHandlerService);
                    if (s_messageHandlerService.Start())
                    {
                        if (s_timerService != null)
                            s_timerService.Start();
                        // audit startup
                        if (s_auditorService != null)
                            s_auditorService.SendAudit(CreateApplicationStartAudit());
                        Trace.TraceInformation("Service Started Successfully");
                        return 0;
                    }
                    else
                    {
                        Trace.TraceError("No message handler service started. Terminating program");
                        Stop();
                        return 1911;
                    }
                }
            }
            catch (Exception e)
            {
                Trace.TraceError("Fatal exception occurred: {0}", e.ToString());
                Stop();
                return 1064;
            }
            finally
            {
            }
        }

        /// <summary>
        /// Stop the service
        /// </summary>
        public static void Stop()
        {
            if (s_messageHandlerService != null)
            {
                if (s_timerService != null)
                    s_timerService.Stop();
                // audit stop
                if (s_auditorService != null)
                    s_auditorService.SendAudit(CreateApplicationStopAudit());
                Trace.TraceInformation("Stopping message handler service {0}", s_messageHandlerService);
                s_messageHandlerService.Stop();

                (s_messageHandlerService.Context as IDisposable).Dispose();
            }
        }

        /// <summary>
        /// Assembly resolution
        /// </summary>
        internal static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
                if (args.Name == asm.FullName)
                    return asm;

            /// Try for an non-same number Version
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                string fAsmName = args.Name;
                if (fAsmName.Contains(","))
                    fAsmName = fAsmName.Substring(0, fAsmName.IndexOf(","));
                if (fAsmName == asm.GetName().Name)
                    return asm;
            }

            return null;
        }

        /// <summary>
        /// Create an application start audit
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Core.DataTypes.AuditData CreateApplicationStartAudit()
        {
            return new SVC.Core.DataTypes.AuditData(
                                DateTime.Now,
                                SVC.Core.DataTypes.ActionType.Execute,
                                SVC.Core.DataTypes.OutcomeIndicator.Success,
                                SVC.Core.DataTypes.EventIdentifierType.ApplicationActivity,
                                new SVC.Core.DataTypes.CodeValue("110120", "DCM") { DisplayName = "Application Start" }
                            )
            {
                Actors = new List<SVC.Core.DataTypes.AuditActorData>() {
                                    new MARC.HI.EHRS.SVC.Core.DataTypes.AuditActorData() {
                                        UserIdentifier = Environment.UserName,
                                        UserIsRequestor = false,
                                        ActorRoleCode = new List<SVC.Core.DataTypes.CodeValue>() { 
                                            new CodeValue("110150","DCM") { DisplayName = "Application" }
                                        }
                                    }
                                }
            };
        }

        /// <summary>
        /// Create an application stop audit
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Core.DataTypes.AuditData CreateApplicationStopAudit()
        {
            return new SVC.Core.DataTypes.AuditData(
                                DateTime.Now,
                                SVC.Core.DataTypes.ActionType.Execute,
                                SVC.Core.DataTypes.OutcomeIndicator.Success,
                                SVC.Core.DataTypes.EventIdentifierType.ApplicationActivity,
                                new SVC.Core.DataTypes.CodeValue("110121", "DCM") { DisplayName = "Application Stop" }
                            )
            {
                Actors = new List<SVC.Core.DataTypes.AuditActorData>() {
                                    new MARC.HI.EHRS.SVC.Core.DataTypes.AuditActorData() {
                                        UserIdentifier = Environment.UserName,
                                        UserIsRequestor = false,
                                        ActorRoleCode = new List<SVC.Core.DataTypes.CodeValue>() { 
                                            new CodeValue("110150","DCM") { DisplayName = "Application" }
                                        }
                                    }
                                }
            };
        }
    }
}
