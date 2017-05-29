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
    public interface IDataUpdate
    {

        /// <summary>
        /// Get the name of the data feature
        /// </summary>
        String Name { get; }
        
        /// <summary>
        /// From version
        /// </summary>
        String Description { get; }

        /// <summary>
        /// Get SQL command which, if returns true, results in the update being suggested
        /// </summary>
        String GetCheckSql(String invariantName);

        /// <summary>
        /// Get the SQL command which will install the feature
        /// </summary>
        String GetDeploySql(String invariantName);
    }
}
