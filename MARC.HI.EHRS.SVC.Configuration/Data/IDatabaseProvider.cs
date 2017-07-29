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
    /// Connection string
    /// </summary>
    public class DbConnectionString
    {

        /// <summary>
        /// Gets the provider of the connection string
        /// </summary>
        public IDatabaseProvider Provider { get; set; }

        /// <summary>
        /// Gets or sets the name of the configuration string
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the database of the configuration string
        /// </summary>
        public String Database { get; set; }

        /// <summary>
        /// Gets or sets the host of the configuration string
        /// </summary>
        public String Host { get; set; }

        /// <summary>
        /// Gets or sets the username of the configuration string
        /// </summary>
        public String UserName { get; set; }

        /// <summary>
        /// Gets or sets the password of the configuration string
        /// </summary>
        public String Password { get; set; }

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
        /// Gets the db provider type
        /// </summary>
        Type DbProviderType { get; }

        /// <summary>
        /// Deploy a specified feature on the database configuration
        /// </summary>
        void DeployFeature(IDataFeature feature, string connectionStringName, XmlDocument configurationDom);

        /// <summary>
        /// Un-deploy a feature
        /// </summary>
        void UnDeployFeature(IDataFeature feature, string connectionStringName, XmlDocument configurationDom);

        /// <summary>
        /// Create connection string
        /// </summary>
        string CreateConnectionString(DbConnectionString connectionString);

        /// <summary>
        /// Parse a connection string
        /// </summary>
        DbConnectionString ParseConnectionString(String connectionString);

        /// <summary>
        /// Get all databases
        /// </summary>
        string[] GetDatabases(DbConnectionString connection);

        /// <summary>
        /// Create a database
        /// </summary>
        void CreateDatabase(DbConnectionString connectionString, string databaseName, string owner);

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
