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

    }
}
