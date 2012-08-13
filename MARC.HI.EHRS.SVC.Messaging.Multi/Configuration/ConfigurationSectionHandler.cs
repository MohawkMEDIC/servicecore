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
