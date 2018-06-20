using System;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Xml;

namespace MARC.HI.EHRS.SVC.Configuration.Wcf
{

    /// <summary>
    /// Gets or sets the binding type
    /// </summary>
    public enum WcfBindingType
    {
        webHttpBinding,
        basicHttpBinding,
        wsHttpBinding
    }

    /// <summary>
    /// Represents endpoint information
    /// </summary>
    public class WcfEndpointInfo
    {

        // The name
        private string m_name;

        /// <summary>
        /// Endpoint xml info
        /// </summary>
        public WcfEndpointInfo(XmlElement epXml)
        {
            this.Endpoint = epXml.Attributes["address"]?.Value;
            this.BindingType = (WcfBindingType)Enum.Parse(typeof(WcfBindingType), epXml.Attributes["binding"]?.Value);
            this.Binding = new WcfBindingInfo(epXml.OwnerDocument.SelectSingleNode($"/configuration/system.serviceModel/bindings/{this.BindingType}/binding[@name='{epXml.Attributes["bindingConfiguration"]?.Value}']") as XmlElement);
            this.Contract = Type.GetType(epXml.Attributes["contract"]?.Value) ??
                AppDomain.CurrentDomain.GetAssemblies().SelectMany(o => o.ExportedTypes).FirstOrDefault(o => o.GetCustomAttribute<ServiceContractAttribute>()?.ConfigurationName == epXml.Attributes["contract"]?.Value);
            this.m_name = epXml.Attributes["name"]?.Value;
        }

        /// <summary>
        /// To XML
        /// </summary>
        public XmlElement ToXml(XmlElement parent)
        {

            var epElement = parent.SelectSingleNode($"./endpoint[@address='{this.Endpoint}']") as XmlElement;
            if(epElement == null)
            {
                epElement = parent.OwnerDocument.CreateElement("endpoint");
                epElement.SetAttribute("endpoint", this.Endpoint);
                parent.AppendChild(epElement);
            }

            // Add other settings
            epElement.SetAttribute("binding", this.BindingType.ToString());
            epElement.SetAttribute("name", this.m_name ?? Guid.NewGuid().ToConfigName());

            // Contract
            epElement.SetAttribute("contract", this.Contract.GetCustomAttribute<ServiceContractAttribute>()?.ConfigurationName ?? this.Contract.AssemblyQualifiedName);

            // Binding configuration
            var bindings = parent.OwnerDocument.SelectSingleNode($"/configuration/system.serviceModel/bindings/{this.BindingType}") as XmlElement;
            var binding = this.Binding.ToXml(bindings);
            epElement.SetAttribute("bindingConfiguration", binding.Attributes["name"]?.Value);

            return epElement;
        }

        /// <summary>
        /// Gets or sets the endpoint
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// Gets or sets the binding type
        /// </summary>
        public WcfBindingType BindingType { get; set; }

        /// <summary>
        /// Gets or sets the contract
        /// </summary>
        public Type Contract { get; set; }

        /// <summary>
        /// Gets or sets the binding
        /// </summary>
        public WcfBindingInfo Binding { get; set; }

    }
}