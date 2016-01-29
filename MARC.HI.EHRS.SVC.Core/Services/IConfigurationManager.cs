using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MARC.HI.EHRS.SVC.Core.Services
{
    /// <summary>
    /// Represents a configuration manager
    /// </summary>
    public interface IConfigurationManager
    {

        /// <summary>
        /// Get the specified section
        /// </summary>
        object GetSection(string sectionName);
    }
}
