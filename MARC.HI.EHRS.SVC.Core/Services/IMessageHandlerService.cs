﻿/* 
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

namespace MARC.HI.EHRS.SVC.Core.Services
{
    /// <summary>
    /// Represents a service that handles messages
    /// </summary>
    public interface IMessageHandlerService : IUsesHostContext
    {
        /// <summary>
        /// Start the service 
        /// </summary>
        /// <returns>True if the service startup was successful</returns>
        bool Start();

        /// <summary>
        /// Stop the service
        /// </summary>
        /// <returns>True if the service stop was successful</returns>
        bool Stop();
    }
}
