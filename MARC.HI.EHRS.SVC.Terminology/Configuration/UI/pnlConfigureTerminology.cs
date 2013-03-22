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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MARC.HI.EHRS.SVC.Core.Configuration;
using System.Xml;

namespace MARC.HI.EHRS.SVC.Terminology.Configuration
{
    public partial class pnlConfigureTerminology : UserControl
    {
        public pnlConfigureTerminology()
        {
            InitializeComponent();
        }

        private void chkEnableDb_CheckedChanged(object sender, EventArgs e)
        {
            dbSelector.Enabled = chkEnableDb.Checked;
        }

        private void chkEnableCTS_CheckedChanged(object sender, EventArgs e)
        {
            txtCTSUrl.Enabled = chkEnableCTS.Checked;
        }

      
        /// <summary>
        /// True if the local validation should be enabled
        /// </summary>
        public bool EnableLocalValidation {
            get { return this.chkEnableDb.Checked; }
            set { this.chkEnableDb.Checked = value; }
        }

        /// <summary>
        /// True if remote validation should be enalbed
        /// </summary>
        public bool EnableRemoteValidation {
            get { return this.chkEnableCTS.Checked; }
            set { this.chkEnableCTS.Checked = value; }
        }

        /// <summary>
        /// Maximum memory cache size
        /// </summary>
        public decimal MaxCacheSize {
            get { return numCacheSize.Value; }
            set { numCacheSize.Value = value; }
        }

        /// <summary>
        /// CTS support
        /// </summary>
        public string[] CtsCS
        {
            get
            {
                List<string> retVal = new List<string>();
                if (chkSNOMED.Checked)
                    retVal.Add("2.16.840.1.113883.6.96");
                if (chkLOINC.Checked)
                    retVal.Add("2.16.840.1.113883.6.1");
                if (chkICD.Checked)
                    retVal.Add("2.16.840.1.113883.6.3");
                return retVal.ToArray();
            }
            set
            {
                if (value == null)
                    return;

                if (value.Contains("2.16.840.1.113883.6.3"))
                    chkICD.Checked = true;
                if (value.Contains("2.16.840.1.113883.6.1"))
                    chkLOINC.Checked = true;
                if (value.Contains("2.16.840.1.113883.6.96"))
                    chkSNOMED.Checked = true;
            }
        }

        /// <summary>
        /// CTS URL
        /// </summary>
        public string CtsUrl {
            get { return txtCTSUrl.Text; }
            set { txtCTSUrl.Text = value; }
        }

        public IDatabaseConfigurator DatabaseConfigurator
        {
            get
            {
                return this.dbSelector.DatabaseConfigurator;
            }
            set
            {
                this.dbSelector.DatabaseConfigurator = value;
            }
        }

        /// <summary>
        /// Get connection string
        /// </summary>
        internal string GetConnectionString(XmlDocument configurationDom)
        {
            return this.dbSelector.GetConnectionString(configurationDom);
        }


        /// <summary>
        /// Set connection string
        /// </summary>
        internal void SetConnectionString(XmlDocument configurationDom, string p)
        {
            this.dbSelector.SetConnectionString(configurationDom, p);
        }
    }
}
