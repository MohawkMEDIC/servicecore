using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Resources;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Backbone
{

    /// <summary>
    /// Bundle resource
    /// </summary>
    [XmlType("Bundle.Resource", Namespace = "http://hl7.org/fhir")]
    public class BundleResrouce : FhirElement
    {

        /// <summary>
        /// 
        /// </summary>
        public BundleResrouce()
        {

        }
        /// <summary>
        /// Creates a new instance of the resource bunlde
        /// </summary>
        /// <param name="r"></param>
        public BundleResrouce(ResourceBase r)
        {
            this.Resource = r;
        }
        /// <summary>
        /// Gets or sets the resource
        /// </summary>
        [XmlElement("Patient", Type = typeof(Patient))]
        [XmlElement("ValueSet", Type = typeof(ValueSet))]
        [XmlElement("Organization", Type = typeof(Organization))]
        [XmlElement("Practitioner", Type = typeof(Practictioner))]
        [XmlElement("RelatedPerson", Type = typeof(RelatedPerson))]
        public ResourceBase Resource { get; set; }

    }
}