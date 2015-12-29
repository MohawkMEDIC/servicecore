using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{
    /// <summary>
    /// Qualification
    /// </summary>
    [XmlType("Qualification", Namespace = "http://hl7.org/fhir")]
    public class Qualification : FhirElement
    {
        /// <summary>
        /// Gets or sets the code
        /// </summary>
        [XmlElement("code")]
        public FhirCodeableConcept Code { get; set; }
        /// <summary>
        /// Gets or sets the period of time
        /// </summary>
        [XmlElement("period")]
        public FhirPeriod Period { get; set; }
        /// <summary>
        /// Gets or sets the issuer organization
        /// </summary>
        [XmlElement("issuer")]
        public Reference<Organization> Issuer { get; set; }

    }
}
