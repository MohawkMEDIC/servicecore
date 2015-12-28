using MARC.HI.EHRS.SVC.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Configuration
{
    /// <summary>
    /// FHIR service configuration
    /// </summary>
    public class FhirServiceConfiguration
    {

        /// <summary>
        /// Creates a new instance of the WcfEndpoint
        /// </summary>
        public FhirServiceConfiguration(string wcfEndpoint, string landingPage)
        {
            this.WcfEndpoint = wcfEndpoint;
            this.LandingPage = landingPage;
            this.ResourceHandlers = new List<Type>();
            this.ActionMap = new Dictionary<string, CodeValue>();
        }

        /// <summary>
        /// Gets the WCF endpoint name that the FHIR service listens on
        /// </summary>
        public string WcfEndpoint { get; private set; }

        /// <summary>
        /// The landing page file
        /// </summary>
        public string LandingPage { get; private set; }

        /// <summary>
        /// Gets the resource handlers registered
        /// </summary>
        public List<Type> ResourceHandlers { get; private set; }

        /// <summary>
        /// Get or set
        /// </summary>
        public Dictionary<String, CodeValue> ActionMap { get; private set; }
    }
}
