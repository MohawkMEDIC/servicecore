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

namespace MARC.HI.EHRS.SVC.QM.Persistence.Data.Configuration.UI.Panels
{
    public partial class pnlConfigureMessagePersistence : UserControl
    {
        public pnlConfigureMessagePersistence()
        {
            InitializeComponent();
            
        }

       
        /// <summary>
        /// Value
        /// </summary>
        public decimal MaxAge
        {
            get
            {
                return this.numMaxAge.Value;
            }
            set
            {
                this.numMaxAge.Value = value;
            }
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
