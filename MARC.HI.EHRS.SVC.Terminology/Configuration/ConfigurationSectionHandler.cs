/*
 * Copyright 2012-2013 Mohawk College of Applied Arts and Technology
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
 * Date: 7-5-2012
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection;
using System.Data;

namespace MARC.HI.EHRS.SVC.Terminology.Configuration
{
    /// <summary>
    /// Message Persistence Configuration Handler
    /// </summary>
    public class ConfigurationSectionHandler : IConfigurationSectionHandler
    {
        #region IConfigurationSectionHandler Members

        /// <summary>
        /// CTS Service Url
        /// </summary>
        public string MessageRuntimeUrl { get; private set; }

        /// <summary>
        /// Enable CTS fallback
        /// </summary>
        public bool EnableCtsFallback { get; set; }

        /// <summary>
        /// Maximum cache size
        /// </summary>
        public int MaxMemoryCacheSize { get; set; }

        /// <summary>
        /// Gets the address of the web proxy to channel requests through
        /// </summary>
        public string ProxyAddress { get; private set; }

        /// <summary>
        /// Gets a list of code sets to fill in 
        /// </summary>
        public List<String> FillInCodeSets { get; private set; }

        /// <summary>
        /// Data provider factory
        /// </summary>
        private DbProviderFactory m_providerFactory;

        /// <summary>
        /// Connection string
        /// </summary>
        private string m_connectionString = String.Empty;

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
            XmlNode ctsSectionConfig = section.SelectSingleNode("./*[local-name() = 'cts']"),
                dbSectionConfig = section.SelectSingleNode("./*[local-name() = 'qdcdb']");

            FillInCodeSets = new List<string>();

            // Memory configuration
            if (section.Attributes["maxMemoryCacheSize"] != null)
                this.MaxMemoryCacheSize = int.Parse(section.Attributes["maxMemoryCacheSize"].Value);

            // CTS Configuration
            if (ctsSectionConfig != null)
            {
                if (ctsSectionConfig.Attributes["messageRuntimeUrl"] != null)
                    MessageRuntimeUrl = ctsSectionConfig.Attributes["messageRuntimeUrl"].Value;
                else
                    throw new ConfigurationErrorsException("When configuring CTS a Service URL must be provided via the 'messageRuntimeUrl' attribute");

                if (ctsSectionConfig.Attributes["proxyAddress"] != null)
                    ProxyAddress = ctsSectionConfig.Attributes["proxyAddress"].Value;

                // Next, we want to fill in code sets
                XmlNodeList fillInCodeSets = ctsSectionConfig.SelectNodes(".//*[local-name() = 'fillInDetails']/@codeSystem");
                foreach (XmlNode v in fillInCodeSets)
                    this.FillInCodeSets.Add(v.Value);
            }
            if (dbSectionConfig != null)
            {
                ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[dbSectionConfig.Attributes["connection"].Value];
                if (settings == null)
                    throw new ConfigurationErrorsException(String.Format("Cannot find the connection string '{0}'", dbSectionConfig.Attributes["connection"].Value), dbSectionConfig);

                // Create the dbProvider and cstring
                m_connectionString = settings.ConnectionString;

                if (dbSectionConfig.Attributes["enableCtsFallback"] != null)
                    this.EnableCtsFallback = Convert.ToBoolean(dbSectionConfig.Attributes["enableCtsFallback"].Value);

                // get the type
                m_providerFactory = DbProviderFactories.GetFactory(settings.ProviderName);
                if (m_providerFactory == null)
                    throw new ConfigurationErrorsException(String.Format("Can't find provider type '{0}'", settings.ProviderName), dbSectionConfig);
            }

            return this;
        }

        #endregion
    }
}
