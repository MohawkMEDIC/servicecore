using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Resources.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{
    /// <summary>
    /// Resource base
    /// </summary>
    [XmlType("ResourceBase", Namespace = "http://hl7.org/fhir")]
    public class ResourceBase : FhirElement
    {
        /// <summary>
        /// ctor
        /// </summary>
        public ResourceBase()
        {
            this.Attributes = new List<ResourceAttributeBase>();

        }
        /// <summary>
        /// Gets or sets the internal identifier for the resource
        /// </summary>
        [XmlIgnore]
        public string Id { get; set; }

        /// <summary>
        /// Version identifier
        /// </summary>
        [XmlIgnore]
        public string VersionId { get; set; }

        /// <summary>
        /// Extended observations about the resource that can be used to tag the resource
        /// </summary>
        [XmlIgnore]
        public List<ResourceAttributeBase> Attributes { get; set; }
        /// <summary>
        /// Last updated timestamp
        /// </summary>
        [XmlIgnore]
        public DateTime Timestamp { get; set; }

    }
}
