using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.Configuration;
using System.Xml;
using System.Net;

namespace MARC.HI.EHRS.SVC.Auditing.Atna.Configuration.UI
{
    /// <summary>
    /// Atna configuration panel
    /// </summary>
    public class AtnaConfigurationPanel : IConfigurationPanel
    {
        #region IConfigurationPanel Members

        // Configuration panel
        private pnlConfigureAudit m_panel = new pnlConfigureAudit();

        // True when UI needs sync
        private bool m_needSync = true;

        /// <summary>
        /// Gets the name of the configuration panel
        /// </summary>
        public string Name
        {
            get { return "Service Core/Auditing"; }
        }

        /// <summary>
        /// Enable ATNA auditing?
        /// </summary>
        public bool EnableConfiguration
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the panel for configuration
        /// </summary>
        public System.Windows.Forms.Control Panel
        {
            get { return this.m_panel; }
        }

        /// <summary>
        /// Configuration
        /// </summary>
        public void Configure(System.Xml.XmlDocument configurationDom)
        {
            if (this.m_panel.Publisher == null || !this.EnableConfiguration)
                return;

            XmlElement configSectionsNode = configurationDom.SelectSingleNode("//*[local-name() = 'configSections']") as XmlElement,
                auditNode = configurationDom.SelectSingleNode("//*[local-name() = 'marc.hi.ehrs.svc.auditing.atna']") as XmlElement,
                coreNode = configurationDom.SelectSingleNode("//*[local-name() = 'marc.hi.ehrs.svc.core']") as XmlElement;

            // Config sections node
            if (configSectionsNode == null)
            {
                configSectionsNode = configurationDom.CreateElement("configSections");
                configurationDom.DocumentElement.PrependChild(configSectionsNode);
            }
            XmlElement configSectionNode = configSectionsNode.SelectSingleNode("./*[local-name() = 'section'][@name = 'marc.hi.ehrs.svc.auditing.atna']") as XmlElement;
            if (configSectionNode == null)
            {
                configSectionNode = configurationDom.CreateElement("section");
                configSectionNode.Attributes.Append(configurationDom.CreateAttribute("name"));
                configSectionNode.Attributes.Append(configurationDom.CreateAttribute("type"));
                configSectionNode.Attributes["name"].Value = "marc.hi.ehrs.svc.auditing.atna";
                configSectionNode.Attributes["type"].Value = typeof(ConfigurationSectionHandler).AssemblyQualifiedName;
                configSectionsNode.AppendChild(configSectionNode);
            }

            // Persistence section node
            if (auditNode == null)
            {
                auditNode = configurationDom.CreateElement("marc.hi.ehrs.svc.auditing.atna");
                configurationDom.DocumentElement.AppendChild(auditNode);
            }

            if (auditNode.Attributes["messagePublisher"] == null)
                auditNode.Attributes.Append(configurationDom.CreateAttribute("messagePublisher"));
            auditNode.Attributes["messagePublisher"].Value = this.m_panel.Publisher.AssemblyQualifiedName;

            XmlElement connectionManager = auditNode.SelectSingleNode("./*[local-name() = 'destination']") as XmlElement;
            if (connectionManager == null)
            {
                connectionManager = configurationDom.CreateElement("destination");
                auditNode.AppendChild(connectionManager);
            }
            if (connectionManager.Attributes["endpoint"] == null)
                connectionManager.Attributes.Append(configurationDom.CreateAttribute("endpoint"));
            connectionManager.Attributes["endpoint"].Value = this.m_panel.Endpoint.ToString();

            // Ensure the assembly is loaded and the provider registered
            if (coreNode == null)
            {
                coreNode = configurationDom.CreateElement("marc.hi.ehrs.svc.core");
                configurationDom.DocumentElement.AppendChild(coreNode);
            }
            XmlElement serviceAssemblyNode = coreNode.SelectSingleNode("./*[local-name() = 'serviceAssemblies']") as XmlElement,
                serviceProviderNode = coreNode.SelectSingleNode("./*[local-name() = 'serviceProviders']") as XmlElement;
            if (serviceAssemblyNode == null)
            {
                serviceAssemblyNode = configurationDom.CreateElement("serviceAssemblies");
                coreNode.AppendChild(serviceAssemblyNode);
            }
            if (serviceProviderNode == null)
            {
                serviceProviderNode = configurationDom.CreateElement("serviceProviders");
                coreNode.AppendChild(serviceProviderNode);
            }


            XmlElement addServiceAsmNode = serviceAssemblyNode.SelectSingleNode("./*[local-name() = 'add'][@assembly = 'MARC.HI.EHRS.SVC.Auditing.Atna.dll']") as XmlElement,
                addServiceProvNode = serviceProviderNode.SelectSingleNode(String.Format("./*[local-name() = 'add'][@type = '{0}']", typeof(AtnaAuditService).AssemblyQualifiedName)) as XmlElement;
            if (addServiceAsmNode == null)
            {
                addServiceAsmNode = configurationDom.CreateElement("add");
                addServiceAsmNode.Attributes.Append(configurationDom.CreateAttribute("assembly"));
                addServiceAsmNode.Attributes["assembly"].Value = "MARC.HI.EHRS.SVC.Auditing.Atna.dll";
                serviceAssemblyNode.AppendChild(addServiceAsmNode);
            }
            if (addServiceProvNode == null)
            {
                addServiceProvNode = configurationDom.CreateElement("add");
                addServiceProvNode.Attributes.Append(configurationDom.CreateAttribute("type"));
                addServiceProvNode.Attributes["type"].Value = typeof(AtnaAuditService).AssemblyQualifiedName;
                serviceProviderNode.AppendChild(addServiceProvNode);
            }

            this.m_needSync = true;
        }

        /// <summary>
        /// Unconfigure the audits
        /// </summary>
        /// <param name="configurationDom"></param>
        public void UnConfigure(System.Xml.XmlDocument configurationDom)
        {
            // Select the relevant items and un-configure
            XmlNode configSection = configurationDom.SelectSingleNode("//*[local-name() = 'section'][@name = 'marc.hi.ehrs.svc.auditing.atna']"),
                persistenceSection = configurationDom.SelectSingleNode("//*[local-name() = 'marc.hi.ehrs.svc.auditing.atna']"),
                addAssemblyNode = configurationDom.SelectSingleNode("//*[local-name() = 'marc.hi.ehrs.svc.core']/*[local-name() = 'serviceAssemblies']/*[local-name() = 'add'][@assembly = 'MARC.HI.EHRS.SVC.Auditing.Atna.dll']"),
                addProviderNode = configurationDom.SelectSingleNode(String.Format("//*[local-name() = 'marc.hi.ehrs.svc.core']/*[local-name() = 'serviceProviders']/*[local-name() = 'add'][@type = '{0}']", typeof(AtnaAuditService).AssemblyQualifiedName));

            if (configSection != null)
                configSection.ParentNode.RemoveChild(configSection);
            if (persistenceSection != null)
                persistenceSection.ParentNode.RemoveChild(persistenceSection);
            if (addAssemblyNode != null)
                addAssemblyNode.ParentNode.RemoveChild(addAssemblyNode);
            if (addProviderNode != null)
                addProviderNode.ParentNode.RemoveChild(addProviderNode);
            this.m_needSync = true;
        }

        /// <summary>
        /// Determine if configuration is enabled
        /// </summary>
        public bool IsConfigured(System.Xml.XmlDocument configurationDom)
        {
            XmlNode configSection = configurationDom.SelectSingleNode("//*[local-name() = 'section'][@name = 'marc.hi.ehrs.svc.auditing.atna']"),
              auditSection = configurationDom.SelectSingleNode("//*[local-name() = 'marc.hi.ehrs.svc.auditing.atna']"),
              addAssemblyNode = configurationDom.SelectSingleNode("//*[local-name() = 'marc.hi.ehrs.svc.core']/*[local-name() = 'serviceAssemblies']/*[local-name() = 'add'][@assembly = 'MARC.HI.EHRS.SVC.Auditing.Atna.dll']"),
              addProviderNode = configurationDom.SelectSingleNode(String.Format("//*[local-name() = 'marc.hi.ehrs.svc.core']/*[local-name() = 'serviceProviders']/*[local-name() = 'add'][@type = '{0}']", typeof(AtnaAuditService).AssemblyQualifiedName));

            bool isConfigured = configSection != null && auditSection != null && addAssemblyNode != null &&
                addProviderNode != null;

            if (!this.m_needSync)
                return isConfigured;
            this.m_needSync = false;

            if (auditSection != null)
            {
                if(auditSection.Attributes["messagePublisher"] != null)
                    this.m_panel.Publisher = Type.GetType(auditSection.Attributes["messagePublisher"].Value);

                var destination = auditSection.SelectSingleNode("./*[local-name() = 'destination']/@endpoint");
                if (destination != null)
                {
                    string[] parts = destination.Value.Split(':');
                    this.m_panel.Endpoint = new IPEndPoint(IPAddress.Parse(parts[0]), Int32.Parse(parts[1]));
                }
            }
            // Set config options
            EnableConfiguration = isConfigured;

            // Enable configuration
            return isConfigured;
        }

        /// <summary>
        /// Validate configuration
        /// </summary>
        public bool Validate(System.Xml.XmlDocument configurationDom)
        {
            try
            {
                if (this.m_panel.Endpoint == null ||
                    this.m_panel.Publisher == null)
                    return false;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Represent for the GUI
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "ATNA Auditing";
        }

        #endregion
    }
}
