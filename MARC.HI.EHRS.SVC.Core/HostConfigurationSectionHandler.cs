/**
 * Copyright 2012-2013 Mohawk College of Applied Arts and Technology
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
 * Date: 17-8-2012
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using MARC.HI.EHRS.SVC.Core.Services;
using MARC.HI.EHRS.SVC.Core.DataTypes;
using MARC.HI.EHRS.SVC.Core.Configuration;

namespace MARC.HI.EHRS.SVC.Core
{
    /// <summary>
    /// This configuration section allows developers to add services to the host instance.
    /// <para>There are several section parameters that can be added:</para>
    /// <example lang="xml">
    /// App.Config Entry
    /// <code lang="xml">
    ///    &lt;configSections> 
    ///     &lt;section type="MARC.HI.EHRS.SVC.Core.HostConfigurationSection, MARC.HI.EHRS.SVC.Core, Version=1.0.0.0" name="MARC.HI.EHRS.SVC.Core"/> 
    ///    &lt;/configSections> 
    ///    &lt;MARC.HI.EHRS.SVC.Core>
    ///     &lt;serviceAssemblies>
    ///         &lt;add assembly="MARC.HI.EHRS.Auditing.ATNA, Version=1.0.0.0"/>
    ///     &lt;/serviceAssemblies>
    ///    &lt;/MARC.HI.EHRS.SVC.Core>
    /// </code>
    /// </example>
    /// </summary>
    public class HostConfigurationSectionHandler : IConfigurationSectionHandler, ISystemConfigurationService
    {

        /// <summary>
        /// Get the modules that are to be loaded 
        /// </summary>
        public List<Assembly> ServiceAssemblies { get; private set; }

        /// <summary>
        /// Get the service providers for this application
        /// </summary>
        public List<object> ServiceProviders { get; private set; }

        /// <summary>
        /// Gets the name of the section the configuration data was loaded
        /// </summary>
        public string SectionName { get; private set; }

        /// <summary>
        /// Gets the identifier (OID) of the current device
        /// </summary>
        public string DeviceIdentifier { get; set; }

        /// <summary>
        /// Gets the logical name of the device
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// Gets the Oid Registrar that can be used to lookup oids
        /// </summary>
        public OidRegistrar OidRegistrar { get; private set; }

        /// <summary>
        /// Valid senders
        /// </summary>
        private List<DomainIdentifier> m_validSenders = new List<DomainIdentifier>();

        /// <summary>
        /// Validate senders
        /// </summary>
        private bool m_validateSenders = true;

        /// <summary>
        /// Determine if the sender is a valid sender
        /// </summary>
        public bool IsRegisteredDevice(DomainIdentifier device)
        {
            return !this.m_validateSenders || this.m_validSenders.Exists(o => o.Domain == device.Domain && o.Identifier == device.Identifier);
        }

        #region IConfigurationSectionHandler Members

        /// <summary>
        /// Create the handler
        /// </summary>
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            if (section == null)
                throw new InvalidOperationException("Can't find configuration section");
            this.SectionName = section.LocalName;
            this.ServiceProviders = new List<object>() { this };
            this.OidRegistrar = new OidRegistrar();
            this.ServiceAssemblies = new List<Assembly>();

            XmlNode serviceAssemblySection = section.SelectSingleNode("./*[local-name() = 'serviceAssemblies']"),
                serviceProviderSection = section.SelectSingleNode("./*[local-name() = 'serviceProviders']"),
                systemSection = section.SelectSingleNode("./*[local-name() = 'system']"),
                jurisdictionSection = section.SelectSingleNode("./*[local-name() = 'jurisdiction']"),
                custodianSection = section.SelectSingleNode("./*[local-name() = 'custodianship']"),
                oidSection = section.SelectSingleNode("./*[local-name() = 'registeredOids']"),
                sendersSection = section.SelectSingleNode("./*[local-name() = 'registeredDevices']");

            // Senders section
            if (sendersSection != null && !(configContext is IConfigurationPanel)) // senders
            {
                // Validate?
                if(sendersSection.Attributes["validateSolicitors"] != null)
                    this.m_validateSenders = Convert.ToBoolean(sendersSection.Attributes["validateSolicitors"].Value);

                // Add registered senders
                foreach(XmlElement nd in sendersSection.SelectNodes("./*[local-name() = 'add']"))
                {
                    this.m_validSenders.Add(new DomainIdentifier() {
                        Domain = nd.Attributes["domain"] == null ? null : nd.Attributes["domain"].Value,
                        Identifier = nd.Attributes["value"] == null ? null : nd.Attributes["value"].Value
                    });
                }
            }

            if (serviceAssemblySection != null && !(configContext is IConfigurationPanel)) // Load assembly data
                foreach (XmlNode nd in serviceAssemblySection.SelectNodes("./*[local-name() = 'add']/@assembly"))
                {
                    string asmFile = Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), nd.Value);
                    if (File.Exists(asmFile))
                    {
                        ServiceAssemblies.Add(Assembly.LoadFile(asmFile));
                        Trace.TraceInformation("Loaded {0}", asmFile);
                    }
                    else if (File.Exists(nd.Value))
                    {
                        ServiceAssemblies.Add(Assembly.LoadFile(nd.Value));
                        Trace.TraceInformation("Loaded {0}", nd.Value);
                    }
                    else
                        Trace.TraceError("HostConfiguration: Can't load {0}", asmFile);
                }
            if (oidSection != null) // load oids
            {
                foreach (XmlNode xn in oidSection.SelectNodes("./*[local-name() = 'add']"))
                {
                    if (xn.Attributes["name"] != null && xn.Attributes["oid"] != null && xn.Attributes["desc"] != null)
                    {
                        var data = OidRegistrar.Register(xn.Attributes["name"].Value, xn.Attributes["oid"].Value, xn.Attributes["desc"].Value, xn.Attributes["ref"] == null ? null : xn.Attributes["ref"].Value);
                        if(xn.ChildNodes != null)
                            foreach (XmlElement child in xn.ChildNodes)
                            {
                                if (child.Name == "attribute" && child.Attributes["name"] != null)
                                    data.Attributes.Add(new KeyValuePair<string,string>(child.Attributes["name"].Value, child.Attributes["value"] != null ? child.Attributes["value"].Value : null));
                            }
                    }
                }
            }
            if (systemSection != null) // load system information
            {
                XmlNode deviceElement = systemSection.SelectSingleNode("./*[local-name() = 'device']");
                if (deviceElement != null)
                {
                    this.DeviceIdentifier = deviceElement.Attributes["id"].Value;
                    this.DeviceName = deviceElement.Attributes["name"].Value;
                }
            }
            if(jurisdictionSection != null) // jurisdiction data
            {
                this.JurisdictionData = new MARC.HI.EHRS.SVC.Core.DataTypes.Jurisdiction();
                XmlNode idElement = jurisdictionSection.SelectSingleNode("./*[local-name() = 'id']"),
                    defaultOnrampDeviceElement = jurisdictionSection.SelectSingleNode("./*[local-name() = 'defaultOnrampDeviceId']");


                if(idElement != null)
                    this.JurisdictionData.Id = new MARC.HI.EHRS.SVC.Core.DataTypes.DomainIdentifier()
                     { Domain = idElement.Attributes["domain"].Value, Identifier = idElement.Attributes["value"].Value };
                if (defaultOnrampDeviceElement != null)
                    this.JurisdictionData.DefaultOnrampDeviceId = new DomainIdentifier()
                    {
                        Domain = defaultOnrampDeviceElement.Attributes["domain"].Value,
                        Identifier = defaultOnrampDeviceElement.Attributes["value"].Value
                    };
                XmlNode data = jurisdictionSection.SelectSingleNode("./*[local-name() = 'name']");
                this.JurisdictionData.Name = data != null ? data.InnerText : null;
                data = jurisdictionSection.SelectSingleNode("./*[local-name() = 'clientExport']/@domain");
                this.JurisdictionData.ClientDomain = data != null ? data.Value : null;
                data = jurisdictionSection.SelectSingleNode("./*[local-name() = 'providerExport']/@domain");
                this.JurisdictionData.ProviderDomain = data != null ? data.Value : null;
                data = jurisdictionSection.SelectSingleNode("./*[local-name() = 'sdlExport']/@domain");
                this.JurisdictionData.PlaceDomain = data != null ? data.Value : null;
                data = jurisdictionSection.SelectSingleNode("./*[local-name() = 'defaultLanguageCode']/@code");
                this.JurisdictionData.DefaultLanguageCode = data != null ? data.Value : null;  
            }
            if(custodianSection != null)
            {
                this.Custodianship = new MARC.HI.EHRS.SVC.Core.DataTypes.CustodianshipData();
                XmlNode idElement = custodianSection.SelectSingleNode("./*[local-name() = 'id']");
                if (idElement != null)
                    this.Custodianship.Id = new MARC.HI.EHRS.SVC.Core.DataTypes.DomainIdentifier() { Domain = idElement.Attributes["domain"].Value, Identifier = idElement.Attributes["value"].Value };
                this.Custodianship.Name = custodianSection.SelectSingleNode("./*[local-name() = 'name']").InnerText;
            }
            if (serviceProviderSection != null && !(configContext is IConfigurationPanel)) // Load providers data
                foreach (XmlNode nd in serviceProviderSection.SelectNodes("./*[local-name() = 'add']/@type"))
                {
                    Type t = Type.GetType(nd.Value);
                    if (t != null)
                    {
                        ConstructorInfo ci = t.GetConstructor(Type.EmptyTypes);
                        if (ci != null)
                        {
                            this.ServiceProviders.Add(ci.Invoke(null));
                            Trace.TraceInformation("Added provider {0}", t.FullName);
                        }
                        else
                            Trace.TraceWarning("Can't find parameterless constructor on type {0}", t.FullName);
                    }
                    else
                        Trace.TraceWarning("Can't find type described by '{0}'", nd.Value);
                }

            return this;
        }

        #endregion



        #region ISystemConfigurationService Members


        /// <summary>
        /// Gets the jurisdiction configuration data
        /// </summary>
        public MARC.HI.EHRS.SVC.Core.DataTypes.Jurisdiction JurisdictionData
        {
            get; private set;
        }

        /// <summary>
        /// Gets the custodial data
        /// </summary>
        public MARC.HI.EHRS.SVC.Core.DataTypes.CustodianshipData Custodianship
        {
            get; private set;
        }

        #endregion
    }
}
