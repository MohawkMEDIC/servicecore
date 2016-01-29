using MARC.HI.EHRS.SVC.Core.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MARC.HI.EHRS.SVC.Core.Configuration
{
    /// <summary>
    /// Represents a configuration manager that uses local App.config files
    /// </summary>
    public class LocalConfigurationManager : IConfigurationManager
    {
        /// <summary>
        /// Get the specified section
        /// </summary>
        public object GetSection(string sectionName)
        {
            return ConfigurationManager.GetSection(sectionName);
        }

        
    }
}
