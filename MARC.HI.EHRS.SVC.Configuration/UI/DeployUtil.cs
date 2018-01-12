using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Xml;
using System.Windows.Forms;
using System.IO;
using MARC.HI.EHRS.SVC.Configuration.UI;

namespace MARC.HI.EHRS.SVC.Configuration.UI
{
    /// <summary>
    /// Deploy utility
    /// </summary>
    public static class DeployUtil
    {

        /// <summary>
        /// Deploy
        /// </summary>
        public static void Deploy(StringCollection deploymentList, Dictionary<String, StringCollection> options)
        {
            frmProgress progress = new frmProgress();

            try
            {
                var configurationDom = new XmlDocument();

                if(File.Exists(ConfigurationApplicationContext.s_configFile))
                    configurationDom.Load(ConfigurationApplicationContext.s_configFile);
                else
                    configurationDom.LoadXml(Resources.Empty);
                // Configure
                progress.Show();
                int i = 0;
                foreach (var pnl in ConfigurationApplicationContext.s_configurationPanels.FindAll(o => deploymentList.Contains(o.Name)))
                {
                    if (pnl.IsConfigured(configurationDom))
                        pnl.UnConfigure(configurationDom);
                    progress.OverallStatusText = String.Format("Configuring {0}...", pnl.ToString()); 
                    progress.OverallStatus = (int)((++i / (float)deploymentList.Count) * 100);
                    Application.DoEvents();
                    pnl.Configure(configurationDom);
                }

                // Always applied stuff changes
                foreach (var pnl in ConfigurationApplicationContext.s_configurationPanels.FindAll(o => o.AlwaysConfigure))
                     pnl.Configure(configurationDom);

                configurationDom.Save(ConfigurationApplicationContext.s_configFile);
                progress.OverallStatusText = "Executing post configuration tasks...";
                ConfigurationApplicationContext.OnConfigurationApplied();
            }
            finally
            {
                progress.Close();
            }
        }

    }
}
