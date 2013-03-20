using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.Configuration;
using MARC.HI.EHRS.SVC.QM.Persistence.Data.Configuration.UI.Panels;
using System.Windows.Forms;
using System.Xml;

namespace MARC.HI.EHRS.QM.Persistence.Data.Configuration.UI
{
    /// <summary>
    /// Configuration for query persistence
    /// </summary>
    public class QueryPersistenceConfigurationPanel : IDataboundConfigurationPanel
    {
        #region IDataboundConfigurationPanel Members

        private pnlConfigureMessagePersistence m_configPanel = new pnlConfigureMessagePersistence();
        private bool m_needSync = false;

        public QueryPersistenceConfigurationPanel()
        {
            this.MaxAge = 1;
        }
        /// <summary>
        /// Gets or sets the connection string
        /// </summary>
        public string ConnectionString
        {
            get;
            set;
        }

        /// <summary>
        /// Maximum age
        /// </summary>
        public decimal MaxAge { get; set; }

        /// <summary>
        /// Gets or sets the database configurator
        /// </summary>
        public IDatabaseConfigurator DatabaseConfigurator
        {
            get;
            set;
        }

        #endregion

        #region IConfigurationPanel Members

        /// <summary>
        /// Gets the name of the configuration panel
        /// </summary>
        public string Name
        {
            get { return "Query Persistence"; }
        }

        /// <summary>
        /// True or false based on configuration state
        /// </summary>
        public bool EnableConfiguration
        {
            get;
            set;
        }

        public Control Panel { get { return this.m_configPanel; } }

        /// <summary>
        /// Configure the option
        /// </summary>
        public void Configure(System.Xml.XmlDocument configurationDom)
        {
            if (this.ConnectionString == null || this.DatabaseConfigurator == null)
                throw new ArgumentNullException("Unable to connect to the database");
            else if (!this.EnableConfiguration)
                return;

            XmlElement configSectionsNode = configurationDom.SelectSingleNode("//*[local-name() = 'configSections']") as XmlElement,
                persistenceNode = configurationDom.SelectSingleNode("//*[local-name() = 'marc.hi.ehrs.qm.persistence.data']") as XmlElement,
                coreNode = configurationDom.SelectSingleNode("//*[local-name() = 'marc.hi.ehrs.svc.core']") as XmlElement;

            // Config sections node
            if (configSectionsNode == null)
            {
                configSectionsNode = configurationDom.CreateElement("configSections");
                configurationDom.DocumentElement.PrependChild(configSectionsNode);
            }
            XmlElement configSectionNode = configSectionsNode.SelectSingleNode("./*[local-name() = 'section'][@name = 'marc.hi.ehrs.qm.persistence.data']") as XmlElement;
            if (configSectionNode == null)
            {
                configSectionNode = configurationDom.CreateElement("section");
                configSectionNode.Attributes.Append(configurationDom.CreateAttribute("name"));
                configSectionNode.Attributes.Append(configurationDom.CreateAttribute("type"));
                configSectionNode.Attributes["name"].Value = "marc.hi.ehrs.qm.persistence.data";
                configSectionNode.Attributes["type"].Value = typeof(ConfigurationHandler).AssemblyQualifiedName;
                configSectionsNode.AppendChild(configSectionNode);
            }

            // Persistence section node
            if (persistenceNode == null)
            {
                persistenceNode = configurationDom.CreateElement("marc.hi.ehrs.qm.persistence.data");
                configurationDom.DocumentElement.AppendChild(persistenceNode);
            }
            XmlElement connectionManager = persistenceNode.SelectSingleNode("./*[local-name() = 'connectionManager']") as XmlElement;
            if (connectionManager == null)
            {
                connectionManager = configurationDom.CreateElement("connectionManager");
                persistenceNode.AppendChild(connectionManager);
            }
            if (connectionManager.Attributes["connection"] == null)
                connectionManager.Attributes.Append(configurationDom.CreateAttribute("connection"));
            connectionManager.Attributes["connection"].Value = this.ConnectionString;
            var limitNode = persistenceNode.SelectSingleNode("./*[local-name() = 'limit']") as XmlElement;
            if (limitNode == null)
                limitNode = persistenceNode.AppendChild(configurationDom.CreateElement("limit")) as XmlElement;
            if (limitNode.Attributes["maxAge"] == null)
                limitNode.Attributes.Append(configurationDom.CreateAttribute("maxAge"));
            limitNode.Attributes["maxAge"].Value = this.MaxAge.ToString();
            
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


            XmlElement addServiceAsmNode = serviceAssemblyNode.SelectSingleNode("./*[local-name() = 'add'][@assembly = 'MARC.HI.EHRS.QM.Persistence.Data.dll']") as XmlElement,
                addServiceProvNode = serviceProviderNode.SelectSingleNode(String.Format("./*[local-name() = 'add'][@type = '{0}']", typeof(AdoQueryPersistenceService).AssemblyQualifiedName)) as XmlElement;
            if (addServiceAsmNode == null)
            {
                addServiceAsmNode = configurationDom.CreateElement("add");
                addServiceAsmNode.Attributes.Append(configurationDom.CreateAttribute("assembly"));
                addServiceAsmNode.Attributes["assembly"].Value = "MARC.HI.EHRS.QM.Persistence.Data.dll";
                serviceAssemblyNode.AppendChild(addServiceAsmNode);
            }
            if (addServiceProvNode == null)
            {
                addServiceProvNode = configurationDom.CreateElement("add");
                addServiceProvNode.Attributes.Append(configurationDom.CreateAttribute("type"));
                addServiceProvNode.Attributes["type"].Value = typeof(AdoQueryPersistenceService).AssemblyQualifiedName;
                serviceProviderNode.AppendChild(addServiceProvNode);
            }

            // Instruct the database to create the feature for core
            bool shouldQuit = false;
            while (!shouldQuit)
                try
                {
                    this.DatabaseConfigurator.DeployFeature("QM-DDL", this.ConnectionString, configurationDom);
                    shouldQuit = true;
                }
                catch (Exception)
                {
                    switch (MessageBox.Show("There was an error deploying the Query Persistence schema to the database. This commonly occurs when an older version of the schema exists in the selected database. Would you like to try removing the old schema and re-deploying? (Selecting No will ignore this error)", "Error during deploy", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                    {
                        case DialogResult.Yes:
                            this.DatabaseConfigurator.UnDeployFeature("QM-DDL", this.ConnectionString, configurationDom);
                            break;
                        case DialogResult.Cancel:
                            throw;
                        case DialogResult.No:
                            shouldQuit = true;
                            break;
                    }
                }

            this.m_needSync = true;
        }

        /// <summary>
        /// Unconfigure the option
        /// </summary>
        public void UnConfigure(System.Xml.XmlDocument configurationDom)
        {
            this.DatabaseConfigurator.UnDeployFeature("QM-DDL", this.ConnectionString, configurationDom);

            // Select the relevant items and un-configure
            XmlNode configSection = configurationDom.SelectSingleNode("//*[local-name() = 'section'][@name = 'marc.hi.ehrs.qm.persistence.data']"),
                persistenceSection = configurationDom.SelectSingleNode("//*[local-name() = 'marc.hi.ehrs.qm.persistence.data']"),
                addAssemblyNode = configurationDom.SelectSingleNode("//*[local-name() = 'marc.hi.ehrs.svc.core']/*[local-name() = 'serviceAssemblies']/*[local-name() = 'add'][@assembly = 'MARC.HI.EHRS.QM.Persistence.Data.dll']"),
                addProviderNode = configurationDom.SelectSingleNode(String.Format("//*[local-name() = 'marc.hi.ehrs.svc.core']/*[local-name() = 'serviceProviders']/*[local-name() = 'add'][@type = '{0}']", typeof(AdoQueryPersistenceService).AssemblyQualifiedName));

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
        /// Determine if the option is configured
        /// </summary>
        public bool IsConfigured(System.Xml.XmlDocument configurationDom)
        {
            XmlNode configSection = configurationDom.SelectSingleNode("//*[local-name() = 'section'][@name = 'marc.hi.ehrs.qm.persistence.data']"),
               persistenceSection = configurationDom.SelectSingleNode("//*[local-name() = 'marc.hi.ehrs.qm.persistence.data']"),
               addAssemblyNode = configurationDom.SelectSingleNode("//*[local-name() = 'marc.hi.ehrs.svc.core']/*[local-name() = 'serviceAssemblies']/*[local-name() = 'add'][@assembly = 'MARC.HI.EHRS.QM.Persistence.Data.dll']"),
               addProviderNode = configurationDom.SelectSingleNode(String.Format("//*[local-name() = 'marc.hi.ehrs.svc.core']/*[local-name() = 'serviceProviders']/*[local-name() = 'add'][@type = '{0}']", typeof(AdoQueryPersistenceService).AssemblyQualifiedName));

            bool isConfigured = configSection != null && persistenceSection != null && addAssemblyNode != null &&
                addProviderNode != null;

            if (!this.m_needSync)
                return isConfigured;
            this.m_needSync = false;

            // Get connection string
            if (persistenceSection != null)
            {
                string connString = persistenceSection.SelectSingleNode("./*[local-name() = 'connectionManager']/@connection").Value,
                    invariantProvider = configurationDom.SelectSingleNode(String.Format("//*[local-name() = 'connectionStrings']/*[local-name() = 'add'][@name = '{0}']/@providerName", connString)).Value;
                var maxAge = persistenceSection.SelectSingleNode("./*[local-name() = 'limit']/@maxAge");
                if (maxAge != null)
                    this.MaxAge = Decimal.Parse(maxAge.Value);

                // First get the provider
                this.DatabaseConfigurator = DatabaseConfiguratorRegistrar.Configurators.Find(o => o.InvariantName == invariantProvider);
                this.ConnectionString = connString;
            }

            // Set config options
            this.m_configPanel.DatabaseConfigurator = this.DatabaseConfigurator;
            this.m_configPanel.SetConnectionString(configurationDom, this.ConnectionString);
            this.m_configPanel.MaxAge = this.MaxAge;
            EnableConfiguration = isConfigured;

            // Enable configuration
            return isConfigured;
        }

        /// <summary>
        /// Validate the configuration options
        /// </summary>
        public bool Validate(System.Xml.XmlDocument configurationDom)
        {
            this.DatabaseConfigurator = m_configPanel.DatabaseConfigurator;
            this.ConnectionString = m_configPanel.GetConnectionString(configurationDom);
            return ConnectionString != null && DatabaseConfigurator != null;
        }

        #endregion

        public override string ToString()
        {
            return this.Name;
        }
    }
}
