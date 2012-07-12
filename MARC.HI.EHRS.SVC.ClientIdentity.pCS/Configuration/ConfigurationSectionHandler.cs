using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace MARC.HI.EHRS.SVC.ClientIdentity.pCS.Configuration
{
    /// <summary>
    /// Configuration Handler
    /// </summary>
    public class ConfigurationSectionHandler : IConfigurationSectionHandler
    {

        /// <summary>
        /// Gets the connection string to the WCF Connector that should be used for fetching a 
        /// client
        /// </summary>
        public string WcfConnectionString { get; private set; }

        /// <summary>
        /// Gets the device identifier for the receiving device
        /// </summary>
        public string DeviceId { get; private set; }

        #region IConfigurationSectionHandler Members

        /// <summary>
        /// Create the configuration section handler
        /// </summary>
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {

            if (section.Attributes["connectionString"] == null)
                throw new ConfigurationErrorsException("Must supply the connection string argument to the client registry configuration");
            else
                this.WcfConnectionString = section.Attributes["connectionString"].Value;

            if (section.Attributes["deviceId"] == null)
                throw new ConfigurationErrorsException("Must supply the device identifier for the client registry configuration");
            else
                this.DeviceId = section.Attributes["deviceId"].Value;

            return this;

        }

        #endregion
    }
}
