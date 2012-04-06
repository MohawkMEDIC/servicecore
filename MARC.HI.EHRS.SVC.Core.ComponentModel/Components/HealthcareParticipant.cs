using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.DataTypes;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Core.ComponentModel.Components
{
    /// <summary>
    /// Represents a healthcare participant within a clinical act
    /// </summary>
    [Serializable][XmlType("HealthcareParticipant")]
    public class HealthcareParticipant : HealthServiceRecordContainer
    {
        /// <summary>
        /// Identifies the healthcare participant type
        /// </summary>
        public enum HealthcareParticipantType
        {
            Person = 0x01,
            Organization = 0x02
        }

        /// <summary>
        /// Gets or sets the type of healthcare participant
        /// </summary>
        [XmlAttribute("classifier")]
        public HealthcareParticipantType Classifier { get; set; }

        /// <summary>
        /// Identifies the type of healthcare worker
        /// </summary>
        [XmlElement("type")]
        public CodeValue Type { get; set; }

        /// <summary>
        /// Gets or sets the primary address of the healthcare participant
        /// </summary>
        [XmlElement("addr")]
        public AddressSet PrimaryAddress { get; set; }

        /// <summary>
        /// Gets or sets the legal name of the healthcare participant
        /// </summary>
        [XmlElement("name")]
        public NameSet LegalName { get; set; }

        /// <summary>
        /// Gets alternative identifiers for the participant
        /// </summary>
        [XmlElement("altId")]
        public List<DomainIdentifier> AlternateIdentifiers { get; set; }

        /// <summary>
        /// Gets a list of telecommunications addresses that the participant can be contacted at
        /// </summary>
        [XmlElement("telecom")]
        public List<TelecommunicationsAddress> TelecomAddresses { get; set; }

        /// <summary>
        /// Creates a new instance of the healthcare participant class
        /// </summary>
        public HealthcareParticipant()
        {
            this.AlternateIdentifiers = new List<DomainIdentifier>();
            this.TelecomAddresses = new List<TelecommunicationsAddress>();
        }
    }
}
