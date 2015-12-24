using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MARC.HI.EHRS.SVC.Core.DataTypes;

namespace MARC.HI.EHRS.SVC.Core.Configuration.UI
{
    public partial class frmAddDomainIdentifier : Form
    {

        private OidRegistrar m_oids;

        /// <summary>
        /// Add domain identifier
        /// </summary>
        /// <param name="oidSource"></param>
        public frmAddDomainIdentifier(OidRegistrar oidSource)
        {
            InitializeComponent();

            this.m_oids = oidSource;
            if (oidSource != null)
                foreach (var oid in oidSource)
                    cbxDomain.Items.Add(oid);
            
        }

        /// <summary>
        /// Gets the domain identifier result
        /// </summary>
        public Identifier Identifier
        {
            get
            {
                return new Identifier()
                {
                    Domain = cbxDomain.SelectedItem != null ? (cbxDomain.SelectedItem as OidRegistrar.OidData).Oid : cbxDomain.Text,
                    Identifier = txtId.Text
                };
            }
            set
            {
                if (value != null)
                {
                    txtId.Text = value.Identifier;
                    var regOid = this.m_oids.FindData(value.Domain);
                    if (regOid != null)
                        cbxDomain.SelectedIndex = cbxDomain.Items.IndexOf(regOid);
                    else
                        cbxDomain.Text = value.Domain;
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.Identifier.Domain) && String.IsNullOrEmpty(this.Identifier.Identifier))
            {
                errDefault.SetError(cbxDomain, "Must have one of Domain or Id");
                errDefault.SetError(txtId, "Must have one of Domain or Id");
            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}

