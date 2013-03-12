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
 * Date: 1-8-2012
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.DataTypes;

namespace MARC.HI.EHRS.SVC.Core.Services
{
    /// <summary>
    /// Identifies a service that contains information about the system
    /// </summary>
    public interface ISystemConfigurationService
    {
        /// <summary>
        /// Gets or sets OID of the machine
        /// </summary>
        string DeviceIdentifier { get; }

        /// <summary>
        /// Gets the name of the device the software is running on
        /// </summary>
        string DeviceName { get; }

        /// <summary>
        /// Gets the data that describes the jurisdiction this service runs within
        /// </summary>
        Jurisdiction JurisdictionData { get; }

        /// <summary>
        /// Gets the data that describes how this service is to act as a custodian 
        /// for records
        /// </summary>
        CustodianshipData Custodianship { get; }

        /// <summary>
        /// Gets the registrar that can be used to locate OIDs based on a friendly name
        /// </summary>
        OidRegistrar OidRegistrar { get; }

        /// <summary>
        /// Determine if the specified device is a valid sender
        /// </summary>
        bool IsRegisteredDevice(DomainIdentifier deviceId);
    }
}
