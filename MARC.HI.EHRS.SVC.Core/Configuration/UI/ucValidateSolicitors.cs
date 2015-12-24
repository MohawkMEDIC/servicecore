using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MARC.HI.EHRS.SVC.Core.DataTypes;

namespace MARC.HI.EHRS.SVC.Core.Configuration.UI
{
    public partial class ucValidateSolicitors : UserControl
    {


        /// <summary>
        /// List of valid OIDs
        /// </summary>
        public OidRegistrar Oids { get; set; }

        /// <summary>
        /// Require validation
        /// </summary>
        public bool RequireValidation
        {
            get
            {
                return this.chkRequire.Checked;
            }
            set
            {
                this.chkRequire.Checked = value;
            }
        }
        /// <summary>
        /// Gets or sets the valid solicitors
        /// </summary>
        public List<Identifier> Solicitors
        {
            get
            {
                List<Identifier> retVal = new List<Identifier>();
                foreach (Identifier itm in lstValidSolicitors.Items)
                    retVal.Add(itm as Identifier);
                return retVal;
            }
            set
            {
                lstValidSolicitors.Items.Clear();
                foreach (var itm in value)
                    lstValidSolicitors.Items.Add(itm);
            }
        }

        public ucValidateSolicitors()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Add a new oid
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddDomainIdentifier domainAdd = new frmAddDomainIdentifier(this.Oids);
            if (domainAdd.ShowDialog() == DialogResult.OK)
            {
                lstValidSolicitors.Items.Add(domainAdd.Identifier);
            }
        }

        /// <summary>
        /// Edit an existing oid
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lstValidSolicitors.SelectedItem == null)
                return;
            frmAddDomainIdentifier domainAdd = new frmAddDomainIdentifier(this.Oids);
            domainAdd.Identifier = lstValidSolicitors.SelectedItem as Identifier;
            domainAdd.ShowDialog();
            var idx = lstValidSolicitors.SelectedIndex;
            lstValidSolicitors.Items.Insert(idx, domainAdd.Identifier);
            lstValidSolicitors.Items.Remove(lstValidSolicitors.SelectedItem);
            lstValidSolicitors.SelectedIndex = idx;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstValidSolicitors.SelectedItem == null)
                return;
            lstValidSolicitors.Items.Remove(lstValidSolicitors.SelectedItem);
        }
    }
}
