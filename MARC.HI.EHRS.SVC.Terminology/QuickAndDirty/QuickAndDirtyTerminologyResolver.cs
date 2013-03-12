/**
 * Copyright 2012-2012 Mohawk College of Applied Arts and Technology
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
using MARC.HI.EHRS.SVC.Core.DataTypes;
using System.Data;
using System.Configuration;
using MARC.HI.EHRS.SVC.Terminology.Configuration;
using MARC.HI.EHRS.SVC.Core.Terminology;
using System.Diagnostics;
using MARC.HI.EHRS.SVC.Terminology.CTS12;
using MARC.HI.EHRS.SVC.Core;
using System.EnterpriseServices;

namespace MARC.HI.EHRS.SVC.Terminology.QuickAndDirty
{
    /// <summary>
    /// Quick and dirty terminology resolver
    /// </summary>
    [Description("Quick and Dirty Terminology Resolver")]
    public class QuickAndDirtyTerminologyResolver : ITerminologyService
    {

        // Context
        private IServiceProvider m_context;

        /// <summary>
        /// Configuration
        /// </summary>
        private static ConfigurationSectionHandler m_configuration;

        /// <summary>
        /// CTS Resolver
        /// </summary>
        private static CtsTerminologyResolver m_ctsResolver;

        /// <summary>
        /// CTS Terminology Resolver
        /// </summary>
        static QuickAndDirtyTerminologyResolver()
        {
            m_configuration = ConfigurationManager.GetSection("marc.hi.ehrs.svc.terminology") as ConfigurationSectionHandler;
        }

        #region ITerminologyService Members

        /// <summary>
        /// Validate a code
        /// </summary>
        public MARC.HI.EHRS.SVC.Core.Terminology.ConceptValidationResult Validate(MARC.HI.EHRS.SVC.Core.DataTypes.CodeValue code)
        {

            // Return value
            var retVal = new MARC.HI.EHRS.SVC.Core.Terminology.ConceptValidationResult()
            {
                Outcome = MARC.HI.EHRS.SVC.Core.Terminology.ValidationOutcome.Valid
            };

            if (code == null)
                return retVal;

            string codeKey = String.Format("{0}@{1}", code.CodeSystem, code.Code);

            CodeValue tCodeCache = null;

            // Connect
            if (!CtsTerminologyResolver.m_cachedCodeLookups.TryGetValue(codeKey, out tCodeCache))
                using (IDbConnection conn = m_configuration.CreateConnection())
                    try
                    {
                        conn.Open();

                        // Create a command to attempt a lookup...
                        IDbCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "qdcdb_lookup_cd";
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parameters
                        IDataParameter pCodeSystem = cmd.CreateParameter(),
                            pMnemonic = cmd.CreateParameter();
                        pCodeSystem.Direction = pMnemonic.Direction = ParameterDirection.Input;
                        pMnemonic.DbType = pCodeSystem.DbType = DbType.String;
                        pCodeSystem.ParameterName = "oid_in";
                        pCodeSystem.Value = code.CodeSystem;
                        pMnemonic.ParameterName = "mnemonic_in";
                        pMnemonic.Value = code.Code;

                        // Add and execute
                        cmd.Parameters.Add(pCodeSystem);
                        cmd.Parameters.Add(pMnemonic);

                        // Execute
                        try
                        {
                            using (IDataReader rdr = cmd.ExecuteReader())
                            {
                                if (rdr.Read())
                                {
                                    string display = Convert.ToString(rdr["cncpt_display"]);
                                    if (!display.Equals(code.DisplayName))
                                        retVal.AddDetail(false, String.Format("{0}:{1} ({2}@{3})", "0001", "Incorrect display name for code", code.CodeSystem, code.Code), "W001");
                                }
                                else if (m_ctsResolver != null)
                                    return m_ctsResolver.Validate(code);
                                else
                                    throw new InvalidOperationException(String.Format("Cannot determine validity of code {0} in domain {1}", code.Code, code.CodeSystem));
                            }
                        }
                        catch (InvalidOperationException e)
                        {
                            retVal.Outcome = ValidationOutcome.ValidWithWarning;
                            retVal.AddDetail(false, e.Message, "INTERR");
                        }
                        catch (Exception e)
                        {
                            Trace.TraceError(e.ToString());
                            retVal.Outcome = ValidationOutcome.ValidWithWarning;
                            retVal.AddDetail(false, String.Format("{0} - Cannot perform terminology validation", e.Message), "INTERR");
                        }
                    }
                    finally
                    {
                        conn.Close();
                    }
            else
            {
                if (!tCodeCache.DisplayName.Equals(code.DisplayName))
                    retVal.AddDetail(false, String.Format("{0}:{1} ({2}@{3})", "0001", "Incorrect display name for code", code.CodeSystem, code.Code), "W001");
            }
            return retVal;
        }

       
        /// <summary>
        /// Validate a code using a named code system
        /// </summary>
        public ConceptValidationResult ValidateEx(string code, string displayName, CodeSystemName codeSystem)
        {
            return Validate(new CodeValue(code, GetCodeSystemDomain(codeSystem)) { DisplayName = displayName });
        }

        /// <summary>
        /// Get the code system's domain
        /// </summary>
        public string GetCodeSystemDomain(CodeSystemName codeSystemName)
        {
            ISystemConfigurationService configService = Context.GetService(typeof(ISystemConfigurationService)) as ISystemConfigurationService;
            return configService.OidRegistrar.GetOid(codeSystemName.ToString()).Oid;
        }
        
        /// <summary>
        /// Translate a code
        /// </summary>
        public MARC.HI.EHRS.SVC.Core.DataTypes.CodeValue Translate(MARC.HI.EHRS.SVC.Core.DataTypes.CodeValue code, string targetDomain)
        {

            CodeValue retVal = null;

            // Connect
            using (IDbConnection conn = m_configuration.CreateConnection())
                try
                {
                    conn.Open();

                    // Create a command to attempt a lookup...
                    IDbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "qdcdb_map_cd";
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parameters
                    IDataParameter pCodeSystem = cmd.CreateParameter(),
                        pMnemonic = cmd.CreateParameter(),
                        pTarget = cmd.CreateParameter();
                    pCodeSystem.Direction = pMnemonic.Direction = pTarget.Direction = ParameterDirection.Input;
                    pTarget.DbType = pMnemonic.DbType = pCodeSystem.DbType = DbType.String;
                    pCodeSystem.ParameterName = "oid_in";
                    pCodeSystem.Value = code.CodeSystem;
                    pMnemonic.ParameterName = "mnemonic_in";
                    pMnemonic.Value = code.Code;
                    pTarget.ParameterName = "oid_target_in";
                    pTarget.Value = targetDomain;

                    // Add and execute
                    cmd.Parameters.Add(pCodeSystem);
                    cmd.Parameters.Add(pMnemonic);
                    cmd.Parameters.Add(pTarget);

                    // Execute
                    try
                    {
                        using (IDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                string display = Convert.ToString(rdr["cncpt_display"]);

                                retVal = new CodeValue(
                                    Convert.ToString(rdr["cncpt_mnemonic"]),
                                    targetDomain)
                                    {
                                        DisplayName = display
                                    };
                            }
                            else if (m_ctsResolver != null)
                                return m_ctsResolver.Translate(code, targetDomain);
                        }
                    }
                    catch (Exception e)
                    {
                        Trace.TraceError(e.ToString());
                    }
                }
                finally
                {
                    conn.Close();
                }
            return retVal;


        }

        /// <summary>
        /// Fill in the details of a code
        /// </summary>
        public MARC.HI.EHRS.SVC.Core.DataTypes.CodeValue FillInDetails(MARC.HI.EHRS.SVC.Core.DataTypes.CodeValue codeValue)
        {
            CodeValue retVal = codeValue;
            string codeKey = String.Format("{0}@{1}", codeValue.CodeSystem, codeValue.Code);

            // Connect
            if (!CtsTerminologyResolver.m_cachedCodeLookups.TryGetValue(codeKey, out retVal))
                using (IDbConnection conn = m_configuration.CreateConnection())
                    try
                    {
                        retVal = codeValue;

                        conn.Open();

                        // Create a command to attempt a lookup...
                        IDbCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "qdcdb_lookup_cd";
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parameters
                        IDataParameter pCodeSystem = cmd.CreateParameter(),
                            pMnemonic = cmd.CreateParameter();
                        pCodeSystem.Direction = pMnemonic.Direction = ParameterDirection.Input;
                        pMnemonic.DbType = pCodeSystem.DbType = DbType.String;
                        pCodeSystem.ParameterName = "oid_in";
                        pCodeSystem.Value = codeValue.CodeSystem;
                        pMnemonic.ParameterName = "mnemonic_in";
                        pMnemonic.Value = codeValue.Code;

                        // Add and execute
                        cmd.Parameters.Add(pCodeSystem);
                        cmd.Parameters.Add(pMnemonic);

                        // Execute
                        try
                        {
                            using (IDataReader rdr = cmd.ExecuteReader())
                            {
                                if (rdr.Read())
                                {
                                    string display = Convert.ToString(rdr["cncpt_display"]);
                                    retVal.DisplayName = display;
                                    CtsTerminologyResolver.AddCachedCode(codeKey, retVal);
                                }
                                else if (m_ctsResolver != null)
                                    return m_ctsResolver.FillInDetails(codeValue);
                            }
                        }
                        catch (Exception e)
                        {
                            Trace.TraceError(e.ToString());
                        }
                    }
                    finally
                    {
                        conn.Close();
                    }
            return retVal;
        }

        #endregion

        #region IUsesHostContext Members

        /// <summary>
        /// Gets or sets the current context of the terminology resolver
        /// </summary>
        public IServiceProvider Context
        {
            get
            {
                return this.m_context;
            }
            set
            {
                this.m_context = value;
                if (this.m_context != null && m_configuration.EnableCtsFallback)
                    m_ctsResolver = new CtsTerminologyResolver() { Context = this.m_context };
            }
        }


        #endregion
    }
}
