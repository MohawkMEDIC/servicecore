using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Resources.Attributes;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{
    /// <summary>
    /// Base for all resources
    /// </summary>
    [XmlType("ResourceBase", Namespace = "http://hl7.org/fhir")]
    public abstract class ResourceBase : Shareable
    {

        /// <summary>
        /// Resource tags
        /// </summary>
        public ResourceBase()
        {
            this.Attributes = new List<ResourceAttributeBase>();
            this.Contained = new List<ContainedResource>();
        }

        // The narrative
        private Narrative m_narrative;

        /// <summary>
        /// A list of contained resources
        /// </summary>
        [XmlElement("contained")]
        public List<ContainedResource> Contained { get; set; }

        /// <summary>
        /// Extended observations about the resource that can be used to tag the resource
        /// </summary>
        [XmlIgnore]
        public List<ResourceAttributeBase> Attributes { get; set; }

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
        /// Gets or sets the narrative text
        /// </summary>
        [XmlElement("text")]
        public Narrative Text
        {
            get
            {
                if (this.m_narrative == null && !this.SuppressText)
                    this.m_narrative = this.GenerateNarrative();
                return this.m_narrative;
            }
            set
            {
                this.m_narrative = value;
            }
        }


        /// <summary>
        /// Suppress generation of text
        /// </summary>
        [XmlIgnore]
        public bool SuppressText { get; set; }

        /// <summary>
        /// Generate a narrative
        /// </summary>
        protected Narrative GenerateNarrative()
        {
            // Create a new narrative
            Narrative retVal = new Narrative();

            XmlDocument narrativeContext = new XmlDocument();
            retVal.Status = new PrimitiveCode<string>("generated");
            StringWriter writer = new StringWriter();

            using (XmlWriter xw = XmlWriter.Create(writer, new XmlWriterSettings() { ConformanceLevel = ConformanceLevel.Fragment }))
            {
                xw.WriteStartElement("body", NS_XHTML);
                this.WriteText(xw);

                xw.WriteEndElement();
            }

            narrativeContext.LoadXml(writer.ToString());

            retVal.Div = new XmlElement[narrativeContext.DocumentElement.ChildNodes.Count];
            for (int i = 0; i < retVal.Div.Elements.Length; i++)
                retVal.Div.Elements[i] = narrativeContext.DocumentElement.ChildNodes[i] as XmlElement;
            return retVal;
        }

        /// <summary>
        /// Write text fragement
        /// </summary>
        internal override void WriteText(XmlWriter w)
        {
            w.WriteStartElement("p", NS_XHTML);
            w.WriteString(this.GetType().Name + " - No text defined for resource");
            w.WriteEndElement();
        }

        /// <summary>
        /// Last updated timestamp
        /// </summary>
        [XmlIgnore]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Add a contained resource
        /// </summary>
        public void AddContainedResource(ResourceBase resource)
        {
            resource.MakeIdRef();
            this.Contained.Add(new ContainedResource() { Item = resource });
        }
    }
}
