using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using System.ComponentModel;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{
    /// <summary>
    /// Identifies a contact entity
    /// </summary>
    [XmlType("ContactEntity", Namespace = "http://hl7.org/fhir")]
    public class ContactEntity : FhirElement
    {
        /// <summary>
        /// Gets or sets the type of contact entity
        /// </summary>
        [XmlElement("purpose")]
        [Description("The type of contact")]
        public FhirCodeableConcept Type { get; set; }
        /// <summary>
        /// Gets or sets the name of the contact entity
        /// </summary>
        [XmlElement("name")]
        [Description("A name associated with the contact entity")]
        public FhirHumanName Name { get; set; }
        /// <summary>
        /// Gets or sets the telecommunications address of the entity
        /// </summary>
        [XmlElement("telecom")]
        [Description("Contact details (telephone, email, etc) for a contact")]
        public List<FhirTelecom> Telecom { get; set; }
        /// <summary>
        /// Gets or sets the address of the entity
        /// </summary>
        [XmlElement("address")]
        [Description("Visiting or postal address for the contact")]
        public FhirAddress Address { get; set; }

        /// <summary>
        /// Identifies the gender of the contact
        /// </summary>
        [XmlElement("gender")]
        [Description("Gender for administrative purposes")]
        [ElementProfile(RemoteBinding = "http://hl7.org/fhir/vs/administrative-gender")]
        public FhirCodeableConcept Gender { get; set; }
    }
}
