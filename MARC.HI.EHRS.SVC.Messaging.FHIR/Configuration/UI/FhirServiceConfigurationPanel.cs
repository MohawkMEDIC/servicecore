using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.Configuration;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using System.Configuration;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Messaging.FHIR.WcfCore;
using System.Net;
using System.ComponentModel;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Configuration.UI
{
    /// <summary>
    /// Configuration panel for the FHIR service
    /// </summary>
    public class FhirServiceConfigurationPanel : IConfigurationPanel
    {

        // Configuration panel for fhir
        private pnlConfigureFhir m_configurationPanel = new pnlConfigureFhir();

        private bool m_needSync = true;

        /// <summary>
        /// Gets the name of the configuration options
        /// </summary>
        public string Name
        {
            get { return "Messaging/FHIR"; }
        }

        /// <summary>
        /// Gets or sets whether configuration is enabled
        /// </summary>
        public bool EnableConfiguration
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the control panel
        /// </summary>
        public System.Windows.Forms.Control Panel
        {
            get { return this.m_configurationPanel; }
        }

        /// <summary>
        /// Configure the feature
        /// </summary>
        public void Configure(System.Xml.XmlDocument configurationDom)
        {
            if (String.IsNullOrEmpty(this.m_configurationPanel.Address))
                return;

            XmlElement configSectionsNode = configurationDom.SelectSingleNode("//*[local-name() = 'configSections']") as XmlElement,
                fhirNode = configurationDom.SelectSingleNode("//*[local-name() = 'marc.hi.ehrs.svc.messaging.fhir']") as XmlElement,
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
            XmlElement configSectionNode = configSectionsNode.SelectSingleNode("./*[local-name() = 'section'][@name = 'marc.hi.ehrs.svc.messaging.fhir']") as XmlElement;
            if (configSectionNode == null)
            {
                configSectionNode = configurationDom.CreateElement("section");
                configSectionNode.Attributes.Append(configurationDom.CreateAttribute("name"));
                configSectionNode.Attributes.Append(configurationDom.CreateAttribute("type"));
                configSectionNode.Attributes["name"].Value = "marc.hi.ehrs.svc.messaging.fhir";
                configSectionNode.Attributes["type"].Value = typeof(ConfigurationSectionHandler).AssemblyQualifiedName;
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
            if (fhirNode == null)
            {
                fhirNode = configurationDom.CreateElement("marc.hi.ehrs.svc.messaging.fhir");
                configurationDom.DocumentElement.AppendChild(fhirNode);
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
            XmlElement addServiceAsmNode = serviceAssemblyNode.SelectSingleNode("./*[local-name() = 'add'][@assembly = 'MARC.HI.EHRS.SVC.Messaging.FHIR.dll']") as XmlElement,
                addServiceProvNode = serviceProviderNode.SelectSingleNode(String.Format("./*[local-name() = 'add'][@type = '{0}']", (mmhType ?? typeof(FhirMessageHandler)).AssemblyQualifiedName)) as XmlElement;
            if (addServiceAsmNode == null)
            {
                addServiceAsmNode = configurationDom.CreateElement("add");
                addServiceAsmNode.Attributes.Append(configurationDom.CreateAttribute("assembly"));
                addServiceAsmNode.Attributes["assembly"].Value = "MARC.HI.EHRS.SVC.Messaging.FHIR.dll";
                serviceAssemblyNode.AppendChild(addServiceAsmNode);
            }
            if (addServiceProvNode == null)
            {
                addServiceProvNode = configurationDom.CreateElement("add");
                addServiceProvNode.Attributes.Append(configurationDom.CreateAttribute("type"));
                addServiceProvNode.Attributes["type"].Value = (mmhType ?? typeof(FhirMessageHandler)).AssemblyQualifiedName;
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
                if (handlerNode.SelectSingleNode(String.Format("./*[local-name() = 'add'][@type = '{0}']", typeof(FhirMessageHandler).AssemblyQualifiedName)) == null)
                {
                    var addNode = handlerNode.AppendChild(configurationDom.CreateElement("add"));
                    addNode.Attributes.Append(configurationDom.CreateAttribute("type")).Value = typeof(FhirMessageHandler).AssemblyQualifiedName;
                }
            }

            // WCF nodes
            if (wcfNode == null)
                wcfNode = configurationDom.DocumentElement.AppendChild(configurationDom.CreateElement("system.serviceModel")) as XmlElement;

            XmlElement serviceNode = fhirNode.SelectSingleNode("./*[local-name() = 'service']") as XmlElement;
            if (serviceNode == null)
            {
                serviceNode = configurationDom.CreateElement("service");
                fhirNode.AppendChild(serviceNode);
            }

            // Index
            if(!String.IsNullOrEmpty(this.m_configurationPanel.IndexFile))
            {
                if (serviceNode.Attributes["landingPage"] == null)
                    serviceNode.Attributes.Append(configurationDom.CreateAttribute("landingPage"));
                serviceNode.Attributes["landingPage"].Value = this.m_configurationPanel.IndexFile;
            }
            else
                serviceNode.Attributes.Remove(serviceNode.Attributes["landingPage"]);

            WcfServiceConfiguration(serviceNode, wcfNode);

            // Handlers
            XmlElement resourceProcessorNode = fhirNode.SelectSingleNode("./*[local-name() = 'resourceProcessors']") as XmlElement;
            if (resourceProcessorNode == null)
            {
                resourceProcessorNode = configurationDom.CreateElement("resourceProcessors");
                fhirNode.AppendChild(resourceProcessorNode);
            }
            resourceProcessorNode.RemoveAll();

            // Create type registers
            foreach (Type hType in this.m_configurationPanel.Handlers)
            {
                XmlElement addElement = configurationDom.CreateElement("add");
                addElement.Attributes.Append(configurationDom.CreateAttribute("type"));
                addElement.Attributes["type"].Value = hType.AssemblyQualifiedName;
                resourceProcessorNode.AppendChild(addElement);
            }

            // Configure the URL / SSL
            try
            {
                if (this.m_configurationPanel.Address.StartsWith("https:"))
                {
                    Uri address = new Uri(this.m_configurationPanel.Address);
                    // Reserve the SSL certificate on the IP address
                    if (address.HostNameType == UriHostNameType.Dns)
                    {
                        var ipAddresses = Dns.GetHostAddresses(address.Host);
                        HttpSslTool.BindCertificate(ipAddresses[0], address.Port, this.m_configurationPanel.Certificate.GetCertHash(), this.m_configurationPanel.StoreName, this.m_configurationPanel.StoreLocation);
                    }
                    else
                        HttpSslTool.BindCertificate(IPAddress.Parse(address.Host), address.Port, this.m_configurationPanel.Certificate.GetCertHash(), this.m_configurationPanel.StoreName, this.m_configurationPanel.StoreLocation);
                }
            }
            catch (Win32Exception e)
            {
                throw new OperationCanceledException(String.Format("Error binding SSL certificate to address. Error was: {0:x} {1}", e.ErrorCode, e.Message), e);
            }


            this.m_needSync = true;
        }

        /// <summary>
        /// Create a WCF Service configuration node
        /// </summary>
        private void WcfServiceConfiguration(XmlElement serviceNode, XmlElement wcfNode)
        {

            XmlDocument configurationDom = serviceNode.OwnerDocument;

            // Create service name if not exists
            if (serviceNode.Attributes["wcfServiceName"] == null)
            {
                XmlAttribute attribute = serviceNode.Attributes.Append(configurationDom.CreateAttribute("wcfServiceName"));
                attribute.Value = "MARC.HI.EHRS.SVC.Messaging.FHIR.WcfCore.FhirServiceBehavior";
            }
            String serviceName = serviceNode.Attributes["wcfServiceName"].Value;
            
            // Get the service
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
                wcfServiceBehaviorNode.Value = "fhir_Behavior";
            }

            // Create behavior?
            WcfBehaviorConfiguration(wcfNode);

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
            wcfAddAddress.Attributes["baseAddress"].Value = this.m_configurationPanel.Address;


            XmlElement wcfEndpointNode = wcfRevisionServiceNode.SelectSingleNode("./*[local-name() = 'endpoint']") as XmlElement;
            if (wcfEndpointNode == null)
                wcfEndpointNode = wcfRevisionServiceNode.AppendChild(configurationDom.CreateElement("endpoint")) as XmlElement;
            if (wcfEndpointNode.Attributes["address"] == null)
                wcfEndpointNode.Attributes.Append(configurationDom.CreateAttribute("address"));
            wcfEndpointNode.Attributes["address"].Value = this.m_configurationPanel.Address;
            if (wcfEndpointNode.Attributes["contract"] == null)
                wcfEndpointNode.Attributes.Append(configurationDom.CreateAttribute("contract"));
            wcfEndpointNode.Attributes["contract"].Value = typeof(IFhirServiceContract).FullName;

            if (wcfEndpointNode.Attributes["binding"] == null)
                wcfEndpointNode.Attributes.Append(configurationDom.CreateAttribute("binding"));
            wcfEndpointNode.Attributes["binding"].Value = "webHttpBinding";

            // Binding config?
            XmlAttribute wcfBindingConfigurationNode = wcfEndpointNode.Attributes["bindingConfiguration"];
            if (wcfBindingConfigurationNode == null)
            {
                wcfBindingConfigurationNode = wcfEndpointNode.Attributes.Append(configurationDom.CreateAttribute("bindingConfiguration")) as XmlAttribute;
                wcfBindingConfigurationNode.Value = "fhir_Binding";
            }
            WcfBindingConfiguration(wcfNode);


        }

        /// <summary>
        /// Create behavior
        /// </summary>
        private void WcfBehaviorConfiguration(XmlElement wcfNode)
        {
            XmlDocument configurationDom = wcfNode.OwnerDocument;
            XmlElement wcfBehaviorNode = wcfNode.SelectSingleNode("./*[local-name() = 'behaviors']") as XmlElement;
            if (wcfBehaviorNode == null)
                wcfBehaviorNode = wcfNode.AppendChild(configurationDom.CreateElement("behaviors")) as XmlElement;

            XmlElement wcfServiceBehaviorNode = wcfBehaviorNode.SelectSingleNode("./*[local-name() = 'serviceBehaviors']") as XmlElement;
            if (wcfServiceBehaviorNode == null)
                wcfServiceBehaviorNode = wcfBehaviorNode.AppendChild(configurationDom.CreateElement("serviceBehaviors")) as XmlElement;

            XmlElement wcfRevisionBehaviorNode = wcfServiceBehaviorNode.SelectSingleNode("./*[local-name() = 'behavior'][@name = 'fhir_Behavior']") as XmlElement;
            if (wcfRevisionBehaviorNode == null)
            {
                wcfRevisionBehaviorNode = wcfServiceBehaviorNode.AppendChild(configurationDom.CreateElement("behavior")) as XmlElement;
                wcfRevisionBehaviorNode.Attributes.Append(configurationDom.CreateAttribute("name")).Value = "fhir_Behavior";
            }

            // Debug
            XmlElement wcfServiceDebugNode = wcfRevisionBehaviorNode.SelectSingleNode("./*[local-name() = 'serviceDebug']") as XmlElement;
            if (wcfServiceDebugNode == null)
                wcfServiceDebugNode = wcfRevisionBehaviorNode.AppendChild(configurationDom.CreateElement("serviceDebug")) as XmlElement;
            if (wcfServiceDebugNode.Attributes["includeExceptionDetailInFaults"] == null)
                wcfServiceDebugNode.Attributes.Append(configurationDom.CreateAttribute("includeExceptionDetailInFaults"));
            wcfServiceDebugNode.Attributes["includeExceptionDetailInFaults"].Value = this.m_configurationPanel.ServiceDebugEnabled.ToString();

            // Meta-data
            XmlElement wcfMetadataNode = wcfRevisionBehaviorNode.SelectSingleNode("./*[local-name() = 'serviceMetadata']") as XmlElement;
            if (wcfMetadataNode == null)
                wcfMetadataNode = wcfRevisionBehaviorNode.AppendChild(configurationDom.CreateElement("serviceMetadata")) as XmlElement;
            Uri urlSvc = new Uri(this.m_configurationPanel.Address);
            String attrNode = String.Format("{0}GetEnabled", urlSvc.Scheme.ToLower());
            if (wcfMetadataNode.Attributes[attrNode] == null)
                wcfMetadataNode.Attributes.Append(configurationDom.CreateAttribute(attrNode));
            wcfMetadataNode.Attributes[attrNode].Value = this.m_configurationPanel.ServiceMetaDataEnabled.ToString();
            attrNode = String.Format("{0}GetUrl", urlSvc.Scheme.ToLower());
            if (wcfMetadataNode.Attributes[attrNode] == null)
                wcfMetadataNode.Attributes.Append(configurationDom.CreateAttribute(attrNode));
            wcfMetadataNode.Attributes[attrNode].Value = this.m_configurationPanel.Address;

            // Security?
            XmlElement wcfServiceCredentialsNode = wcfRevisionBehaviorNode.SelectSingleNode("./*[local-name() = 'serviceCredentials']") as XmlElement;
            if (attrNode == "httpsGetUrl") // Security is enabled
            {
                if (wcfServiceCredentialsNode == null)
                    wcfServiceCredentialsNode = wcfRevisionBehaviorNode.AppendChild(configurationDom.CreateElement("serviceCredentials")) as XmlElement;
                wcfServiceCredentialsNode.RemoveAll();

                XmlElement wcfServiceCertificateNode = wcfServiceCredentialsNode.AppendChild(configurationDom.CreateElement("serviceCertificate")) as XmlElement;
                wcfServiceCertificateNode.Attributes.Append(configurationDom.CreateAttribute("storeLocation")).Value = this.m_configurationPanel.StoreLocation.ToString();
                wcfServiceCertificateNode.Attributes.Append(configurationDom.CreateAttribute("storeName")).Value = this.m_configurationPanel.StoreName.ToString();
                wcfServiceCertificateNode.Attributes.Append(configurationDom.CreateAttribute("x509FindType")).Value = X509FindType.FindByThumbprint.ToString();
                wcfServiceCertificateNode.Attributes.Append(configurationDom.CreateAttribute("findValue")).Value = this.m_configurationPanel.Certificate.Thumbprint;

                // Client certificates?
                if (this.m_configurationPanel.RequireClientCerts)
                {
                    XmlElement clientCertNode = wcfServiceCredentialsNode.AppendChild(configurationDom.CreateElement("clientCertificate")) as XmlElement,
                        authNode = clientCertNode.AppendChild(configurationDom.CreateElement("authentication")) as XmlElement;
                    authNode.Attributes.Append(configurationDom.CreateAttribute("certificateValidationMode"));
                    authNode.Attributes["certificateValidationMode"].Value = "ChainTrust";
                    authNode.Attributes.Append(configurationDom.CreateAttribute("trustedStoreLocation"));
                    authNode.Attributes["trustedStoreLocation"].Value = "LocalMachine";

                }

            }
            else if (wcfServiceCredentialsNode != null) // Remove the credentials node
                wcfRevisionBehaviorNode.RemoveChild(wcfServiceCredentialsNode);
        }

        /// <summary>
        /// Binding Configuration
        /// </summary>
        private void WcfBindingConfiguration(XmlElement wcfNode)
        {
            XmlDocument configurationDom = wcfNode.OwnerDocument;
            XmlElement wcfBindingNode = wcfNode.SelectSingleNode("./*[local-name() = 'bindings']") as XmlElement;
            if (wcfBindingNode == null)
                wcfBindingNode = wcfNode.AppendChild(configurationDom.CreateElement("bindings")) as XmlElement;

            // Get the binding name
            var bindingType = wcfNode.SelectSingleNode(".//*[local-name() = 'service']//*[local-name() = 'endpoint'][@bindingConfiguration = 'fhir_Binding']/@binding");
            if (bindingType == null)
                throw new ConfigurationErrorsException("Cannot determing the binding for the specified configuration, does the endpoint have the binding attribute?");

            XmlElement wcfBindingTypeNode = wcfBindingNode.SelectSingleNode(String.Format("./*[local-name() = '{0}']", bindingType.Value)) as XmlElement;
            if (wcfBindingTypeNode == null)
                wcfBindingTypeNode = wcfBindingNode.AppendChild(configurationDom.CreateElement(bindingType.Value)) as XmlElement;

            // Is there a binding with our name on it?
            XmlElement wcfBindingConfigurationNode = wcfBindingTypeNode.SelectSingleNode("./*[local-name() = 'binding'][@name = 'fhir_Binding']") as XmlElement;

            if (wcfBindingConfigurationNode == null)
            {
                wcfBindingConfigurationNode = configurationDom.CreateElement("binding");
                wcfBindingTypeNode.AppendChild(wcfBindingConfigurationNode);
            }
                     
            if (wcfBindingConfigurationNode.Attributes["name"] == null)
                wcfBindingConfigurationNode.Attributes.Append(configurationDom.CreateAttribute("name"));
            wcfBindingConfigurationNode.Attributes["name"].Value = "fhir_Binding";

            // Security?
            XmlElement wcfSecurityModeNode = wcfBindingConfigurationNode.SelectSingleNode("./*[local-name() = 'security']") as XmlElement;
            if (wcfSecurityModeNode == null)
                wcfSecurityModeNode = wcfBindingConfigurationNode.AppendChild(configurationDom.CreateElement("security")) as XmlElement;
            if (wcfSecurityModeNode.Attributes["mode"] == null)
                wcfSecurityModeNode.Attributes.Append(configurationDom.CreateAttribute("mode"));

            if (this.m_configurationPanel.Address.ToLower().StartsWith("https"))
            {
                wcfSecurityModeNode.RemoveAll();
                wcfSecurityModeNode.Attributes.Append(configurationDom.CreateAttribute("mode")).Value = "Transport";
                // Transport options
                var wcfTransportNode = wcfSecurityModeNode.AppendChild(configurationDom.CreateElement("transport")) as XmlElement;
                wcfTransportNode.Attributes.Append(configurationDom.CreateAttribute("clientCredentialType")).Value = this.m_configurationPanel.RequireClientCerts ? "Certificate" : "None";
            }
            else
                wcfSecurityModeNode.Attributes["mode"].Value = "None";

        }

        /// <summary>
        /// Un-configure the feature
        /// </summary>
        public void UnConfigure(System.Xml.XmlDocument configurationDom)
        {
            // This is a complex configuration so here we go.
            XmlElement configSectionNode = configurationDom.SelectSingleNode("//*[local-name() = 'configSections']/*[local-name() = 'section'][@name = 'marc.hi.ehrs.svc.messaging.fhir']") as XmlElement,
                configRoot = configurationDom.SelectSingleNode("//*[local-name() = 'marc.hi.ehrs.svc.messaging.fhir']") as XmlElement,
                wcfRoot = configurationDom.SelectSingleNode("//*[local-name() = 'system.serviceModel']") as XmlElement,
                multiNode = configurationDom.SelectSingleNode(String.Format("//*[local-name() = 'marc.hi.ehrs.svc.messaging.multi']//*[local-name() = 'add'][@type = '{0}']", typeof(FhirMessageHandler).AssemblyQualifiedName)) as XmlElement;


            // Remove the sections
            if (configSectionNode != null)
                configSectionNode.ParentNode.RemoveChild(configSectionNode);
            if (configRoot != null)
                configRoot.ParentNode.RemoveChild(configRoot);
            if (multiNode != null)
                multiNode.ParentNode.RemoveChild(multiNode);
            if (wcfRoot != null)
            {
                        X509Certificate2 serviceCert = null;
                        StoreLocation certificateLocation = StoreLocation.LocalMachine;
                        StoreName certificateStore = StoreName.My;
                        // Lookup the service information
                        XmlElement serviceNode = wcfRoot.SelectSingleNode(".//*[local-name() = 'service'][@name = 'MARC.HI.EHRS.SVC.Messaging.FHIR.WcfCore.FhirServiceBehavior']") as XmlElement;
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
                            if (serviceCert != null)
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

            this.m_needSync = true;
        }

        /// <summary>
        /// Determine whether the feature is configured
        /// </summary>
        public bool IsConfigured(System.Xml.XmlDocument configurationDom)
        {
            // This is a complex configuration so here we go.
            XmlElement configSectionNode = configurationDom.SelectSingleNode("//*[local-name() = 'configSections']/*[local-name() = 'section'][@name = 'marc.hi.ehrs.svc.messaging.fhir']") as XmlElement,
                configRoot = configurationDom.SelectSingleNode("//*[local-name() = 'marc.hi.ehrs.svc.messaging.fhir']") as XmlElement,
                wcfRoot = configurationDom.SelectSingleNode("//*[local-name() = 'system.serviceModel']") as XmlElement,
                multiNode = configurationDom.SelectSingleNode(String.Format("//*[local-name() = 'marc.hi.ehrs.svc.messaging.multi']//*[local-name() = 'add'][@type = '{0}']", typeof(FhirMessageHandler).AssemblyQualifiedName)) as XmlElement;

            XmlNodeList revisions = configurationDom.SelectNodes("//*[local-name() = 'marc.hi.ehrs.svc.messaging.fhir']/*[local-name() = 'resourceProcessors']/*[local-name() = 'add']");

            bool isConfigured = configSectionNode != null && configRoot != null &&
                wcfRoot != null && multiNode != null ;
            if (!this.m_needSync)
                return isConfigured;
            this.EnableConfiguration = isConfigured;
            this.m_needSync = false;
            if (configRoot == null) // makes the following logic clearer
                configRoot = configurationDom.CreateElement("marc.hi.ehrs.svc.messaging.fhir");

            FhirServiceConfiguration configuration = new FhirServiceConfiguration(null, null);
            try
            {
                configuration = new ConfigurationSectionHandler().Create(null, null, configRoot) as FhirServiceConfiguration;
            }
            catch { }

            // Load the service model config
            if (wcfRoot == null) return false;
            XmlElement serviceElement = wcfRoot.SelectSingleNode(String.Format("./*[local-name() = 'services']/*[local-name() = 'service'][@name = '{0}']", configuration.WcfEndpoint)) as XmlElement;
            if (serviceElement == null)
                serviceElement = configurationDom.CreateElement("service");

            // Configuration parameters
            String behaviorConfigurationName = null,
                baseAddress = null,
                endpointAddress = null,
                bindingName = null,
                bindingConfigurationName = null;

            if (serviceElement.Attributes["behaviorConfiguration"] != null)
                behaviorConfigurationName = serviceElement.Attributes["behaviorConfiguration"].Value;

            XmlElement endpointElement = serviceElement.SelectSingleNode("./*[local-name() = 'endpoint']") as XmlElement,
                hostElement = serviceElement.SelectSingleNode("./*[local-name() = 'host']/*[local-name() = 'baseAddresses']/*[local-name() = 'add']") as XmlElement;

            // Base address
            if (hostElement != null && hostElement.Attributes["baseAddress"] != null)
                baseAddress = hostElement.Attributes["baseAddress"].Value;
            // EP element
            if (endpointElement != null)
            {
                if (endpointElement.Attributes["address"] != null)
                    endpointAddress = endpointElement.Attributes["address"].Value;
                if (endpointElement.Attributes["binding"] != null)
                    bindingName = endpointElement.Attributes["binding"].Value;
                if (endpointElement.Attributes["bindingConfiguration"] != null)
                    bindingConfigurationName = endpointElement.Attributes["bindingConfiguration"].Value;
            }
            else
            {
                endpointAddress = "http://127.0.0.1:8080/fhir";
                this.m_configurationPanel.StoreLocation = StoreLocation.LocalMachine;
                this.m_configurationPanel.StoreName = StoreName.My;
            }
            this.m_configurationPanel.Address = endpointAddress;

            // Behavior
            XmlElement behaviorElement = wcfRoot.SelectSingleNode(String.Format("./*[local-name() = 'behaviors']/*[local-name() = 'serviceBehaviors']/*[local-name() = 'behavior'][@name = '{0}']", behaviorConfigurationName)) as XmlElement;
            if (behaviorElement != null)
            {
                // Service debug?
                string addrScheme = "http";
                try
                {
                    Uri tUri = new Uri(endpointAddress);
                    addrScheme = tUri.Scheme;
                }
                catch { }

                XmlNode serviceDebug = behaviorElement.SelectSingleNode("./*[local-name() = 'serviceDebug']/@includeExceptionDetailInFaults"),
                    serviceMetaData = behaviorElement.SelectSingleNode(String.Format("./*[local-name() = 'serviceMetadata']/@{0}GetEnabled", addrScheme.ToLower()));
                XmlElement credentials = behaviorElement.SelectSingleNode("./*[local-name() = 'serviceCredentials']/*[local-name() = 'serviceCertificate']") as XmlElement;

                if (serviceDebug != null)
                    this.m_configurationPanel.ServiceDebugEnabled = Boolean.Parse(serviceDebug.Value);
                if (serviceMetaData != null)
                    this.m_configurationPanel.ServiceMetaDataEnabled = Boolean.Parse(serviceMetaData.Value);
                if (credentials != null)
                {
                    if (credentials.Attributes["storeName"] != null)
                        this.m_configurationPanel.StoreName = (StoreName)Enum.Parse(typeof(StoreName), credentials.Attributes["storeName"].Value);
                    if (credentials.Attributes["storeLocation"] != null)
                        this.m_configurationPanel.StoreLocation = (StoreLocation)Enum.Parse(typeof(StoreLocation), credentials.Attributes["storeLocation"].Value);
                    if (credentials.Attributes["findValue"] != null && credentials.Attributes["x509FindType"] != null)
                    {
                        X509FindType findType = (X509FindType)Enum.Parse(typeof(X509FindType), credentials.Attributes["x509FindType"].Value);
                        X509Store store = new X509Store(this.m_configurationPanel.StoreName, this.m_configurationPanel.StoreLocation);
                        try
                        {
                            store.Open(OpenFlags.ReadOnly);
                            var certs = store.Certificates.Find(findType, credentials.Attributes["findValue"].Value, false);
                            if (certs.Count == 1)
                                this.m_configurationPanel.Certificate = certs[0];
                            else
                                MessageBox.Show("Could not locate the specified certificate for endpoint");
                        }
                        finally
                        {
                            store.Close();
                        }
                    }
                }

            }

            // Binding
            XmlElement bindingElement = wcfRoot.SelectSingleNode(String.Format("./*[local-name() = 'bindings']/*[local-name() = '{0}']/*[local-name() = 'binding'][@name = '{1}']", bindingName, bindingConfigurationName)) as XmlElement;
            if (bindingElement != null)
            {
                // Client credentials (ignore the rest)
                XmlElement credentials = bindingElement.SelectSingleNode("./*[local-name() = 'security']/*[local-name() = 'transport']") as XmlElement;
                this.m_configurationPanel.RequireClientCerts = credentials != null && credentials.Attributes["clientCredentialType"] != null && credentials.Attributes["clientCredentialType"].Value == "Certificate";
            }

            this.m_configurationPanel.IndexFile = configuration.LandingPage;

            // Loop through the configuration templates 
            this.m_configurationPanel.Handlers = configuration.ResourceHandlers;

            return isConfigured;
        }

        /// <summary>
        /// Validate the configuration
        /// </summary>
        public bool Validate(System.Xml.XmlDocument configurationDom)
        {
            // Validate the certificate information
            Uri revUrl = new Uri(this.m_configurationPanel.Address);
            if (revUrl.Scheme.ToLower() == "https" && this.m_configurationPanel.Certificate == null)
                throw new ConfigurationErrorsException("Secure addresses (https://) require a certificate to be selected");

            List<String> usedResources = new List<string>();
            foreach (Type t in this.m_configurationPanel.Handlers)
            {
                ResourceProfileAttribute rpa = t.GetCustomAttribute<ResourceProfileAttribute>();
                String resourceName = rpa.Resource.GetCustomAttribute<XmlRootAttribute>().ElementName;
                if (usedResources.Contains(resourceName))
                    throw new ConfigurationErrorsException(String.Format("Resource '{0}' is assigned more than one active handler", resourceName));
                else
                    usedResources.Add(resourceName);
            }
            return true;
        }

        /// <summary>
        /// Get the string representation
        /// </summary>
        public override string ToString()
        {
            return "FHIR Message Handler";
        }
    }
}
