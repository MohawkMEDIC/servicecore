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

namespace MARC.HI.EHRS.SVC.Messaging.Persistence.Data.Configuration
{
    /// <summary>
    /// Message Persistence Configuration Handler
    /// </summary>
    public class ConfigurationSectionHandler : IConfigurationSectionHandler
    {
        #region IConfigurationSectionHandler Members

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
                if(m_providerFactory == null)
                    throw new ConfigurationErrorsException(String.Format("Can't find provider type '{0}'", settings.ProviderName), connectionManagerConfig);
            }
            else
            {
                Trace.TraceError("Cannot determine the connection string settings");
                throw new ConfigurationErrorsException("Cannot determine the connection string to use", connectionManagerConfig);
            }

            return this;
        }

        #endregion
    }
}
