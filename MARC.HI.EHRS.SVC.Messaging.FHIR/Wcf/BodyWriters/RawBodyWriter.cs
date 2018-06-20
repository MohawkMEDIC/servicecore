using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Wcf.BodyWriters
{
    /// <summary>
    /// Text body writer
    /// </summary>
    public class RawBodyWriter : BodyWriter
    {
        private byte[] m_bytes;
        private Stream m_stream;

        public RawBodyWriter(byte[] message)
            : base(true)
        {
            this.m_bytes = message;
        }

        /// <summary>
        /// Creates a new raw body writer with specified stream
        /// </summary>
        public RawBodyWriter(Stream s) : base(true)
        {
            this.m_stream = s;
        }

        /// <summary>
        /// Write body contents
        /// </summary>
        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            writer.WriteStartElement("Binary");
            if (this.m_stream == null)
                writer.WriteBase64(this.m_bytes, 0, this.m_bytes.Length);
            else
            {
                byte[] buffer = new byte[this.m_stream.Length];
                this.m_stream.Read(buffer, 0, (int)this.m_stream.Length);
                writer.WriteBase64(buffer, 0, (int)this.m_stream.Length);
            }
            writer.WriteEndElement();
        }
    }
}
