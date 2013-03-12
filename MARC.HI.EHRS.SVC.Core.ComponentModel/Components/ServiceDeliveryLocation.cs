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
 * Date: 7-5-2012
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.DataTypes;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Core.ComponentModel.Components
{
    /// <summary>
    /// Identifies a service delivery location
    /// </summary>
    [Serializable][XmlType("ServiceDeliveryLocation")]
    public class ServiceDeliveryLocation : HealthServiceRecordComponent
    {
        /// <summary>
        /// Gets or sets the SHRID of the service delivery location
        /// </summary>
        [XmlAttribute("id")]
        public decimal Id { get; set; }
        /// <summary>
        /// Gets or sets the Name of the service delivery location
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the perminant address of the service delivery location
        /// </summary>
        [XmlElement("addr")]
        public AddressSet Address { get; set; }
        /// <summary>
        /// Gets or sets the location type of the service delivery location
        /// </summary>
        [XmlElement("code")]
        public CodeValue LocationType { get; set; }
        /// <summary>
        /// Gets a list of alternate identifiers for the SDL
        /// </summary>
        [XmlElement("altId")]
        public List<DomainIdentifier> AlternateIdentifiers { get; set; }
        /// <summary>
        /// Telecommunications addresses
        /// </summary>
        [XmlElement("telecom")]
        public List<TelecommunicationsAddress> TelecomAddresses { get; set; }

        /// <summary>
        /// Creates a new instance of the service delivery location
        /// </summary>
        public ServiceDeliveryLocation()
        {
            this.AlternateIdentifiers = new List<DomainIdentifier>();
            this.TelecomAddresses = new List<TelecommunicationsAddress>();
        }
    }
}
