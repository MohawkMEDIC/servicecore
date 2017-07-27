﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using MARC.HI.EHRS.SVC.Configuration.Data;

namespace MARC.HI.EHRS.SVC.Configuration.UI
{
    public partial class DatabaseSelector : UserControl
    {
        public DatabaseSelector()
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
        public IDatabaseProvider DatabaseConfigurator
        {
            get { return this.cbxProviderType.SelectedItem as IDatabaseProvider; }
            set { this.cbxProviderType.SelectedItem = value; }
        }

        /// <summary>
        /// Get connection string
        /// </summary>
        public string GetConnectionString(XmlDocument configurationDom)
        {
            var dbp = this.cbxProviderType.SelectedItem as IDatabaseProvider;
            if (dbp != null && cbxDatabase.Text != "")
                return dbp.CreateConnectionStringElement(configurationDom, txtDatabaseAddress.Text, txtUserName.Text, txtPassword.Text, cbxDatabase.Text);
            return null;
        }

        /// <summary>
        /// Set connection string stuff
        /// </summary>
        public void SetConnectionString(XmlDocument configurationDom, string connectionString)
        {
            IDatabaseProvider dpc = this.cbxProviderType.SelectedItem as IDatabaseProvider;
            if (dpc == null)
                return;
            cbxDatabase.Text = dpc.GetConnectionStringElement(configurationDom, ConnectionStringPartType.Database, connectionString);
            txtUserName.Text = dpc.GetConnectionStringElement(configurationDom, ConnectionStringPartType.UserName, connectionString);
            txtPassword.Text = dpc.GetConnectionStringElement(configurationDom, ConnectionStringPartType.Password, connectionString);
            txtDatabaseAddress.Text = dpc.GetConnectionStringElement(configurationDom, ConnectionStringPartType.Host, connectionString);
            connectionParameter_Validated(null, EventArgs.Empty);
        }

        /// <summary>
        /// Gets the connection string
        /// </summary>
        /// <returns></returns>
        public string CreateConnectionString(XmlDocument config)
        {
            return this.DatabaseConfigurator.CreateConnectionStringElement(config, txtDatabaseAddress.Text, txtUserName.Text, txtPassword.Text, cbxDatabase.SelectedItem.ToString());
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
            if (cbxDatabase.Enabled && cbxDatabase.SelectedItem != null)
                this.OnValidated(EventArgs.Empty);
        }

        /// <summary>
        /// Drop down
        /// </summary>
        private void cbxDatabase_DropDown(object sender, EventArgs e)
        {
            cbxDatabase.Items.Clear();
            IDatabaseProvider conf = cbxProviderType.SelectedItem as IDatabaseProvider;
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
        /// Create a new database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, EventArgs e)
        {

            frmNewDatabase newDatabase = new frmNewDatabase(this.DatabaseConfigurator, this.txtDatabaseAddress.Text);
            if (newDatabase.ShowDialog() == DialogResult.OK)
            {
                
                if(newDatabase.DatabaseConfigurator != this.DatabaseConfigurator)
                {
                    this.DatabaseConfigurator = newDatabase.DatabaseConfigurator;
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
