using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MARC.HI.EHRS.SVC.Configuration.Wcf
{
    /// <summary>
    /// Represent WCF Service information
    /// </summary>
    public class WcfServiceInfo
    {
        /// <summary>
        /// Create service information from xml element
        /// </summary>
        public WcfServiceInfo(XmlElement xmlDescription)
        {
            this.Name = xmlDescription.Attributes["name"]?.Value;

            if (xmlDescription.Attributes["behaviorConfiguration"] != null) {
                var behaviorName = xmlDescription.Attributes["behaviorConfiguration"]?.Value;
                var behaviorXml = xmlDescription.OwnerDocument.SelectSingleNode($"/system.serviceModel/behaviors/serviceBehaviors/behavior[@name='{behaviorName}']");
                if (behaviorXml != null)
                    this.Behavior = new WcfBehaviorInfo(behaviorXml);
            }

            this.Endpoints = new List<WcfEndpointInfo>();
            foreach (var epXml in xmlDescription.SelectNodes("./endpoint"))
                this.Endpoints.Add(new WcfEndpointInfo(epXml as XmlElement));
        }

        /// <summary>
        /// Represent as XML
        /// </summary>
        public XmlElement ToXml(XmlElement parent)
        {
            var serviceElement = parent.SelectSingleNode($"./service[@name = '{this.Name}']") as XmlElement;
            if(serviceElement == null)
            {
                serviceElement = parent.OwnerDocument.CreateElement("service");
                serviceElement.SetAttribute("name", this.Name ?? Guid.NewGuid().ToConfigName());
                parent.AppendChild(serviceElement);
            }

            // Behavior
            if(this.Behavior != null)
            {
                var behaviors = parent.OwnerDocument.GetOrCreateElement("/configuration/system.serviceModel/behaviors/serviceBehaviors");
                var behave = this.Behavior.ToXml(behaviors);
                serviceElement.SetAttribute("behaviorConfiguration", behave.Attributes["name"]?.Value);
            };

            // endpoints
            var hostElement = serviceElement.GetOrCreateElement("host/baseAddresses");
            hostElement.RemoveAll();
            foreach(var ep in this.Endpoints)
            {
                ep.ToXml(serviceElement);
                var he = parent.OwnerDocument.CreateElement("add");
                he.SetAttribute("baseAddress", ep.Endpoint);
                hostElement.AppendChild(he);
            }

            return serviceElement;
        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets endpoint information
        /// </summary>
        public List<WcfEndpointInfo> Endpoints { get; set; }

        /// <summary>
        /// Gets or sets the binding info
        /// </summary>
        public WcfBehaviorInfo Behavior { get; set; }
    }
}
