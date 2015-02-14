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
 * Date: 16-7-2012
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
    /// Identifies a client of a healthcare transaction
    /// </summary>
    [Serializable]
    [XmlType("Client", Namespace = "urn:marc-hi:svc:componentModel")]
    public class Client : HealthServiceRecordContainer
    {
        /// <summary>
        /// Gets or sets the perminant address of the client at the time of the last encounter
        /// </summary>
        [XmlElement("addr")]
        public virtual AddressSet PerminantAddress { get; set; }

        /// <summary>
        /// Gets or sets the last legal name of the client
        /// </summary>
        [XmlElement("legalName")]
        public virtual NameSet LegalName { get; set; }

        /// <summary>
        /// Gets the set of alternate domain identifiers that the client is known as
        /// </summary>
        [XmlElement("altId")]
        public List<DomainIdentifier> AlternateIdentifiers { get; set; }

        /// <summary>
        /// Gets a list of telecommunications addresses for the client
        /// </summary>
        [XmlElement("telecom")]
        public List<TelecommunicationsAddress> TelecomAddresses { get; set; }

        /// <summary>
        /// The birth time
        /// </summary>
        [XmlElement("birthTime")]
        public TimestampPart BirthTime { get; set; }

        /// <summary>
        /// The gender code
        /// </summary>
        [XmlAttribute("genderCode")]
        public String GenderCode { get; set; }

        /// <summary>
        /// Creates a new instance of the client class
        /// </summary>
        public Client()
        {
            this.AlternateIdentifiers = new List<DomainIdentifier>();
            this.TelecomAddresses = new List<TelecommunicationsAddress>();
        }

    }
}
