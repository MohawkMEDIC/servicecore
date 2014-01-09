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
 * Date: 17-10-2012
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.Services;
using MARC.HI.EHRS.QM.Persistence.Data.Configuration;
using System.Configuration;
using MARC.HI.EHRS.SVC.Core.DataTypes;
using System.Data;
using MARC.HI.EHRS.QM.Core.Exception;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Timers;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MARC.HI.EHRS.QM.Persistence.Data
{
    /// <summary>
    /// A class the implements the QueryManager using Ado
    /// </summary>
    [Description("ADO.NET Query Registration Service")]
    public class AdoQueryPersistenceService : IQueryPersistenceService
    {

        /// <summary>
        /// Configuration handler
        /// </summary>
        private static ConfigurationHandler m_configuration;

        /// <summary>
        /// Ado Query Persistence Service
        /// </summary>
        static AdoQueryPersistenceService()
        {
            m_configuration = ConfigurationManager.GetSection("marc.hi.ehrs.qm.persistence.data") as ConfigurationHandler;
        }


        #region IQueryPersistenceService Members

        /// <summary>
        /// Register a query set 
        /// </summary>
        public bool RegisterQuerySet(string queryId, MARC.HI.EHRS.SVC.Core.DataTypes.VersionedDomainIdentifier[] results, object tag)
        {
            IDbConnection dbc = m_configuration.CreateConnection();
            try
            {
                dbc.Open();

                if (IsRegistered(queryId))
                    throw new Exception(String.Format("Query '{0}' has already been registered with the QueryManager", queryId));

                // Register the query
                RegisterQuery(dbc, queryId, results.Length, tag);

                // Push each result into 
                foreach (var id in results)
                    PushResult(dbc, queryId, id);
                // Return true
                return true;
            }
            catch (Exception e)
            {
                throw new QueryPersistenceException(e.Message);
            }
            finally
            {
                dbc.Close();
                dbc.Dispose();
            }
        }

        /// <summary>
        /// Push a result into the data store
        /// </summary>
        private void PushResult(IDbConnection conn, string queryId, VersionedDomainIdentifier resultId)
        {
            IDbCommand cmd = conn.CreateCommand();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "push_qry_rslt";

                // Setup parameters
                IDataParameter qryIdParam = cmd.CreateParameter(),
                    qryRsltParam = cmd.CreateParameter(),
                    qryVrsnParam = cmd.CreateParameter();
                qryIdParam.DbType = DbType.String;
                qryVrsnParam.DbType = qryRsltParam.DbType = DbType.Decimal;
                qryVrsnParam.Direction = qryIdParam.Direction = qryRsltParam.Direction = ParameterDirection.Input;
                qryIdParam.ParameterName = "qry_id_in";
                qryRsltParam.ParameterName = "rslt_ent_id_in";
                qryVrsnParam.ParameterName = "rslt_vrsn_id_in";
                qryRsltParam.Value = Decimal.Parse(resultId.Identifier);
                qryVrsnParam.Value = resultId.Version == null ? DBNull.Value : (object)Decimal.Parse(resultId.Version);
                qryIdParam.Value = queryId;

                // Add parameters
                cmd.Parameters.Add(qryIdParam);
                cmd.Parameters.Add(qryRsltParam);
                cmd.Parameters.Add(qryVrsnParam);

                // Execute
                cmd.ExecuteNonQuery();
            }
            finally
            {
                cmd.Dispose();
            }
        }

        /// <summary>
        /// Register the query 
        /// </summary>
        private void RegisterQuery(IDbConnection conn, string queryId, int nRecords, object tag)
        {
            // Create command
            IDbCommand cmd = conn.CreateCommand();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "reg_qry";
                
                // Setup Parameters
                IDataParameter qryIdParam = cmd.CreateParameter(),
                    qryCntParam = cmd.CreateParameter(),
                    qryDmnParam = cmd.CreateParameter();

                qryIdParam.DbType = DbType.String;
                qryCntParam.DbType = DbType.Decimal;
                qryDmnParam.DbType = DbType.Binary;
                qryIdParam.Value = queryId;
                qryCntParam.Value = nRecords.ToString();

                // Serialize the tag
                if (tag != null)
                {
                    MemoryStream ms = new MemoryStream();
                    try
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        bf.Serialize(ms, tag);
                        ms.Flush();
                        qryDmnParam.Value = ms.GetBuffer();
                    }
                    finally
                    {
                        ms.Dispose();
                    }
                }
                else
                    qryDmnParam.Value = DBNull.Value;

                qryIdParam.Direction = qryCntParam.Direction = qryDmnParam.Direction = ParameterDirection.Input;
                qryIdParam.ParameterName = "qry_id_in";
                qryCntParam.ParameterName = "qry_rslt_cnt_in";
                qryDmnParam.ParameterName = "qry_tag_in";

                // Add Parameters
                cmd.Parameters.Add(qryIdParam);
                cmd.Parameters.Add(qryCntParam);
                cmd.Parameters.Add(qryDmnParam);

                // Execute the parameter
                cmd.ExecuteNonQuery();
            }
            finally
            {
                cmd.Dispose();
            }

        }

        /// <summary>
        /// Determine if the query has been registered
        /// </summary>
        /// <param name="queryId"></param>
        /// <returns></returns>
        public bool IsRegistered(string queryId)
        {
            // Create connection
            IDbConnection dbc = m_configuration.CreateConnection();

            try
            {
                dbc.Open();

                // Create command
                IDbCommand cmd = dbc.CreateCommand();
                try
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "is_qry_reg";

                    // Setup parameters
                    IDataParameter qryIdParam = cmd.CreateParameter();
                    qryIdParam.Direction = ParameterDirection.Input;
                    qryIdParam.ParameterName = "qry_id_in";
                    qryIdParam.Value = queryId;
                    qryIdParam.DbType = DbType.String;
                    cmd.Parameters.Add(qryIdParam);

                    // Execute
                    return Convert.ToBoolean(cmd.ExecuteScalar());
                }
                finally
                {
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                throw new QueryPersistenceException(e.Message);
            }
            finally
            {
                dbc.Close();
                dbc.Dispose();
            }
        }

        /// <summary>
        /// Get Query Results from the database
        /// </summary>
        public MARC.HI.EHRS.SVC.Core.DataTypes.VersionedDomainIdentifier[] GetQueryResults(string queryId, int startRecord, int nRecords)
        {
            IDbConnection dbc = m_configuration.CreateConnection();
            try
            {
                dbc.Open();

                IDbCommand cmd = dbc.CreateCommand();
                try
                {
                    // Setup command
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "get_qry_rslts";

                    // Setup parameters
                    IDataParameter qryIdParam = cmd.CreateParameter(),
                        qryStartParam = cmd.CreateParameter(),
                        qryQtyParam = cmd.CreateParameter();
                    qryStartParam.DbType = qryQtyParam.DbType = DbType.Decimal;
                    qryIdParam.Direction = qryQtyParam.Direction = qryStartParam.Direction = ParameterDirection.Input;
                    qryIdParam.ParameterName = "qry_id_in";
                    qryStartParam.ParameterName = "str_in";
                    qryQtyParam.ParameterName = "qty_in";
                    qryIdParam.Value = queryId;
                    qryQtyParam.Value = nRecords;
                    qryStartParam.Value = startRecord == -1 ? DBNull.Value : (object)startRecord;

                    // Add parameters
                    cmd.Parameters.Add(qryIdParam);
                    cmd.Parameters.Add(qryStartParam);
                    cmd.Parameters.Add(qryQtyParam);

                    // Execute reader
                    List<VersionedDomainIdentifier> domainId = new List<VersionedDomainIdentifier>(nRecords);
                    IDataReader rdr = cmd.ExecuteReader();
                    try
                    {
                        while (rdr.Read())
                            domainId.Add(new VersionedDomainIdentifier()
                            {
                                Identifier = Convert.ToString(rdr["ent_id"]),
                                Version = Convert.ToString(rdr["vrsn_id"])
                            });
                    }
                    finally
                    {
                        rdr.Close();
                        rdr.Dispose();
                    }

                    return domainId.ToArray();
                }
                finally
                {
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                throw new QueryPersistenceException(e.Message);
            }
            finally
            {
                dbc.Close();
                dbc.Dispose();
            }
        }

        /// <summary>
        /// Get the result remaining quantity
        /// </summary>
        public long QueryResultTotalQuantity(string queryId)
        {
            IDbConnection dbc = m_configuration.CreateConnection();
            try
            {
                dbc.Open();

                IDbCommand cmd = dbc.CreateCommand();
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "get_qry_cnt";

                    IDataParameter qryIdParam = cmd.CreateParameter();
                    qryIdParam.DbType = DbType.String;
                    qryIdParam.Direction = ParameterDirection.Input;
                    qryIdParam.ParameterName = "qry_id_in";
                    qryIdParam.Value = queryId;
                    cmd.Parameters.Add(qryIdParam);

                    // Execute and return
                    return Convert.ToInt64(cmd.ExecuteScalar());
                }
                finally
                {
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                throw new QueryPersistenceException(e.Message);
            }
            finally
            {
                dbc.Close();
                dbc.Dispose();
            }
        }

        #endregion

        #region IUsesHostContext Members

        public IServiceProvider Context
        {
            get;
            set;
        }

        /// <summary>
        /// Get the Tagged value for a query
        /// </summary>
        public object GetQueryTag(string queryId)
        {
            IDbConnection conn = m_configuration.CreateConnection();
            try
            {
                conn.Open();
                IDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "get_qry_tag";
                cmd.CommandType = CommandType.StoredProcedure;

                IDataParameter idParam = cmd.CreateParameter();
                idParam.Direction = ParameterDirection.Input;
                idParam.DbType = DbType.String;
                idParam.ParameterName = "qry_id_in";
                idParam.Value = queryId;
                cmd.Parameters.Add(idParam);

                var serData = cmd.ExecuteScalar();

                if (serData != DBNull.Value)
                {
                    MemoryStream ms = new MemoryStream((byte[])serData);
                    try
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        return bf.Deserialize(ms);
                    }
                    finally
                    {
                        ms.Dispose();
                    }
                }
                else
                    return null;

            }
            catch (Exception e)
            {
                throw new QueryPersistenceException(e.Message);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        #endregion
    }
}
