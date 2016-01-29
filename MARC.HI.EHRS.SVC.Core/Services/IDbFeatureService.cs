using MARC.HI.EHRS.SVC.Core.Configuration.DbXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MARC.HI.EHRS.SVC.Core.Services
{
    /// <summary>
    /// Represents a service which is responsible for maintaining feature sets installed
    /// </summary>
    public interface IDbFeatureService
    {

        /// <summary>
        /// Determines whether a feature is installed
        /// </summary>
        bool IsFeatureInstalled(Feature feature);

        /// <summary>
        /// Get the installed features
        /// </summary>
        IEnumerable<Feature> GetInstalledFeature();

        /// <summary>
        /// Register that a feature has been installed
        /// </summary>
        void RegisterFeature(Feature feature);

        /// <summary>
        /// Deletes a feature from the feature store
        /// </summary>
        void DeleteFeature(Feature feature);

    }
}
