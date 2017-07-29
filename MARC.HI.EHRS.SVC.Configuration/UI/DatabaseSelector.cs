using System;
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

        // Connection string name
        private string m_connectionStringName = null;

        /// <summary>
        /// Gets or sets the connection string
        /// </summary>
        public DbConnectionString ConnectionString {
            get
            {
                return new DbConnectionString()
                {
                    Database = this.cbxDatabase.Text,
                    UserName = this.txtUserName.Text,
                    Password = this.txtPassword.Text,
                    Host = this.txtDatabaseAddress.Text,
                    Name = this.m_connectionStringName,
                    Provider = this.cbxProviderType.SelectedItem as IDatabaseProvider
                };
            }
            set
            {
                this.txtUserName.Text = value.UserName;
                this.txtPassword.Text = value.Password;
                this.txtDatabaseAddress.Text = value.Host;
                this.cbxDatabase.Text = value.Database;
                this.cbxProviderType.SelectedItem = value.Provider;
                this.m_connectionStringName = value.Name;
            }
        }

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
            this.ConnectionString = new DbConnectionString()
            {

            };
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
                cbxDatabase.Items.AddRange(conf.GetDatabases(this.ConnectionString));
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

            frmNewDatabase newDatabase = new frmNewDatabase(this.ConnectionString);
            if (newDatabase.ShowDialog() == DialogResult.OK)
            {
                
                if(newDatabase.ConnectionString.Provider != this.ConnectionString.Provider)
                {
                    this.ConnectionString = newDatabase.ConnectionString;
                }

                
            }
        }

    }
}
