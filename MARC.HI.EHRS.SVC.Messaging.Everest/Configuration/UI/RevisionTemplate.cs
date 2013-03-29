using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace MARC.HI.EHRS.SVC.Messaging.Everest.Configuration.UI
{
    /// <summary>
    /// Everest revision template
    /// </summary>
    [XmlType("EverestRevisionTemplate", Namespace = "urn:marc-hi:ca/svc")]
    [XmlRoot("everestRevisionTemplate", Namespace = "urn:marc-hi:ca/svc")]
    public class RevisionTemplate
    {

        /// <summary>
        /// The binding for the configuration
        /// </summary>
        [XmlElement("binding")]
        public string WcfBindingType { get; set; }

        /// <summary>
        /// Name of the template
        /// </summary>
        [XmlElement("name")]
        public string Name { get; set; }

        /// <summary>
        /// Default Uri
        /// </summary>
        [XmlElement("defaultUrl")]
        public string DefaultUrl { get; set; }

        /// <summary>
        /// An xpath to verify installation
        /// </summary>
        [XmlElement("installCheck")]
        public string InstallationCheckXPath { get; set; }

        /// <summary>
        /// Revision section configuration
        /// </summary>
        [XmlElement("revisionConfiguration")]
        public XmlElement EverestConfiguration { get; set; }

        /// <summary>
        /// Binding configuration
        /// </summary>
        [XmlElement("bindingConfiguration")]
        public XmlElement BindingConfiguration { get; set; }
    }
}
