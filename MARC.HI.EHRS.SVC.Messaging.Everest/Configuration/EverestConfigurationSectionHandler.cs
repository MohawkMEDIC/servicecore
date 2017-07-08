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
 * Date: 17-10-2012
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Diagnostics;
using System.Reflection;

namespace MARC.HI.EHRS.SVC.Messaging.Everest.Configuration
{
    /// <summary>
    /// Configuration section handler for Everest
    /// </summary>
    public class EverestConfigurationSectionHandler : IConfigurationSectionHandler
    {
        #region IConfigurationSectionHandler Members

        /// <summary>
        /// Gets a list of its configuration data
        /// </summary>
        public List<RevisionConfiguration> Revisions { get; private set; }

        /// <summary>
        /// If true, the file is in config mode
        /// </summary>
        public bool ConfigMode { get; set; }

        /// <summary>
        /// Create the configuration section handler
        /// </summary>
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            this.Revisions = new List<RevisionConfiguration>();

            // Get its sections
            XmlNodeList itsSectionList = section.SelectNodes("./*[local-name() = 'revision']");
            foreach (XmlNode revSection in itsSectionList)
            {
                RevisionConfiguration revConfig = new RevisionConfiguration();
                revConfig.Assembly = null;
                // Load formatter, etc...
                if(revSection.Attributes["formatter"] != null)
                    revConfig.Formatter = Type.GetType(revSection.Attributes["formatter"].Value);
                if(revSection.Attributes["aide"] != null)
                    revConfig.GraphAide = Type.GetType(revSection.Attributes["aide"].Value);
                if (revSection.Attributes["assembly"] != null)
                    revConfig.Assembly = Assembly.Load(new AssemblyName(revSection.Attributes["assembly"].Value));
                if (revSection.Attributes["name"] != null)
                    revConfig.Name = revSection.Attributes["name"].Value;
                if (revSection.Attributes["validate"] != null)
                    revConfig.ValidateInstances = Convert.ToBoolean(revSection.Attributes["validate"].Value);
                if (revSection.Attributes["messageIdFormat"] != null)
                    revConfig.MessageIdentifierFormat = revSection.Attributes["messageIdFormat"].Value;
                else
                    revConfig.MessageIdentifierFormat = "{0}";
                
                // Sanity check
                if (revConfig.Formatter == null)
                {
                    Trace.TraceError("{0}, 'revision' element will be ignored", ErrorDescriptions.ERR_MISSING_FMTR_ATTRIBUTE);
                    continue;
                }
                else if (revConfig.Assembly == null)
                {
                    Trace.TraceError("{0}, 'revision' element will be ignored", ErrorDescriptions.ERR_MISSING_ASM_ATTRIBUTE);
                    continue;
                }

                // Load the listener configs
                foreach(XmlNode listenSection in revSection.SelectNodes("./*[local-name() = 'listen']"))
                {
                    ListenConfiguration listenConfig = new ListenConfiguration();
                    if (listenSection.Attributes["type"] != null)
                        listenConfig.ConnectorType = Type.GetType(listenSection.Attributes["type"].Value);
                    if (listenSection.Attributes["connectionString"] != null)
                        listenConfig.ConnectionString = listenSection.Attributes["connectionString"].Value;
                    if (listenSection.Attributes["mode"] != null)
                        listenConfig.Mode = listenSection.Attributes["mode"].Value == "listWaitRespond" ? ListenConfiguration.ModeType.ListenWaitRespond : ListenConfiguration.ModeType.ListenWait;
                    else
                        listenConfig.Mode = ListenConfiguration.ModeType.ListenWait;

                    // Sanity check
                    if (listenConfig.ConnectorType == null)
                    {
                        Trace.TraceError("{0}, 'listen' element will be ignored", ErrorDescriptions.ERR_MISSING_TYPE_ATTRIBUTE);
                        continue;
                    }
                    revConfig.Listeners.Add(listenConfig);
                }

                // Now load the type cache to build
                foreach (XmlNode cacheSection in revSection.SelectNodes("./*[local-name() = 'cacheTypes']"))
                {
                    if (cacheSection.Attributes["namespace"] == null) // sanity checl
                    {
                        Trace.TraceError("{0}, 'cacheTypes' element in {1} will be ignored", ErrorDescriptions.ERR_MISSING_NS_ATTRIBUTE, revConfig.Formatter.FullName);
                        continue;
                    }

                    // Setup cache
                    string ns = cacheSection.Attributes["namespace"].Value;

                    // Sanity check
                    foreach (XmlNode addCache in cacheSection.SelectNodes("./*[local-name() = 'add']/@name"))
                    {
                        Type getType = revConfig.Assembly.GetType(String.Format("{0}.{1}", ns, addCache.Value));
                        if (getType == null)
                            Trace.TraceWarning(ErrorDescriptions.ERR_CANT_FIND_TYPE, ns, addCache.Value);
                        else
                            revConfig.CacheTypes.Add(getType);
                    }
                }

                // Trigger handlers
                foreach (XmlNode handlerSection in revSection.SelectNodes("./*[local-name() = 'handler']"))
                {
                    MessageHandlerConfiguration handlerConfig = new MessageHandlerConfiguration();
                    Type handlerType = null;

                    if (handlerSection.Attributes["type"] != null)
                        handlerType = Type.GetType(handlerSection.Attributes["type"].Value);
                    
                    // check type was proper
                    if(handlerType == null)
                    {
                        Trace.TraceError("{0}, 'handler' element will be ignored", ErrorDescriptions.ERR_MISSING_TYPE_ATTRIBUTE);
                        continue;
                    }

                    // get constructor
                    ConstructorInfo ci = handlerType.GetConstructor(Type.EmptyTypes);
                    if(!ConfigMode && ci == null)
                    {
                        Trace.TraceError(ErrorDescriptions.ERR_CANT_FIND_CTOR, handlerType);
                        continue;
                    }
                    
                    // Create the handler
                    handlerConfig.Handler = ci.Invoke(null) as IEverestMessageReceiver;

                    if (handlerConfig.Handler == null)
                    {
                        Trace.TraceError(ErrorDescriptions.ERR_INVALID_INTERFACE, handlerType, typeof(IEverestMessageReceiver));
                        continue;
                    }

                    // Get trigger events
                    foreach (XmlNode triggerEvent in handlerSection.SelectNodes("./*[local-name() = 'interactionId']"))
                    {
                        InteractionConfiguration config = new InteractionConfiguration();
                        if (triggerEvent.Attributes["name"] == null)
                            throw new ConfigurationErrorsException("Interaction is missing name attribute");
                        else
                            config.Id = triggerEvent.Attributes["name"].Value;

                        if (triggerEvent.Attributes["disclosure"] != null)
                            config.Disclosure = Boolean.Parse(triggerEvent.Attributes["disclosure"].Value);
                        // Load the headers
                        var responseHeaderNode = triggerEvent.SelectSingleNode("./*[local-name() = 'responseHeaders']");
                        if (responseHeaderNode != null)
                            config.ResponseHeaders = responseHeaderNode.ChildNodes;
                        handlerConfig.Interactions.Add(config);
                    }

                    // Add the handler config
                    revConfig.MessageHandlers.Add(handlerConfig);
                }

                this.Revisions.Add(revConfig);
            }

            return this;
        }

        #endregion
    }
}
