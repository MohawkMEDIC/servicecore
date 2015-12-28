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
 * Date: 16-7-2012
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.Services;
using MARC.HI.EHRS.SVC.Messaging.Multi.Configuration;
using System.Configuration;
using System.Diagnostics;
using System.ComponentModel;

namespace MARC.HI.EHRS.SVC.Messaging.Multi
{
    /// <summary>
    /// A message handler that starts and enables multiple message handlers
    /// </summary>
    [Description("Multi-Interface Message Handler")]
    public class MultiMessageHandler : IMessageHandlerService
    {

        // Configuration section handler
        private static ConfigurationSectionHandler s_configuration;

        /// <summary>
        /// Fired when the multi-service is starting
        /// </summary>
        public event EventHandler Starting;
        /// <summary>
        /// Fired when the multi-service is stopping
        /// </summary>
        public event EventHandler Stopping;
        /// <summary>
        /// Fired when the multi-service is starting
        /// </summary>
        public event EventHandler Started;
        /// <summary>
        /// Fired when the multi-service is stopping
        /// </summary>
        public event EventHandler Stopped;

        /// <summary>
        /// Static constructor for the multi-message handler
        /// </summary>
        static MultiMessageHandler()
        {
            s_configuration = ConfigurationManager.GetSection("marc.hi.ehrs.svc.messaging.multi") as ConfigurationSectionHandler;
        }

        #region IMessageHandlerService Members

        /// <summary>
        /// Start the multiple message handler
        /// </summary>
        public bool Start()
        {
            bool success = true;
            this.Starting?.Invoke(this, EventArgs.Empty);

            // Start each of the dependent services
            foreach (var svc in s_configuration.MessageHandlers)
            {
                Trace.TraceInformation("MMH: Starting message handler service {0}", svc);
                success &= svc.Start();
            }


            if(success)
                this.Started?.Invoke(this, EventArgs.Empty);
            this.IsRunning = success;

            return success;
        }

        /// <summary>
        /// Stop the multiple message handler
        /// </summary>
        public bool Stop()
        {
            bool success = true;
            this.Stopping?.Invoke(this, EventArgs.Empty);

            // Stop each of the dependent services
            foreach (var svc in s_configuration.MessageHandlers)
                success &= svc.Stop();

            // Success?
            if (success)
            {
                this.Stopped?.Invoke(this, EventArgs.Empty);
                this.IsRunning = false;
            }

            return success;
        }

        #endregion

        /// <summary>
        /// Running?
        /// </summary>
        public bool IsRunning
        {
            get;private set;
        }
    }
}
