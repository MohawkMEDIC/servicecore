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
using System.Data;
using MARC.HI.EHRS.QM.Core.Exception;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Timers;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MARC.HI.EHRS.SVC.Core.Data;
using MARC.HI.EHRS.SVC.Core;

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
            m_configuration = ApplicationContext.Current.GetService<IConfigurationManager>().GetSection("marc.hi.ehrs.qm.persistence.data") as ConfigurationHandler;
        }


        #region IQueryPersistenceService Members

        /// <summary>
        /// Register a query set 
        /// </summary>
        public bool RegisterQuerySet<TIdentifier>(string queryId, Identifier<TIdentifier>[] results, object tag)
        {
            IDbConnection dbc = m_configuration.CreateConnection();
            IDbTransaction tx = null;
            try
            {
                dbc.Open();
                tx = dbc.BeginTransaction();

                if (IsRegistered(queryId))
                    throw new Exception(String.Format("Query '{0}' has already been registered with the QueryManager", queryId));

                // Register the query
                RegisterQuery(dbc, tx, queryId, results.Length, tag);

                // Push each result into 
                try
                {
                    PushResults(dbc, tx, queryId, results);
                }
                catch (Exception e)
                {
                    Trace.TraceError("Error pushing bulk: {0}", e);
                    foreach(var id in results)
                        this.PushResult(dbc, tx, queryId, id);
                }

                tx.Commit();

                // Return true
                return true;
            }
            catch (Exception e)
            {
                tx.Rollback();
                throw new QueryPersistenceException(e.Message, e);
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
        private void PushResults<TIdentifier>(IDbConnection conn, IDbTransaction tx, string queryId, Identifier<TIdentifier>[] resultId)
        {
            IDbCommand cmd = conn.CreateCommand();
            try
            {
                
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "push_qry_rslts";
                cmd.Transaction = tx;

                StringBuilder resultIds = new StringBuilder("{");
                foreach (var id in resultId)
                {
                    resultIds.AppendFormat("{{{0},{1}}},", id.Id, id.VersionId?.Equals(default(TIdentifier)) == false ? id.VersionId.ToString() : "null");
                }
                resultIds.Remove(resultIds.Length - 1, 1);
                resultIds.Append("}");

                // Setup parameters
                IDataParameter qryIdParam = cmd.CreateParameter(),
                    qryRsltParam = cmd.CreateParameter();
                qryIdParam.DbType = DbType.String;
                qryRsltParam.DbType = DbType.String;
                qryIdParam.Direction = qryRsltParam.Direction = ParameterDirection.Input;
                qryIdParam.ParameterName = "qry_id_in";
                qryRsltParam.ParameterName = "rslt_ent_id_in";
                qryRsltParam.Value = resultIds.ToString();
                qryIdParam.Value = queryId;

                // Add parameters
                cmd.Parameters.Add(qryIdParam);
                cmd.Parameters.Add(qryRsltParam);

                // Execute
                cmd.ExecuteNonQuery();
            }
            finally
            {
                cmd.Dispose();
            }
        }


        /// <summary>
        /// Push a result into the data store
        /// </summary>
        private void PushResult<TIdentifier>(IDbConnection conn, IDbTransaction tx, string queryId, Identifier<TIdentifier> resultId)
        {
            IDbCommand cmd = conn.CreateCommand();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "push_qry_rslt";
                cmd.Transaction = tx;

                // Setup parameters
                IDataParameter qryIdParam = cmd.CreateParameter(),
                    qryRsltParam = cmd.CreateParameter(),
                    qryVrsnParam = cmd.CreateParameter();
                qryIdParam.DbType = DbType.String;
                qryVrsnParam.DbType = qryRsltParam.DbType = DbType.String;
                qryVrsnParam.Direction = qryIdParam.Direction = qryRsltParam.Direction = ParameterDirection.Input;
                qryIdParam.ParameterName = "qry_id_in";
                qryRsltParam.ParameterName = "rslt_ent_id_in";
                qryVrsnParam.ParameterName = "rslt_vrsn_id_in";
                qryRsltParam.Value = resultId.Id.ToString();
                qryVrsnParam.Value = resultId.VersionId?.Equals(default(TIdentifier)) == false ? (Object)resultId.VersionId.ToString() : DBNull.Value;
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
        private void RegisterQuery(IDbConnection conn, IDbTransaction tx, string queryId, int nRecords, object tag)
        {
            // Create command
            IDbCommand cmd = conn.CreateCommand();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "reg_qry";
                cmd.Transaction = tx;

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
                    catch(Exception e)
                    {
                        qryDmnParam.Value = Encoding.UTF8.GetBytes(tag.ToString());
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
                throw new QueryPersistenceException(e.Message, e);
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
        public Identifier<TIdentifier>[] GetQueryResults<TIdentifier>(string queryId, int startRecord, int nRecords)
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
                    qryStartParam.Value = startRecord;

                    // Add parameters
                    cmd.Parameters.Add(qryIdParam);
                    cmd.Parameters.Add(qryStartParam);
                    cmd.Parameters.Add(qryQtyParam);

                    // Execute reader
                    List<Identifier<TIdentifier>> domainId = new List<Identifier<TIdentifier>>(nRecords);
                    IDataReader rdr = cmd.ExecuteReader();
                    try
                    {
                        while (rdr.Read())
                        {
                            object rValue = rdr["ent_id"],
                                vValue = rdr["vrsn_id"];

                            if(typeof(TIdentifier) == typeof(Guid))
                            {
                                rValue = Guid.Parse(rValue.ToString());
                                vValue = vValue == DBNull.Value ? default(Guid) : Guid.Parse(vValue.ToString());
                            }
                            else if(typeof(TIdentifier) == typeof(String))
                            {
                                rValue = rValue.ToString();
                                vValue = vValue == DBNull.Value ? null: vValue.ToString();
                            }
                            else if (typeof(TIdentifier) == typeof(Decimal))
                            {
                                rValue = Decimal.Parse(rValue.ToString());
                                vValue = vValue == DBNull.Value ? default(Decimal) : Decimal.Parse(vValue.ToString());
                            }

                            domainId.Add(new Identifier<TIdentifier>()
                            {
                                Id = (TIdentifier)rValue,
                                VersionId = (TIdentifier)vValue
                            });
                        }
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
                throw new QueryPersistenceException(e.Message, e);
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
                throw new QueryPersistenceException(e.Message, e);
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
                throw new QueryPersistenceException(e.Message, e);
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
