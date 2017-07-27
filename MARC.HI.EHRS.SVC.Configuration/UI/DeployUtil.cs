/*
 * Copyright 2013-2013 Mohawk College of Applied Arts and Technology
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you 
 * may not use this file except in compliance with the License. You may 
 * obtain a copy of the License at 
 * 
 * http://www.apache.org/licenses/LICENSE-2.0 
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations under 
 * the License.
 * 
 * User: fyfej
 * Date: 24-5-2013
 */
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
                    progress.StatusText = String.Format("Configuring {0}...", pnl.ToString()); 
                    progress.Status = (int)((++i / (float)deploymentList.Count) * 100);
                    Application.DoEvents();
                    pnl.Configure(configurationDom);
                }

                // Always applied stuff changes
                foreach (var pnl in ConfigurationApplicationContext.s_configurationPanels.FindAll(o => o.AlwaysConfigure))
                     pnl.Configure(configurationDom);

                configurationDom.Save(ConfigurationApplicationContext.s_configFile);
                progress.StatusText = "Executing post configuration tasks...";
                ConfigurationApplicationContext.OnConfigurationApplied();
            }
            finally
            {
                progress.Close();
            }
        }

    }
}
