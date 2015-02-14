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
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Core.DataTypes
{
    /// <summary>
    /// Represents a set of address components
    /// </summary>
    [Serializable]
    [XmlType("AddressSet", Namespace = "urn:marc-hi:svc:componentModel")]
    public class AddressSet : Datatype
    {
        /// <summary>
        /// Uses for an address
        /// </summary>
        [Flags]
        public enum AddressSetUse
        {
            /// <summary>
            /// Search address
            /// </summary>
            Search = 0x0,
            /// <summary>
            /// A Home address for the entity
            /// </summary>
            HomeAddress = 0x1,
            /// <summary>
            /// The address to reach the entity after business hours
            /// </summary>
            PrimaryHome = 0x2,
            /// <summary>
            /// Direct address, usually used for telecommunications 
            /// </summary>
            Direct = 0x4,
            /// <summary>
            /// A vacation home or cottage
            /// </summary>
            VacationHome = 0x8,
            /// <summary>
            /// A place of work
            /// </summary>
            WorkPlace = 0x10,
            /// <summary>
            /// A public address for the entity
            /// </summary>
            Public = 0x20,
           /// <summary>
           /// Flags the address as a "bad" address
           /// </summary>
            BadAddress = 0x40,
            /// <summary>
            /// An address that can be used for a physical visit
            /// </summary>
            PhysicalVisit = 0x80,
            /// <summary>
            /// An address used to send mail (ie: post office box)
            /// </summary>
            PostalAddress = 0x100,
            /// <summary>
            /// A temporary address for sending mail
            /// </summary>
            TemporaryAddress = 0x200
        }

        /// <summary>
        /// Identifies the parts of the address
        /// </summary>
        [XmlElement("part")]
        public List<AddressPart> Parts { get; set; }

        /// <summary>
        /// Gets or sets the contexts under which the address set can be used
        /// </summary>
        [XmlAttribute("use")]
        public AddressSetUse Use { get; set; }

        /// <summary>
        /// Creates a new instance of the address set
        /// </summary>
        public AddressSet()
        {
            this.Parts = new List<AddressPart>();
        }
    }
}
