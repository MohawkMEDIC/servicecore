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
using System.IO;
using MARC.HI.EHRS.SVC.Core.Event;
using MARC.HI.EHRS.SVC.Core.Data;

namespace MARC.HI.EHRS.SVC.Core.Services
{

    /// <summary>
    /// Identifies the status of a message
    /// </summary>
    public enum MessageState
    {
        /// <summary>
        /// The message has never been received by the system
        /// </summary>
        New,
        /// <summary>
        /// The message has been received by the system and is in process
        /// </summary>
        Active,
        /// <summary>
        /// The message has been received by the system and processing is complete
        /// </summary>
        Complete
    }

    /// <summary>
    /// Message information
    /// </summary>
    public class MessageInfo 
    {
        /// <summary>
        /// Gets the id of the message
        /// </summary>
        public String Id { get; set; }
        /// <summary>
        /// Gets the message id that this message responds to or the response of this message.
        /// </summary>
        public String Response { get; set; }
        /// <summary>
        /// Gets the remote endpoint of the message
        /// </summary>
        public Uri Source { get; set; }
        /// <summary>
        /// Gets the local endpoint of the message
        /// </summary>
        public Uri Destination { get; set; }
        /// <summary>
        /// Gets the time the message was received
        /// </summary>
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Gets the body of the message
        /// </summary>
        public byte[] Body { get; set; }
        /// <summary>
        /// Gets or sets the state of the message
        /// </summary>
        public MessageState State { get; set; }

    }

    /// <summary>
    /// Identifies a structure for message persistence service implementations
    /// </summary>
    public interface IMessagePersistenceService
    {

        /// <summary>
        /// Get the state of a message
        /// </summary>
        MessageState GetMessageState(string messageId);

        /// <summary>
        /// Persists the message 
        /// </summary>
        void PersistMessage(string messageId, Stream message);

        /// <summary>
        /// Persist message extension
        /// </summary>
        void PersistMessageInfo(MessageInfo message);

        /// <summary>
        /// Get the identifier of the message that represents the response to the current message
        /// </summary>
        Stream GetMessageResponseMessage(string messageId);

        /// <summary>
        /// Get a message
        /// </summary>
        /// <param name="messageId">Body</param>
        /// <returns></returns>
        Stream GetMessage(string messageId);

        /// <summary>
        /// Persist
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="response"></param>
        void PersistResultMessage(string messageId, string respondsToId, Stream response);

        /// <summary>
        /// Get all message ids between the specified time(s)
        /// </summary>
        IEnumerable<String> GetMessageIds(DateTime from, DateTime to);

        /// <summary>
        /// Get message extended attribute
        /// </summary>
        MessageInfo GetMessageInfo(String messageId);

        /// <summary>
        /// Fired prior to persisting
        /// </summary>
        event EventHandler<PrePersistenceEventArgs<MessageInfo>> Persisting;
        /// <summary>
        /// Fired after persisting
        /// </summary>
        event EventHandler<PostPersistenceEventArgs<MessageInfo>> Persisted;
        /// <summary>
        /// Fired before message information is persisted
        /// </summary>
        event EventHandler<PreRetrievalEventArgs<MessageInfo>> Retrieving;
        /// <summary>
        /// Fired after message information is retrieved
        /// </summary>
        event EventHandler<PostRetrievalEventArgs<MessageInfo>> Retrieved;

    }
}
