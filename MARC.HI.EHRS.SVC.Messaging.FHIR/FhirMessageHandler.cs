using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.Services;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Configuration;
using System.Configuration;
using System.ServiceModel.Web;
using System.Diagnostics;
using System.ServiceModel;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Util;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Handlers;
using System.Reflection;
using MARC.HI.EHRS.SVC.Core;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Wcf;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Wcf.Serialization;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Wcf.Behavior;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR
{
    /// <summary>
    /// Message handler for FHIR
    /// </summary>
    public class FhirMessageHandler : IMessageHandlerService
    {

        #region IMessageHandlerService Members

        private TraceSource m_traceSource = new TraceSource("MARC.HI.EHRS.SVC.Messaging.FHIR");

        // Configuration
        private FhirServiceConfiguration m_configuration;

        // Web host
        private WebServiceHost m_webHost;

        /// <summary>
        /// Fired when the FHIR message handler is starting
        /// </summary>
        public event EventHandler Starting;
        /// <summary>
        /// Fired when the FHIR message handler is stopping
        /// </summary>
        public event EventHandler Stopping;
        /// <summary>
        /// Fired when the FHIR message handler has started 
        /// </summary>
        public event EventHandler Started;
        /// <summary>
        /// Fired when the FHIR message handler has stopped
        /// </summary>
        public event EventHandler Stopped;

        /// <summary>
        /// Constructor, load configuration
        /// </summary>
        public FhirMessageHandler()
        {
            this.m_configuration = ApplicationContext.Current.GetService<IConfigurationManager>().GetSection("marc.hi.ehrs.svc.messaging.fhir") as FhirServiceConfiguration;
        }

        /// <summary>
        /// Start the FHIR message handler
        /// </summary>
        public bool Start()
        {
            try
            {

                this.Starting?.Invoke(this, EventArgs.Empty);

                this.m_webHost = new WebServiceHost(typeof(FhirServiceBehavior));
                this.m_webHost.Description.ConfigurationName = this.m_configuration.WcfEndpoint;

                foreach (var endpoint in this.m_webHost.Description.Endpoints)
                {
                    this.m_traceSource.TraceInformation("Starting FHIR on {0}...", endpoint.Address);

                    (endpoint.Binding as WebHttpBinding).ContentTypeMapper = new FhirContentTypeHandler();
                    endpoint.EndpointBehaviors.Add(new FhirRestEndpointBehavior());
                    endpoint.EndpointBehaviors.Add(new FhirErrorEndpointBehavior());

                }

                // Configuration 
                foreach (Type t in this.m_configuration.ResourceHandlers)
                {
                    ConstructorInfo ci = t.GetConstructor(Type.EmptyTypes);
                    if (ci == null)
                    {
                        this.m_traceSource.TraceEvent(TraceEventType.Warning, 0, "Type {0} has no default constructor", t.FullName);
                        continue;
                    }
                    FhirResourceHandlerUtil.RegisterResourceHandler(ci.Invoke(null) as IFhirResourceHandler);
                }

                // Start the web host
                this.m_webHost.Open();

                this.Started?.Invoke(this, EventArgs.Empty);

                return true;
            }
            catch (Exception e)
            {
                this.m_traceSource.TraceEvent(TraceEventType.Error, e.HResult, e.ToString());
                return false;
            }
            
        }

        /// <summary>
        /// Stop the FHIR message handler
        /// </summary>
        /// <returns></returns>
        public bool Stop()
        {
            this.Stopping?.Invoke(this, EventArgs.Empty);

            if (this.m_webHost != null)
            {
                this.m_webHost.Close();
                this.m_webHost = null;
            }

            this.Stopped?.Invoke(this, EventArgs.Empty);

            return true;
        }

        #endregion

        public bool IsRunning
        {
            get
            {
                return this.m_webHost != null;
            }
        }
    }
}
