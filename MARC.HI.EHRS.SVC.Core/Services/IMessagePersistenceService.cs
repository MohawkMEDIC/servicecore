/* 
 * Copyright 2008-2011 Mohawk College of Applied Arts and Technology
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
 * User: Justin Fyfe
 * Date: 08-24-2011
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.DataTypes;
using System.IO;

namespace MARC.HI.EHRS.SVC.Core.Services
{
    /// <summary>
    /// Identifies a structure for message persistence service implementations
    /// </summary>
    public interface IMessagePersistenceService : IUsesHostContext
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
        /// Get the identifier of the message that represents the response to the current message
        /// </summary>
        Stream GetMessageResponseMessage(string messageId);

        /// <summary>
        /// Get a message
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        Stream GetMessage(string messageId);

        /// <summary>
        /// Persist
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="response"></param>
        void PersistResultMessage(string messageId, string respondsToId, Stream response);
    }
}
