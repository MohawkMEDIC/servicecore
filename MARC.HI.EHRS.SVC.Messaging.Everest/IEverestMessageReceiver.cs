﻿/**
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
using MARC.Everest.Interfaces;
using MARC.Everest.Connectors;
using MARC.HI.EHRS.SVC.Core.Services;

namespace MARC.HI.EHRS.SVC.Messaging.Everest
{
    /// <summary>
    /// Defines a framework for writing receivers for message receivers for the everest
    /// message handler
    /// </summary>
    public interface IEverestMessageReceiver : IUsesHostContext, ICloneable
    {

        /// <summary>
        /// Called when a message is received from a connector
        /// </summary>
        /// <param name="source">The connector that sent the message</param>
        /// <param name="receivedMessage">The message data</param>
        /// <returns>A response to the message</returns>
        IGraphable HandleMessageReceived(object sender, UnsolicitedDataEventArgs e, IReceiveResult receivedMessage); 
    }
}
