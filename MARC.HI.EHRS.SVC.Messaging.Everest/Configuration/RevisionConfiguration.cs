/*
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
using System.Reflection;

namespace MARC.HI.EHRS.SVC.Messaging.Everest.Configuration
{
    /// <summary>
    /// Represents an ITS configuration
    /// </summary>
    public class RevisionConfiguration
    {
        /// <summary>
        /// Gets or sets the name of the configuration revision
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Formatter for the host
        /// </summary>
        public Type Formatter { get; set; }
        /// <summary>
        /// Graph aide for the host
        /// </summary>
        public Type GraphAide { get; set; }
        /// <summary>
        /// If true, will enable everest validation
        /// </summary>
        public bool ValidateInstances { get; set; }
        /// <summary>
        /// Identifier format
        /// </summary>
        public string MessageIdentifierFormat { get; set; }

        /// <summary>
        /// Gets a list of Listeners
        /// </summary>
        public List<ListenConfiguration> Listeners { get; private set; }
        /// <summary>
        /// Gets the list of types to cache
        /// </summary>
        public List<Type> CacheTypes { get; private set; }
        /// <summary>
        /// Gets the list of message handlers
        /// </summary>
        public List<MessageHandlerConfiguration> MessageHandlers { get; private set; }
        /// <summary>
        /// Gets or sets the assembly that this revision uses for messages
        /// </summary>
        public Assembly Assembly { get; set; }
        /// <summary>
        /// Creates a new instance of an ITSConfiguration class
        /// </summary>
        public RevisionConfiguration()
        {
            this.Listeners = new List<ListenConfiguration>();
            this.CacheTypes = new List<Type>();
            this.MessageHandlers = new List<MessageHandlerConfiguration>();
        }
    }
}
