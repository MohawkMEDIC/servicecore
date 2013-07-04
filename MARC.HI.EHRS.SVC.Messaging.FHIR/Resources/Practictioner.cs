using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes;
using System.ComponentModel;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{
    /// <summary>
    /// Identifies a practitioner
    /// </summary>
    [XmlType("Practitioner", Namespace = "http://hl7.org/fhir")]
    [XmlRoot("Practitioner", Namespace = "http://hl7.org/fhir")]
    [Profile(ProfileId = "svccore")]
    [ResourceProfile(Name = "Service Core Resource - Practictioner")]
    public class Practictioner : ResourceBase
    {

        /// <summary>
        /// Default constructor
        /// </summary>
        public Practictioner()
        {
            this.Identifier = new List<Identifier>();
            this.Name = new List<HumanName>();
            this.Telecom = new List<Telecom>();
            this.Address = new List<Address>();
            this.Role = new List<CodeableConcept>();
            this.Photo = new List<Attachment>();
            this.Specialty = new List<CodeableConcept>();
            this.Communication = new List<CodeableConcept>();
        }

        /// <summary>
        /// Gets or sets identifier
        /// </summary>
        [XmlElement("identifier")]
        public List<Identifier> Identifier { get; set; }

        /// <summary>
        /// The name of the individual
        /// </summary>
        [XmlElement("name")]
        [Description("A name associated with the individual")]
        public List<HumanName> Name { get; set; }

        /// <summary>
        /// The telecommunications addresses for the individual
        /// </summary>
        [XmlElement("telecom")]
        [Description("A contact detail for the individual")]
        public List<Telecom> Telecom { get; set; }

        /// <summary>
        /// Gets or sets the addresses of the user
        /// </summary>
        [XmlElement("address")]
        [Description("Addresses for the individual")]
        public List<Address> Address { get; set; }

        /// <summary>
        /// The gender of the individual
        /// </summary>
        [XmlElement("gender")]
        [Description("Gender for administrative purposes")]
        [ElementProfile(RemoteBinding = "http://hl7.org/fhir/vs/administrative-gender")]
        public CodeableConcept Gender { get; set; }

        /// <summary>
        /// The birth date of the individual
        /// </summary>
        [XmlElement("birthDate")]
        [Description("The date and time of birth for the individual")]
        public Date BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the photograph of the user
        /// </summary>
        [XmlElement("photo")]
        [Description("Image of the person")]
        public List<Attachment> Photo { get; set; }

        /// <summary>
        /// Gets or sets the organization
        /// </summary>
        [XmlElement("organization")]
        [Description("The represented organization")]
        public Resource<Organization> Organization { get; set; }

        /// <summary>
        /// Gets or sets the role
        /// </summary>
        [XmlElement("role")]
        [Description("A role the practitioner has")]
        public List<CodeableConcept> Role { get; set; }

        /// <summary>
        /// Gets or sets the specialty
        /// </summary>
        [XmlElement("specialty")]
        [Description("Specific speciality of the practitioner")]
        public List<CodeableConcept> Specialty { get; set; }

        /// <summary>
        /// Gets or sets the period
        /// </summary>
        [XmlElement("period")]
        [Description("The Period during which the person is authorized to perform the service")]
        public Period Period { get; set; }

        /// <summary>
        /// Gets or sets the qualifications of the practicioner
        /// </summary>
        [XmlElement("qualification")]
        [Description("Qualifications relevant to the provided service")]
        public List<Qualification> Qualification { get; set; }

        /// <summary>
        /// Gets or sets communications the practitioner can use
        /// </summary>
        [XmlElement("communication")]
        [Description("A language the practitioner is able to use in patient communication")]
        [ElementProfile(RemoteBinding = "http://tools.ietf.org/html/bcp47")]
        public List<CodeableConcept> Communication { get; set; }

    }
}
