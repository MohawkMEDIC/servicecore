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

namespace MARC.HI.EHRS.SVC.Messaging.Persistence.Data.Configuration.UI.Panels
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
