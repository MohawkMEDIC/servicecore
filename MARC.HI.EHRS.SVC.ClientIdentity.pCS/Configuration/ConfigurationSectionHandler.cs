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
 * Date: 10-7-2012
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace MARC.HI.EHRS.SVC.ClientIdentity.pCS.Configuration
{
    /// <summary>
    /// Configuration Handler
    /// </summary>
    /// <marc.hi.ehrs.svc.clientid.pcs connectionString="WCFConnector Connection String" deviceId="ID of the CR"/>
    public class ConfigurationSectionHandler : IConfigurationSectionHandler
    {

        /// <summary>
        /// Gets the connection string to the WCF Connector that should be used for fetching a 
        /// client
        /// </summary>
        public string WcfConnectionString { get; private set; }

        /// <summary>
        /// Gets the device identifier for the receiving device
        /// </summary>
        public string DeviceId { get; private set; }

        #region IConfigurationSectionHandler Members

        /// <summary>
        /// Create the configuration section handler
        /// </summary>
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {

            if (section.Attributes["connectionString"] == null)
                throw new ConfigurationErrorsException("Must supply the connection string argument to the client registry configuration");
            else
                this.WcfConnectionString = section.Attributes["connectionString"].Value;

            if (section.Attributes["deviceId"] == null)
                throw new ConfigurationErrorsException("Must supply the device identifier for the client registry configuration");
            else
                this.DeviceId = section.Attributes["deviceId"].Value;

            return this;

        }

        #endregion
    }
}
