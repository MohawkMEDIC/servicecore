using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using System.Xml.Serialization;
using System.ComponentModel;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{
    /// <summary>
    /// Operation definition
    /// </summary>
    [XmlType("Operation", Namespace = "http://hl7.org/fhir")]
    public class OperationDefinition : Shareable
    {

        /// <summary>
        /// Type of operation
        /// </summary>
        [Description("Type of operation")]
        [XmlElement("code")]
        public PrimitiveCode<String> Type { get; set; }

        /// <summary>
        /// Documentation related to the operation
        /// </summary>
        [Description("Documentation related to the operation")]
        [XmlElement("description")]
        public FhirString Documentation { get; set; }

    }
}
