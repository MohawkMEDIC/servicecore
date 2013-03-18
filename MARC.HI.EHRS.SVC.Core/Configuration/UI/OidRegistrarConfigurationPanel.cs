using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MARC.HI.EHRS.SVC.Core.Configuration.UI
{
    /// <summary>
    /// Configuration panel for the core services
    /// </summary>
    public class OidRegistrarConfigurationPanel : IConfigurationPanel
    {
        #region IConfigurationPanel Members

        // Configuration
        private HostConfigurationSectionHandler m_configuration;

        /// <summary>
        /// Gets the name of the configuration panel
        /// </summary>
        public string Name
        {
            get { return "OID Registrar"; }
        }

        /// <summary>
        /// OID Registration must be enabled
        /// </summary>
        public bool EnableConfiguration
        {
            get { return true; }
            set { }
        }

        /// <summary>
        /// Get the panel which controls the configuration
        /// </summary>
        public System.Windows.Forms.Control Panel
        {
            get { return new System.Windows.Forms.Label() { Text = "No Configuration Yet Supported" }; }
        }

        /// <summary>
        /// Configure the OID registration
        /// </summary>
        public void Configure(System.Xml.XmlDocument configurationDom)
        {
            XmlElement coreElement = configurationDom.SelectSingleNode("./*[local-name() = 'marc.hi.ehrs.svc.core']") as XmlElement;
            if(coreElement != null)
                this.m_configuration = new HostConfigurationSectionHandler().Create(null, null, coreElement) as HostConfigurationSectionHandler;

        }

        /// <summary>
        /// Un-Configuration the OID registration
        /// </summary>
        public void UnConfigure(System.Xml.XmlDocument configurationDom)
        {
            throw new InvalidOperationException("Cannot un-configure OID Registrar");
        }

        /// <summary>
        /// Return true if the OID registrar is configured
        /// </summary>
        public bool IsConfigured(System.Xml.XmlDocument configurationDom)
        {
            return false; // todo:
        }

        /// <summary>
        /// Validate the configuration
        /// </summary>
        public bool Validate(System.Xml.XmlDocument configurationDom)
        {
            return true; // todo:
        }

        #endregion
    }
}
