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
using MARC.HI.EHRS.SVC.Core.Configuration;
using System.ComponentModel;
using MARC.HI.EHRS.SVC.Core.Data;
using MARC.HI.EHRS.SVC.Configuration;

namespace MARC.HI.EHRS.SVC.Core.Configuration
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
    [Description("MARC-HI Service Core Configuration")]
    public class HostConfigurationSectionHandler : IConfigurationSectionHandler
    {


        #region IConfigurationSectionHandler Members

        /// <summary>
        /// Create the handler
        /// </summary>
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            if (section == null)
                throw new InvalidOperationException("Can't find configuration section");

            HostConfiguration retVal = new HostConfiguration();
            retVal.ServiceProviders = new List<Type>();
            retVal.ServiceAssemblies = new List<Assembly>();

            XmlNode serviceAssemblySection = section.SelectSingleNode("./*[local-name() = 'serviceAssemblies']"),
                serviceProviderSection = section.SelectSingleNode("./*[local-name() = 'serviceProviders']"),
                systemSection = section.SelectSingleNode("./*[local-name() = 'system']"),
                jurisdictionSection = section.SelectSingleNode("./*[local-name() = 'jurisdiction']"),
                custodianSection = section.SelectSingleNode("./*[local-name() = 'custodianship']");


            if (serviceAssemblySection != null && !(configContext is IConfigurableFeature)) // Load assembly data
                foreach (XmlNode nd in serviceAssemblySection.SelectNodes("./*[local-name() = 'add']/@assembly"))
                {
                    string asmFile = Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), nd.Value);
                    if (File.Exists(asmFile))
                    {
                        retVal.ServiceAssemblies.Add(Assembly.LoadFile(asmFile));
                        Trace.TraceInformation("Loaded {0}", asmFile);
                    }
                    else if (File.Exists(nd.Value))
                    {
                        retVal.ServiceAssemblies.Add(Assembly.LoadFile(nd.Value));
                        Trace.TraceInformation("Loaded {0}", nd.Value);
                    }
                    else
                        Trace.TraceError("HostConfiguration: Can't load {0}", asmFile);
                }


            if (systemSection != null) // load system information
            {
                XmlNode deviceElement = systemSection.SelectSingleNode("./*[local-name() = 'device']");
                if (deviceElement != null)
                {
                    retVal.DeviceIdentifier = deviceElement.Attributes["id"].Value;
                    retVal.DeviceName = deviceElement.Attributes["name"].Value;
                }
            }

            if(jurisdictionSection != null) // jurisdiction data
            {
                retVal.JurisdictionData = new Jurisdiction();
                XmlNode idElement = jurisdictionSection.SelectSingleNode("./*[local-name() = 'id']");


                if(idElement != null)
                    retVal.JurisdictionData.Id = new Identifier<String>()
                     { Id = idElement.Attributes["value"].Value };

                XmlNode data = jurisdictionSection.SelectSingleNode("./*[local-name() = 'name']");
                retVal.JurisdictionData.Name = data != null ? data.InnerText : null;
                data = jurisdictionSection.SelectSingleNode("./*[local-name() = 'defaultLanguageCode']/@code");
                retVal.JurisdictionData.DefaultLanguageCode = data != null ? data.Value : null;  
            }

            if(custodianSection != null)
            {
                retVal.Custodianship = new CustodianshipData();
                XmlNode idElement = custodianSection.SelectSingleNode("./*[local-name() = 'id']");
                if (idElement != null)
                    retVal.Custodianship.Id = new Identifier<String>() { Id = idElement.Attributes["value"].Value };
                retVal.Custodianship.Name = custodianSection.SelectSingleNode("./*[local-name() = 'name']").InnerText;

                retVal.Custodianship.Contact = new CustodianshipContact()
                {
                    Email = custodianSection.SelectSingleNode("./*[local-name() = 'contact']/@email")?.Value,
                    Name = custodianSection.SelectSingleNode("./*[local-name() = 'contact']/@name")?.Value,
                    Organization = custodianSection.SelectSingleNode("./*[local-name() = 'contact']/@organization")?.Value,
                };
            }

            if (serviceProviderSection != null && !(configContext is IConfigurableFeature)) // Load providers data
                foreach (XmlNode nd in serviceProviderSection.SelectNodes("./*[local-name() = 'add']/@type"))
                {
                    Type t = Type.GetType(nd.Value);
                    if (t != null)
                    {
                        ConstructorInfo ci = t.GetConstructor(Type.EmptyTypes);
                        if (ci != null)
                        {
                            retVal.ServiceProviders.Add(t);
                            Trace.TraceInformation("Added provider {0}", t.FullName);
                        }
                        else
                            Trace.TraceWarning("Can't find parameterless constructor on type {0}", t.FullName);
                    }
                    else
                        Trace.TraceWarning("Can't find type described by '{0}'", nd.Value);
                }

            
          
            return retVal;
        }

        #endregion



    }
}
