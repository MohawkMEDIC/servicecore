using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
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

        /// <summary>
        /// App settings
        /// </summary>
        NameValueCollection AppSettings { get; }

        /// <summary>
        /// Connection Strings
        /// </summary>
        ConnectionStringSettingsCollection ConnectionStrings { get; }
    }
}
