using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{
    /// <summary>
    /// Import for a profile
    /// </summary>
    [XmlType("Import", Namespace = "http://hl7.org/fhir")]
    public class ProfileImport : FhirElement
    {

        /// <summary>
        /// Fhir URI of the import
        /// </summary>
        [XmlElement("uri")]
        public FhirUri Uri { get; set; }

    }
}
