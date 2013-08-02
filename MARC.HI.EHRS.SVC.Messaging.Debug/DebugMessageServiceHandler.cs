using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.Services;
using System.ServiceModel.Web;
using System.Diagnostics;

namespace MARC.HI.EHRS.SVC.Messaging.Debug
{
    /// <summary>
    /// Message handler service
    /// </summary>
    public class DebugMessageServiceHandler : IMessageHandlerService
    {
        #region IMessageHandlerService Members

        // Web host
        private WebServiceHost m_webHost;

        /// <summary>
        /// Start the FHIR message handler
        /// </summary>
        public bool Start()
        {
            try
            {
                // Set the context
                ApplicationContext.CurrentContext = this.Context;

                this.m_webHost = new WebServiceHost(typeof(DebugServiceBehavior));
                
                // Start the web host
                this.m_webHost.Open();
                return true;
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                return false;
            }
            
        }

        /// <summary>
        /// Stop the FHIR message handler
        /// </summary>
        /// <returns></returns>
        public bool Stop()
        {
            if(this.m_webHost != null)
                this.m_webHost.Close();
            return true;
        }

        #endregion

        #region IUsesHostContext Members

        /// <summary>
        /// Context
        /// </summary>
        public IServiceProvider Context
        {
            get;
            set;
        }

        #endregion
    }
}
