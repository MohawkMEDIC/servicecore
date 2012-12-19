/**
 * Copyright 2012-2012 Mohawk College of Applied Arts and Technology
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you 
 * may not use this file except in compliance with the License. You may 
 * obtain a copy of the License at 
 * 
 * http://www.apache.org/licenses/LICENSE-2.0 
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations under 
 * the License.
 * 
 * User: fyfej
 * Date: 5-12-2012
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.Configuration;
using System.Xml;
using System.Windows.Forms;
using MARC.HI.EHRS.SVC.Messaging.Persistence.Data.Configuration.UI.Panels;

namespace MARC.HI.EHRS.SVC.Messaging.Persistence.Data.Configuration.UI
{
    /// <summary>
    /// Message Persistence
    /// </summary>
    public class MessagePersistenceConfigurationPanel : IDataboundConfigurationPanel
    {
        private pnlConfigureMessagePersistence m_configPanel = new pnlConfigureMessagePersistence();
        private string serviceName = typeof(MARC.HI.EHRS.SVC.Messaging.Persistence.Data.AdoMessagePersister).AssemblyQualifiedName;

        
        #region IConfigurationPanel Members

        /// <summary>
        /// Message persistence
        /// </summary>
        public string Name
        {
            get { return "Message Persistence"; }
        }

        /// <summary>
        /// Enable configuration
        /// </summary>
        public bool EnableConfiguration { get; set; }

        /// <summary>
        /// Gets the configuration panel
        /// </summary>
        public System.Windows.Forms.Control Panel
        {
            get { return this.m_configPanel; }
        }

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
                persistenceNode = configurationDom.SelectSingleNode("//*[local-name() = 'marc.hi.ehrs.svc.messaging.persistence']") as XmlElement,
                coreNode = configurationDom.SelectSingleNode("//*[local-name() = 'marc.hi.ehrs.svc.core']") as XmlElement;

            // Config sections node
            if (configSectionsNode == null)
            {
                configSectionsNode = configurationDom.CreateElement("configSections");
                configurationDom.DocumentElement.PrependChild(configSectionsNode);
            }
            XmlElement configSectionNode = configSectionsNode.SelectSingleNode("./*[local-name() = 'section'][@name = 'marc.hi.ehrs.svc.messaging.persistence']") as XmlElement;
            if (configSectionNode == null)
            {
                configSectionNode = configurationDom.CreateElement("section");
                configSectionNode.Attributes.Append(configurationDom.CreateAttribute("name"));
                configSectionNode.Attributes.Append(configurationDom.CreateAttribute("type"));
                configSectionNode.Attributes["name"].Value = "marc.hi.ehrs.svc.messaging.persistence";
                configSectionNode.Attributes["type"].Value = "MARC.HI.EHRS.SVC.Messaging.Persistence.Data.Configuration.ConfigurationSectionHandler, MARC.HI.EHRS.SVC.Messaging.Persistence.Data, Version=1.0.0.0";
                configSectionsNode.AppendChild(configSectionNode);
            }

            // Persistence section node
            if (persistenceNode == null)
            {
                persistenceNode = configurationDom.CreateElement("marc.hi.ehrs.svc.messaging.persistence");
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

            
            XmlElement addServiceAsmNode = serviceAssemblyNode.SelectSingleNode("./*[local-name() = 'add'][@assembly = 'MARC.HI.EHRS.SVC.Messaging.Persistence.Data.dll']") as XmlElement,
                addServiceProvNode = serviceProviderNode.SelectSingleNode(String.Format("./*[local-name() = 'add'][@type = '{0}']", serviceName)) as XmlElement;
            if (addServiceAsmNode == null)
            {
                addServiceAsmNode = configurationDom.CreateElement("add");
                addServiceAsmNode.Attributes.Append(configurationDom.CreateAttribute("assembly"));
                addServiceAsmNode.Attributes["assembly"].Value = "MARC.HI.EHRS.SVC.Messaging.Persistence.Data.dll";
                serviceAssemblyNode.AppendChild(addServiceAsmNode);
            }
            if (addServiceProvNode == null)
            {
                addServiceProvNode = configurationDom.CreateElement("add");
                addServiceProvNode.Attributes.Append(configurationDom.CreateAttribute("type"));
                addServiceProvNode.Attributes["type"].Value = serviceName;
                serviceProviderNode.AppendChild(addServiceProvNode);
            }

            // Instruct the database to create the feature for core
            bool shouldQuit = false;
            while (!shouldQuit)
                try
                {
                    this.DatabaseConfigurator.DeployFeature("MDB", this.ConnectionString, configurationDom);
                    shouldQuit = true;
                }
                catch (Exception)
                {
                    switch (MessageBox.Show("There was an error deploying the Message box schema to the database. This commonly occurs when an older version of the schema exists in the selected database. Would you like to try removing the old schema and re-deploying? (Selecting No will ignore this error)", "Error during deploy", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                    {
                        case DialogResult.Yes:
                            this.DatabaseConfigurator.UnDeployFeature("MDB", this.ConnectionString, configurationDom);
                            break;
                        case DialogResult.Cancel:
                            throw;
                        case DialogResult.No:
                            shouldQuit = true;
                            break;
                    }
                }

        }

        /// <summary>
        /// Unconfigure the option
        /// </summary>
        public void UnConfigure(System.Xml.XmlDocument configurationDom)
        {
            this.DatabaseConfigurator.UnDeployFeature("MDB", this.ConnectionString, configurationDom);

            // Select the relevant items and un-configure
            XmlNode configSection = configurationDom.SelectSingleNode("//*[local-name() = 'section'][@name = 'marc.hi.ehrs.svc.messaging.persistence']"),
                persistenceSection = configurationDom.SelectSingleNode("//*[local-name() = 'marc.hi.ehrs.svc.messaging.persistence']"),
                addAssemblyNode = configurationDom.SelectSingleNode("//*[local-name() = 'marc.hi.ehrs.svc.core']/*[local-name() = 'serviceAssemblies']/*[local-name() = 'add'][@assembly = 'MARC.HI.EHRS.SVC.Messaging.Persistence.Data.dll']"),
                addProviderNode = configurationDom.SelectSingleNode(String.Format("//*[local-name() = 'marc.hi.ehrs.svc.core']/*[local-name() = 'serviceProviders']/*[local-name() = 'add'][@type = '{0}']", serviceName));

            if (configSection != null)
                configSection.ParentNode.RemoveChild(configSection);
            if (persistenceSection != null)
                persistenceSection.ParentNode.RemoveChild(persistenceSection);
            if (addAssemblyNode != null)
                addAssemblyNode.ParentNode.RemoveChild(addAssemblyNode);
            if (addProviderNode != null)
                addProviderNode.ParentNode.RemoveChild(addProviderNode);
        }

        /// <summary>
        /// Determine if the option is configured
        /// </summary>
        public bool IsConfigured(System.Xml.XmlDocument configurationDom)
        {
            XmlNode configSection = configurationDom.SelectSingleNode("//*[local-name() = 'section'][@name = 'marc.hi.ehrs.svc.messaging.persistence']"),
               persistenceSection = configurationDom.SelectSingleNode("//*[local-name() = 'marc.hi.ehrs.svc.messaging.persistence']"),
               addAssemblyNode = configurationDom.SelectSingleNode("//*[local-name() = 'marc.hi.ehrs.svc.core']/*[local-name() = 'serviceAssemblies']/*[local-name() = 'add'][@assembly = 'MARC.HI.EHRS.SVC.Messaging.Persistence.Data.dll']"),
               addProviderNode = configurationDom.SelectSingleNode(String.Format("//*[local-name() = 'marc.hi.ehrs.svc.core']/*[local-name() = 'serviceProviders']/*[local-name() = 'add'][@type = '{0}']", serviceName));

             // Get connection string
            if (persistenceSection != null)
            {
                string connString = persistenceSection.SelectSingleNode("./*[local-name() = 'connectionManager']/@connection").Value,
                    invariantProvider = configurationDom.SelectSingleNode(String.Format("//*[local-name() = 'connectionStrings']/*[local-name() = 'add'][@name = '{0}']/@providerName", connString)).Value;
                // First get the provider
                this.DatabaseConfigurator = DatabaseConfiguratorRegistrar.Configurators.Find(o => o.InvariantName == invariantProvider);
                this.ConnectionString = connString;
            }

            // Set config options
            this.m_configPanel.DatabaseConfigurator = this.DatabaseConfigurator;
            this.m_configPanel.SetConnectionString(configurationDom, this.ConnectionString);

            if (configSection != null && persistenceSection != null && addAssemblyNode != null &&
                addProviderNode != null && !EnableConfiguration)
                EnableConfiguration = true;

            // Enable configuration
            return configSection != null && persistenceSection != null && addAssemblyNode != null &&
                addProviderNode != null;
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

        #region IDataboundConfigurationPanel Members

        /// <summary>
        /// Gets or sets the connection string for the message persistence
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the database configurator
        /// </summary>
        public IDatabaseConfigurator DatabaseConfigurator { get; set; }

        #endregion

        /// <summary>
        /// Override of tostring
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
