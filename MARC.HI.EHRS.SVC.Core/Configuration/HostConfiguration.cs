/*
 * Copyright 2010-2018 Mohawk College of Applied Arts and Technology
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
 * Date: 1-9-2017
 */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MARC.HI.EHRS.SVC.Core.Configuration
{
    /// <summary>
    /// Represents the host configuration
    /// </summary>
    public class HostConfiguration : ConfigurationSection
    {

        /// <summary>
        /// Get the modules that are to be loaded 
        /// </summary>
        public List<Assembly> ServiceAssemblies { get; internal set; }

        /// <summary>
        /// Get the service providers for this application
        /// </summary>
        public List<Type> ServiceProviders { get; internal set; }

        /// <summary>
        /// Gets the name of the section the configuration data was loaded
        /// </summary>
        public string SectionName { get; internal set; }

        /// <summary>
        /// Gets the identifier (OID) of the current device
        /// </summary>
        public string DeviceIdentifier { get; set; }

        /// <summary>
        /// Gets the logical name of the device
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// Gets the jurisdiction configuration data
        /// </summary>
        public Jurisdiction JurisdictionData
        {
            get; internal set;
        }

        /// <summary>
        /// Gets the custodial data
        /// </summary>
        public CustodianshipData Custodianship
        {
            get; internal set;
        }
    }
}
