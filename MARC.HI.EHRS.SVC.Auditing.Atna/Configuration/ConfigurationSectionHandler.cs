/*
 * Copyright 2010-2018 Mohawk College of Applied Arts and Technology
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
 * Date: 1-9-2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Net;
using System.Reflection;
using AtnaApi.Transport;
using System.Security.Cryptography.X509Certificates;

namespace MARC.HI.EHRS.SVC.Auditing.Atna.Configuration
{
    /// <summary>
    /// Identifies the configuration for the ATNA auditing
    /// </summary>
    public class ConfigurationSectionHandler : IConfigurationSectionHandler
    {
        #region IConfigurationSectionHandler Members

        /// <summary>
        /// Configuration Section Handler
        /// </summary>
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            var retVal = new AuditConfiguration();
            XmlElement auditTargetNode = section.SelectSingleNode("./*[local-name() = 'destination']") as XmlElement;

            if (auditTargetNode == null)
                throw new ConfigurationErrorsException("Missing the 'destination' element in the ATNA configuration", section);

            if (section.Attributes["messagePublisher"] == null)
                throw new ConfigurationErrorsException("You must specify a message publisher interface to use for sending audit messages", section);
            

            // Create the message handler
            Type auditType = Type.GetType(section.Attributes["messagePublisher"].Value);
            if (auditType == null)
                throw new ConfigurationErrorsException(String.Format("Can't find the type described by '{0}'", section.Attributes["messagePublisher"].Value), section);
            ConstructorInfo ci = auditType.GetConstructor(new Type[] { typeof(IPEndPoint) });
            if (ci == null)
                throw new ConfigurationErrorsException(String.Format("Type '{0}' does not have a constructor expecting a parameter of type System.Net.IPEndPoint", auditType.FullName));

            if (auditTargetNode.Attributes["endpoint"] == null)
                throw new ConfigurationErrorsException("The URI element in the auditTarget element is missing", section);

            // Parse IP Address
            string ipAddress = auditTargetNode.Attributes["endpoint"].Value;
            int port = 512;
            if(ipAddress.Contains(':'))
            {
                port = Int32.Parse(ipAddress.Substring(ipAddress.IndexOf(":") + 1));
                ipAddress = ipAddress.Substring(0, ipAddress.IndexOf(":"));
            }

            // Create the audit target
            retVal.AuditTarget = new IPEndPoint(IPAddress.Parse(ipAddress), port);

            // Create the publisher
            retVal.MessagePublisher = ci.Invoke(new object[] { retVal.AuditTarget }) as AtnaApi.Transport.ITransporter;

            // Secure?
            if (retVal.MessagePublisher is AtnaApi.Transport.STcpSyslogTransport)
            {
                var secureTransport = retVal.MessagePublisher as STcpSyslogTransport;
                if (auditTargetNode.Attributes["certificateThumbprint"] == null)
                    throw new ConfigurationErrorsException("When secure TCP is used a certificate must be specified");
                secureTransport.ClientCertificate = ConfigurationSectionHandler.FindCertificate(StoreName.My, StoreLocation.LocalMachine, X509FindType.FindByThumbprint, auditTargetNode.Attributes["certificateThumbprint"].Value);
                if (auditTargetNode.Attributes["serverCertificateThumbprint"] != null)
                    secureTransport.ServerCertificate = ConfigurationSectionHandler.FindCertificate(StoreName.My, StoreLocation.LocalMachine, X509FindType.FindByThumbprint, auditTargetNode.Attributes["serverCertificateThumbprint"].Value);

            }

            if (section.Attributes["format"] != null && section.Attributes["format"].Value == "DICOM")
                retVal.MessagePublisher.MessageFormat = MessageFormatType.DICOM;
            else
                retVal.MessagePublisher.MessageFormat = MessageFormatType.RFC3881;

            return retVal;
        }

        #endregion

        /// <summary>
        /// Find a certificate
        /// </summary>
        public static X509Certificate2 FindCertificate(StoreName storeName, StoreLocation storeLocation, X509FindType findType, string findValue)
        {
            X509Store store = new X509Store(storeName, storeLocation);
            try
            {
                store.Open(OpenFlags.ReadOnly);
                var certs = store.Certificates.Find(findType, findValue, false);
                if (certs.Count > 0)
                    return certs[0];
                else
                    throw new InvalidOperationException("Cannot locate certificate");

            }
            finally
            {
                store.Close();
            }

        }

    }
}
