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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MARC.HI.EHRS.SVC.Core.Configuration;
using System.Xml;

namespace MARC.HI.EHRS.SVC.QM.Persistence.Data.Configuration.UI.Panels
{
    public partial class pnlConfigureMessagePersistence : UserControl
    {
        public pnlConfigureMessagePersistence()
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
        /// Database connector
        /// </summary>
        public IDatabaseConfigurator DatabaseConfigurator 
        {
            get { return this.cbxProviderType.SelectedItem as IDatabaseConfigurator; }
            set { this.cbxProviderType.SelectedItem = value; }
        }

        /// <summary>
        /// Value
        /// </summary>
        public decimal MaxAge
        {
            get
            {
                return this.numMaxAge.Value;
            }
            set
            {
                this.numMaxAge.Value = value;
            }
        }

        /// <summary>
        /// Get connection string
        /// </summary>
        public string GetConnectionString(XmlDocument configurationDom)
        {
            var dbp = this.cbxProviderType.SelectedItem as IDatabaseConfigurator;
            if (dbp != null && cbxDatabase.Text != "")
                return dbp.CreateConnectionStringElement(configurationDom, txtDatabaseAddress.Text, txtUserName.Text, txtPassword.Text, cbxDatabase.Text);
            return null;
        }

        /// <summary>
        /// Set connection string stuff
        /// </summary>
        public void SetConnectionString(XmlDocument configurationDom, string connectionString)
        {
            IDatabaseConfigurator dpc = this.cbxProviderType.SelectedItem as IDatabaseConfigurator;
            if (dpc == null)
                return;
            cbxDatabase.Text = dpc.GetConnectionStringElement(configurationDom, ConnectionStringPartType.Database, connectionString);
            txtUserName.Text = dpc.GetConnectionStringElement(configurationDom, ConnectionStringPartType.UserName, connectionString);
            txtPassword.Text = dpc.GetConnectionStringElement(configurationDom, ConnectionStringPartType.Password, connectionString);
            txtDatabaseAddress.Text = dpc.GetConnectionStringElement(configurationDom, ConnectionStringPartType.Host, connectionString);
            connectionParameter_Validated(null, EventArgs.Empty);
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
        }

        /// <summary>
        /// Drop down
        /// </summary>
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


    }
}
