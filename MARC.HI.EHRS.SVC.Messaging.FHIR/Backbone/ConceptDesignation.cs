﻿using MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Backbone
{
    /// <summary>
    /// Concept designation
    /// </summary>
    [XmlType("ValueSet.Concept.Designation", Namespace = "http://hl7.org/fhir")]
    public class ConceptDesignation : BackboneElement
    {

        /// <summary>
        /// Gets or sets the language of the designation
        /// </summary>
        [XmlElement("language")]
        [Description("Human language of the designation")]
        [ElementProfile(RemoteBinding = "http://tools.ietf.org/html/bcp47")]
        public FhirCode<String> Language { get; set; }

        /// <summary>
        /// Gets or sets how the designation should be used
        /// </summary>
        [XmlElement("use")]
        [Description("Details how this designation would be used")]
        [ElementProfile(RemoteBinding = "http://hl7.org/fhir/ValueSet/designation-use")]
        public FhirCoding Use { get; set; }

        /// <summary>
        /// Gets or sets the value of the designation
        /// </summary>
        [ElementProfile(MinOccurs = 1)]
        [XmlElement("value")]
        [Description("The text value for the designation")]
        public FhirString Value { get; set; }

    }
}
