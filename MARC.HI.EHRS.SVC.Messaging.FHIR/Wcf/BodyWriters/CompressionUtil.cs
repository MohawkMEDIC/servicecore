﻿/*
 * Copyright 2016-2016 Mohawk College of Applied Arts and Technology
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you 
 * may not use this file except in compliance with the License. You may 
 * obtain a copy of the License at 
 * 
 * http://www.apache.org/licenses/LICENSE-2.0 
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations under 
 * the License.
 * 
 * User: fyfej
 * Date: 2016-1-24
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Wcf.BodyWriters
{
    /// <summary>
    /// Compression utilities
    /// </summary>
    public static class CompressionUtil
    {

        // Buffer size
        public const int BUFFER_SIZE = 1024;

        /// <summary>
        /// Get compression scheme
        /// </summary>
        public static ICompressionScheme GetCompressionScheme(String schemeName)
        {
            switch(schemeName)
            {
                case "gzip":
                    return new GzipCompressionScheme();
                case "deflate":
                    return new DeflateCompressionScheme();
                default:
                    return null;
            }
        }

        /// <summary>
        /// De-compress message
        /// </summary>
        public static void DeCompressMessage(ref Message message, ICompressionScheme scheme, WebContentFormat format)
        {
            byte[] binaryData = null;
            using (MemoryStream ms = new MemoryStream())
            {
                // Write out the XML
                using (var xdr = XmlDictionaryWriter.CreateTextWriter(ms, Encoding.UTF8, false))
                    message.WriteBodyContents(xdr);

                // Read in the binary data
                ms.Seek(0, SeekOrigin.Begin);
                using (MemoryStream binMs = new MemoryStream())
                {
                    using (XmlReader rdr = XmlReader.Create(ms))
                    {
                        while (rdr.Read())
                        {
                            if (rdr.LocalName == "Binary")
                            {
                                byte[] buffer = new byte[BUFFER_SIZE];
                                int br = BUFFER_SIZE;
                                while (br == BUFFER_SIZE)
                                {
                                    br = rdr.ReadContentAsBase64(buffer, 0, BUFFER_SIZE);
                                    binMs.Write(buffer, 0, br);
                                }
                            }
                        }
                    }
                    binaryData = binMs.ToArray();
                }
            }

            var outMs = scheme.CreateDecompressionStream(new MemoryStream(binaryData));
            Message outMessage = null;
            if (format == WebContentFormat.Json)
            {
                var jdr = JsonReaderWriterFactory.CreateJsonReader(outMs, XmlDictionaryReaderQuotas.Max);
                outMessage = Message.CreateMessage(message.Version, message.Headers.Action, jdr);
            }
            else if (format == WebContentFormat.Xml)
                outMessage = Message.CreateMessage(message.Version, message.Headers.Action, XmlReader.Create(outMs));
            else
            {
                outMessage = Message.CreateMessage(message.Version, message.Headers.Action, new RawBodyWriter(binaryData));
                outMs.Dispose();
            }

            outMessage.Properties.CopyProperties(message.Properties);
            outMessage.Headers.CopyHeadersFrom(message.Headers);
            message = outMessage;
        }

    }
}
