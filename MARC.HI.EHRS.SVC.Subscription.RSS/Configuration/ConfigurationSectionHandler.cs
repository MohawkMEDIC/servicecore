using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using System.Data.Common;
using System.Xml;
using System.Data;
using System.ServiceModel.Syndication;
using System.Reflection;

namespace MARC.HI.EHRS.SVC.Subscription.Data.Configuration
{
    /// <summary>
    /// Configuration for the syndication handler
    /// </summary>
    public class ConfigurationSectionHandler : IConfigurationSectionHandler
    {

        /// <summary>
        /// The DbProviderFactory object that represents the factory to call
        /// in order to create new connections
        /// </summary>
        private DbProviderFactory m_providerFactory;


        /// <summary>
        /// The connection string to use when connecting to the database
        /// </summary>
        private string m_connectionString;

        /// <summary>
        /// Gets or sets the syndication uri
        /// </summary>
        public Uri Address { get; set; }
 
        /// <summary>
        /// Gets or sets the maximum number of records
        /// </summary>
        public int MaximumRecords { get; set; }

        /// <summary>
        /// Creates a new connection to the database
        /// </summary>
        public IDbConnection CreateConnection()
        {
            #if DEBUG
            Trace.TraceInformation("Creating a connection {0}", m_connectionString);
            #endif
            IDbConnection retVal = m_providerFactory.CreateConnection();
            retVal.ConnectionString = m_connectionString;
            return retVal;
        }

        /// <summary>
        /// Create the configuration section
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="configContext"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            // Connection manager configuration
            XmlNode connectionManagerConfig = section.SelectSingleNode("./*[local-name() = 'connectionManager']");
            
            if (connectionManagerConfig == null)
                throw new ConfigurationErrorsException("connection must be specified", section);

            // Connection manager configuration
            if (connectionManagerConfig.Attributes["connection"] != null)
            {
                ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[connectionManagerConfig.Attributes["connection"].Value];
                if (settings == null)
                    throw new ConfigurationErrorsException(String.Format("Cannot find the connection string '{0}'", connectionManagerConfig.Attributes["connection"].Value), connectionManagerConfig);

                // Create the dbProvider and cstring
                m_connectionString = settings.ConnectionString;

                // get the type
                m_providerFactory = DbProviderFactories.GetFactory(settings.ProviderName);
                if (m_providerFactory == null)
                    throw new ConfigurationErrorsException(String.Format("Can't find provider type '{0}'", settings.ProviderName), connectionManagerConfig);
            }
            else
            {
                Trace.TraceError("Cannot determine the connection string settings");
                throw new ConfigurationErrorsException("Cannot determine the connection string to use", connectionManagerConfig);
            }


            // URI
            if (section.Attributes["address"] == null)
                throw new ConfigurationErrorsException("No syndication address was specified");
            else
                this.Address = new Uri(section.Attributes["address"].Value);

            // Max results
            int maxRec = 10;
            if (section.Attributes["maxRecords"] != null && !Int32.TryParse(section.Attributes["maxRecords"].Value, out maxRec))
                throw new ConfigurationErrorsException(String.Format("'{0}' is not a valid value for this parameter", section.Attributes["maxRecords"].Value));
            this.MaximumRecords = maxRec;
               
            return this;
        }


    }
}
