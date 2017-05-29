using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MARC.HI.EHRS.SVC.Configuration.Data
{

    /// <summary>
    /// Database configuration registrar
    /// </summary>
    public static class DatabaseConfiguratorRegistrar
    {
        
        /// <summary>
        /// Gets the list of configurators that are present
        /// </summary>
        public static List<IDatabaseProvider> Configurators { get; private set; }

        /// <summary>
        /// Features to be configured
        /// </summary>
        public static List<IDataFeature> Features { get; private set; }

        /// <summary>
        /// Updates to be configured
        /// </summary>
        public static List<IDataUpdate> Updates { get; private set; }

        /// <summary>
        /// Static constructor for the database configurator registrar
        /// </summary>
        static DatabaseConfiguratorRegistrar()
        {
            Configurators = new List<IDatabaseProvider>(10);
            Updates = new List<IDataUpdate>(10);
            Features = new List<IDataFeature>(10);
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
    /// Database provider which provides a link from a data feature to 
    /// a database software
    /// </summary>
    public interface IDatabaseProvider
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
        void DeployFeature(IDataFeature feature, string connectionStringName, XmlDocument configurationDom);

        /// <summary>
        /// Un-deploy a feature
        /// </summary>
        void UnDeployFeature(IDataFeature feature, string connectionStringName, XmlDocument configurationDom);

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

        /// <summary>
        /// Get a list of available updates
        /// </summary>
        List<IDataUpdate> GetUpdates(string connectionStringName, XmlDocument configurationDom);

        /// <summary>
        /// Deploy an update
        /// </summary>
        /// <param name="update">The update to be deployed</param>
        /// <param name="connectionStringName">The connection string to deploy the update</param>
        /// <param name="configurationDom">The configuration file to look for the update</param>
        void DeployUpdate(IDataUpdate update, string connectionStringName, XmlDocument configurationDom);

    }
}
