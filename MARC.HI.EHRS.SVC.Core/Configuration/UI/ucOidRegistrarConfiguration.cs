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
    public partial class ucOidRegistrarConfiguration : UserControl
    {

        private OidRegistrar m_oidReg;

        /// <summary>
        /// Oids
        /// </summary>
        public OidRegistrar Oids
        {
            get
            {
                return this.m_oidReg;
            }
            set
            {
                this.m_oidReg = value;

                lsvOids.Items.Clear();

                // Loop and add
                foreach (var oid in this.m_oidReg)
                {
                    ListViewItem lsiOid = lsvOids.Items.Add(oid.Name, oid.Name, -1); 
                    lsiOid.SubItems.Add(oid.Oid);
                    lsiOid.SubItems.Add(oid.Description);
                    lsiOid.Tag = oid;
                    
                }
                lsvOids.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
        }

        public ucOidRegistrarConfiguration()
        {
            InitializeComponent();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if(lsvOids.SelectedItems.Count == 0) return;

            frmAddOid addOid = new frmAddOid();
            addOid.Registrar = this.Oids;
            addOid.Oid = lsvOids.SelectedItems[0].Tag as MARC.HI.EHRS.SVC.Core.DataTypes.OidRegistrar.OidData;
            if (addOid.ShowDialog() == DialogResult.OK)
            {
                lsvOids.SelectedItems[0].Text = addOid.Oid.Name;
                lsvOids.SelectedItems[0].SubItems[1].Text = addOid.Oid.Oid;
                lsvOids.SelectedItems[0].SubItems[2].Text = addOid.Oid.Description;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddOid addOid = new frmAddOid();
            addOid.Registrar = this.Oids;

            addOid.Oid = new OidRegistrar.OidData();

            if (addOid.ShowDialog() == DialogResult.OK)
            {
                Oids.Register(addOid.Oid);
                this.Oids = this.Oids; // force redraw
                var oidLvi = this.lsvOids.Items.Find(addOid.Oid.Name, false)[0];
                oidLvi.Selected = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lsvOids.SelectedItems.Count == 0) return;

            if (MessageBox.Show(string.Format("Are you sure you want to remove the OID registration '{0}'?", lsvOids.SelectedItems[0].Text), "Confirm Removal", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Oids.Remove(lsvOids.SelectedItems[0].Tag as MARC.HI.EHRS.SVC.Core.DataTypes.OidRegistrar.OidData);
                this.Oids = this.Oids;
            }

        }
    }
}
