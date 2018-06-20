using System.Xml;

namespace MARC.HI.EHRS.SVC.Configuration.Wcf
{
    public class WcfCertificateValidationInfo
    {

        /// <summary>
        /// Creates a new validation info object
        /// </summary>
        public WcfCertificateValidationInfo(XmlNode clientCert)
        {
            this.ValidationMode = clientCert.Attributes["certificateValidationMode"]?.Value;
            this.StoreLocation = clientCert.Attributes["trustedStoreLocation"]?.Value;
        }

        /// <summary>
        /// Represent as XML
        /// </summary>
        public XmlElement ToXml(XmlElement parent)
        {
            XmlElement xel = parent.GetOrCreateElement("authentication") as XmlElement;
            xel.SetAttribute("certificateValidationMode", this.ValidationMode);
            xel.SetAttribute("trustedStoreLocation", this.StoreLocation);
            return xel;
        }

        /// <summary>
        /// Validation mode 
        /// </summary>
        public string ValidationMode { get; set; }

        /// <summary>
        /// Store location
        /// </summary>
        public string StoreLocation { get; set; }

    }
}