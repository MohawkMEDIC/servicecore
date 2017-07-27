using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MARC.HI.EHRS.SVC.Configuration
{
    /// <summary>
    /// Represents a configuration feature that can be scripted
    /// </summary>
    public interface IScriptableConfigurableFeature : IConfigurableFeature
    {

        /// <summary>
        /// Gets the type that the un-deploy arguments are described by
        /// </summary>
        Type UnDeployArgumentType { get; }

        /// <summary>
        /// Gets the type that the deploy arguments are described by
        /// </summary>
        Type DeployArgumentType { get; }

        /// <summary>
        /// Deploy the specified feature
        /// </summary>
        void Deploy(String[] args);

        /// <summary>
        /// Un-deploy the specified feature
        /// </summary>
        void Undeploy(String[] args);

    }
}
