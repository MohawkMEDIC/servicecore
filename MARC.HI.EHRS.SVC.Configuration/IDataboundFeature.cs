using MARC.HI.EHRS.SVC.Configuration.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MARC.HI.EHRS.SVC.Configuration
{
    /// <summary>
    /// Databound configuration panel
    /// </summary>
    public interface IDataboundFeature : IConfigurableFeature
    {

        /// <summary>
        /// Gets or sets the name of the system wide connection string
        /// </summary>
        DbConnectionString ConnectionString { get; set; }

        /// <summary>
        /// Gets a list of data features this panel can/will deploy
        /// </summary>
        List<IDataFeature> DataFeatures { get; }

        /// <summary>
        /// Get all updates
        /// </summary>
        List<IDataUpdate> Updates { get; }

        /// <summary>
        /// Call after a deploy is completed
        /// </summary>
        void AfterDeploy();


        /// <summary>
        /// Call after a update is completed
        /// </summary>
        void AfterUpdate();

        /// <summary>
        /// Called after undeploy is complete
        /// </summary>
        void AfterUnDeploy();
    }
}
