using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using System.Xml.Serialization;
using System.ComponentModel;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{
    /// <summary>
    /// Contact information
    /// </summary>
    [XmlType("ContactInfo", Namespace = "http://hl7.org/fhir")]
    public class Contact : Shareable
    {

        /// <summary>
        /// Gets or sets the relationships between the container
        /// </summary>
        [XmlElement("relationship")]
        [Description("The kind of relationship")]
        public List<CodeableConcept> Relationship { get; set; }

        // The name of the individual
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
        /// The gender of the individual
        /// </summary>
        [XmlElement("gender")]
        [Description("Gender for administrative purposes")]
        [ElementProfile(RemoteBinding = "http://hl7.org/fhir/vs/administrative-gender")]
        public CodeableConcept Gender { get; set; }

        /// <summary>
        /// Gets or sets the addresses of the user
        /// </summary>
        [XmlElement("address")]
        [Description("Addresses for the individual")]
        public List<Address> Address { get; set; }

        /// <summary>
        /// Gets or sets the organization
        /// </summary>
        [XmlElement("organization")]
        [Description("Organization that is associated with the contact")]
        public Resource<Organization> Organization { get; set; }

        /// <summary>
        /// Write the contact information
        /// </summary>
        internal override void WriteText(System.Xml.XmlWriter xw)
        {
            if (this.Name != null)
                this.Name.First().WriteText(xw);

            xw.WriteString(" - ");
            foreach (var rel in this.Relationship)
            {
                rel.WriteText(xw);
                xw.WriteRaw(", ");
            }
        }
    }
}
