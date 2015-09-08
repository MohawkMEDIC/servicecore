using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.Configuration;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using System.Configuration;
using MARC.Everest.Connectors;
using System.Security.Cryptography.X509Certificates;
using MARC.Everest.Connectors.WCF.Core;
using MARC.Everest.Connectors.WCF;
using System.Net;
using System.ComponentModel;
using System.Collections.Specialized;

namespace MARC.HI.EHRS.SVC.Messaging.Everest.Configuration.UI
{
    /// <summary>
    /// Represents the Everest messaging configuration panel
    /// </summary>
    public class EverestConfigurationPanel : IAutoDeployConfigurationPanel
    {

        private pnlEverestConfiguration m_panel = new pnlEverestConfiguration();
        private bool m_needSync = true;

        // Revision templates
        private List<RevisionTemplate> m_everestRevisionTemplates = new List<RevisionTemplate>();

        // Section handler (holds existing config elements)
        private EverestConfigurationSectionHandler m_sectionHandler = new EverestConfigurationSectionHandler();

        /// <summary>
        /// Create a new instance of the EverestConfigurationPanel
        /// </summary>
        public EverestConfigurationPanel()
        {
            String etplPath = Path.Combine(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "config"), "everest");
            XmlSerializer xsz = new XmlSerializer(typeof(RevisionTemplate));
            foreach (var etpFileName in Directory.GetFiles(etplPath))
            {
                try
                {
                    this.m_everestRevisionTemplates.Add(xsz.Deserialize(File.OpenRead(etpFileName)) as RevisionTemplate);
                }
                catch { }
            }

        }
        #region IConfigurationPanel Members

        /// <summary>
        /// Message persistence
        /// </summary>
        public string Name
        {
            get { return "Messaging/Everest"; }
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
            get { return this.m_panel; }
        }

        /// <summary>
        /// Configure the option
        /// </summary>
        public void Configure(System.Xml.XmlDocument configurationDom)
        {

            if (!this.m_panel.RevisionConfigPanels.Exists(o => o.IsConfigurationEnabled))
                return; // No active configurations

            XmlElement configSectionsNode = configurationDom.SelectSingleNode("//*[local-name() = 'configSections']") as XmlElement,
                everestNode = configurationDom.SelectSingleNode("//*[local-name() = 'marc.hi.ehrs.svc.messaging.everest']") as XmlElement,
                multiNode = configurationDom.SelectSingleNode("//*[local-name() = 'marc.hi.ehrs.svc.messaging.multi']") as XmlElement,
                coreNode = configurationDom.SelectSingleNode("//*[local-name() = 'marc.hi.ehrs.svc.core']") as XmlElement,
                wcfNode = configurationDom.SelectSingleNode("//*[local-name() = 'system.serviceModel']") as XmlElement;

            Type mmhType = Type.GetType("MARC.HI.EHRS.SVC.Messaging.Multi.MultiMessageHandler, MARC.HI.EHRS.SVC.Messaging.Multi"),
                mmhConfigType = Type.GetType("MARC.HI.EHRS.SVC.Messaging.Multi.Configuration.ConfigurationSectionHandler, MARC.HI.EHRS.SVC.Messaging.Multi");

            // Ensure the assembly is loaded and the provider registered
            if (coreNode == null)
            {
                coreNode = configurationDom.CreateElement("marc.hi.ehrs.svc.core");
                configurationDom.DocumentElement.AppendChild(coreNode);
            }
            // Config sections node
            if (configSectionsNode == null)
            {
                configSectionsNode = configurationDom.CreateElement("configSections");
                configurationDom.DocumentElement.PrependChild(configSectionsNode);
            }
            XmlElement configSectionNode = configSectionsNode.SelectSingleNode("./*[local-name() = 'section'][@name = 'marc.hi.ehrs.svc.messaging.everest']") as XmlElement;
            if (configSectionNode == null)
            {
                configSectionNode = configurationDom.CreateElement("section");
                configSectionNode.Attributes.Append(configurationDom.CreateAttribute("name"));
                configSectionNode.Attributes.Append(configurationDom.CreateAttribute("type"));
                configSectionNode.Attributes["name"].Value = "marc.hi.ehrs.svc.messaging.everest";
                configSectionNode.Attributes["type"].Value = typeof(EverestConfigurationSectionHandler).AssemblyQualifiedName;
                configSectionsNode.AppendChild(configSectionNode);
            }

            configSectionNode = configSectionsNode.SelectSingleNode("./*[local-name() = 'section'][@name = 'marc.hi.ehrs.svc.messaging.multi']") as XmlElement;
            if (configSectionNode == null && mmhConfigType != null)
            {
                configSectionNode = configurationDom.CreateElement("section");
                configSectionNode.Attributes.Append(configurationDom.CreateAttribute("name"));
                configSectionNode.Attributes.Append(configurationDom.CreateAttribute("type"));
                configSectionNode.Attributes["name"].Value = "marc.hi.ehrs.svc.messaging.multi";
                configSectionNode.Attributes["type"].Value = mmhConfigType.AssemblyQualifiedName;
                configSectionsNode.AppendChild(configSectionNode);
            }

            // Persistence section node
            if (everestNode == null)
            {
                everestNode = configurationDom.CreateElement("marc.hi.ehrs.svc.messaging.everest");
                configurationDom.DocumentElement.AppendChild(everestNode);
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

            // Add service provider (Multi if available, Everest if otherwise)
            XmlElement addServiceAsmNode = serviceAssemblyNode.SelectSingleNode("./*[local-name() = 'add'][@assembly = 'MARC.HI.EHRS.SVC.Messaging.Everest.dll']") as XmlElement,
                addServiceProvNode = serviceProviderNode.SelectSingleNode(String.Format("./*[local-name() = 'add'][@type = '{0}']", (mmhType ?? typeof(MessageHandler)).AssemblyQualifiedName)) as XmlElement;
            if (addServiceAsmNode == null)
            {
                addServiceAsmNode = configurationDom.CreateElement("add");
                addServiceAsmNode.Attributes.Append(configurationDom.CreateAttribute("assembly"));
                addServiceAsmNode.Attributes["assembly"].Value = "MARC.HI.EHRS.SVC.Messaging.Everest.dll";
                serviceAssemblyNode.AppendChild(addServiceAsmNode);
            }
            if (addServiceProvNode == null)
            {
                addServiceProvNode = configurationDom.CreateElement("add");
                addServiceProvNode.Attributes.Append(configurationDom.CreateAttribute("type"));
                addServiceProvNode.Attributes["type"].Value = (mmhType ?? typeof(MessageHandler)).AssemblyQualifiedName;
                serviceProviderNode.AppendChild(addServiceProvNode);
            }

            // Multi-message handler registration?
            if (mmhType != null)
            {
                XmlElement mmhNode = configurationDom.SelectSingleNode(".//*[local-name() = 'marc.hi.ehrs.svc.messaging.multi']") as XmlElement;
                if (mmhNode == null)
                    mmhNode = configurationDom.DocumentElement.AppendChild(configurationDom.CreateElement("marc.hi.ehrs.svc.messaging.multi")) as XmlElement;
                // Handler node
                XmlElement handlerNode = mmhNode.SelectSingleNode("./*[local-name() = 'handlers']") as XmlElement;
                if (handlerNode == null)
                    handlerNode = mmhNode.AppendChild(configurationDom.CreateElement("handlers")) as XmlElement;
                // Add node?
                if (handlerNode.SelectSingleNode(String.Format("./*[local-name() = 'add'][@type = '{0}']", typeof(MessageHandler).AssemblyQualifiedName)) == null)
                {
                    var addNode = handlerNode.AppendChild(configurationDom.CreateElement("add"));
                    addNode.Attributes.Append(configurationDom.CreateAttribute("type")).Value = typeof(MessageHandler).AssemblyQualifiedName;
                }
            }

            // WCF nodes
            if (wcfNode == null)
                wcfNode = configurationDom.DocumentElement.AppendChild(configurationDom.CreateElement("system.serviceModel")) as XmlElement;

            // Loop through enabled revisions and see if we need to configure them
            foreach (var revPanel in this.m_panel.RevisionConfigPanels)
            {
                if (!revPanel.IsConfigurationEnabled)
                    continue;
                // Look for an existing configuration
                XmlElement revEverestConfig = everestNode.SelectSingleNode(String.Format("./*[local-name() = 'revision'][@name = '{0}']", revPanel.Title)) as XmlElement;
                RevisionTemplate tpl = this.m_everestRevisionTemplates.Find(o=>o.Name == revPanel.Title);
                // new config?
                if (revEverestConfig == null)
                    revEverestConfig = everestNode.AppendChild(configurationDom.ImportNode(tpl.EverestConfiguration, true)) as XmlElement;
                
                // Listener node
                XmlElement listenNode = revEverestConfig.SelectSingleNode("./*[local-name() = 'listen'][@type = 'MARC.Everest.Connectors.WCF.WcfServerConnector, MARC.Everest.Connectors.WCF, Version=1.1.0.0, Culture=neutral, PublicKeyToken=99dcf2dae6474efd']") as XmlElement;
                if (listenNode == null)
                {
                    listenNode = everestNode.AppendChild(configurationDom.CreateElement("listen")) as XmlElement;
                    listenNode.Attributes.Append(configurationDom.CreateAttribute("type")).Value = "MARC.Everest.Connectors.WCF.WcfServerConnector, MARC.Everest.Connectors.WCF, Version=1.1.0.0, Culture=neutral, PublicKeyToken=99dcf2dae6474efd";
                }

                // Get the connection string
                var connectionStringNode = listenNode.Attributes["connectionString"];
                if (connectionStringNode == null) // load existing
                    connectionStringNode = listenNode.Attributes.Append(configurationDom.CreateAttribute("connectionString")) as XmlAttribute;
                string connectionString = connectionStringNode.Value;

                // Now look for a service with the specified name
                var connectionStringContents = ConnectionStringParser.ParseConnectionString(connectionStringNode.Value);
                List<string> serviceName = null;
                if (!connectionStringContents.TryGetValue("serviceName", out serviceName))
                {
                    serviceName = new List<string>() { Guid.NewGuid().ToString().Substring(0, 7) };
                    connectionString = String.Format("serviceName={1};{0}", connectionString, serviceName[0]);
                }
                connectionStringNode.Value = connectionString; // update config file after changes

                WcfServiceConfiguration(serviceName[0], revPanel, wcfNode);

                // Configure the URL / SSL
                try
                {
                    if (revPanel.Address.StartsWith("https:"))
                    {
                        Uri address = new Uri(revPanel.Address);
                        // Reserve the SSL certificate on the IP address
                        if (address.HostNameType == UriHostNameType.Dns)
                        {
                            var ipAddresses = Dns.GetHostAddresses(address.Host);
                            HttpSslTool.BindCertificate(ipAddresses[0], address.Port, revPanel.Certificate.GetCertHash(), revPanel.StoreName, revPanel.StoreLocation);
                        }
                        else
                            HttpSslTool.BindCertificate(IPAddress.Parse(address.Host), address.Port, revPanel.Certificate.GetCertHash(), revPanel.StoreName, revPanel.StoreLocation);
                    }
                }
                catch (Win32Exception e)
                {
                    throw new OperationCanceledException(String.Format("Error binding SSL certificate to address. Error was: {0:x} {1}", e.ErrorCode, e.Message), e);
                }
            }


            this.m_needSync = true;
        }

        /// <summary>
        /// Create a WCF Service configuration node
        /// </summary>
        private void WcfServiceConfiguration(string serviceName, pnlTemplateConfigure revPanel, XmlElement wcfNode)
        {
            
            XmlDocument configurationDom = wcfNode.OwnerDocument;
            XmlElement wcfServiceNode = wcfNode.SelectSingleNode("./*[local-name() = 'services']") as XmlElement;
            if (wcfServiceNode == null)
                wcfServiceNode = wcfNode.AppendChild(configurationDom.CreateElement("services")) as XmlElement;
            XmlElement wcfRevisionServiceNode = wcfServiceNode.SelectSingleNode(string.Format("./*[local-name() = 'service'][@name = '{0}']", serviceName[0])) as XmlElement;


            if (wcfRevisionServiceNode == null)
            {
                wcfRevisionServiceNode = wcfServiceNode.AppendChild(configurationDom.CreateElement("service")) as XmlElement;
                wcfRevisionServiceNode.Attributes.Append(configurationDom.CreateAttribute("name")).Value = serviceName;
            }

            // Behavior config?
            XmlAttribute wcfServiceBehaviorNode = wcfRevisionServiceNode.Attributes["behaviorConfiguration"];
            if (wcfServiceBehaviorNode == null)
            {
                wcfServiceBehaviorNode = wcfRevisionServiceNode.Attributes.Append(configurationDom.CreateAttribute("behaviorConfiguration")) as XmlAttribute;
                wcfServiceBehaviorNode.Value = String.Format("{0}_Behavior", serviceName);
            }

            // Create behavior?
            WcfBehaviorConfiguration(wcfServiceBehaviorNode.Value, revPanel, wcfNode);

            // Host element?
            XmlElement wcfHostElement = wcfRevisionServiceNode.SelectSingleNode("./*[local-name() = 'host']") as XmlElement;
            if (wcfHostElement == null)
                wcfHostElement = wcfRevisionServiceNode.AppendChild(configurationDom.CreateElement("host")) as XmlElement;
            XmlElement wcfBaseElement = wcfHostElement.SelectSingleNode("./*[local-name() = 'baseAddresses']") as XmlElement;
            if (wcfBaseElement == null)
                wcfBaseElement = wcfHostElement.AppendChild(configurationDom.CreateElement("baseAddresses")) as XmlElement;
            XmlElement wcfAddAddress = wcfBaseElement.SelectSingleNode("./*[local-name() = 'add']") as XmlElement;
            if (wcfAddAddress == null)
                wcfAddAddress = wcfBaseElement.AppendChild(configurationDom.CreateElement("add")) as XmlElement;
            if (wcfAddAddress.Attributes["baseAddress"] == null)
                wcfAddAddress.Attributes.Append(configurationDom.CreateAttribute("baseAddress"));
            wcfAddAddress.Attributes["baseAddress"].Value = revPanel.Address;

            // Endpoint element
            RevisionTemplate tpl = this.m_everestRevisionTemplates.Find(o => o.Name == revPanel.Title);
            
            XmlElement wcfEndpointNode = wcfRevisionServiceNode.SelectSingleNode("./*[local-name() = 'endpoint']") as XmlElement;
            if (wcfEndpointNode == null)
                wcfEndpointNode = wcfRevisionServiceNode.AppendChild(configurationDom.CreateElement("endpoint")) as XmlElement;
            if (wcfEndpointNode.Attributes["address"] == null)
                wcfEndpointNode.Attributes.Append(configurationDom.CreateAttribute("address"));
            wcfEndpointNode.Attributes["address"].Value = revPanel.Address;
            if (wcfEndpointNode.Attributes["contract"] == null)
                wcfEndpointNode.Attributes.Append(configurationDom.CreateAttribute("contract"));
            wcfEndpointNode.Attributes["contract"].Value = typeof(IConnectorContract).FullName ;

            if (tpl != null)
            {
                if (wcfEndpointNode.Attributes["binding"] == null)
                    wcfEndpointNode.Attributes.Append(configurationDom.CreateAttribute("binding"));
                wcfEndpointNode.Attributes["binding"].Value = tpl.WcfBindingType;
            }

            // Binding config?
            XmlAttribute wcfBindingConfigurationNode = wcfEndpointNode.Attributes["bindingConfiguration"];
            if (wcfBindingConfigurationNode == null)
            {
                wcfBindingConfigurationNode = wcfEndpointNode.Attributes.Append(configurationDom.CreateAttribute("bindingConfiguration")) as XmlAttribute;
                wcfBindingConfigurationNode.Value = String.Format("{0}_Binding", serviceName);
            }
            WcfBindingConfiguration(wcfBindingConfigurationNode.Value, revPanel, wcfNode);


        }

        /// <summary>
        /// Create behavior
        /// </summary>
        private void WcfBehaviorConfiguration(string behaviorName, pnlTemplateConfigure revPanel, XmlElement wcfNode)
        {
            XmlDocument configurationDom = wcfNode.OwnerDocument;
            XmlElement wcfBehaviorNode = wcfNode.SelectSingleNode("./*[local-name() = 'behaviors']") as XmlElement;
            if (wcfBehaviorNode == null)
                wcfBehaviorNode = wcfNode.AppendChild(configurationDom.CreateElement("behaviors")) as XmlElement;
            
            XmlElement wcfServiceBehaviorNode = wcfBehaviorNode.SelectSingleNode("./*[local-name() = 'serviceBehaviors']") as XmlElement;
            if (wcfServiceBehaviorNode == null)
                wcfServiceBehaviorNode = wcfBehaviorNode.AppendChild(configurationDom.CreateElement("serviceBehaviors")) as XmlElement;

            XmlElement wcfRevisionBehaviorNode = wcfServiceBehaviorNode.SelectSingleNode(String.Format("./*[local-name() = 'behavior'][@name = '{0}']", behaviorName)) as XmlElement;
            if (wcfRevisionBehaviorNode == null)
            {
                wcfRevisionBehaviorNode = wcfServiceBehaviorNode.AppendChild(configurationDom.CreateElement("behavior")) as XmlElement;
                wcfRevisionBehaviorNode.Attributes.Append(configurationDom.CreateAttribute("name")).Value = behaviorName;
            }

            // Debug
            XmlElement wcfServiceDebugNode = wcfRevisionBehaviorNode.SelectSingleNode("./*[local-name() = 'serviceDebug']") as XmlElement;
            if (wcfServiceDebugNode == null)
                wcfServiceDebugNode = wcfRevisionBehaviorNode.AppendChild(configurationDom.CreateElement("serviceDebug")) as XmlElement;
            if (wcfServiceDebugNode.Attributes["includeExceptionDetailInFaults"] == null)
                wcfServiceDebugNode.Attributes.Append(configurationDom.CreateAttribute("includeExceptionDetailInFaults"));
            wcfServiceDebugNode.Attributes["includeExceptionDetailInFaults"].Value = revPanel.ServiceDebugEnabled.ToString();

            // Meta-data
            XmlElement wcfMetadataNode = wcfRevisionBehaviorNode.SelectSingleNode("./*[local-name() = 'serviceMetadata']") as XmlElement;
            if (wcfMetadataNode == null)
                wcfMetadataNode = wcfRevisionBehaviorNode.AppendChild(configurationDom.CreateElement("serviceMetadata")) as XmlElement;
            Uri urlSvc = new Uri(revPanel.Address);
            String attrNode = String.Format("{0}GetEnabled", urlSvc.Scheme.ToLower());
            if (wcfMetadataNode.Attributes[attrNode] == null)
                wcfMetadataNode.Attributes.Append(configurationDom.CreateAttribute(attrNode));
            wcfMetadataNode.Attributes[attrNode].Value = revPanel.ServiceMetaDataEnabled.ToString();
            attrNode = String.Format("{0}GetUrl", urlSvc.Scheme.ToLower());
            if(wcfMetadataNode.Attributes[attrNode] == null)
                wcfMetadataNode.Attributes.Append(configurationDom.CreateAttribute(attrNode));
            wcfMetadataNode.Attributes[attrNode].Value = revPanel.Address;

            // Security?
            XmlElement wcfServiceCredentialsNode = wcfRevisionBehaviorNode.SelectSingleNode("./*[local-name() = 'serviceCredentials']") as XmlElement;
            if (attrNode == "httpsGetUrl") // Security is enabled
            {
                if (wcfServiceCredentialsNode == null)
                    wcfServiceCredentialsNode = wcfRevisionBehaviorNode.AppendChild(configurationDom.CreateElement("serviceCredentials")) as XmlElement;
                wcfServiceCredentialsNode.RemoveAll();

                XmlElement wcfServiceCertificateNode = wcfServiceCredentialsNode.AppendChild(configurationDom.CreateElement("serviceCertificate")) as XmlElement;
                wcfServiceCertificateNode.Attributes.Append(configurationDom.CreateAttribute("storeLocation")).Value = revPanel.StoreLocation.ToString();
                wcfServiceCertificateNode.Attributes.Append(configurationDom.CreateAttribute("storeName")).Value = revPanel.StoreName.ToString();
                wcfServiceCertificateNode.Attributes.Append(configurationDom.CreateAttribute("x509FindType")).Value = X509FindType.FindByThumbprint.ToString();
                wcfServiceCertificateNode.Attributes.Append(configurationDom.CreateAttribute("findValue")).Value = revPanel.Certificate.Thumbprint;

                // Client certificates?
                if (revPanel.RequireClientCerts)
                {
                    XmlElement clientCertNode = wcfServiceCredentialsNode.AppendChild(configurationDom.CreateElement("clientCertificate")) as XmlElement,
                        authNode = clientCertNode.AppendChild(configurationDom.CreateElement("authentication")) as XmlElement;
                    authNode.Attributes.Append(configurationDom.CreateAttribute("certificateValidationMode"));
                    authNode.Attributes["certificateValidationMode"].Value = "ChainTrust";
                    authNode.Attributes.Append(configurationDom.CreateAttribute("trustedStoreLocation"));
                    authNode.Attributes["trustedStoreLocation"].Value ="LocalMachine";

                }

            }
            else if (wcfServiceCredentialsNode != null) // Remove the credentials node
                wcfRevisionBehaviorNode.RemoveChild(wcfServiceCredentialsNode);
        }

        /// <summary>
        /// Binding Configuration
        /// </summary>
        private void WcfBindingConfiguration(string bindingName, pnlTemplateConfigure revPanel, XmlElement wcfNode)
        {
            XmlDocument configurationDom = wcfNode.OwnerDocument;
            XmlElement wcfBindingNode = wcfNode.SelectSingleNode("./*[local-name() = 'bindings']") as XmlElement;
            if (wcfBindingNode == null)
                wcfBindingNode = wcfNode.AppendChild(configurationDom.CreateElement("bindings")) as XmlElement;

            // Get the binding name
            var bindingType = wcfNode.SelectSingleNode(string.Format(".//*[local-name() = 'service']//*[local-name() = 'endpoint'][@bindingConfiguration = '{0}']/@binding", bindingName));
            if(bindingType == null)
                throw new ConfigurationErrorsException("Cannot determing the binding for the specified configuration, does the endpoint have the binding attribute?");
            
            XmlElement wcfBindingTypeNode = wcfBindingNode.SelectSingleNode(String.Format("./*[local-name() = '{0}']", bindingType.Value)) as XmlElement;
            if(wcfBindingTypeNode == null)
                wcfBindingTypeNode = wcfBindingNode.AppendChild(configurationDom.CreateElement(bindingType.Value)) as XmlElement;

            // Is there a binding with our name on it?
            XmlElement wcfBindingConfigurationNode = wcfBindingTypeNode.SelectSingleNode(string.Format("./*[local-name() = 'binding'][@name = '{0}']", bindingName))as XmlElement;
            if(wcfBindingConfigurationNode == null)
            {
                // Template?
                RevisionTemplate tpl = this.m_everestRevisionTemplates.Find(o=>o.Name == revPanel.Title);
                if(tpl != null)
                    wcfBindingConfigurationNode = wcfBindingTypeNode.AppendChild(configurationDom.ImportNode(tpl.BindingConfiguration, true)) as XmlElement;
                else
                    wcfBindingConfigurationNode = wcfBindingTypeNode.AppendChild(configurationDom.CreateElement("binding")) as XmlElement;
            }
            if (wcfBindingConfigurationNode.Attributes["name"] == null)
                wcfBindingConfigurationNode.Attributes.Append(configurationDom.CreateAttribute("name"));
            wcfBindingConfigurationNode.Attributes["name"].Value = bindingName;

            // Security?
            XmlElement wcfSecurityModeNode = wcfBindingConfigurationNode.SelectSingleNode("./*[local-name() = 'security']") as XmlElement;
            if(wcfSecurityModeNode == null)
                wcfSecurityModeNode = wcfBindingConfigurationNode.AppendChild(configurationDom.CreateElement("security")) as XmlElement;
            if(wcfSecurityModeNode.Attributes["mode"] == null)
                wcfSecurityModeNode.Attributes.Append(configurationDom.CreateAttribute("mode"));

            if(revPanel.Address.ToLower().StartsWith("https"))
            {
                wcfSecurityModeNode.RemoveAll();
                wcfSecurityModeNode.Attributes.Append(configurationDom.CreateAttribute("mode")).Value = "Transport";
                // Transport options
                var wcfTransportNode = wcfSecurityModeNode.AppendChild(configurationDom.CreateElement("transport"))as XmlElement;
                wcfTransportNode.Attributes.Append(configurationDom.CreateAttribute("clientCredentialType")).Value = revPanel.RequireClientCerts ? "Certificate" : "None";
            }
            else
                wcfSecurityModeNode.Attributes["mode"].Value = "None";

        }

        /// <summary>
        /// Unconfigure the option
        /// </summary>
        public void UnConfigure(System.Xml.XmlDocument configurationDom)
        {

            // This is a complex configuration so here we go.
            XmlElement configSectionNode = configurationDom.SelectSingleNode("//*[local-name() = 'configSections']/*[local-name() = 'section'][@name = 'marc.hi.ehrs.svc.messaging.everest']") as XmlElement,
                configRoot = configurationDom.SelectSingleNode("//*[local-name() = 'marc.hi.ehrs.svc.messaging.everest']") as XmlElement,
                wcfRoot = configurationDom.SelectSingleNode("//*[local-name() = 'system.serviceModel']") as XmlElement,
                multiNode = configurationDom.SelectSingleNode(String.Format("//*[local-name() = 'marc.hi.ehrs.svc.messaging.multi']//*[local-name() = 'add'][@type = '{0}']", typeof(MessageHandler).AssemblyQualifiedName)) as XmlElement;

            
            // Remove the sections
            if (configSectionNode != null)
                configSectionNode.ParentNode.RemoveChild(configSectionNode);
            if (configRoot != null)
                configRoot.ParentNode.RemoveChild(configRoot);
            if (multiNode != null)
                multiNode.ParentNode.RemoveChild(multiNode);
            if (wcfRoot != null && this.m_sectionHandler.Revisions != null)
            {
                // Remove each WCF configuration
                foreach (var rev in this.m_sectionHandler.Revisions)
                    foreach(var listnr in rev.Listeners)
                    {
                        X509Certificate2 serviceCert = null;
                        StoreLocation certificateLocation = StoreLocation.LocalMachine;
                        StoreName certificateStore = StoreName.My;
                        if (listnr.ConnectorType != typeof(WcfServerConnector)) continue;
                        var connectionString = ConnectionStringParser.ParseConnectionString(listnr.ConnectionString);
                        var serviceName = connectionString["servicename"][0];
                        // Lookup the service information
                        XmlElement serviceNode = wcfRoot.SelectSingleNode(String.Format(".//*[local-name() = 'service'][@name = '{0}']", serviceName)) as XmlElement;
                        if (serviceNode == null) continue;
                        if (serviceNode.Attributes["behaviorConfiguration"] != null)
                        {
                            XmlElement behavior = wcfRoot.SelectSingleNode(String.Format(".//*[local-name() = 'behavior'][@name = '{0}']", serviceNode.Attributes["behaviorConfiguration"].Value)) as XmlElement;
                            if (behavior != null)
                            {
                                XmlElement serviceCertificateNode = behavior.SelectSingleNode(".//*[local-name() = 'serviceCertificate']") as XmlElement;
                                if (serviceCertificateNode != null)
                                {
                                    certificateStore = (StoreName)Enum.Parse(typeof(StoreName), serviceCertificateNode.Attributes["storeName"].Value);
                                    certificateLocation = (StoreLocation)Enum.Parse(typeof(StoreLocation), serviceCertificateNode.Attributes["storeLocation"].Value);
                                    X509Store store = new X509Store(
                                        certificateStore,
                                        certificateLocation    
                                    );
                                    try
                                    {
                                        store.Open(OpenFlags.ReadOnly);
                                        var cert = store.Certificates.Find((X509FindType)Enum.Parse(typeof(X509FindType), serviceCertificateNode.Attributes["x509FindType"].Value), serviceCertificateNode.Attributes["findValue"].Value, false);
                                        if (cert.Count > 0)
                                            serviceCert = cert[0];
                                    }
                                    catch (System.Exception e)
                                    {
                                        MessageBox.Show("Cannot retrieve certification information");
                                    }
                                    finally
                                    {
                                        store.Close();
                                    }
                                }

                                behavior.ParentNode.RemoveChild(behavior);
                            }
                        }

                        // Remove the bindings
                        XmlNodeList endpoints = serviceNode.SelectNodes(".//*[local-name() = 'endpoint']");
                        foreach (XmlElement ep in endpoints)
                        {
                            if (ep.Attributes["bindingConfiguration"] != null)
                            {
                                var binding = wcfRoot.SelectSingleNode(String.Format(".//*[local-name() = 'binding'][@name = '{0}']", ep.Attributes["bindingConfiguration"].Value)) as XmlElement;

                                if (binding != null)
                                    binding.ParentNode.RemoveChild(binding);
                            }
                            
                            // Un-bind the certificate
                            if(serviceCert != null)
                            {
                                Uri address = new Uri(ep.Attributes["address"].Value);
                                // Reserve the SSL certificate on the IP address
                                if (address.HostNameType == UriHostNameType.Dns)
                                {
                                    var ipAddresses = Dns.GetHostAddresses(address.Host);
                                    HttpSslTool.RemoveCertificate(ipAddresses[0], address.Port, serviceCert.GetCertHash(), certificateStore, certificateLocation);
                                }
                                else
                                    HttpSslTool.RemoveCertificate(IPAddress.Parse(address.Host), address.Port, serviceCert.GetCertHash(), certificateStore, certificateLocation);
                            }
                        }
                        serviceNode.ParentNode.RemoveChild(serviceNode);


                    }
            }

            this.m_needSync = true;
        }

        /// <summary>
        /// Determine if the option is configured
        /// </summary>
        public bool IsConfigured(System.Xml.XmlDocument configurationDom)
        {
            // This is a complex configuration so here we go.
            XmlElement configSectionNode = configurationDom.SelectSingleNode("//*[local-name() = 'configSections']/*[local-name() = 'section'][@name = 'marc.hi.ehrs.svc.messaging.everest']") as XmlElement,
                configRoot = configurationDom.SelectSingleNode("//*[local-name() = 'marc.hi.ehrs.svc.messaging.everest']") as XmlElement,
                wcfRoot = configurationDom.SelectSingleNode("//*[local-name() = 'system.serviceModel']") as XmlElement,
                multiNode = configurationDom.SelectSingleNode(String.Format("//*[local-name() = 'marc.hi.ehrs.svc.messaging.multi']//*[local-name() = 'add'][@type = '{0}']", typeof(MessageHandler).AssemblyQualifiedName)) as XmlElement;

            XmlNodeList revisions = configurationDom.SelectNodes("//*[local-name() = 'marc.hi.ehrs.svc.messaging.everest']/*[local-name() = 'revision']");

            // Load the current config if applicable
            if (configRoot != null)
                this.m_sectionHandler = new EverestConfigurationSectionHandler().Create(null, null, configRoot) as EverestConfigurationSectionHandler;
            else
                this.m_sectionHandler = new EverestConfigurationSectionHandler();

            bool isConfigured = configSectionNode != null && configRoot != null &&
                wcfRoot != null && this.m_sectionHandler != null && this.m_sectionHandler.Revisions != null && this.m_sectionHandler.Revisions.Count > 0 && multiNode != null;
            if (!this.m_needSync)
                return isConfigured;
            this.EnableConfiguration = isConfigured;

            this.m_needSync = false;
            this.m_panel.ClearRevisionPanels();

            if (configRoot == null) // makes the following logic clearer
                configRoot = configurationDom.CreateElement("marc.hi.ehrs.svc.messaging.everest");

            // Loop through the configuration templates 
            var tRevisionList = new List<RevisionConfiguration>(this.m_sectionHandler.Revisions ?? new List<RevisionConfiguration>());
            foreach (var tpl in this.m_everestRevisionTemplates)
            {
                XmlElement revConfig = configRoot.SelectSingleNode(tpl.InstallationCheckXPath) as XmlElement;
                if (revConfig != null)
                {
                    var revisionConfigObj = this.m_sectionHandler.Revisions.Find(o => o.Name == revConfig.Attributes["name"].Value);
                    this.m_panel.AddRevisionPanel(wcfRoot, revisionConfigObj);
                    tRevisionList.Remove(revisionConfigObj);
                }
                else
                    this.m_panel.AddRevisionPanel(wcfRoot, tpl);
            }
            // Loop through the remaining (unrecognized) configurations
            foreach (var cnf in tRevisionList)
                this.m_panel.AddRevisionPanel(wcfRoot, cnf);

            return isConfigured;

        }

        /// <summary>
        /// Validate the configuration options
        /// </summary>
        public bool Validate(System.Xml.XmlDocument configurationDom)
        {

            List<String> usedEndpoints = new List<string>();
            // Validate the configuration of this panel 
            foreach (var rev in this.m_panel.RevisionConfigPanels)
            {
                if (!rev.IsConfigurationEnabled)
                    continue;

                // Validate the endpoints
                if(usedEndpoints.Contains(rev.Address))
                    throw new ConfigurationErrorsException(String.Format("Address {0} is used on more than one handler", rev.Address));
                usedEndpoints.Add(rev.Address);

                // Validate the certificate information
                Uri revUrl = new Uri(rev.Address);
                if (revUrl.Scheme.ToLower() == "https" && rev.Certificate == null)
                    throw new ConfigurationErrorsException("Secure addresses (https://) require a certificate to be selected");

            }

            return true;
        }

        public override string ToString()
        {
            return "MARC-HI Everest Framework Listeners";
        }
        #endregion


        #region IAutoDeployConfigurationPanel Members

        /// <summary>
        /// Deploy with the specified options
        /// </summary>
        public void PrepareConfigure(XmlDocument configurationDom, Dictionary<string, StringCollection> deploymentOptions)
        {
            // Look for the templates
            StringCollection templateNames = new StringCollection();
            if (!deploymentOptions.TryGetValue("etpl", out templateNames))
                return;

            this.EnableConfiguration = true;

            foreach(var rev in this.m_panel.RevisionConfigPanels)
                if (templateNames.Contains(rev.Title))
                    rev.IsConfigurationEnabled = true;

        }

        #endregion

    }
}
