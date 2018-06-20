using MARC.HI.EHRS.SVC.Configuration.Security;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

namespace MARC.HI.EHRS.SVC.Configuration.Wcf
{
    /// <summary>
    /// Represents data related to a certificate credential
    /// </summary>
    public class WcfCertificateCredentialInfo
    {

        /// <summary>
        /// Creates a new certificate credential info object
        /// </summary>
        public WcfCertificateCredentialInfo(XmlNode serviceCert)
        {

            this.StoreName = serviceCert.Attributes["storeName"]?.Value;
            this.StoreLocation = serviceCert.Attributes["storeLocation"]?.Value;

            this.Certificate = X509CertificateUtils.FindCertificate(
                serviceCert.Attributes["findType"]?.Value,
                this.StoreLocation,
                this.StoreName,
                serviceCert.Attributes["findValue"]?.Value
            );

        }

        /// <summary>
        /// Represent as XML
        /// </summary>
        public XmlElement ToXml(XmlElement parent)
        {
            var xel = parent.GetOrCreateElement("serviceCertificate") as XmlElement;
            xel.SetAttribute("x509FindType", "FindByThumbprint");
            xel.SetAttribute("findValue", this.Certificate.Thumbprint);
            xel.SetAttribute("storeName", this.StoreName);
            xel.SetAttribute("storeLocation", this.StoreLocation);
            return parent;
        }

        /// <summary>
        /// Gets or sets the store location
        /// </summary>
        public string StoreLocation { get; set; }

        /// <summary>
        /// Gets or sets the store name
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// Gets or sets the actual certificate
        /// </summary>
        public X509Certificate2 Certificate { get; set; }

    }
}