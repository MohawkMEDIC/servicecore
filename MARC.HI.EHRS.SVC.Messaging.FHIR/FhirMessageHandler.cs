using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.Services;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Configuration;
using System.Configuration;
using System.ServiceModel.Web;
using MARC.HI.EHRS.SVC.Messaging.FHIR.WcfCore;
using System.Diagnostics;
using System.ServiceModel;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Util;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR
{
    /// <summary>
    /// Message handler for FHIR
    /// </summary>
    public class FhirMessageHandler : IMessageHandlerService
    {

        #region IMessageHandlerService Members

        // Configuration
        private FhirServiceConfiguration m_configuration;

        // Web host
        private WebServiceHost m_webHost;

        /// <summary>
        /// Constructor, load configuration
        /// </summary>
        public FhirMessageHandler()
        {
            this.m_configuration = ConfigurationManager.GetSection("marc.hi.ehrs.svc.messaging.fhir") as FhirServiceConfiguration;
        }

        /// <summary>
        /// Start the FHIR message handler
        /// </summary>
        public bool Start()
        {
            try
            {
                // Set the context
                ApplicationContext.CurrentContext = this.Context;


                this.m_webHost = new WebServiceHost(typeof(FhirServiceBehavior));
                this.m_webHost.Description.ConfigurationName = this.m_configuration.WcfEndpoint;
                (this.m_webHost.Description.Endpoints[0].Binding as WebHttpBinding).ContentTypeMapper = new FhirContentTypeHandler();
                this.m_webHost.Description.Endpoints[0].Behaviors.Add(new FhirRestEndpointBehavior());

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
        /// Gets or sets the hosting context
        /// </summary>
        public IServiceProvider Context
        {
            get;
            set;
        }

        #endregion
    }
}
