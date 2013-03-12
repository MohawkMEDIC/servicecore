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
 * Date: 1-8-2012
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.Services;
using MARC.HI.EHRS.SVC.Core.DataTypes;
using MARC.HI.EHRS.SVC.Messaging.Persistence.Data.Configuration;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.ComponentModel;
using System.Threading;

namespace MARC.HI.EHRS.SVC.Messaging.Persistence.Data
{
    /// <summary>
    /// Provides data based message persistence
    /// </summary>
    [Description("ADO.NET Message Persistence Service")]
    public class AdoMessagePersister : IMessagePersistenceService
    {
        /// <summary>
        /// Message persistence arguments
        /// </summary>
        private class AdoMessagePersistanceArgs
        {
            /// <summary>
            /// Gets or sets the id of the message
            /// </summary>
            public String MessageId { get; set; }
            /// <summary>
            /// Gets or sets the id of the response
            /// </summary>
            public String ResponseToId { get; set; }
            /// <summary>
            /// Gets or sets the body stream
            /// </summary>
            public byte[] MessageBody { get; set; }
        }

        /// <summary>
        /// Configuration for the persistence service
        /// </summary>
        private static ConfigurationSectionHandler m_configuration;

        /// <summary>
        /// Static ctor for the database message persister
        /// </summary>
        static AdoMessagePersister()
        {
            m_configuration = ConfigurationManager.GetSection("marc.hi.ehrs.svc.messaging.persistence") as ConfigurationSectionHandler;
        }

        #region IMessagePersistenceService Members

        /// <summary>
        /// Get the state of the message in the persistence store
        /// </summary>
        public MessageState GetMessageState(String messageId)
        {
            IDbConnection conn = m_configuration.CreateConnection();

            try
            {
                conn.Open();

                // Create the database command
                IDbCommand cmd = conn.CreateCommand();

                try
                {
                    // Setup command
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "qry_msg_state";

                    // Setup parameter
                    IDataParameter msgIdParm = cmd.CreateParameter();
                    msgIdParm.DbType = DbType.String;
                    msgIdParm.Value = messageId;
                    msgIdParm.Direction = ParameterDirection.Input;
                    msgIdParm.ParameterName = "msg_id_in";
                    cmd.Parameters.Add(msgIdParm);

                    // Execute
                    switch (cmd.ExecuteScalar().ToString())
                    {
                        case "N":
                            return MessageState.New;
                        case "A":
                            return MessageState.Active;
                        case "C":
                            return MessageState.Complete;
                        default:
                            return MessageState.New;
                    }
                }
                finally
                {
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                return MessageState.New;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        /// <summary>
        /// Persist a message to the persistence store
        /// </summary>
        public void PersistMessage(String messageId, Stream message)
        {
            PersistResultMessage(messageId, String.Empty, message);
        }

        /// <summary>
        /// Get the message that responds to the specified message identifier
        /// </summary>
        public System.IO.Stream GetMessageResponseMessage(String messageId)
        {

            IDbConnection conn = m_configuration.CreateConnection();

            try
            {
                conn.Open();

                // Create the database command
                IDbCommand cmd = conn.CreateCommand();

                try
                {
                    // Setup command
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "get_rsp_msg";

                    // Setup parameter
                    IDataParameter msgIdParm = cmd.CreateParameter();
                    msgIdParm.DbType = DbType.String;
                    msgIdParm.Value = messageId;
                    msgIdParm.Direction = ParameterDirection.Input;
                    msgIdParm.ParameterName = "msg_id_in";
                    cmd.Parameters.Add(msgIdParm);

                    // Execute
                    byte[] resp = (byte[])cmd.ExecuteScalar();
                    return new MemoryStream(resp);
                }
                finally
                {
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        /// <summary>
        /// Persist a response message to the data store
        /// </summary>
        public void PersistResultMessage(String messageId, String respondsToId, Stream response)
        {
            ThreadPool.QueueUserWorkItem(DoPersistResultMessage, new AdoMessagePersistanceArgs()
            {
                ResponseToId = respondsToId,
                MessageBody = GetMessageBody(response),
                MessageId = messageId
            });
        }

        /// <summary>
        /// Get message body
        /// </summary>
        private byte[] GetMessageBody(Stream response)
        {
            if(response.CanSeek)
                response.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[response.Length];
            response.Read(buffer, 0, (int)response.Length);
            return buffer;
        }

        internal void DoPersistResultMessage(object state)
        {
            // Persist a message to the database
            IDbConnection conn = m_configuration.CreateConnection();
            AdoMessagePersistanceArgs args = state as AdoMessagePersistanceArgs;

            try
            {
                conn.Open();

                // Create the database command
                IDbCommand cmd = conn.CreateCommand();

                try
                {
                    // Setup command
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "reg_msg";
                    cmd.CommandTimeout = 30;

                    // Setup parameter for message id
                    IDataParameter msgIdParm = cmd.CreateParameter();
                    msgIdParm.DbType = DbType.String;
                    msgIdParm.Value = args.MessageId;
                    msgIdParm.Direction = ParameterDirection.Input;
                    msgIdParm.ParameterName = "msg_id_in";
                    cmd.Parameters.Add(msgIdParm);

                    // Setup parameter for the message body
                    IDataParameter msgBodyParm = cmd.CreateParameter();
                    msgBodyParm.DbType = DbType.Binary;
                    msgBodyParm.Value = args.MessageBody;
                    msgBodyParm.Direction = ParameterDirection.Input;
                    msgBodyParm.ParameterName = "msg_body_in";
                    cmd.Parameters.Add(msgBodyParm);


                    // Setup parameter for msg_rsp_in
                    IDataParameter msgRspParm = cmd.CreateParameter();
                    msgRspParm.DbType = DbType.String;
                    msgRspParm.Value = String.IsNullOrEmpty(args.ResponseToId) ? DBNull.Value : (object)args.ResponseToId;
                    msgRspParm.Direction = ParameterDirection.Input;
                    msgRspParm.ParameterName = "msg_rsp_in";
                    cmd.Parameters.Add(msgRspParm);

                    // Execute the query without result
                    
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
            }
            finally
            {
                conn.Close();
                conn.Dispose();
                args.MessageBody = null;
                System.GC.Collect();
            }
        }

        #endregion

        #region IUsesHostContext Members

        /// <summary>
        /// Gets or sets the host context that this message persister operates within
        /// </summary>
        public IServiceProvider Context
        {
            get;
            set; 
        }

        #endregion

        #region IMessagePersistenceService Members

        /// <summary>
        /// GEt a message
        /// </summary>
        public Stream GetMessage(String messageId)
        {
            IDbConnection conn = m_configuration.CreateConnection();

            try
            {
                conn.Open();

                // Create the database command
                IDbCommand cmd = conn.CreateCommand();

                try
                {
                    // Setup command
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "get_msg";

                    // Setup parameter
                    IDataParameter msgIdParm = cmd.CreateParameter();
                    msgIdParm.DbType = DbType.String;
                    msgIdParm.Value = messageId;
                    msgIdParm.Direction = ParameterDirection.Input;
                    msgIdParm.ParameterName = "msg_id_in";
                    cmd.Parameters.Add(msgIdParm);

                    // Execute
                    byte[] resp = (byte[])cmd.ExecuteScalar();
                    return new MemoryStream(resp);
                }
                finally
                {
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                throw;
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
