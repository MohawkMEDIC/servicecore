using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MARC.HI.EHRS.SVC.Core.Configuration;

namespace MARC.HI.EHRS.SVC.ConfigurationApplciation
{
    public partial class frmNewDatabase : Form
    {
        public frmNewDatabase(IDatabaseConfigurator configurator, String server)
        {
            InitializeComponent();
            PopulateConfigurators();
            this.DatabaseConfigurator = configurator;
            this.Server = server;
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
        /// Gets or sets the server
        /// </summary>
        public string Server
        {
            get
            {
                return this.txtDatabaseAddress.Text;
            }
            set
            {
                txtDatabaseAddress.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the database
        /// </summary>
        public string DatabaseName { get { return cbxDatabase.Text; } }

        /// <summary>
        /// Gets user name
        /// </summary>
        public string UserName { get { return txtOwner.Text; } }
        
        /// <summary>
        /// Validated connection parameter
        /// </summary>
        private void connectionParameter_Validated(object sender, EventArgs e)
        {
            pnlNewDb.Enabled = btnOk.Enabled = cbxProviderType.SelectedItem != null &&
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


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (cbxProviderType.SelectedItem == null)
            {
                errMain.SetError(cbxProviderType, "Database provider not selected");
                return;
            }
            else if (txtUserName.Text == "")
            {
                errMain.SetError(txtUserName, "Superuser must be provided");
                return;
            }
            else if (txtPassword.Text == "")
            {
                errMain.SetError(txtPassword, "Password must be provided");
                return;
            }
            
            // Is this a current database?
            if (cbxDatabase.SelectedIndex != -1)
            {
                MessageBox.Show("Cannot create an already existing database, please enter a different name", "Duplicate Name");
                return;
            }

            // Create
            try
            {
                this.DatabaseConfigurator.CreateDatabase(this.Server, this.txtUserName.Text, this.txtPassword.Text, this.DatabaseName, this.UserName);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Create database failed, error was : {0}", ex.Message), "Creation Error");
            }
        }
    }
}
