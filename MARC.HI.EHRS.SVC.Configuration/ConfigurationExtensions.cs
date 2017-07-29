using MARC.HI.EHRS.SVC.Configuration.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MARC.HI.EHRS.SVC.Configuration
{
    /// <summary>
    /// Configuration extension methods
    /// </summary>
    public static class ConfigurationExtensions
    {

        /// <summary>
        /// Gets or creates the specified element
        /// </summary>
        public static XmlNode GetOrCreateElement(this XmlNode parent, string path)
        {

            var elementMatch = parent.SelectSingleNode(path);
            if (elementMatch == null) // Create
            {
                var ppath = path.Contains("/") ? path.Substring(0, path.LastIndexOf("/")) : path;
                if (ppath == "/") // Root, makes no sense
                    return parent.OwnerDocument.CreateElement(path.Substring(1));
                else
                {
                    elementMatch = (parent as XmlDocument ?? parent.OwnerDocument).CreateElement(path.Substring(ppath.Length + 1));
                    var directParent = parent.GetOrCreateElement(ppath);
                    directParent.AppendChild(elementMatch);
                    return elementMatch;
                }
            }
            else
                return elementMatch;

        }

        /// <summary>
        /// Add section definition
        /// </summary>
        public static XmlElement GetOrAddSectionDefinition(this XmlDocument config, String sectionName, Type sectionHandler)
        {

            var sectionsElement = config.GetOrCreateElement($"/configuration/configSections");

            // Does the section already exist?
            var sectionElement = sectionsElement.SelectSingleNode($"./section[@name='{sectionName}']") as XmlElement;
            if (sectionElement == null) // create
            {
                sectionElement = config.CreateElement("section");
                sectionElement.SetAttribute("name", sectionName);
                sectionsElement.AppendChild(sectionElement);
            }
            sectionElement.SetAttribute("type", sectionHandler.AssemblyQualifiedName);

            // Create the section definition
            return config.GetOrCreateElement($"/configuration/{sectionName}") as XmlElement;
        }

        /// <summary>
        /// Add section definition
        /// </summary>
        public static XmlElement GetSectionDefinition(this XmlDocument config, String sectionName)
        {

            var sectionsElement = config.GetOrCreateElement($"/configuration/configSections");

            // Does the section already exist?
            return sectionsElement.SelectSingleNode($"./section[@name='{sectionName}']") as XmlElement;
        }

        /// <summary>
        /// Get section element
        /// </summary>
        public static XmlElement GetSectionXml(this XmlDocument config, String sectionName)
        {
            return config.SelectSingleNode($"/configuration/{sectionName}") as XmlElement;
        }

        /// <summary>
        /// Get the section according to the processor object
        /// </summary>
        public static Object GetSection(this XmlDocument config, String sectionName)
        {

            var sectionElement = config.GetSectionDefinition(sectionName);
            if (sectionElement == null)
                return null;

            var sectionType = Type.GetType(sectionElement.Attributes["type"].Value);
            if (sectionType == null)
                throw new ConfigurationErrorsException($"Cannot find type described by {sectionElement.Attributes["type"].Value}");

            var instance = Activator.CreateInstance(sectionType) as IConfigurationSectionHandler;
            if (instance == null) return null;
            else
                return instance.Create(null, config, config.GetSectionXml(sectionName));
        }

        /// <summary>
        /// Create attribute value
        /// </summary>
        public static XmlAttribute CreateAttributeValue(this XmlDocument config, String name, String value)
        {
            var retVal = config.CreateAttribute(name);
            retVal.Value = value;
            return retVal;
        }

        /// <summary>
        /// Add data provider
        /// </summary>
        public static void RegisterDataProvider(this XmlDocument config, IDatabaseProvider dbProvider)
        {

            var dbProviderRegistry = config.GetOrCreateElement("/configuration/system.data/DbProviderFactories");

            var providerEntry = dbProviderRegistry.SelectSingleNode($"./add[@invariant='{dbProvider.InvariantName}']");
            if (providerEntry == null)
            {
                providerEntry = config.CreateElement("add");
                providerEntry.Attributes.Append(config.CreateAttributeValue("name", dbProvider.Name));
                providerEntry.Attributes.Append(config.CreateAttributeValue("invariant", dbProvider.InvariantName));
                providerEntry.Attributes.Append(config.CreateAttributeValue("type", dbProvider.DbProviderType.AssemblyQualifiedName));
                dbProviderRegistry.AppendChild(providerEntry);
            }
            
        }

        /// <summary>
        /// Create a connection string in the configuration file
        /// </summary>
        public static String CreateConnectionString(this XmlDocument config, DbConnectionString connectionString)
        {

            config.RegisterDataProvider(connectionString.Provider);

            // First is there already a connectionstring with the specified name?
            if (!String.IsNullOrEmpty(connectionString.Name))
            {
                var existingConnectionString = config.GetConnectionString(connectionString.Name);
                if (existingConnectionString.Provider != null) // update
                {
                    var connectionStrings = config.GetOrCreateElement("/configuration/connectionStrings");
                    var connectionStringXml = config.SelectSingleNode($"./add[@name='{connectionString.Name}']");
                    connectionStringXml.Attributes["connectionString"].Value = connectionString.Provider.CreateConnectionString(connectionString);
                    return connectionString.Name;
                }
            }


            // Anonymous scope
            {
                if (String.IsNullOrEmpty(connectionString.Name))
                    connectionString.Name = new string(Guid.NewGuid().ToByteArray().Select(o => (char)('A' + (char)(o % 20))).ToArray());
                var connectionStrings = config.GetOrCreateElement("/configuration/connectionStrings");
                var connectionStringXml = config.CreateElement("add");
                connectionStringXml.Attributes.Append(config.CreateAttributeValue("name", connectionString.Name));
                connectionStringXml.Attributes.Append(config.CreateAttributeValue("providerName", connectionString.Provider.InvariantName));
                connectionStringXml.Attributes.Append(config.CreateAttributeValue("connectionString", connectionString.Provider.CreateConnectionString(connectionString)));
                connectionStrings.AppendChild(connectionStringXml);
            }
            return connectionString.Name;
        }

        /// <summary>
        /// Gets or adds the service element
        /// </summary>
        public static void RegisterService(this XmlDocument config, Type serviceType)
        {
            var coreElement = config.GetOrAddSectionDefinition("marc.hi.ehrs.svc.core", Type.GetType("MARC.HI.EHRS.SVC.Core.Configuration.HostConfigurationSectionHandler, MARC.HI.EHRS.SVC.Core, Version=2.0.0.0"));
            var servicesElement = config.GetOrCreateElement("/configuration/marc.hi.ehrs.svc.core/serviceProviders");
            var serviceElement = servicesElement.SelectNodes("./add").OfType<XmlElement>().FirstOrDefault(o => o.Attributes["type"].Value.StartsWith(serviceType.FullName));
            if (serviceElement == null)
            {
                serviceElement = config.CreateElement("add");
                serviceElement.Attributes.Append(config.CreateAttributeValue("type", serviceType.AssemblyQualifiedName));
                servicesElement.AppendChild(serviceElement);
            }
            else
                serviceElement.Attributes["type"].Value = serviceType.AssemblyQualifiedName;

        }

        /// <summary>
        /// Get the specified connection string
        /// </summary>
        public static DbConnectionString GetConnectionString(this XmlDocument config, String connectionStringName)
        {
            var connectionStrings= config.GetOrCreateElement("/configuration/connectionStrings");
            var connectionString = config.SelectSingleNode($"./add[@name='{connectionStringName}']");
            if (connectionString == null)
                return null;
            else
            {
                var dbp = DatabaseConfiguratorRegistrar.Configurators.Find(o => o.InvariantName == config.Attributes["providerName"]?.Value);
                if (dbp != null)
                {
                    var retval = dbp.ParseConnectionString(connectionString.Attributes["connectionString"]?.Value);
                    retval.Name = connectionStringName;
                    return retval;
                }
                else
                    return null;
            }
        }

    }
}
