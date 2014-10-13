using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Reflection;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Handlers;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Configuration.UI
{
    public partial class pnlConfigureFhir : UserControl
    {

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
        /// Index file
        /// </summary>
        public string IndexFile {
            get { return this.txtIndex.Text; }
            set { this.txtIndex.Text = value; }
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
        /// Handlers
        /// </summary>
        public List<Type> Handlers
        {
            get
            {
                List<Type> retVal = new List<Type>();
                foreach (ListViewItem lsv in this.lsvProfiles.Items)
                    if (lsv.Checked)
                        retVal.Add((Type)lsv.Tag);
                return retVal;
            }
            set
            {
                while(lsvProfiles.CheckedItems.Count > 0)
                    lsvProfiles.CheckedItems[0].Checked = false;

                foreach (ListViewItem lsv in this.lsvProfiles.Items)
                    lsv.Checked = value.Contains(lsv.Tag);
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

        public pnlConfigureFhir()
        {
            InitializeComponent();
            InitializeStores();
            InitializeHandlers();
        }

        /// <summary>
        /// Initialize handlers
        /// </summary>
        private void InitializeHandlers()
        {
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (asm == this.GetType().Assembly) continue;

                foreach (Type typ in asm.GetTypes().Where(o => o.GetInterface(typeof(IFhirResourceHandler).FullName) != null))
                {

                    ProfileAttribute profile = typ.GetCustomAttribute<ProfileAttribute>();
                    ResourceProfileAttribute resource = typ.GetCustomAttribute<ResourceProfileAttribute>();

                    if (resource == null) continue;
                    ListViewItem lsv = lsvProfiles.Items.Add(typ.AssemblyQualifiedName, resource.Resource.GetCustomAttribute<XmlRootAttribute>().ElementName, 0);
                    lsv.SubItems.Add(profile.ProfileId);
                    lsv.Tag = typ;
                }
            }
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

        /// <summary>
        /// Choose the index file
        /// </summary>
        private void btnChooseIndex_Click(object sender, EventArgs e)
        {
            String directory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            if (!String.IsNullOrEmpty(txtIndex.Text) && Path.IsPathRooted(txtIndex.Text))
                directory = Path.GetDirectoryName(txtIndex.Text);
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "HTML Files (*.html;*.shtml;*.htm)|*.html;*.shtml;*.htm",
                Title = "Select Index File",
                RestoreDirectory = true,
                InitialDirectory = directory
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtIndex.Text = ofd.FileName;
            }
        }

        
    }
}
