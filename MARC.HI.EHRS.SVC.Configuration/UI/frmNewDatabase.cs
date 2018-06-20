using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MARC.HI.EHRS.SVC.Configuration.Data;

namespace MARC.HI.EHRS.SVC.Configuration.UI
{
    public partial class frmNewDatabase : Form
    {
        public frmNewDatabase(DbConnectionString connectionString)
        {
            InitializeComponent();
            PopulateConfigurators();
            this.ConnectionString = connectionString;
        }

        /// <summary>
        /// Gets or sets the connection string
        /// </summary>
        public DbConnectionString ConnectionString
        {
            get
            {
                return new DbConnectionString()
                {
                    Database = this.cbxDatabase.SelectedValue?.ToString(),
                    UserName = this.txtUserName.Text,
                    Password = this.txtPassword.Text,
                    Host = this.txtDatabaseAddress.Text,
                    Provider = this.cbxProviderType.SelectedItem as IDatabaseProvider
                };
            }
            set
            {
                this.txtUserName.Text = value.UserName;
                this.txtPassword.Text = value.Password;
                this.txtDatabaseAddress.Text = value.Host;
                this.cbxDatabase.SelectedValue = value.Database;
                this.cbxProviderType.SelectedItem = value.Provider;
            }
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
            IDatabaseProvider conf = cbxProviderType.SelectedItem as IDatabaseProvider;
            try
            {
                cbxDatabase.Items.AddRange(conf.GetDatabases(this.ConnectionString));
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
                this.ConnectionString.Provider.CreateDatabase(this.ConnectionString, this.cbxDatabase.Text, this.txtOwner.Text);
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
