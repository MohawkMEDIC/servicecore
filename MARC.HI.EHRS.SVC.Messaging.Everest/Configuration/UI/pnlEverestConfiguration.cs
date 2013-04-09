using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using MARC.Everest.Connectors;
using System.Security.Cryptography.X509Certificates;

namespace MARC.HI.EHRS.SVC.Messaging.Everest.Configuration.UI
{
    public partial class pnlEverestConfiguration : UserControl
    {

        /// <summary>
        /// Revision configuration panels
        /// </summary>
        public List<pnlTemplateConfigure> RevisionConfigPanels { get; set; }

        public void ClearRevisionPanels()
        {
            this.Controls.Clear();
            this.RevisionConfigPanels.Clear();
        }

        /// <summary>
        /// Add a revision panel
        /// </summary>
        public void AddRevisionPanel(XmlElement wcfConfig, RevisionConfiguration revision)
        {
            string behaviorConfigurationName = String.Empty,
                endpointAddress = string.Empty,
                baseAddress = string.Empty,
                bindingName = string.Empty,
                bindingConfigurationName = string.Empty;

            pnlTemplateConfigure panel = new pnlTemplateConfigure();
            panel.Dock = DockStyle.Top;
            panel.Title = revision.Name;
            panel.IsConfigurationEnabled = true;
            this.Controls.Add(panel);
            panel.BringToFront();
            this.RevisionConfigPanels.Add(panel);

            // Now comes the fun parts, lookup the WCF configuration
            if(revision.Listeners == null || revision.Listeners.Count == 0)
                return; // no point, no listener is enabled
            var wcfListen = revision.Listeners.Find(o=>o.ConnectorType == typeof(MARC.Everest.Connectors.WCF.WcfServerConnector));
            if(wcfListen == null)
                return;
            var connectionString = ConnectionStringParser.ParseConnectionString(wcfListen.ConnectionString);
            List<String> serviceName = null;
            if(!connectionString.TryGetValue("servicename", out serviceName))
                return;

            // Have everything, now load from XML
            XmlElement serviceElement = wcfConfig.SelectSingleNode(String.Format("./*[local-name() = 'services']/*[local-name() = 'service'][@name = '{0}']", serviceName[0])) as XmlElement;
            if (serviceElement == null)
                return;
            if (serviceElement.Attributes["behaviorConfiguration"] != null)
                behaviorConfigurationName = serviceElement.Attributes["behaviorConfiguration"].Value;

            XmlElement endpointElement = serviceElement.SelectSingleNode("./*[local-name() = 'endpoint']") as XmlElement,
                hostElement = serviceElement.SelectSingleNode("./*[local-name() = 'host']/*[local-name() = 'baseAddresses']/*[local-name() = 'add']") as XmlElement;
            if (endpointElement == null)
                return; // invalid WCF config with no ep element
            
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

            panel.Address = endpointAddress;

            // Behavior
            XmlElement behaviorElement = wcfConfig.SelectSingleNode(String.Format("./*[local-name() = 'behaviors']/*[local-name() = 'serviceBehaviors']/*[local-name() = 'behavior'][@name = '{0}']", behaviorConfigurationName)) as XmlElement;
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
                    panel.ServiceDebugEnabled = Boolean.Parse(serviceDebug.Value);
                if (serviceMetaData != null)
                    panel.ServiceMetaDataEnabled = Boolean.Parse(serviceMetaData.Value);
                if (credentials != null)
                {
                    if (credentials.Attributes["storeName"] != null)
                        panel.StoreName = (StoreName)Enum.Parse(typeof(StoreName), credentials.Attributes["storeName"].Value);
                    if(credentials.Attributes["storeLocation"] != null)
                        panel.StoreLocation = (StoreLocation)Enum.Parse(typeof(StoreLocation), credentials.Attributes["storeLocation"].Value);
                    if (credentials.Attributes["findValue"] != null && credentials.Attributes["x509FindType"] != null)
                    {
                        X509FindType findType = (X509FindType)Enum.Parse(typeof(X509FindType), credentials.Attributes["x509FindType"].Value);
                        X509Store store = new X509Store(panel.StoreName, panel.StoreLocation);
                        try
                        {
                            store.Open(OpenFlags.ReadOnly);
                            var certs = store.Certificates.Find(findType, credentials.Attributes["findValue"].Value, false);
                            if(certs.Count == 1)
                                panel.Certificate = certs[0];
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
            XmlElement bindingElement = wcfConfig.SelectSingleNode(String.Format("./*[local-name() = 'bindings']/*[local-name() = '{0}']/*[local-name() = 'binding'][@name = '{1}']", bindingName, bindingConfigurationName)) as XmlElement;
            if (bindingElement != null)
            {
                // Client credentials (ignore the rest)
                XmlElement credentials = bindingElement.SelectSingleNode("./*[local-name() = 'security']/*[local-name() = 'transport']") as XmlElement;
                panel.RequireClientCerts = credentials != null && credentials.Attributes["clientCredentialType"] != null && credentials.Attributes["clientCredentialType"].Value == "Certificate";
            }


        }

        /// <summary>
        /// Add a revision panel
        /// </summary>
        public void AddRevisionPanel(XmlElement wcfConfig, RevisionTemplate template)
        {
            pnlTemplateConfigure panel = new pnlTemplateConfigure();
            panel.Dock = DockStyle.Top;
            panel.Title = template.Name;
            panel.IsConfigurationEnabled = false;
            panel.Address = template.DefaultUrl;
            panel.StoreLocation = StoreLocation.LocalMachine;
            panel.StoreName = StoreName.My;
            this.Controls.Add(panel);
            panel.BringToFront();

            this.RevisionConfigPanels.Add(panel);


            // Binding
            if (template.BindingConfiguration != null)
            {
                // Client credentials (ignore the rest)
                XmlElement credentials = template.BindingConfiguration.SelectSingleNode("./*[local-name() = 'security']/*[local-name() = 'transport']") as XmlElement;
                panel.RequireClientCerts = credentials != null && credentials.Attributes["clientCredentialType"] != null && credentials.Attributes["clientCredentialType"].Value == "Certificate";
            }
        }

        public pnlEverestConfiguration()
        {
            InitializeComponent();
            this.RevisionConfigPanels = new List<pnlTemplateConfigure>();
        }
    }
}
