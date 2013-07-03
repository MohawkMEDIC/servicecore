using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{
    /// <summary>
    /// Composed from other systems
    /// </summary>
    [XmlType("Compose", Namespace = "http://hl7.org/fhir")]
    public class ComposeDefinition
    {

        /// <summary>
        /// Compse a definition
        /// </summary>
        public ComposeDefinition()
        {
            this.Import = new List<FhirUri>();
            this.Include = new List<ConceptSet>();
        }

        /// <summary>
        /// The uri of an import
        /// </summary>
        [XmlElement("import")]
        public List<FhirUri> Import { get; set; }

        /// <summary>
        /// Included concepts
        /// </summary>
        [XmlElement("include")]
        public List<ConceptSet> Include { get; set; }
    }
}
