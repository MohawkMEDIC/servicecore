using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using System.Xml.Serialization;
using System.ComponentModel;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Backbone
{
    /// <summary>
    /// Operation definition
    /// </summary>
    [XmlType("InteractionDefinition", Namespace = "http://hl7.org/fhir")]
    public class InteractionDefinition : BackboneElement
    {

        /// <summary>
        /// Type of operation
        /// </summary>
        [Description("Type of operation")]
        [XmlElement("code")]
        public FhirCode<String> Type { get; set; }

        /// <summary>
        /// Documentation related to the operation
        /// </summary>
        [Description("Documentation related to the operation")]
        [XmlElement("documentation")]
        public FhirString Documentation { get; set; }

    }
}
