using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.Configuration;

namespace ServiceConfigurator
{
    /// <summary>
    /// Configuration application context
    /// </summary>
    public static class ConfigurationApplicationContext
    {

        // Configuration panels
        public static List<IConfigurationPanel> s_configurationPanels = new List<IConfigurationPanel>(10);

        // Configuration File
        public static string s_configFile = null;

    }
}
