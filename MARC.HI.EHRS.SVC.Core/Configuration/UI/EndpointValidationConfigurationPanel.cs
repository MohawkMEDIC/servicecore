using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using MARC.HI.EHRS.SVC.Core.DataTypes;

namespace MARC.HI.EHRS.SVC.Core.Configuration.UI
{
    /// <summary>
    /// Endpoint validation configuration
    /// </summary>
    public class EndpointValidationConfigurationPanel : IAlwaysDeployedConfigurationPanel
    {
        // Validate solicitors
        private ucValidateSolicitors m_panel = new ucValidateSolicitors();

        #region IConfigurationPanel Members

        /// <summary>
        /// Endpoint validation
        /// </summary>
        public string Name
        {
            get { return "Service Core/Endpoint Validation"; }
        }

        /// <summary>
        /// True when configuration is enabled
        /// </summary>
        public bool EnableConfiguration
        {
            get;
            set;
        }

        /// <summary>
        /// Solicitors
        /// </summary>
        public List<DomainIdentifier> Solicitors { get; set; }
        
        /// <summary>
        /// Gets the configuration panel
        /// </summary>
        public System.Windows.Forms.Control Panel
        {
            get { return this.m_panel; }
        }

        /// <summary>
        /// Configure
        /// </summary>
        public void Configure(System.Xml.XmlDocument configurationDom)
        {
            XmlElement configSectionsNode = configurationDom.SelectSingleNode("//*[local-name() = 'configSections']") as XmlElement,
              coreNode = configurationDom.SelectSingleNode("//*[local-name() = 'marc.hi.ehrs.svc.core']") as XmlElement;

            // Configuration section registration
            XmlElement configSectionNode = configSectionsNode.SelectSingleNode("./*[local-name() = 'section'][@name = 'marc.hi.ehrs.svc.core']") as XmlElement;
            if (configSectionNode == null)
            {
                configSectionNode = configurationDom.CreateElement("section");
                configSectionNode.Attributes.Append(configurationDom.CreateAttribute("name"));
                configSectionNode.Attributes.Append(configurationDom.CreateAttribute("type"));
                configSectionNode.Attributes["name"].Value = "marc.hi.ehrs.svc.core";
                configSectionNode.Attributes["type"].Value = typeof(HostConfigurationSectionHandler).AssemblyQualifiedName;
                configSectionsNode.AppendChild(configSectionNode);
            }

            // Create the core node?
            if (coreNode == null)
            {
                coreNode = configurationDom.CreateElement("marc.hi.ehrs.svc.core");
                configurationDom.DocumentElement.AppendChild(coreNode);
            }

            // Add the oid registration section
            XmlElement regDevNode = coreNode.SelectSingleNode("./*[local-name() = 'registeredDevices']") as XmlElement;
            if (regDevNode == null)
            {
                regDevNode = configurationDom.CreateElement("registeredDevices");
                coreNode.AppendChild(regDevNode);
            }

            // Add the configured oids
            regDevNode.RemoveAll();

            if (regDevNode.Attributes["validateSolicitors"] == null)
                regDevNode.Attributes.Append(configurationDom.CreateAttribute("validateSolicitors"));
            regDevNode.Attributes["validateSolicitors"].Value = m_panel.RequireValidation.ToString() ;

            foreach (var oid in this.m_panel.Solicitors)
            {
                var devElement = configurationDom.CreateElement("add");
                regDevNode.AppendChild(devElement);

                // Add core oid data
                if (!String.IsNullOrEmpty(oid.Domain))
                {
                    if (devElement.Attributes["domain"] == null)
                        devElement.Attributes.Append(configurationDom.CreateAttribute("domain"));
                    devElement.Attributes["domain"].Value = oid.Domain;
                }

                if (!String.IsNullOrEmpty(oid.Identifier))
                {
                    if (devElement.Attributes["value"] == null)
                        devElement.Attributes.Append(configurationDom.CreateAttribute("value"));
                    devElement.Attributes["value"].Value = oid.Identifier;
                }

            }
            
        }

        /// <summary>
        /// Unconfigured
        /// </summary>
        public void UnConfigure(System.Xml.XmlDocument configurationDom)
        {
            throw new InvalidOperationException("Cannot un-configure Endpoint Validation");
        }

        /// <summary>
        /// True if configured
        /// </summary>
        public bool IsConfigured(System.Xml.XmlDocument configurationDom)
        {

            if (m_panel.Oids == null)
                m_panel.Oids = OidRegistrarConfigurationPanel.LoadOidRegistrar(configurationDom);

            XmlElement regDevNode = configurationDom.SelectSingleNode("//*[local-name() = 'marc.hi.ehrs.svc.core']/*[local-name() = 'registeredDevices']") as XmlElement;
            if (regDevNode == null)
            {
                regDevNode = configurationDom.CreateElement("registeredDevices");
            }

            //this.m_panel.Solicitors.Clear();

            if (regDevNode.Attributes["validateSolicitors"] != null)
                m_panel.RequireValidation = Boolean.Parse(regDevNode.Attributes["validateSolicitors"].Value);

            this.Solicitors = this.m_panel.Solicitors;

            // Load each oid
            foreach (XmlNode nde in regDevNode.SelectNodes("./*[local-name() = 'add']"))
            {
                XmlElement oidNode = nde as XmlElement;
                if (oidNode == null) continue; // could not interpret

                // Load
                string domain = null, value = null;
                if (oidNode.Attributes["domain"] != null)
                    domain = oidNode.Attributes["domain"].Value;
                if (oidNode.Attributes["value"] != null)
                    value = oidNode.Attributes["value"].Value;

                // register the oid
                if(!this.Solicitors.Exists(o=>(o.Domain ?? "") == (domain ?? "") && (o.Identifier ?? "") == (value ?? "")))
                    this.Solicitors.Add(new DataTypes.DomainIdentifier()
                    {
                        Identifier = value,
                        Domain = domain
                    });

            }
            this.m_panel.Solicitors = this.Solicitors;

            return true;
        }

        /// <summary>
        /// Validate 
        /// </summary>
        public bool Validate(System.Xml.XmlDocument configurationDom)
        {
            this.Solicitors = this.m_panel.Solicitors;
            return true;
        }

        #endregion

        public override string ToString()
        {
            return this.Name;
        }
    }
}
