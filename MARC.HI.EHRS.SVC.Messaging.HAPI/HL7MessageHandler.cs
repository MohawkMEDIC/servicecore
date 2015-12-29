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
 * Date: 13-8-2012
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.Services;
using System.Threading;
using MARC.HI.EHRS.SVC.Messaging.HAPI.Configuration;
using System.Configuration;
using System.Diagnostics;

namespace MARC.HI.EHRS.SVC.Messaging.HAPI
{
    /// <summary>
    /// Message handler service
    /// </summary>
    public class HL7MessageHandler : IMessageHandlerService
    {
        #region IMessageHandlerService Members

        // Configuration 
        private HL7ConfigurationSection m_configuration;

        // Threads that are listening for messages
        private List<Thread> m_listenerThreads = new List<Thread>();

        /// <summary>
        /// Load configuration
        /// </summary>
        public HL7MessageHandler()
        {
            this.m_configuration = ConfigurationManager.GetSection("MARC.HI.EHRS.SVC.Messaging.HAPI") as HL7ConfigurationSection;
        }

        /// <summary>
        /// Start the v2 message handler
        /// </summary>
        public bool Start()
        {
            this.Starting?.Invoke(this, EventArgs.Empty);

            foreach (var sd in this.m_configuration.Services)
            {

                var sh = new ServiceHandler(sd);
                Thread thdSh = new Thread(sh.Run);
                thdSh.IsBackground = true;
                this.m_listenerThreads.Add(thdSh);
                Trace.TraceInformation("Starting HL7 Service '{0}'...", sd.Name);
                thdSh.Start();
            }

            this.Started?.Invoke(this, EventArgs.Empty);

            return true;
        }

        /// <summary>
        /// Stop the v2 message handler
        /// </summary>
        public bool Stop()
        {
            this.Stopping?.Invoke(this, EventArgs.Empty);
            foreach (var thd in this.m_listenerThreads)
                if (thd.IsAlive)
                    thd.Abort();
            this.Stopped?.Invoke(this, EventArgs.Empty);
            return true;
        }

        #endregion

        // Host context
        private IServiceProvider m_context;

        /// <summary>
        /// Fired when the service is starting
        /// </summary>
        public event EventHandler Starting;
        /// <summary>
        /// Fired when the service is stopping
        /// </summary>
        public event EventHandler Stopping;
        /// <summary>
        /// Fired when the service has stopped
        /// </summary>
        public event EventHandler Started;
        /// <summary>
        /// Fired when the service has stopped
        /// </summary>
        public event EventHandler Stopped;

        /// <summary>
        /// Returns true with the service is running
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return this.m_listenerThreads?.Count > 0;
            }
        }

    }
}
