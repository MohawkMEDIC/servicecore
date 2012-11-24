using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using MARC.HI.EHRS.SVC.Core.Configuration;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace ServiceConfigurator
{
    public partial class frmMain : Form
    {

        // XmlConfiguration
        private XmlDocument m_xmlConfiguration = new XmlDocument();
 
        public frmMain()
        {
            InitializeComponent();
            PopulateConfigurationOptions();
        }

        /// <summary>
        /// Populate configuration options
        /// </summary>
        private void PopulateConfigurationOptions()
        {
            trvOptions.Nodes.Clear();

            try
            {
                m_xmlConfiguration.Load(ConfigurationApplicationContext.s_configFile);
                foreach (var itm in ConfigurationApplicationContext.s_configurationPanels)
                {
                    TreeNode tn = new TreeNode(itm.Name);
                    tn.Tag = itm;

                    tn.SelectedImageIndex = tn.ImageIndex = itm.IsConfigured(m_xmlConfiguration) ? 0 : 1;
                    trvOptions.Nodes.Add(tn);
                }
                trvOptions.SelectedNode = trvOptions.Nodes[0];
            }
            catch (Exception e)
            {

                // Error with the file
                switch(MessageBox.Show(String.Format("Couldn't load configuration:\r\n\r\n{0}\r\n\r\nWould you like to remove the configuration file and start over?",
                    e.Message), "Configuration Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error))
                {
                    case DialogResult.Yes:
                        File.Delete(ConfigurationApplicationContext.s_configFile);
                        
                        // Restart
                        ProcessStartInfo pi = new ProcessStartInfo(Assembly.GetEntryAssembly().Location);
                        Process.Start(pi);
                        Environment.Exit(0);
                        break;
                    case DialogResult.No:
                        MessageBox.Show("Could not read configuration file");
                        Environment.Exit(10);
                        break;
                }

            }
            
        }

        /// <summary>
        /// Un-Configure features
        /// </summary>
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            frmApply.UnConfigureFeatures();
            PopulateConfigurationOptions();
        }

        /// <summary>
        /// Configure features
        /// </summary>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            frmApply.ConfigureFeatures();
            PopulateConfigurationOptions();
        }

        private void trvOptions_AfterSelect(object sender, TreeViewEventArgs e)
        {

            chkEnableFeature.Text = String.Format("Enable Configuration for {0}", e.Node.Text);
            chkEnableFeature.Enabled = true;
            chkEnableFeature.Checked = (e.Node.Tag as IConfigurationPanel).EnableConfiguration;
            pnlConfigure.Controls.Clear();
            var pnl = (e.Node.Tag as IConfigurationPanel).Panel;
            pnlConfigure.Controls.Add(pnl);
            pnl.Dock = DockStyle.Fill;
            if ((e.Node.Tag as IConfigurationPanel).IsConfigured(m_xmlConfiguration))
            {
                chkEnableFeature.Enabled = false;
                pnlConfigure.Enabled = false;
            }
        }

        private void chkEnableFeature_CheckedChanged(object sender, EventArgs e)
        {
            pnlConfigure.Enabled = chkEnableFeature.Checked;
            (trvOptions.SelectedNode.Tag as IConfigurationPanel).EnableConfiguration = chkEnableFeature.Checked;
        }
    }
}
