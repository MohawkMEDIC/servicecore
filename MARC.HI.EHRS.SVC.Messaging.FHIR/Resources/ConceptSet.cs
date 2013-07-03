using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{
    /// <summary>
    /// Identifies a concept set
    /// </summary>
    [XmlType("ConceptSet", Namespace = "http://hl7.org/fhir")]
    public class ConceptSet
    {
        /// <summary>
        /// Concept set
        /// </summary>
        public ConceptSet()
        {
            this.Code = new List<PrimitiveCode<string>>();
        }
        /// <summary>
        /// Gets or sets the codification system from which codes are included
        /// </summary>
        [XmlElement("system")]
        [ElementProfile(MinOccurs = 1)]
        public FhirUri System { get; set; }
        /// <summary>
        /// Gets or sets the version of the code system
        /// </summary>
        [XmlElement("version")]
        public FhirString Version { get; set; }
        /// <summary>
        /// Gets or sets the codes to be imported
        /// </summary>
        [XmlElement("code")]
        public List<PrimitiveCode<String>> Code { get; set; }

    }
}
