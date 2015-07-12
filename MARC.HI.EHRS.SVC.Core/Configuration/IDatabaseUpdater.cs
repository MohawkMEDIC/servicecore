using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using MARC.HI.EHRS.SVC.Core.Configuration.Update;

namespace MARC.HI.EHRS.SVC.Core.Configuration
{
    /// <summary>
    /// Represents an interface which allows plugins to determine if/when
    /// updates are installed and if/when updates should be applied
    /// </summary>
    public interface IDatabaseUpdater : IDatabaseConfigurator
    {
        /// <summary>
        /// Get a list of available updates
        /// </summary>
        List<DbSchemaUpdate> GetUpdates(string connectionStringName, XmlDocument configurationDom);

        /// <summary>
        /// Deploy an update
        /// </summary>
        /// <param name="update">The update to be deployed</param>
        /// <param name="connectionStringName">The connection string to deploy the update</param>
        /// <param name="configurationDom">The configuration file to look for the update</param>
        void DeployUpdate(DbSchemaUpdate update, string connectionStringName, XmlDocument configurationDom);

    }
}
