using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.Services;
using MARC.HI.EHRS.SVC.Messaging.Multi.Configuration;
using System.Configuration;

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
        public Core.HostContext Context
        {
            get;
            set;
        }

        #endregion
    }
}
