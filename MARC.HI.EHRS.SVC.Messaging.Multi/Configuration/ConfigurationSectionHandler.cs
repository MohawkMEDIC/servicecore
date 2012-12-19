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
 * Date: 13-8-2012
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using MARC.HI.EHRS.SVC.Core.Services;
using System.Xml;
using System.Reflection;
using System.Diagnostics;

namespace MARC.HI.EHRS.SVC.Messaging.Multi.Configuration
{
    /// <summary>
    /// Configuration section handler for the multiple listener message handler
    /// </summary>
    /// <marc.hi.ehrs.svc.messaging.multi>
    ///     <handlers>
    ///         <add type="FQN of type"/>
    ///     </handlers>
    /// </marc.hi.ehrs.svc.messaging.multi>
    /// 
    public class ConfigurationSectionHandler : IConfigurationSectionHandler
    {

        /// <summary>
        /// Gets the message handlers that should be enlisted to receive messages
        /// </summary>
        public List<IMessageHandlerService> MessageHandlers { get; private set; }

        #region IConfigurationSectionHandler Members

        /// <summary>
        /// Create the configuration section
        /// </summary>
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {

            this.MessageHandlers = new List<IMessageHandlerService>();

            // get the handler
            XmlElement addNodes = section.SelectSingleNode(".//*[local-name() = 'handlers']") as XmlElement;
            foreach(XmlElement nd in addNodes.ChildNodes)
                if (nd.Attributes["type"] != null)
                {
                    Type t = Type.GetType(nd.Attributes["type"].Value);
                    if (t != null)
                    {
                        ConstructorInfo ci = t.GetConstructor(Type.EmptyTypes);
                        if (ci != null)
                        {
                            this.MessageHandlers.Add(ci.Invoke(null) as IMessageHandlerService);
                            Trace.TraceInformation("Added message handler {0}", t.FullName);
                        }
                        else
                            Trace.TraceWarning("Can't find parameterless constructor on type {0}", t.FullName);
                    }
                    else
                        Trace.TraceWarning("Can't find type described by '{0}'", nd.Attributes["type"].Value);
                }

            return this;
        }

        #endregion
    }
}
