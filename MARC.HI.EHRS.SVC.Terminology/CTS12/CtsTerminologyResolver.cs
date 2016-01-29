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
 * Date: 7-5-2012
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.Services;
using MARC.HI.EHRS.SVC.Terminology.Configuration;
using System.Configuration;
using System.Diagnostics;
using MARC.HI.EHRS.SVC.Core.Terminology;
using System.Text.RegularExpressions;
using System.Net;
using System.ComponentModel;
using MARC.HI.EHRS.SVC.Terminology.CTSService;
using MARC.HI.EHRS.SVC.Core.Data;
using MARC.HI.EHRS.SVC.Core;

namespace MARC.HI.EHRS.SVC.Terminology.CTS12
{
    /// <summary>
    /// Implements a terminology service that resolves codes against a HL7 Common Terminology
    /// Services server
    /// </summary>
    [Description("HL7 CTS Terminology Service")]
    public class CtsTerminologyResolver : ITerminologyService
    {

        /// <summary>
        /// Configuration
        /// </summary>
        private static ConfigurationSectionHandler m_configuration;

        /// <summary>
        /// Cached code lookups
        /// </summary>
        internal static Dictionary<String, CodeValue> m_cachedCodeLookups = new Dictionary<string, CodeValue>(10);

        /// <summary>
        /// Synchronization object
        /// </summary>
        private static object m_syncObject = new object();

        /// <summary>
        /// CTS Terminology Resolver
        /// </summary>
        static CtsTerminologyResolver()
        {
            m_configuration = ApplicationContext.Current.GetService<IConfigurationManager>().GetSection("marc.hi.ehrs.svc.terminology") as ConfigurationSectionHandler;
        }

        #region ITerminologyService Members

        /// <summary>
        /// Validate a Code
        /// </summary>
        public ConceptValidationResult Validate(CodeValue code)
        {
            if (code == null)
                return new ConceptValidationResult() { Outcome = ValidationOutcome.Valid };

            string codeKey = String.Format("{0}@{1}", code.CodeSystem, code.Code);

            CodeValue tCodeCache = null;
            ConceptValidationResult retVal = new ConceptValidationResult();


            // Check cache first
            if (!m_cachedCodeLookups.TryGetValue(codeKey, out tCodeCache))
            {
                MessageRuntime mrt = new MessageRuntime();

                // Host URI
                mrt.Url = m_configuration.MessageRuntimeUrl;

                if (!String.IsNullOrEmpty(m_configuration.ProxyAddress))
                    mrt.Proxy = new WebProxy(m_configuration.ProxyAddress);

                // CTS Doesn't need to be used to validate ISO639 Language Codes, so we can regex match this
                if (code.CodeSystem == "2.16.840.1.113883.6.99")
                {
                    if(new Regex("\\w{2}\\-?\\w{0,2}").IsMatch(code.Code))
                        return new ConceptValidationResult() { Outcome = ValidationOutcome.Valid };
                    else
                       return new ConceptValidationResult() { Outcome = ValidationOutcome.Invalid };
                }

                // Attempt to validate the code
                try
                {
                    CD codeToValidate = CreateCTSConceptDescriptor(code);
                    var response = mrt.validateCode(null, CreateCTSConceptDescriptor(code), null, new BL() { v = true }, new BL() { v = false });

                    // Translate response
                    if (response.nErrors.v == 0 && response.nWarnings.v == 0)
                        retVal.Outcome = ValidationOutcome.Valid;
                    else if (response.nErrors.v != 0)
                        retVal.Outcome = ValidationOutcome.Error;
                    else if (response.nWarnings.v != 0)
                        retVal.Outcome = ValidationOutcome.ValidWithWarning;

                    // Details
                    foreach (var dtl in response.detail)
                        retVal.AddDetail(dtl.isError.v, String.Format("{0}:{1} ({2}@{3})", dtl.error_id.v, dtl.errorText.v, code.CodeSystem, code.Code), dtl.error_id.v);

                    return retVal;
                }
                catch (Exception e)
                {
                    Trace.TraceError(e.ToString());
                    retVal.Outcome = ValidationOutcome.Error;
                    retVal.AddDetail(true, e.Message, "INTERR");
                }
            }
            else
            {
                if (!tCodeCache.DisplayName.Equals(code.DisplayName))
                    retVal.AddDetail(false, String.Format("{0}:{1} ({2}@{3})", "0001", "Incorrect display name for code", code.CodeSystem, code.Code), "W001");
            }
            return retVal;
        }

        /// <summary>
        /// Create a CTS concept descriptor from the internal code structure
        /// </summary>
        private CD CreateCTSConceptDescriptor(CodeValue code)
        {
            CD retVal = new CD()
            {
                code = code.Code,
                codeSystem = code.CodeSystem,
                displayName = code.DisplayName
            };

            return retVal;
        }

        /// <summary>
        /// Create CTS coded value
        /// </summary>
        private CV CreateCTSCodedValue(CodeValue codeValue)
        {
            return new CV()
            {
                code = codeValue.Code,
                codeSystem = codeValue.CodeSystem,
                displayName = codeValue.DisplayName
            };
        }

        /// <summary>
        /// Validate a code using a named code system
        /// </summary>
        public ConceptValidationResult ValidateEx(string code, string displayName, String codeSystem)
        {
            return Validate(new CodeValue(code, codeSystem) { DisplayName = displayName });
        }

        /// <summary>
        /// Get the domain OID from a code system name
        /// </summary>
        public string GetCodeSystemDomain(String codeSystemName)
        {
            var oidService = ApplicationContext.Current.GetService<IOidRegistrarService>();
            if (oidService == null)
                throw new InvalidOperationException("OID Registrar service not registered");

            return oidService.GetOid(codeSystemName.ToString())?.Oid;
        }

        /// <summary>
        /// Translate a code from one code system to another
        /// </summary>
        public CodeValue Translate(CodeValue code, string targetDomain)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IUsesHostContext Members
        
        /// <summary>
        /// Gets or sets the host context in which this service runs
        /// </summary>
        public IServiceProvider Context { get; set; }

        #endregion

        #region ITerminologyService Members

        /// <summary>
        /// Fill in code details
        /// </summary>
        public CodeValue FillInDetails(CodeValue codeValue)
        {
            
            CodeValue retVal = null;
            string codeKey = String.Format("{0}@{1}", codeValue.CodeSystem, codeValue.Code);

            if (!m_cachedCodeLookups.TryGetValue(codeKey, out retVal))
            {
                // Does this code need details filled in?
                // CTS should only be used for valid
                if (!m_configuration.FillInCodeSets.Contains(codeValue.CodeSystem))
                    return codeValue;
                // Fill in code details

                MessageRuntime mrt = new MessageRuntime();
                // Host URI
                mrt.Url = m_configuration.MessageRuntimeUrl;

                if (!String.IsNullOrEmpty(m_configuration.ProxyAddress))
                    mrt.Proxy = new WebProxy(m_configuration.ProxyAddress);

                CD ret = mrt.fillInDetails(CreateCTSConceptDescriptor(codeValue), new ST() { v = ApplicationContext.Current.Configuration.JurisdictionData.DefaultLanguageCode });

                // Create the CodeValue from the CD
                retVal = CreateCodeValue(ret);


                AddCachedCode(codeKey, retVal);

            }
            return retVal;
        }

        /// <summary>
        /// Add a cached code
        /// </summary>
        internal static void AddCachedCode(string codeKey, CodeValue code)
        {
            // Don't cache
            if (m_configuration.MaxMemoryCacheSize == 0)
                return;

            // Clear the cache if it is too large
            if (m_cachedCodeLookups.Count > m_configuration.MaxMemoryCacheSize)
            {
                Trace.TraceInformation("Memory cache limit reached, cleaning....");
                m_cachedCodeLookups.Clear();
            }

            lock (m_syncObject)
                m_cachedCodeLookups.Add(codeKey, code);
        }

        /// <summary>
        /// Create a coded value from the specified concept descriptor
        /// </summary>
        private CodeValue CreateCodeValue(CD code)
        {
            CodeValue retVal = new CodeValue(code.code, code.codeSystem)
            {
                CodeSystemVersion = code.codeSystemVersion,
                CodeSystemName = code.codeSystemName,
                DisplayName = code.displayName
            };

            return retVal;
        }

        #endregion
    }
}
