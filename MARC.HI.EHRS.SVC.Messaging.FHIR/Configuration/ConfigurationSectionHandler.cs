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
            XmlNodeList resourceElements = section.SelectNodes("./*[local-name()= 'resourceProcessors']/*[local-name() = 'add']");

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

            var retVal = new FhirServiceConfiguration(wcfServiceName, landingPage);

            // Add instructions
            foreach (XmlElement addInstruction in resourceElements)
            {
                if (addInstruction.Attributes["type"] == null)
                    throw new ConfigurationErrorsException("add instruction missing @type attribute");
                Type tType = Type.GetType(addInstruction.Attributes["type"].Value);
                if (tType == null)
                    throw new ConfigurationErrorsException(String.Format("Could not find type described by '{0}'", addInstruction.Attributes["type"].Value));
                retVal.ResourceHandlers.Add(tType);
            }

            return retVal;
        }

        #endregion
    }
}
