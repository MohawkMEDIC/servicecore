

using MARC.HI.EHRS.SVC.Core.Configuration.DbXml;
/**
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
* Date: 5-12-2012
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MARC.HI.EHRS.SVC.Core.Configuration
{

    /// <summary>
    /// Database configuration registrar
    /// </summary>
    public static class DatabaseConfiguratorRegistrar
    {
        /// <summary>
        /// Gets the list of configurators that are present
        /// </summary>
        public static List<IDatabaseConfigurator> Configurators { get; private set;  }

        /// <summary>
        /// Static constructor for the database configurator registrar
        /// </summary>
        static DatabaseConfiguratorRegistrar()
        {
            Configurators = new List<IDatabaseConfigurator>(10);
        }
    }

    
    /// <summary>
    /// Connection string part type
    /// </summary>
    public enum ConnectionStringPartType
    {
        Host,
        Database,
        UserName,
        Password
    }

    /// <summary>
    /// Represents a database configurator
    /// </summary>
    public interface IDatabaseConfigurator
    {

        /// <summary>
        /// Gets the name 
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the invariant name
        /// </summary>
        string InvariantName { get; }

        /// <summary>
        /// Deploy a specified feature on the database configuration
        /// </summary>
        void DeployFeature(Feature featureName, string connectionStringName, XmlDocument configurationDom);

        /// <summary>
        /// Un-deploy a feature
        /// </summary>
        void UnDeployFeature(Feature featureName, string connectionStringName, XmlDocument configurationDom);
        
        /// <summary>
        /// Create a .config connection string entry
        /// </summary>
        /// <returns>The name of the created connection string</returns>
        string CreateConnectionStringElement(XmlDocument configurationDom, string serverName, string userName, string password, string databaseName);

        /// <summary>
        /// Get a connection string part
        /// </summary>
        string GetConnectionStringElement(XmlDocument configurationDom, ConnectionStringPartType partType, string connectionString);

        /// <summary>
        /// Get all databases
        /// </summary>
        string[] GetDatabases(string serverName, string userName, string password);

        /// <summary>
        /// Create a database
        /// </summary>
        void CreateDatabase(string serverName, string superUser, string password, string databaseName, string owner);

    }
}
