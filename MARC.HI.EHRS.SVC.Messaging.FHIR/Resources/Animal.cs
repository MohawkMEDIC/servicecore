using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using System.ComponentModel;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{
    /// <summary>
    /// Represents data related to animal patients
    /// </summary>
    [XmlType("Animal", Namespace = "http://hl7.org/fhir")]
    public class Animal : Shareable
    {
        /// <summary>
        /// Gets or sets the species code
        /// </summary>
        [XmlElement("species")]
        [Description("E.g. Dog, Cow")]
        public CodeableConcept Species { get; set; }

        /// <summary>
        /// Gets or sets the breed code
        /// </summary>
        [XmlElement("breed")]
        [Description("E.g. Poodle, Angus")]
        public CodeableConcept Breed { get; set; }

        /// <summary>
        /// Gets or sets the status of the gender
        /// </summary>
        [XmlElement("genderStatus")]
        [Description("E.g. Neutered, Intact")]
        public CodeableConcept GenderStatus { get; set; }

    }
}
