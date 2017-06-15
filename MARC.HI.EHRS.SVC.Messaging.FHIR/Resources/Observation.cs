﻿using MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{
    /// <summary>
    /// Filter operators
    /// </summary>
    [XmlType("ObservationStatus", Namespace = "http://hl7.org/fhir")]
    [FhirValueSet(Uri = "http://hl7.org/fhir/ValueSet/observationStatus")]
    public enum ObservationStatus
    {
        [XmlEnum("registered")]
        Registered,
        [XmlEnum("preliminary")]
        Preliminary,
        [XmlEnum("final")]
        Final,
        [XmlEnum("amended")]
        Amended,
        [XmlEnum("corrected")]
        Corrected,
        [XmlEnum("cancelled")]
        Cancelled,
        [XmlEnum("entered-in-error")]
        EnteredInError,
        [XmlEnum("unknown")]
        Unknown
    }
    /// <summary>
    /// Observation 
    /// </summary>
    [XmlType("Observation", Namespace = "http://hl7.org/fhir")]
    [XmlRoot("Observation", Namespace = "http://hl7.org/fhir")]
    public class Observation : ResourceBase
    {

        /// <summary>
        /// Default ctor
        /// </summary>
        public Observation()
        {
            this.Identifier = new List<FhirIdentifier>();
        }

        /// <summary>
        /// Gets or sets the identifiers for the observation
        /// </summary>
        [XmlElement("identifier")]
        public List<FhirIdentifier> Identifier { get; set; }

        /// <summary>
        /// Gets or sets the status of the observation
        /// </summary>
        [XmlElement("status")]
        public FhirCode<ObservationStatus> Status { get; set; }

        /// <summary>
        /// Gets or sets the category
        /// </summary>
        [XmlElement("category")]
        public FhirCodeableConcept Category { get; set; }

        /// <summary>
        /// Gets or sets the code or type of observation
        /// </summary>
        [XmlElement("code")]
        public FhirCodeableConcept Code { get; set; }

        /// <summary>
        /// Gets or sets the subject of the observation
        /// </summary>
        [XmlElement("subject")]
        public Reference<Patient> Subject { get; set; }

        //[XmlElement("context")]
        //public Reference<Encounter> Context { get; set; }

            /// <summary>
            /// Gets or sets the time that the observation was made
            /// </summary>
        [XmlElement("effectiveDateTime")]
        public FhirDateTime EffectiveDateTime { get; set; }

        /// <summary>
        /// Gets or sets the date or time that the observation became available
        /// </summary>
        [XmlElement("issued")]
        public FhirInstant Issued { get; set; }

        /// <summary>
        /// Gets or sets the performer of the observation
        /// </summary>
        [XmlElement("performer")]
        public Reference<Practictioner> Performer { get; set; }

        /// <summary>
        /// Gets or sets the value of the observation
        /// </summary>
        [XmlElement("valueQuantity", typeof(FhirQuantity))]
        [XmlElement("valueCodeableConcept", typeof(FhirCodeableConcept))]
        [XmlElement("valueString", typeof(FhirString))]
        public Object Value { get; set; }

        /// <summary>
        /// Gets or sets the reason why data is not present
        /// </summary>
        [XmlElement("dataAbsentReason")]
        public FhirCodeableConcept DataAbsentReason { get; set; }

        /// <summary>
        /// Gets or sets the interpretation of the observation
        /// </summary>
        [XmlElement("interpretation")]
        public FhirCodeableConcept Interpretation { get; set; }

        /// <summary>
        /// Gets or sets the comment related to the observation
        /// </summary>
        [XmlElement("comment")]
        public String Comment { get; set; }

    }
}
