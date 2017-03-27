using MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Backbone
{
    /// <summary>
    /// Represents an immunization criterion
    /// </summary>
    [XmlType("ImmunizationRecommendation.DateCriterion", Namespace = "http://hl7.org/fhir")]
    public class ImmunizationRecommendationDateCriterion : BackboneElement
    {
        /// <summary>
        /// Gets or sets the code
        /// </summary>
        [XmlElement("code")]
        [FhirElement(MinOccurs = 1)]
        public FhirCodeableConcept Code { get; set; }

        /// <summary>
        /// Value of the date
        /// </summary>
        [XmlElement("value")]
        [FhirElement(MinOccurs = 1)]
        public FhirDateTime Value { get; set; }
    }
}