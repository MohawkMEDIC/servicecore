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

namespace MARC.HI.EHRS.SVC.Messaging.Multi
{
    /// <summary>
    /// A message handler that starts and enables multiple message handlers
    /// </summary>
    public class MultiMessageHandler : IMessageHandlerService
    {

        // Configuration section handler
        private static ConfigurationSectionHandler s_configuration;

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

            // Start each of the dependent services
            foreach (var svc in s_configuration.MessageHandlers)
            {
                svc.Context = this.Context;
                Trace.TraceInformation("MMH: Starting message handler service {0}", svc);
                success &= svc.Start();
            }
            return success;
        }

        /// <summary>
        /// Stop the multiple message handler
        /// </summary>
        public bool Stop()
        {
            bool success = true;

            // Stop each of the dependent services
            foreach (var svc in s_configuration.MessageHandlers)
                success &= svc.Stop();
            return success;
        }

        #endregion

        #region IUsesHostContext Members

        /// <summary>
        /// Gets or sets the context
        /// </summary>
        public IServiceProvider Context
        {
            get;
            set;
        }

        #endregion
    }
}
