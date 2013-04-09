using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;

namespace MARC.HI.EHRS.SVC.Messaging.Everest.Configuration.UI
{
    public partial class pnlTemplateConfigure : UserControl
    {

        /// <summary>
        /// Gets or sets the title of the configuration
        /// </summary>
        public string Title
        {
            get
            {
                return this.lblTitle.Text;
            }
            set
            {
                this.lblTitle.Text = value;
                this.chkEnable.Text = String.Format("Enable configuration for {0}", value);
            }
        }

        /// <summary>
        /// True if configuration is enabled
        /// </summary>
        public bool IsConfigurationEnabled
        {
            get
            {
                return this.chkEnable.Checked;
            }
            set
            {
                this.chkEnable.Checked = value;
            }
        }

        /// <summary>
        /// Gets or sets the service debug
        /// </summary>
        public bool ServiceDebugEnabled
        {
            get
            {
                return chkDebug.Checked;
            }
            set
            {
                chkDebug.Checked = value;
            }
        }

        /// <summary>
        /// Gets or sets the meta-data enable
        /// </summary>
        public bool ServiceMetaDataEnabled
        {
            get
            {
                return chkMetaData.Checked;
            }
            set
            {
                chkMetaData.Checked = value;
            }
        }

        /// <summary>
        /// Gets or sets the requirement of client certificates
        /// </summary>
        public bool RequireClientCerts
        {
            get
            {
                return chkPKI.Checked;
            }
            set
            {
                chkPKI.Checked = value;
            }
        }

        /// <summary>
        /// Gets or sets the endpoint address
        /// </summary>
        public string Address
        {
            get
            {
                return txtAddress.Text;
            }
            set
            {
                txtAddress.Text = value;
                RescanScheme();
            }
        }

        /// <summary>
        /// Gets or sets the store name to locate certificates
        /// </summary>
        public StoreName StoreName
        {
            get
            {
                return (StoreName)cbxStore.SelectedItem;
            }
            set
            {
                cbxStore.SelectedItem = value;
            }
        }

        /// <summary>
        /// Gets or sets the store name to locate certificates
        /// </summary>
        public StoreLocation StoreLocation
        {
            get
            {
                return (StoreLocation)cbxStoreLocation.SelectedItem;
            }
            set
            {
                cbxStoreLocation.SelectedItem = value;
            }
        }

        /// <summary>
        /// Gets or sets the certificate
        /// </summary>
        public X509Certificate2 Certificate
        {
            get
            {
                return (X509Certificate2)txtCertificate.Tag;
            }
            set
            {
                txtCertificate.Tag = value;
                txtCertificate.Text = value.GetSerialNumberString();
            }
        }

        /// <summary>
        /// Configure the template
        /// </summary>
        public pnlTemplateConfigure()
        {
            InitializeComponent();
            InitializeStores();
        }

        /// <summary>
        /// Initialize certificate stores
        /// </summary>
        private void InitializeStores()
        {
            foreach (var sv in Enum.GetValues(typeof(StoreLocation)))
                cbxStoreLocation.Items.Add(sv);
            foreach (var sv in Enum.GetValues(typeof(StoreName)))
                cbxStore.Items.Add(sv);
            cbxStoreLocation.SelectedIndex = 0;
            cbxStore.SelectedIndex = 0;
        }

        /// <summary>
        /// Checked has changed
        /// </summary>
        private void chkEnable_CheckedChanged(object sender, EventArgs e)
        {
            pnlConfiguration.Enabled = chkEnable.Checked;
        }

       
        private void txtAddress_Validated(object sender, EventArgs e)
        {
            RescanScheme();
        }

        /// <summary>
        /// Enable security based on URI scheme
        /// </summary>
        private void RescanScheme()
        {
            try
            {
                Uri myAddr = new Uri(txtAddress.Text);
                grpSSL.Enabled = myAddr.Scheme == "https";
                
            }
            catch { }
        }

        private void btnChooseCert_Click(object sender, EventArgs e)
        {
            X509Store store = new X509Store(this.StoreName, this.StoreLocation);
            try
            {
                store.Open(OpenFlags.ReadOnly);
                var certs = store.Certificates.Find(X509FindType.FindByApplicationPolicy, "1.3.6.1.5.5.7.3.1", true);
                var selected = X509Certificate2UI.SelectFromCollection(certs, "Select Certificate", "Select a server certificate for this endpoint", X509SelectionFlag.SingleSelection);

                if (selected.Count > 0) 
                    this.Certificate = selected[0];
            }
            finally
            {
                store.Close();
            }
        }

   
       
    }
}
