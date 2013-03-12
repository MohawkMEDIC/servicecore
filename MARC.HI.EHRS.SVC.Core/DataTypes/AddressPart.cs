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
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Core.DataTypes
{
    /// <summary>
    /// Represents one part of an address
    /// </summary>
    [Serializable][XmlType("AddressPart")]
    public class AddressPart
    {
        /// <summary>
        /// Represents the type of street address
        /// </summary>
        public enum AddressPartType
        {
            AddressLine = 0x01,
            AdditionalLocator = 0x02,
            UnitIdentifier = 0x03,
            UnitDesignator = 0x04,
            DeliveryAddressLine = 0x05,
            DeliveryInstallationType = 0x06,
            DeliveryInstallationArea = 0x07,
            DeliveryInstallationQualifier = 0x08,
            DeliveryMode = 0x09,
            DeliveryModeIdentifier = 0x0a,
            StreetAddressLine = 0x0b,
            BuildingNumber = 0x0c,
            BuildingNumberSuffix = 0x0d,
            StreetName = 0x0e,
            StreetNameBase = 0x0f,
            StreetType = 0x10,
            Direction = 0x11,
            CareOf = 0x12,
            CensusTract = 0x13,
            Country = 0x14,
            County = 0x15,
            City = 0x16,
            Delimeter = 0x17,
            PostBox = 0x18,
            Precinct = 0x19,
            State = 0x1a,
            PostalCode = 0x1b
        }

        /// <summary>
        /// Gets or sets the type of address part being represented
        /// </summary>
        [XmlAttribute("type")]
        public AddressPartType PartType { get; set; }

        /// <summary>
        /// Represents the value of the address part
        /// </summary>
        [XmlAttribute("value")]
        public string AddressValue { get; set; }
    }
}
