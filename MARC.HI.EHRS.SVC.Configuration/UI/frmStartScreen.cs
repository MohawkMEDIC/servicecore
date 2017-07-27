/*
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
using MARC.HI.EHRS.SVC.Configuration.UI;
using MARC.HI.EHRS.SVC.Configuration.Data;

namespace MARC.HI.EHRS.SVC.Configuration.UI
{
    public partial class frmStartScreen : Form
    {
        public frmStartScreen()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Start the configuration process...
        /// </summary>
        private void btnContinue_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            btnContinue.Enabled = false;
            try
            {
                // Start by creating the configuration file
                XmlDocument configFile = new XmlDocument();
                configFile.LoadXml(Resources.Empty);
                IDatabaseProvider conf = dbSelector.DatabaseConfigurator;

                // Do an easy config ... first with the connection strings
                foreach (IConfigurableFeature pnl in ConfigurationApplicationContext.s_configurationPanels)
                    if (pnl is IDataboundFeature)
                    {
                        (pnl as IDataboundFeature).ConnectionString = dbSelector.CreateConnectionString(configFile);
                        (pnl as IDataboundFeature).DataProvider = conf;
                    }

                // Easy or complex?
                if (rdoEasy.Checked)
                {
                    // Save the configuration
                    var progress = new frmProgress();
                    int i = 0;
                    try
                    {
                        progress.Show();
                        
                        foreach (IConfigurableFeature pnl in ConfigurationApplicationContext.s_configurationPanels)
                        {
                            progress.Status = (int)((++i / (float)ConfigurationApplicationContext.s_configurationPanels.Count) * 100);
                            progress.StatusText = String.Format("Applying Configuration for {0}...", pnl.ToString());
                            //pnl.EnableConfiguration = true;
                            pnl.EasyConfigure(configFile);
                        }
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        MessageBox.Show(ex.ToString(), "Error Configuring Service");

#else
                        MessageBox.Show(ex.Message, "Error Configuring Service");
#endif
                        foreach (IConfigurableFeature pnl in ConfigurationApplicationContext.s_configurationPanels)
                        {

                            progress.Status = (int)((i-- / (float)ConfigurationApplicationContext.s_configurationPanels.Count) * 100);
                            progress.StatusText = String.Format("Removing Configuration for {0}...", pnl.ToString());
                            pnl.UnConfigure(configFile);
                        }

                        return;
                    }
                    finally
                    {
                        progress.Close();
                    }
                    configFile.Save(ConfigurationApplicationContext.s_configFile);
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    configFile.Save(ConfigurationApplicationContext.s_configFile);
                    this.DialogResult = DialogResult.OK;
                }

                this.Close();
            }
            finally
            {
                Cursor = Cursors.Default;
                btnContinue.Enabled = true;
            }
        }

        /// <summary>
        /// Object was validated
        /// </summary>
        private void dbSelector_Validated(object sender, EventArgs e)
        {
            btnContinue.Enabled = true;
        }
    }
}
