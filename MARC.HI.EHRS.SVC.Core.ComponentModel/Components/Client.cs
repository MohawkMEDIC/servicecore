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
    [Serializable][XmlType("Client")]
    public class Client : HealthServiceRecordContainer
    {
        /// <summary>
        /// Gets or sets the perminant address of the client at the time of the last encounter
        /// </summary>
        [XmlElement("addr")]
        public AddressSet PerminantAddress { get; set; }

        /// <summary>
        /// Gets or sets the last legal name of the client
        /// </summary>
        [XmlElement("legalName")]
        public NameSet LegalName { get; set; }

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
