using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.IO.Compression;
using System.IO;
using System.Diagnostics;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.WcfCore
{
    /// <summary>
    /// Write the body contents
    /// </summary>
    public class GZipWriter : BodyWriter
    {
        
        private byte[] m_data;

        /// <summary>
        /// GZip writer
        /// </summary>
        public GZipWriter(byte[] data) : base(false)
        {
            this.m_data = data;
        }

        /// <summary>
        /// Write body contents
        /// </summary>
        protected override void OnWriteBodyContents(System.Xml.XmlDictionaryWriter writer)
        {

            writer.WriteStartDocument();
            
            using (MemoryStream ms = new MemoryStream())
            {
                using (Stream gzs = new GZipStream(ms, CompressionMode.Compress))
                {
                    gzs.Write(this.m_data, 0, this.m_data.Length);
                    gzs.Flush();
                }
                writer.WriteStartElement("Binary");
                byte[] arr = ms.ToArray();
                writer.WriteBase64(arr, 0, arr.Length);
                writer.WriteEndElement();
                
            }
            writer.WriteEndDocument();
        }
    }
}
