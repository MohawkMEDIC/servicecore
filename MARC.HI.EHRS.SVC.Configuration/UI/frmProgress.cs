/*
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

namespace MARC.HI.EHRS.SVC.Configuration.UI
{
    public partial class frmProgress : Form
    {
        public frmProgress()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the status text
        /// </summary>
        public int ActionStatus { get { return pgAction.Value; } set { pgAction.Value = value; Application.DoEvents(); } }

        /// <summary>
        /// Gets or sets the status text
        /// </summary>
        public string ActionStatusText { get { return label2.Text; } set { label2.Text = value; Application.DoEvents(); } }


        /// <summary>
        /// Gets or sets the status text
        /// </summary>
        internal int OverallStatus { get { return pgMain.Value; } set { pgMain.Value = value; Application.DoEvents(); } }

        /// <summary>
        /// Gets or sets the status text
        /// </summary>
        internal string OverallStatusText { get { return label1.Text; } set { label1.Text = value; Application.DoEvents(); } }

    }
}
