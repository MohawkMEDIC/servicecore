using System;
using System.Xml;

namespace MARC.HI.EHRS.SVC.Configuration.Wcf
{

    /// <summary>
    /// Security type
    /// </summary>
    public enum WcfSecurityType
    {
        None = 0x0,
        Basic = 0x1,
        Https = 0x2,
    }

    /// <summary>
    /// Binding information
    /// </summary>
    public class WcfBindingInfo
    {
        // The name of the binding
        private string m_name;

        /// <summary>
        /// Creates a new binding
        /// </summary>
        public WcfBindingInfo()
        {

        }

        /// <summary>
        /// Creates a new binding
        /// </summary>
        public WcfBindingInfo(XmlElement xmlNode)
        {
            switch(xmlNode.SelectSingleNode("./security/@mode")?.Value)
            {
                case "Transport":
                    this.SecurityType = WcfSecurityType.Https;
                    break;
                default:
                    this.SecurityType = WcfSecurityType.None;
                    break;
            }

            if (xmlNode.SelectSingleNode("./security/transport[@clientCredentialType = 'Basic']") != null)
                this.SecurityType |= WcfSecurityType.Basic;

            this.m_name = xmlNode.Attributes["name"]?.Value;
        }

        /// <summary>
        /// ToXML
        /// </summary>
        public XmlElement ToXml(XmlElement parent)
        {
            var bindingElement = parent.SelectSingleNode($"./binding[@name = '{this.m_name}']") as XmlElement;
            if(bindingElement == null)
            {
                bindingElement = parent.OwnerDocument.CreateElement("binding");
                bindingElement.SetAttribute("name", this.m_name ?? Guid.NewGuid().ToConfigName());
                parent.AppendChild(bindingElement);
            }

            bindingElement.UpdateOrCreateChildElement("security", new { mode = this.SecurityType.HasFlag(WcfSecurityType.Https) ? "Transport" : this.SecurityType.HasFlag(WcfSecurityType.Basic) ? "TransportCredentialOnly" : "None" })
                .UpdateOrCreateChildElement("transport", new { clientCredentialType = this.SecurityType.HasFlag(WcfSecurityType.Basic) ? "Basic" : "None" });
            return bindingElement;
        }

        /// <summary>
        /// Security type
        /// </summary>
        public WcfSecurityType SecurityType { get; set; }

    }
}