using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.WcfCore
{
    /// <summary>
    /// Object serializer
    /// </summary>
    public class FhirXmlObjectSerializer : XmlObjectSerializer
    {

        private XmlSerializerNamespaces m_namespaces = new XmlSerializerNamespaces();

        /// <summary>
        /// Default ctor
        /// </summary>
        public FhirXmlObjectSerializer()
        {
            this.m_namespaces.Add("", "http://hl7.org/fhir");
        }

        /// <summary>
        /// Is start object
        /// </summary>
        public override bool IsStartObject(System.Xml.XmlDictionaryReader reader)
        {
            return false;
        }

        /// <summary>
        /// Read an object
        /// </summary>
        public override object ReadObject(System.Xml.XmlDictionaryReader reader, bool verifyObjectName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Write end
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteEndObject(System.Xml.XmlDictionaryWriter writer)
        {
        }

        /// <summary>
        /// Write content
        /// </summary>
        public override void WriteObjectContent(System.Xml.XmlDictionaryWriter writer, object graph)
        {
        }

        /// <summary>
        /// Write the start of an object
        /// </summary>
        public override void WriteStartObject(System.Xml.XmlDictionaryWriter writer, object graph)
        {
            XmlSerializer xsz = new XmlSerializer(graph.GetType());
            xsz.Serialize(writer, graph, this.m_namespaces);
        }
    }
}
