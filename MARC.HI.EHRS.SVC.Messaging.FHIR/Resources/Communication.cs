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
    /// Represents a language of communication
    /// </summary>
    [XmlType("Communication", Namespace = "http://hl7.org/fhir")]
    public class Communication : Shareable
    {
        /// <summary>
        /// Gets or sets the language code
        /// </summary>
        [XmlElement("language")]
        [Description("Language with optional region")]
        [ElementProfile(MinOccurs = 1, RemoteBinding = "http://tools.ietf.org/html/bcp47")]
        public CodeableConcept Value { get; set; }
        /// <summary>
        /// Gets or sets the mode of communication
        /// </summary>
        [XmlElement("mode")]
        [Description("Method of expression")]
        [ElementProfile(RemoteBinding = "http://hl7.org/fhir/v3/vs/LanguageAbilityMode")]
        public CodeableConcept Mode { get; set; }
        /// <summary>
        /// Gets or sets the proficiency level
        /// </summary>
        [XmlElement("proficiencyLevel")]
        [Description("How well understood/expressed")]
        [ElementProfile(RemoteBinding = "http://hl7.org/fhir/v3/vs/LanguageAbilityProficiency")]
        public CodeableConcept ProficiencyLevel { get; set; }
        /// <summary>
        /// Gets or sets the preference indicator
        /// </summary>
        [XmlElement("preference")]
        [Description("Language preference indicator")]
        public FhirBoolean Preference { get; set; }
    }
}
