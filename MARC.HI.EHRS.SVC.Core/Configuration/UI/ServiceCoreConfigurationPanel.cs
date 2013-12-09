using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using MARC.HI.EHRS.SVC.Core.DataTypes;
using System.Reflection;
using System.Globalization;

namespace MARC.HI.EHRS.SVC.Core.Configuration.UI
{
    /// <summary>
    /// Service core configuration panel
    /// </summary>
    public class ServiceCoreConfigurationPanel : IAlwaysDeployedConfigurationPanel
    {
        #region IConfigurationPanel Members

        private CustodianshipData m_custodianData;
        private string m_languageCode;
        private Jurisdiction m_jurisdictionData;
        private DomainIdentifier m_deviceId;
        private OidRegistrar m_oidRegistrar;
        private ucCoreProperties m_panel = new ucCoreProperties();

        /// <summary>
        /// Initialize default configuration
        /// </summary>
        public ServiceCoreConfigurationPanel()
        {
            this.CreateDefaultConfiguration();

        }

        public void CreateDefaultConfiguration()
        {
            this.m_custodianData = new CustodianshipData()
            {
                Id = new DomainIdentifier() { Domain = "OID OF YOUR CLUSTERS", Identifier = "SVC CLUSTER ID" },
                Name = Assembly.GetEntryAssembly().GetName().Name
            };
            this.m_languageCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            this.m_jurisdictionData = new Jurisdiction()
            {
                Id = new DomainIdentifier() { Domain = "OID OF JURISDICTION", Identifier = "FAKE_JURISDICTION" },
                Name = "Fake Jurisdiction",
                ClientDomain = "OID OF ENTERPRISE CLIENT ID",
                ProviderDomain = "OID OF ENTERPRISE PROVIDER ID",
                PlaceDomain = "OID OF ENTERPRISE LOCATION ID"
            };
            this.m_deviceId = new DomainIdentifier()
            {
                Domain = "OID OF YOUR MACHINES",
                Identifier = Environment.MachineName.ToUpper()
            };
        }
        /// <summary>
        /// Gets the name of the configuration panel
        /// </summary>
        public string Name
        {
            get { return "Service Core" ; }
        }

        /// <summary>
        /// Gets or sets whether the configuration is saved
        /// </summary>
        public bool EnableConfiguration
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the panel
        /// </summary>
        public System.Windows.Forms.Control Panel
        {
            get { return this.m_panel; }
        }

        /// <summary>
        /// Configure the service core node
        /// </summary>
        public void Configure(System.Xml.XmlDocument configurationDom)
        {

            XmlElement configSectionsNode = configurationDom.SelectSingleNode("//*[local-name() = 'configSections']") as XmlElement,
               coreNode = configurationDom.SelectSingleNode("//*[local-name() = 'marc.hi.ehrs.svc.core']") as XmlElement;

            if (configSectionsNode == null)
                configSectionsNode = configurationDom.DocumentElement.AppendChild(configurationDom.CreateElement("configSections")) as XmlElement;

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
            
            // System ID, Jurisdiction and Custodian
            XmlElement jurisdictionNode = coreNode.SelectSingleNode("./*[local-name() = 'jurisdiction']") as XmlElement,
                custodianNode = coreNode.SelectSingleNode("./*[local-name() = 'custodianship']") as XmlElement,
                systemNode = coreNode.SelectSingleNode("./*[local-name() = 'system']") as XmlElement,
                serviceProviderNode = coreNode.SelectSingleNode("./*[local-name() = 'serviceProviders']") as XmlElement;

            if (serviceProviderNode == null)
            {
                serviceProviderNode = configurationDom.CreateElement("serviceProviders");
                coreNode.AppendChild(serviceProviderNode);
            }
            // Is there an XmlLocalization Service?
            var xmlLocalizationServiceAsm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a=>a.GetTypes().FirstOrDefault(t=>t.Name == "XmlLocalizationService") != null);
            var xmlLocalizationService = xmlLocalizationServiceAsm.GetTypes().FirstOrDefault(o => o.Name == "XmlLocalizationService");
            if (serviceProviderNode.SelectSingleNode(String.Format("./*[local-name() = 'add' and ./@type = '{0}']", xmlLocalizationService.AssemblyQualifiedName)) == null)
            {
                var localeServiceNode = serviceProviderNode.AppendChild(configurationDom.CreateElement("add"));
                var typeAttribute = localeServiceNode.Attributes.Append(configurationDom.CreateAttribute("type"));
                typeAttribute.Value = xmlLocalizationService.AssemblyQualifiedName;
            }

            // System data
            if (systemNode == null)
                systemNode = coreNode.AppendChild(configurationDom.CreateElement("system")) as XmlElement;
            XmlElement idNode = systemNode.SelectSingleNode("./*[local-name() = 'device']") as XmlElement;
            if (idNode == null)
                idNode = systemNode.AppendChild(configurationDom.CreateElement("device")) as XmlElement;
            if (idNode.Attributes["id"] == null)
                idNode.Attributes.Append(configurationDom.CreateAttribute("id"));
            idNode.Attributes["id"].Value = this.m_deviceId.Domain;
            if (idNode.Attributes["name"] == null)
                idNode.Attributes.Append(configurationDom.CreateAttribute("name"));
            idNode.Attributes["name"].Value = this.m_deviceId.Identifier;

            // Custodian data
            if (custodianNode == null)
                custodianNode = coreNode.AppendChild(configurationDom.CreateElement("custodianship")) as XmlElement;
            XmlElement nameNode = custodianNode.SelectSingleNode("./*[local-name() = 'name']") as XmlElement;
            if (nameNode == null)
                nameNode = custodianNode.AppendChild(configurationDom.CreateElement("name")) as XmlElement;
            nameNode.InnerText = this.m_custodianData.Name;
            
            idNode = custodianNode.SelectSingleNode("./*[local-name() = 'id']") as XmlElement;
            if (idNode == null)
                idNode = custodianNode.AppendChild(configurationDom.CreateElement("id")) as XmlElement;
            this.PopulateIdNode(this.m_custodianData.Id, idNode, configurationDom);

            // Jurisdiction data
            // Custodian data
            if (jurisdictionNode == null)
                jurisdictionNode = coreNode.AppendChild(configurationDom.CreateElement("jurisdiction")) as XmlElement;
            nameNode = jurisdictionNode.SelectSingleNode("./*[local-name() = 'name']") as XmlElement;
            if (nameNode == null)
                nameNode = jurisdictionNode.AppendChild(configurationDom.CreateElement("name")) as XmlElement;
            nameNode.InnerText = this.m_jurisdictionData.Name;
            idNode = jurisdictionNode.SelectSingleNode("./*[local-name() = 'id']") as XmlElement;
            if (idNode == null)
                idNode = jurisdictionNode.AppendChild(configurationDom.CreateElement("id")) as XmlElement;
            this.PopulateIdNode(this.m_jurisdictionData.Id, idNode, configurationDom);
            idNode = jurisdictionNode.SelectSingleNode("./*[local-name() = 'clientExport']") as XmlElement;
            if (idNode == null)
                idNode = jurisdictionNode.AppendChild(configurationDom.CreateElement("clientExport")) as XmlElement;
            this.PopulateIdNode(new DomainIdentifier() { Domain = this.m_jurisdictionData.ClientDomain }, idNode, configurationDom);
            idNode = jurisdictionNode.SelectSingleNode("./*[local-name() = 'providerExport']") as XmlElement;
            if (idNode == null)
                idNode = jurisdictionNode.AppendChild(configurationDom.CreateElement("providerExport")) as XmlElement;
            this.PopulateIdNode(new DomainIdentifier() { Domain = this.m_jurisdictionData.ProviderDomain }, idNode, configurationDom);
            idNode = jurisdictionNode.SelectSingleNode("./*[local-name() = 'sdlExport']") as XmlElement;
            if (idNode == null)
                idNode = jurisdictionNode.AppendChild(configurationDom.CreateElement("sdlExport")) as XmlElement;
            this.PopulateIdNode(new DomainIdentifier() { Domain = this.m_jurisdictionData.PlaceDomain }, idNode, configurationDom);
            var languageNode = jurisdictionNode.SelectSingleNode("./*[local-name() = 'defaultLanguageCode']") as XmlElement;
            if (languageNode == null)
                languageNode = jurisdictionNode.AppendChild(configurationDom.CreateElement("defaultLanguageCode")) as XmlElement;
            if(languageNode.Attributes["code"] == null)
                languageNode.Attributes.Append(configurationDom.CreateAttribute("code"));
            languageNode.Attributes["code"].Value = this.m_languageCode;

            this.m_oidRegistrar = null;
        }

        /// <summary>
        /// Populate ID Node
        /// </summary>
        private void PopulateIdNode(DomainIdentifier domainIdentifier, XmlElement idNode, XmlDocument configurationDom)
        {
            if (!String.IsNullOrEmpty(domainIdentifier.Domain))
            {
                if (idNode.Attributes["domain"] == null)
                    idNode.Attributes.Append(configurationDom.CreateAttribute("domain"));
                idNode.Attributes["domain"].Value = domainIdentifier.Domain;
            }
            else if(idNode.Attributes["domain"] != null)
                idNode.Attributes.Remove(idNode.Attributes["domain"]);

            if(!String.IsNullOrEmpty(domainIdentifier.Identifier))
            {
                if (idNode.Attributes["value"] == null)
                    idNode.Attributes.Append(configurationDom.CreateAttribute("value"));
                idNode.Attributes["value"].Value = domainIdentifier.Identifier;
            }
            else if (idNode.Attributes["value"] != null)
                idNode.Attributes.Remove(idNode.Attributes["value"]);

        }

        /// <summary>
        /// Unconfigure the service node
        /// </summary>
        public void UnConfigure(System.Xml.XmlDocument configurationDom)
        {
        }

        /// <summary>
        /// Returns true if the service is configured
        /// </summary>
        public bool IsConfigured(System.Xml.XmlDocument configurationDom)
        {

            if (this.m_oidRegistrar == null) // Not yet loaded
            {
                this.m_oidRegistrar = OidRegistrarConfigurationPanel.LoadOidRegistrar(configurationDom);
                this.m_panel.OidData = this.m_oidRegistrar;

                XmlElement configSection = configurationDom.SelectSingleNode("//*[local-name() = 'marc.hi.ehrs.svc.core']") as XmlElement;

                if (configSection != null)
                {

                    XmlElement jurisdictionNode = configSection.SelectSingleNode("./*[local-name() = 'jurisdiction']") as XmlElement,
                        custodianNode = configSection.SelectSingleNode("./*[local-name() = 'custodianship']") as XmlElement,
                        systemNode = configSection.SelectSingleNode("./*[local-name() = 'system']") as XmlElement;

                    var configuration = new HostConfigurationSectionHandler().Create(null, this, configSection) as HostConfigurationSectionHandler;

                    if (systemNode != null)
                        this.m_deviceId = new DomainIdentifier() { Identifier = configuration.DeviceName, Domain = configuration.DeviceIdentifier };
                    if (jurisdictionNode != null)
                    {
                        this.m_jurisdictionData = configuration.JurisdictionData;
                        this.m_languageCode = this.m_jurisdictionData.DefaultLanguageCode;
                    }
                    if (custodianNode != null)
                    {
                        this.m_custodianData = configuration.Custodianship;
                    }
                }

                this.m_panel.Jurisdiction = this.m_jurisdictionData;
                this.m_panel.Custodianship = this.m_custodianData;
                this.m_panel.DeviceId = this.m_deviceId;

            }

            return true;
        }

        /// <summary>
        /// Validate the configuration
        /// </summary>
        public bool Validate(System.Xml.XmlDocument configurationDom)
        {
            // Validation , pull from config panel
            this.m_deviceId = this.m_panel.DeviceId;
            this.m_custodianData = this.m_panel.Custodianship;
            this.m_deviceId = this.m_panel.DeviceId;
            this.m_jurisdictionData = this.m_panel.Jurisdiction;
            
            return !String.IsNullOrEmpty(this.m_jurisdictionData.ClientDomain) &&
                !String.IsNullOrEmpty(this.m_jurisdictionData.ProviderDomain) &&
                !String.IsNullOrEmpty(this.m_jurisdictionData.PlaceDomain) &&
                this.m_jurisdictionData.Id != null &&
                !String.IsNullOrEmpty(this.m_jurisdictionData.Name) &&
                this.m_custodianData != null &&
                !String.IsNullOrEmpty(this.m_custodianData.Name) &&
                this.m_deviceId != null;

        }

        #endregion

        public override string ToString()
        {
            return this.Name;
        }
    }
}
