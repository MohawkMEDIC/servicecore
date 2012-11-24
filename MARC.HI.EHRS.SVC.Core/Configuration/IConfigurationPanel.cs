/* 
 * Copyright 2008-2011 Mohawk College of Applied Arts and Technology
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
 * User: Justin Fyfe
 * Date: 08-24-2011
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace MARC.HI.EHRS.SVC.Core.Configuration
{
    /// <summary>
    /// Represents a configuration panel for the SVC Core Configuration Tool
    /// </summary>
    public interface IConfigurationPanel
    {

        /// <summary>
        /// Gets or sets the configuration panel name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Enable configuration
        /// </summary>
        bool EnableConfiguration { get; set;  }

        /// <summary>
        /// Gets the configuration panel
        /// </summary>
        Control Panel { get; }

        /// <summary>
        /// Configure the panel options
        /// </summary>
        void Configure(XmlDocument configurationDom);

        /// <summary>
        /// UnConfigure the panel options
        /// </summary>
        void UnConfigure(XmlDocument configurationDom);

        /// <summary>
        /// Determine if the configuration option is configured
        /// </summary>
        bool IsConfigured(XmlDocument configurationDom);

        /// <summary>
        /// Validate configuration
        /// </summary>
        bool Validate(XmlDocument configurationDom);
    }
}
