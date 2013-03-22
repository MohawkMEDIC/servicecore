/**
 * Copyright 2012-2013 Mohawk College of Applied Arts and Technology
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you 
 * may not use this file except in compliance with the License. You may 
 * obtain a copy of the License at 
 * 
 * http://www.apache.org/licenses/LICENSE-2.0 
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations under 
 * the License.
 * 
 * User: fyfej
 * Date: 5-12-2012
 */

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

        /// <summary>
        /// Dummy configuration panel
        /// </summary>
        private class DummyConfigPanel : IAlwaysDeployedConfigurationPanel
        {

            #region IConfigurationPanel Members

            public string Name
            {
                get { return "NA"; }
            }

            public bool EnableConfiguration
            {
                get;
                set;
            }

            public Control Panel
            {
                get { return new Label() { Text = "Configuration Not Supported", TextAlign = ContentAlignment.MiddleCenter }; }
            }

            public void Configure(XmlDocument configurationDom)
            {
            }

            public void UnConfigure(XmlDocument configurationDom)
            {
            }

            public bool IsConfigured(XmlDocument configurationDom)
            {
                return true;
            }

            public bool Validate(XmlDocument configurationDom)
            {
                return true;
            }

            #endregion
        }

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
                    if (itm is DummyConfigPanel)
                        continue;

                    TreeNodeCollection addToCollection = trvOptions.Nodes;

                    string displayName = itm.Name;
                    // Does the node have a / ?
                    if (itm.Name.Contains("/"))
                    {
                        string[] comps = itm.Name.Split('/');
                        displayName = comps[1];
                        string parentName = comps[0];

                        // Locate the parent
                        var parentNode = addToCollection.Find(parentName, true);
                        if (parentNode.Length == 0) // create the parent node
                        {
                            parentNode = new TreeNode[] { addToCollection.Add(parentName, parentName) };
                            parentNode[0].Tag = new DummyConfigPanel();
                            parentNode[0].SelectedImageIndex = parentNode[0].ImageIndex = 0;

                        }
                        addToCollection = parentNode[0].Nodes;
                    }

                    // See if the display name is already set
                    var alreadyDn = addToCollection.Find(displayName, true);
                    if (alreadyDn.Length == 0)
                    {

                        TreeNode tn = addToCollection.Add(displayName, displayName);
                        tn.Tag = itm;

                        tn.SelectedImageIndex = tn.ImageIndex = itm.IsConfigured(m_xmlConfiguration) || itm is IAlwaysDeployedConfigurationPanel ? 0 : 1;
                    }
                    else
                    {
                        alreadyDn[0].Tag = itm;
                        alreadyDn[0].SelectedImageIndex = alreadyDn[0].ImageIndex = itm.IsConfigured(m_xmlConfiguration) || itm is IAlwaysDeployedConfigurationPanel ? 0 : 1;
                    }
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

            pnlConfigure.Controls.Clear();
            var pnl = (e.Node.Tag as IConfigurationPanel).Panel;
            pnlConfigure.Controls.Add(pnl);
            pnl.Dock = DockStyle.Fill;
            chkEnableFeature.Text = String.Format("Enable Configuration for {0}", e.Node.Text);

            if (e.Node.Tag is IAlwaysDeployedConfigurationPanel)
            {
                chkEnableFeature.Enabled = false;
                pnlConfigure.Enabled = true;
                chkEnableFeature.Checked = true;
            }
            else
            {
                chkEnableFeature.Enabled = true;
                chkEnableFeature.Checked = (e.Node.Tag as IConfigurationPanel).EnableConfiguration;
                pnlConfigure.Enabled = chkEnableFeature.Checked;

                if ((e.Node.Tag as IConfigurationPanel).IsConfigured(m_xmlConfiguration))
                {
                    chkEnableFeature.Enabled = false;
                    pnlConfigure.Enabled = false;
                }
            }
        }

        private void chkEnableFeature_CheckedChanged(object sender, EventArgs e)
        {
            pnlConfigure.Enabled = chkEnableFeature.Checked;
            (trvOptions.SelectedNode.Tag as IConfigurationPanel).EnableConfiguration = chkEnableFeature.Checked;
        }
    }
}
