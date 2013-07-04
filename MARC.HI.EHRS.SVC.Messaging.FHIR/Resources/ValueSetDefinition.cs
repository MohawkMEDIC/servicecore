using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{
    /// <summary>
    /// Value set definition
    /// </summary>
    [XmlType("Define", Namespace = "http://hl7.org/fhir")]
    public class ValueSetDefinition : Shareable
    {

        /// <summary>
        /// Value set definition
        /// </summary>
        public ValueSetDefinition()
        {
            this.Concept = new List<ConceptDefinition>();
        }

        /// <summary>
        /// The code system which is defined by this value set
        /// </summary>
        [XmlElement("system")]
        public FhirUri System { get; set; }

        /// <summary>
        /// Gets or sets the list of concepts
        /// </summary>
        [XmlElement("concept")]
        public List<ConceptDefinition> Concept { get; set; }
    }
}
