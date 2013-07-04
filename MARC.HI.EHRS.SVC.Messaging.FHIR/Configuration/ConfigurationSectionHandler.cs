using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Configuration
{
    /// <summary>
    /// Configuration section handler for FHIR
    /// </summary>
    public class ConfigurationSectionHandler : IConfigurationSectionHandler
    {
        #region IConfigurationSectionHandler Members

        /// <summary>
        /// Create the configuration object
        /// </summary>
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            
            // Section
            XmlElement serviceElement = section.SelectSingleNode("./*[local-name() = 'service']") as XmlElement;
            string wcfServiceName = String.Empty,
                landingPage = String.Empty;

            if (serviceElement != null)
            {
                XmlAttribute serviceName = serviceElement.Attributes["wcfServiceName"],
                    landingPageNode = serviceElement.Attributes["landingPage"];
                if (serviceName != null)
                    wcfServiceName = serviceName.Value;
                else
                    throw new ConfigurationErrorsException("Missing wcfServiceName attribute", serviceElement);
                if (landingPageNode != null)
                    landingPage = landingPageNode.Value;
            }
            else
                throw new ConfigurationErrorsException("Missing serviceElement", section);

            return new FhirServiceConfiguration(wcfServiceName, landingPage);
        }

        #endregion
    }
}
