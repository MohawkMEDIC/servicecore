using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MARC.HI.EHRS.SVC.Core.DataTypes;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;

namespace MARC.HI.EHRS.SVC.Core.Configuration.UI
{
    public partial class ucCoreProperties : UserControl
    {

        private HostConfigurationSectionHandler m_configuration;

        /// <summary>
        /// Oid registrar
        /// </summary>
        public OidRegistrar OidData { get; set; }

        /// <summary>
        /// Gets or sets the custodial configuration
        /// </summary>
        public CustodianshipData Custodianship {
            get
            {
                CustodianshipData retVal = new CustodianshipData()
                {
                    Id = txtCustodianId.Tag as DomainIdentifier,
                    Name = txtCustodianName.Text
                };
                return retVal;
            }
            set
            {
                this.ShowIdOnTextBox(txtCustodianId, value.Id);
                txtCustodianName.Text = value.Name;
            }
        }

       

        /// <summary>
        /// Gets or sets the device identifier configuration
        /// </summary>
        public DomainIdentifier DeviceId 
        {
            get
            {
                return txtDeviceId.Tag as DomainIdentifier;
            }
            set
            {
                this.ShowIdOnTextBox(txtDeviceId, value);
            }
        }

        public Jurisdiction Jurisdiction 
        {
            get
            {
                return new Jurisdiction()
                {
                    Id = txtJurisdictionId.Tag as DomainIdentifier,
                    ClientDomain = (txtECID.Tag as DomainIdentifier).Domain,
                    DefaultLanguageCode = this.Locale,
                    PlaceDomain = (txtELID.Tag as DomainIdentifier).Domain,
                    ProviderDomain = (txtEPID.Tag as DomainIdentifier).Domain,
                    Name = txtJurisdictionName.Text,
                    
                };
            }
            set
            {

                txtJurisdictionName.Text = value.Name;

                this.ShowIdOnTextBox(txtJurisdictionId, value.Id);
                this.ShowIdOnTextBox(txtECID, new DomainIdentifier() { Domain = value.ClientDomain });
                this.ShowIdOnTextBox(txtEPID, new DomainIdentifier() { Domain = value.ProviderDomain });
                this.ShowIdOnTextBox(txtELID, new DomainIdentifier() { Domain = value.PlaceDomain });
            }
        }

        public string Locale
        {
            get
            {
                return cbxLanguage.Text;
            }
            set
            {
                cbxLanguage.Text = value;
            }
        }

        public ucCoreProperties()
        {
            InitializeComponent();

            // Scan this directory for the configuration file name
            foreach (var f in Directory.GetFiles(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "*.??.xml"))
            {
                // Load the file
                XmlDocument doc = new XmlDocument();
                doc.Load(f);

                // Get the name of the locale
                var node = doc.SelectSingleNode("//*[local-name() = 'locale' and namespace-uri() = 'urn:marc-hi:ca/localization']/@name");
                if (node == null)
                    continue;
                else
                    cbxLanguage.Items.Add(node.Value);

            }
            cbxLanguage.SelectedIndex = 0;

        }

        /// <summary>
        /// Show an ID in texbox
        /// </summary>
        /// <param name="target"></param>
        /// <param name="domainIdentifier"></param>
        private void ShowIdOnTextBox(TextBox target, DomainIdentifier domainIdentifier)
        {
            target.Tag = domainIdentifier;
            if (domainIdentifier != null)
                target.Text = domainIdentifier.ToString();
        }
        /// <summary>
        /// Browse domain identifier
        /// </summary>
        private void BrowseDomainIdentifier(TextBox target)
        {
            frmAddDomainIdentifier addId = new frmAddDomainIdentifier(this.OidData);
            addId.Identifier = target.Tag as DomainIdentifier;
            if (addId.ShowDialog() == DialogResult.OK)
            {
                target.Tag = addId.Identifier;
                target.Text = addId.Identifier.ToString();
            }
        }

        private void btnBrowseDevice_Click(object sender, EventArgs e)
        {
            this.BrowseDomainIdentifier(txtDeviceId);
        }

        private void btnBrowseCustodian_Click(object sender, EventArgs e)
        {
            this.BrowseDomainIdentifier(txtCustodianId);
        }

        private void btnBrowseJurisdiction_Click(object sender, EventArgs e)
        {
            this.BrowseDomainIdentifier(txtJurisdictionId);
        }

        private void btnBrowseECID_Click(object sender, EventArgs e)
        {
            this.BrowseDomainIdentifier(txtECID);
        }

        private void btnBrowseEPID_Click(object sender, EventArgs e)
        {
            this.BrowseDomainIdentifier(txtEPID);
        }

        private void btnBrowseELID_Click(object sender, EventArgs e)
        {
            this.BrowseDomainIdentifier(txtELID);
        }
    }
}
