using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MARC.HI.EHRS.SVC.Configuration.Data
{
    /// <summary>
    /// Configurable data feature
    /// </summary>
    public interface IDataFeature
    {

        /// <summary>
        /// Get the name of the data feature
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Get the SQL required to deploy the feature
        /// </summary>
        String GetDeploySql(String invariantName);

        /// <summary>
        /// Un-deploy sql
        /// </summary>
        String GetUnDeploySql(String invariantName);

        /// <summary>
        /// Get SQL required to determine if feature is installed
        /// </summary>
        String GetCheckSql(String invariantName);
    }
}
