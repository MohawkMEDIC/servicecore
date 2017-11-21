﻿using MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Backbone
{

    /// <summary>
    /// Slicing rules
    /// </summary>
    [XmlType("SlicingRules", Namespace = "http://hl7.org/fhir")]
    [FhirValueSet(Uri = "http://hl7.org/fhir/ValueSet/resource-slicing-rules")]
    public enum SlicingRules
    {
        [XmlEnum("closed")]
        Closed,
        [XmlEnum("open")]
        Open,
        [XmlEnum("openAtEnd")]
        OpenAtEnd
    }

    /// <summary>
    /// Identifies the sliced
    /// </summary>
    [XmlType("ElementSlicing", Namespace = "http://hl7.org/fhir")]
    public class ElementSlicing : FhirElement
    {

        /// <summary>
        /// Constructs a new element slice
        /// </summary>
        public ElementSlicing()
        {
            this.Discriminator = new List<FhirString>();
        }

        /// <summary>
        /// Gets or sets element values that are used to distinguish the slices
        /// </summary>
        [XmlElement("discriminator")]
        [Description("Element values that used to distinguis the slices")]
        public List<FhirString> Discriminator { get; set; }

        /// <summary>
        /// Gets or sets the text description of how this slicing works
        /// </summary>
        [XmlElement("description")]
        [Description("Text description of how slicing works")]
        public FhirString Description { get; set; }

        /// <summary>
        /// Gets or sets if elements must be in the same order as slices
        /// </summary>
        [XmlElement("ordered")]
        [Description("If elements must be in same order as slices")]
        public FhirBoolean Ordered { get; set; }

        /// <summary>
        /// Gets or sets the rules
        /// </summary>
        [XmlElement("rules")]
        [Description("Rules for slice")]
        [FhirElement(MinOccurs = 1)]
        public FhirCode<SlicingRules> Rules { get; set; }

    }
}