/**
 * Copyright 2012-2013 Mohawk College of Applied Arts and Technology
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
 * Date: 5-12-2012
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using MARC.HI.EHRS.SVC.Core.Configuration;

namespace ServiceConfigurator
{
    public partial class frmApply : Form
    {
        public frmApply(bool config)
        {
            InitializeComponent();
            ShowConfigurationPanels(config);
        }

        /// <summary>
        /// Show configuration panels
        /// </summary>
        private void ShowConfigurationPanels(bool config)
        {
            XmlDocument configDocument = new XmlDocument();
                    configDocument.Load(ConfigurationApplicationContext.s_configFile);
            foreach (var itm in ConfigurationApplicationContext.s_configurationPanels)
                if (itm.EnableConfiguration)
                {
                    if(itm.IsConfigured(configDocument) ^ config &&
                        !(itm is IAlwaysDeployedConfigurationPanel))
                        chkActions.Items.Add(itm);
                }
        }

        /// <summary>
        /// Configure features
        /// </summary>
        public static void ConfigureFeatures()
        {
            frmApply apply = new frmApply(true);
            apply.Text = "Configure Features";
            apply.lblDescription.Text = "Select the features that you would like to apply configuration for";
            if (apply.ShowDialog() == DialogResult.OK)
            {
                var progress = new frmProgress();
                try
                {
                    progress.Show();
                    XmlDocument configDocument = new XmlDocument();
                    configDocument.Load(ConfigurationApplicationContext.s_configFile);
                    int i = 0;
                    foreach (IConfigurationPanel itm in apply.chkActions.CheckedItems)
                    {
                        if (!itm.Validate(configDocument))
                        {
                            MessageBox.Show(String.Format("Configuration of item '{0}' failed, validation failed", itm), "Validation Failure");
                            continue;
                        }
                        progress.Status = (int)((++i / (float)apply.chkActions.CheckedItems.Count) * 100);
                        progress.StatusText = String.Format("Configuring {0}...", itm.ToString());
                        itm.Configure(configDocument);
                    }

                    // Always applied stuff changes
                    foreach (var itm in ConfigurationApplicationContext.s_configurationPanels.FindAll(o => o is IAlwaysDeployedConfigurationPanel))
                    {
                        if (!itm.Validate(configDocument))
                        {
                            MessageBox.Show(String.Format("Configuration of item '{0}' failed, validation failed", itm), "Validation Failure");
                            continue;
                        }
                        itm.Configure(configDocument);
                    }
                    configDocument.Save(ConfigurationApplicationContext.s_configFile);
                    ConfigurationApplicationContext.OnConfigurationApplied();
                }
                finally
                {
                    progress.Close();
                }
            }
        }

        /// <summary>
        /// UnConfigure Features
        /// </summary>
        public static void UnConfigureFeatures()
        {
            frmApply apply = new frmApply(false);
            apply.Text = "UnConfigure Features";
            apply.lblDescription.Text = "Select the features that you would like to remove configuration for";
            
            if (apply.ShowDialog() == DialogResult.OK)
            {
                var progress = new frmProgress();
                try
                {
                    progress.Show();
                    XmlDocument configDocument = new XmlDocument();
                    configDocument.Load(ConfigurationApplicationContext.s_configFile);
                    int i = 0;
                    foreach (IConfigurationPanel itm in apply.chkActions.CheckedItems)
                    {
                        progress.Status = (int)((++i / (float)apply.chkActions.CheckedItems.Count) * 100);
                        progress.StatusText = String.Format("Removing Configuration for {0}...", itm.ToString());
                        itm.UnConfigure(configDocument);
                    }
                    configDocument.Save(ConfigurationApplicationContext.s_configFile);
                    ConfigurationApplicationContext.OnConfigurationApplied();

                }
                finally
                {
                    progress.Close();
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
