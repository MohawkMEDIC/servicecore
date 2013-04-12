using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using MARC.HI.EHRS.SVC.Core.DataTypes;
using System.IO;
using System.Reflection;

namespace MARC.HI.EHRS.SVC.Core.Configuration.UI
{
    /// <summary>
    /// Configuration panel for the core services
    /// </summary>
    public class OidRegistrarConfigurationPanel : IAlwaysDeployedConfigurationPanel
    {
        #region IConfigurationPanel Members

        private ucOidRegistrarConfiguration m_configPanel = new ucOidRegistrarConfiguration();

        /// <summary>
        /// Creates a new instance of the OID registrar
        /// </summary>
        public OidRegistrarConfigurationPanel()
        {
            
            OidRegistrar.ExtendedAttributes.Add("AssigningAuthorityName", typeof(String));
            OidRegistrar.ExtendedAttributes.Add("OIDType", typeof(HL7IdentifierType));
            OidRegistrar.ExtendedAttributes.Add("HL70396Name", typeof(String));
        }

        /// <summary>
        /// GEts or sets the OID Registrar
        /// </summary>
        public OidRegistrar OidRegistrar { get; set; }

        /// <summary>
        /// Gets the name of the configuration panel
        /// </summary>
        public string Name
        {
            get { return "Service Core/OID Registrar"; }
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
            get { return this.m_configPanel; }
        }

        /// <summary>
        /// Configure the OID registration
        /// </summary>
        public void Configure(System.Xml.XmlDocument configurationDom)
        {

            if (this.OidRegistrar == null)
                this.OidRegistrar = LoadOidRegistrar(configurationDom);

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

            // Add the oid registration section
            XmlElement oidRegNode = coreNode.SelectSingleNode("./*[local-name() = 'registeredOids']") as XmlElement;
            if (oidRegNode == null)
            {
                oidRegNode = configurationDom.CreateElement("registeredOids");
                coreNode.AppendChild(oidRegNode);

                // Import default oid list
                if (File.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "DefaultOids.xml")))
                {
                    XmlDocument oidMerge = new XmlDocument();
                    oidMerge.Load(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "DefaultOids.xml"));
                    // Get all registered oids
                    foreach (XmlElement oidReg in oidMerge.SelectNodes(".//*[local-name() = 'registeredOids']/*[local-name() = 'add']"))
                        oidRegNode.AppendChild(configurationDom.ImportNode(oidReg, true));
                }
            }

            // Add the configured oids
            foreach (var oid in OidRegistrar)
            {
                XmlElement oidElement = oidRegNode.SelectSingleNode(String.Format("./*[local-name() = 'add'][@name = '{0}']", oid.Name)) as XmlElement;
                if (oidElement == null)
                {
                    oidElement = configurationDom.CreateElement("add");
                    oidRegNode.AppendChild(oidElement);
                }
                oidElement.RemoveAll();

                // Add core oid data
                if (oidElement.Attributes["name"] == null)
                    oidElement.Attributes.Append(configurationDom.CreateAttribute("name"));
                oidElement.Attributes["name"].Value = oid.Name;
                if (!String.IsNullOrEmpty(oid.Description))
                {
                    if (oidElement.Attributes["desc"] == null)
                        oidElement.Attributes.Append(configurationDom.CreateAttribute("desc"));
                    oidElement.Attributes["desc"].Value = oid.Description;
                }
                else
                    if (oidElement.Attributes["desc"] != null)
                        oidElement.Attributes.Remove(oidElement.Attributes["desc"]);

                if(!String.IsNullOrEmpty(oid.Oid))
                {
                    if (oidElement.Attributes["oid"] == null)
                        oidElement.Attributes.Append(configurationDom.CreateAttribute("oid"));
                    oidElement.Attributes["oid"].Value = oid.Oid;
                }
               
                if(oid.Ref != null && oid.Ref.Scheme != "oid")
                {
                    if (oidElement.Attributes["ref"] == null)
                        oidElement.Attributes.Append(configurationDom.CreateAttribute("ref"));
                    oidElement.Attributes["ref"].Value = oid.Ref.ToString();
                }
                else
                    if (oidElement.Attributes["ref"] != null)
                        oidElement.Attributes.Remove(oidElement.Attributes["ref"]);

                // Add attributes
                foreach (var attr in oid.Attributes)
                {
                    XmlElement attrElement = oidElement.SelectSingleNode(String.Format("./*[local-name() = 'attribute'][@name = '{0}']", attr.Key)) as XmlElement;
                    if (attrElement == null)
                    {
                        attrElement = configurationDom.CreateElement("attribute");
                        oidElement.AppendChild(attrElement);
                    }
                    
                    // Name
                    if (attrElement.Attributes["name"] == null)
                        attrElement.Attributes.Append(configurationDom.CreateAttribute("name"));
                    attrElement.Attributes["name"].Value = attr.Key;
                    if (!String.IsNullOrEmpty(attr.Value))
                    {
                        if (attrElement.Attributes["value"] == null)
                            attrElement.Attributes.Append(configurationDom.CreateAttribute("value"));
                        attrElement.Attributes["value"].Value = attr.Value;
                    }
                }
            }
        }

        /// <summary>
        /// Un-Configuration the OID registration
        /// </summary>
        public void UnConfigure(System.Xml.XmlDocument configurationDom)
        {
        }

        /// <summary>
        /// Return true if the OID registrar is configured
        /// </summary>
        public bool IsConfigured(System.Xml.XmlDocument configurationDom)
        {
            if(this.OidRegistrar == null)
                this.OidRegistrar = LoadOidRegistrar(configurationDom);
            this.m_configPanel.Oids = this.OidRegistrar;
            return false; // todo:
        }

        public static OidRegistrar LoadOidRegistrar(XmlDocument configurationDom)
        {
            // This is where we can load the configuration
            var retVal = new OidRegistrar();

            XmlElement oidRegNode = configurationDom.SelectSingleNode("//*[local-name() = 'marc.hi.ehrs.svc.core']/*[local-name() = 'registeredOids']") as XmlElement;
            if (oidRegNode == null)
            {
                oidRegNode = configurationDom.CreateElement("registeredOids");

                // Import default oid list
                if (File.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "DefaultOids.xml")))
                {
                    XmlDocument oidMerge = new XmlDocument();
                    oidMerge.Load(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "DefaultOids.xml"));
                    // Get all registered oids
                    foreach (XmlElement oidReg in oidMerge.SelectNodes(".//*[local-name() = 'registeredOids']/*[local-name() = 'add']"))
                        oidRegNode.AppendChild(configurationDom.ImportNode(oidReg, true));
                }
            }

            // Load each oid
            foreach (XmlNode nde in oidRegNode.SelectNodes("./*[local-name() = 'add']"))
            {
                XmlElement oidNode = nde as XmlElement;
                if (oidNode == null) continue; // could not interpret

                // Load
                string name = null, desc = null,
                    refr = null, oid = null;
                if (oidNode.Attributes["name"] != null)
                    name = oidNode.Attributes["name"].Value;
                else
                    continue; // can;t load invalid

                if (oidNode.Attributes["desc"] != null)
                    desc = oidNode.Attributes["desc"].Value;
                if (oidNode.Attributes["ref"] != null)
                    refr = oidNode.Attributes["ref"].Value;
                if (oidNode.Attributes["oid"] != null)
                    oid = oidNode.Attributes["oid"].Value;
                
                // register the oid
                var oidData = retVal.Register(name, oid, desc, refr);

                // Attributes
                foreach (XmlNode attrNode in oidNode.SelectNodes("./*[local-name() = 'attribute']"))
                {
                    string key = null, value = null;
                    if (attrNode.Attributes["name"] != null)
                        key = attrNode.Attributes["name"].Value;
                    else
                        continue; // can't load invalid
                    if (attrNode.Attributes["value"] != null)
                        value = attrNode.Attributes["value"].Value;
                    oidData.Attributes.Add(new KeyValuePair<string, string>(key, value));
                }

            }

            return retVal;
        }

        /// <summary>
        /// Validate the configuration
        /// </summary>
        public bool Validate(System.Xml.XmlDocument configurationDom)
        {
            return true; // todo:
        }

        #endregion

        public override string  ToString()
        {
            return this.Name;
        }
    }
}
