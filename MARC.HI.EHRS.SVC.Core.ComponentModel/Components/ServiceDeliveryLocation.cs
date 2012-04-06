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
