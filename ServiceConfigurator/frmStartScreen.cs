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
using MARC.HI.EHRS.SVC.Core.Configuration;
using System.Xml;
using MARC.HI.EHRS.SVC.ConfigurationApplciation;

namespace ServiceConfigurator
{
    public partial class frmStartScreen : Form
    {
        public frmStartScreen()
        {
            InitializeComponent();
            PopulateConfigurators();
        }

        /// <summary>
        /// Populate configurators
        /// </summary>
        private void PopulateConfigurators()
        {
            foreach (var config in DatabaseConfiguratorRegistrar.Configurators)
                cbxProviderType.Items.Add(config);
        }

        /// <summary>
        /// Validated connection parameter
        /// </summary>
        private void connectionParameter_Validated(object sender, EventArgs e)
        {
            cbxDatabase.Enabled = cbxProviderType.SelectedItem != null &&
                !String.IsNullOrEmpty(txtDatabaseAddress.Text) &&
                !String.IsNullOrEmpty(txtPassword.Text) &&
                !String.IsNullOrEmpty(txtUserName.Text);
            btnContinue.Enabled = cbxDatabase.Enabled && cbxDatabase.SelectedItem != null;
        }

        private void cbxDatabase_DropDown(object sender, EventArgs e)
        {
            cbxDatabase.Items.Clear();
            IDatabaseConfigurator conf = cbxProviderType.SelectedItem as IDatabaseConfigurator;
            try
            {
                cbxDatabase.Items.AddRange(conf.GetDatabases(txtDatabaseAddress.Text, txtUserName.Text, txtPassword.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                cbxDatabase.Enabled = false;
            }
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
                configFile.LoadXml(Resources.empty);
                IDatabaseConfigurator conf = cbxProviderType.SelectedItem as IDatabaseConfigurator;

                // Do an easy config ... first with the connection strings
                foreach (IConfigurationPanel pnl in ConfigurationApplicationContext.s_configurationPanels)
                    if (pnl is IDataboundConfigurationPanel)
                    {
                        (pnl as IDataboundConfigurationPanel).ConnectionString = conf.CreateConnectionStringElement(configFile, txtDatabaseAddress.Text, txtUserName.Text, txtPassword.Text, cbxDatabase.SelectedItem.ToString());
                        (pnl as IDataboundConfigurationPanel).DatabaseConfigurator = conf;
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
                        
                        foreach (IConfigurationPanel pnl in ConfigurationApplicationContext.s_configurationPanels)
                        {
                            progress.Status = (int)((++i / (float)ConfigurationApplicationContext.s_configurationPanels.Count) * 100);
                            progress.StatusText = String.Format("Applying Configuration for {0}...", pnl.ToString());
                            pnl.EnableConfiguration = true;
                            pnl.Configure(configFile);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error Configuring Service");

                        foreach (IConfigurationPanel pnl in ConfigurationApplicationContext.s_configurationPanels)
                        {

                            progress.Status = (int)((i-- / (float)ConfigurationApplicationContext.s_configurationPanels.Count) * 100);
                            progress.StatusText = String.Format("Removing Configuration for {0}...", pnl.ToString());
                            pnl.EnableConfiguration = true;
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

        private void btnNew_Click(object sender, EventArgs e)
        {

            frmNewDatabase newDatabase = new frmNewDatabase(cbxProviderType.SelectedItem as IDatabaseConfigurator, this.txtDatabaseAddress.Text);
            if (newDatabase.ShowDialog() == DialogResult.OK)
            {

                if (newDatabase.DatabaseConfigurator != cbxProviderType.SelectedItem)
                {
                    cbxProviderType.SelectedItem = newDatabase.DatabaseConfigurator;
                    txtDatabaseAddress.Text = "";
                    txtUserName.Text = "";
                    cbxDatabase.Text = "";
                }

                if (txtDatabaseAddress.Text == "")
                    txtDatabaseAddress.Text = newDatabase.Server;
                if (txtUserName.Text == "")
                    txtUserName.Text = newDatabase.Server;
                if (cbxDatabase.Text == "")
                    cbxDatabase.Text = newDatabase.DatabaseName;
            }
        }

    }
}
