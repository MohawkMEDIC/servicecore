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
 * Date: 1-8-2012
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.Services;
using MARC.HI.EHRS.SVC.Messaging.Persistence.Data.Configuration;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.ComponentModel;
using System.Threading;
using MARC.HI.EHRS.SVC.Core.Event;
using System.Security.Claims;

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
            ///// <summary>
            ///// Gets or sets the id of the message
            ///// </summary>
            //public String MessageId { get; set; }
            ///// <summary>
            ///// Gets or sets the id of the response
            ///// </summary>
            //public String ResponseToId { get; set; }
            ///// <summary>
            ///// Gets or sets the body stream
            ///// </summary>
            //public byte[] MessageBody { get; set; }
            /// <summary>
            /// The message information to persist
            /// </summary>
            public MessageInfo MessageInfo { get; set; }
        }

        /// <summary>
        /// Configuration for the persistence service
        /// </summary>
        private static ConfigurationSectionHandler m_configuration;

        /// <summary>
        /// Fired when a message is being persisted
        /// </summary>
        public event EventHandler<PrePersistenceEventArgs<MessageInfo>> Persisting;
        /// <summary>
        /// Fired when a message has been persisted
        /// </summary>
        public event EventHandler<PostPersistenceEventArgs<MessageInfo>> Persisted;
        /// <summary>
        /// Fired when a message is being retrieved
        /// </summary>
        public event EventHandler<PreRetrievalEventArgs<MessageInfo>> Retrieving;
        /// <summary>
        /// Fired after a message has been retrieved
        /// </summary>
        public event EventHandler<PostRetrievalEventArgs<MessageInfo>> Retrieved;

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
            this.PersistMessageInfo(
                new MessageInfo()
                {
                    Response = respondsToId,
                    Body = GetMessageBody(response),
                    Id = messageId
                }
            );
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

                var mpe = new PrePersistenceEventArgs<MessageInfo>(args.MessageInfo);
                this.Persisting?.Invoke(this, mpe);

                if (mpe.Cancel)
                {
                    Trace.TraceInformation("Message persistence event indicates cancel");
                    return;
                }

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
                    msgIdParm.Value = args.MessageInfo.Id;
                    msgIdParm.Direction = ParameterDirection.Input;
                    msgIdParm.ParameterName = "msg_id_in";
                    cmd.Parameters.Add(msgIdParm);

                    // Setup parameter for the message body
                    IDataParameter msgBodyParm = cmd.CreateParameter();
                    msgBodyParm.DbType = DbType.Binary;
                    msgBodyParm.Value = args.MessageInfo.Body;
                    msgBodyParm.Direction = ParameterDirection.Input;
                    msgBodyParm.ParameterName = "msg_body_in";
                    cmd.Parameters.Add(msgBodyParm);


                    // Setup parameter for msg_rsp_in
                    IDataParameter msgRspParm = cmd.CreateParameter();
                    msgRspParm.DbType = DbType.String;
                    msgRspParm.Value = String.IsNullOrEmpty(args.MessageInfo.Response) ? DBNull.Value : (object)args.MessageInfo.Response;
                    msgRspParm.Direction = ParameterDirection.Input;
                    msgRspParm.ParameterName = "msg_rsp_in";
                    cmd.Parameters.Add(msgRspParm);

                    // Setup parameter for msg_rsp_in
                    IDataParameter msgSrcParm = cmd.CreateParameter();
                    msgSrcParm.DbType = DbType.String;
                    msgSrcParm.Value = args.MessageInfo.Source == null ? DBNull.Value : (object)args.MessageInfo.Source.ToString();
                    msgSrcParm.Direction = ParameterDirection.Input;
                    msgSrcParm.ParameterName = "src_in";
                    cmd.Parameters.Add(msgSrcParm);

                    // Setup parameter for msg_rsp_in
                    IDataParameter msgDstParm = cmd.CreateParameter();
                    msgDstParm.DbType = DbType.String;
                    msgDstParm.Value = args.MessageInfo.Destination == null ? DBNull.Value : (object)args.MessageInfo.Destination.ToString();
                    msgDstParm.Direction = ParameterDirection.Input;
                    msgDstParm.ParameterName = "dst_in";
                    cmd.Parameters.Add(msgDstParm);

                    // Execute the query without result
                    cmd.ExecuteNonQuery();

                    this.Persisted?.Invoke(this, new PostPersistenceEventArgs<MessageInfo>(args.MessageInfo));

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
                args.MessageInfo = null;
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
            var messageInfo = this.GetMessageInfo(messageId);
            if (messageInfo != null)
                return new MemoryStream(messageInfo.Body);
            else
                return null;
        }

        #endregion

        #region IMessagePersistenceService Members

        /// <summary>
        /// Get all message request identifiers between specified times
        /// </summary>
        public IEnumerable<string> GetMessageIds(DateTime from, DateTime to)
        {
            List<String> retVal = new List<string>();
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
                    cmd.CommandText = "qry_msgs";

                    // Setup parameter
                    IDataParameter fromParm = cmd.CreateParameter(),
                        toParm = cmd.CreateParameter();
                    fromParm.DbType = toParm.DbType = DbType.DateTime;
                    fromParm.Value = from;
                    toParm.Value = to;
                    fromParm.Direction = toParm.Direction= ParameterDirection.Input;
                    fromParm.ParameterName = "msg_utc_from_in";
                    toParm.ParameterName = "msg_utc_to_in";
                    cmd.Parameters.Add(fromParm);
                    cmd.Parameters.Add(toParm);

                    // Execute
                    using (IDataReader rdr = cmd.ExecuteReader())
                        while (rdr.Read())
                            retVal.Add(Convert.ToString(rdr[0]));
                    return retVal;
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

        #region IMessagePersistenceService Members

        /// <summary>
        /// Persist detailed message information
        /// </summary>
        public void PersistMessageInfo(MessageInfo message)
        {
            ThreadPool.QueueUserWorkItem(DoPersistResultMessage, new AdoMessagePersistanceArgs()
            {
                MessageInfo = message
            });
        }

        /// <summary>
        /// Get message info
        /// </summary>
        public MessageInfo GetMessageInfo(string messageId)
        {
            IDbConnection conn = m_configuration.CreateConnection();

            try
            {

                var mpe = new PreRetrievalEventArgs<MessageInfo>(new MessageInfo() { Id = messageId });
                this.Retrieving?.Invoke(this, mpe);
                if(mpe.Cancel)
                {
                    Trace.TraceInformation("GetMessageInfo: Event handler indicates cancel");
                    return null;
                }

                conn.Open();

                // Create the database command
                IDbCommand cmd = conn.CreateCommand();

                try
                {
                    // Setup command
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "get_msg_tbl";

                    // Setup parameter
                    IDataParameter msgIdParm = cmd.CreateParameter();
                    msgIdParm.DbType = DbType.String;
                    msgIdParm.Value = messageId;
                    msgIdParm.Direction = ParameterDirection.Input;
                    msgIdParm.ParameterName = "msg_id_in";
                    cmd.Parameters.Add(msgIdParm);

                    // Execute
                    using (IDataReader rdr = cmd.ExecuteReader())
                        if (rdr.Read())
                        {
                            var retVal = new MessageInfo()
                            {
                                Body = (byte[])rdr["msg_body"],
                                Destination = rdr["msg_dst"] == DBNull.Value ? null : new Uri(Convert.ToString(rdr["msg_dst"])),
                                Id = Convert.ToString(rdr["msg_id"]),
                                Response = rdr["msg_rsp_id"] == DBNull.Value ? null : Convert.ToString(rdr["msg_rsp_id"]),
                                Source = rdr["msg_src"] == DBNull.Value ? null : new Uri(Convert.ToString(rdr["msg_src"])),
                                Timestamp = (DateTime)rdr["msg_utc"]
                            };

                            this.Retrieved?.Invoke(this, new PostRetrievalEventArgs<MessageInfo>(retVal));
                            return retVal;
                        }
                        else
                        {
                            this.Retrieved?.Invoke(this, new PostRetrievalEventArgs<MessageInfo>(null));
                            return null;
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
