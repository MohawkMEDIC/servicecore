/* 
 * Copyright 2008-2011 Mohawk College of Applied Arts and Technology
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
 * User: Justin Fyfe
 * Date: 08-24-2011
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using MARC.HI.EHRS.SVC.Auditing.Atna.Format;
using System.Diagnostics;
using System.Net.Sockets;
using System.IO;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Auditing.Atna
{
    /// <summary>
    /// Represents an ATNA Client
    /// </summary>
    public class SyslogPublisher : IMessagePublisher
    {
        // Represents the remote endpoint that is being connected
        private IPEndPoint m_remoteEndpoint;

        // Represents the syslog facility to use 
        public const int SYSLOG_FACILITY = 13;

        /// <summary>
        /// Creates a new instance of the ATNA client
        /// </summary>
        public SyslogPublisher(IPEndPoint endpoint)
        {
            this.m_remoteEndpoint = endpoint;
        }

        /// <summary>
        /// Create the endpoint
        /// </summary>
        public IPEndPoint EndPoint { get { return m_remoteEndpoint; } }

        /// <summary>
        /// Send a message to the ATNA client
        /// </summary>
        public void SendMessage(AuditMessage am)
        {
            UdpClient udpClient = new UdpClient();
            try
            {
                udpClient.Connect(this.m_remoteEndpoint);
                StringBuilder syslogmessage = new StringBuilder();
                syslogmessage.AppendFormat("<{0}> {1:MMM dd HH:mm:ss} {2} ",
                    SYSLOG_FACILITY, DateTime.Now, Environment.MachineName);
                syslogmessage.Append(CreateMessageBody(am));

                // Send the message
                // Create the dgram
                byte[] dgram = System.Text.Encoding.ASCII.GetBytes(syslogmessage.ToString());
                udpClient.Send(dgram, (int)dgram.Length);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
            }
            finally
            {
                udpClient.Close();
            }
        }

        /// <summary>
        /// Create the message body
        /// </summary>
        private string CreateMessageBody(AuditMessage am)
        {
            StringWriter sw = new StringWriter();
            XmlSerializer xsz = new XmlSerializer(typeof(AuditMessage));
            xsz.Serialize(sw, am);
            return sw.ToString();
        }

    }
}
