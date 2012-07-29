using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.Everest.DataTypes;
using System.IO;
using System.Xml;
using System.ComponentModel;
using MARC.HI.EHRS.SVC.Messaging.Everest.Configuration;
using System.Configuration;
using System.Reflection;
using System.Diagnostics;
using MARC.Everest.Connectors;
using MARC.Everest.Exceptions;
using MARC.HI.EHRS.SVC.Core.Services;
using MARC.HI.EHRS.SVC.Core;
using MARC.Everest.Interfaces;
using MARC.HI.EHRS.SVC.Messaging.Everest.Exception;
using MARC.HI.EHRS.SVC.Core.DataTypes;
using System.Runtime.InteropServices;

namespace MARC.HI.EHRS.SVC.Messaging.Everest
{
    /// <summary>
    /// Handles messages for HL7v3
    /// </summary>
    [Description("Everest Based Message Handler Service")]
    public class MessageHandler : IMessageHandlerService
    {

        /// <summary>
        /// Active connectors
        /// </summary>
        private List<IListenWaitConnector> m_activeConnectors = new List<IListenWaitConnector>();

        /// <summary>
        /// The configuraiton for the everest message handler
        /// </summary>
        private EverestConfigurationSectionHandler m_configuration;

        /// <summary>
        /// Create a new instance of the MessageHandler
        /// </summary>
        public MessageHandler()
        {
            m_configuration = ConfigurationManager.GetSection("marc.hi.ehrs.svc.messaging.everest") as EverestConfigurationSectionHandler;
        }

        #region IMessageHandlerService Members

        public bool Start()
        {
            
            // Start each of the listeners for each of the ITS(es)
            foreach (var itsConfig in m_configuration.Revisions)
            {
                ConstructorInfo ciFormatter = itsConfig.Formatter.GetConstructor(Type.EmptyTypes);
                if (ciFormatter == null)
                {
                    Trace.TraceError("Cannot create listener for {0} as the formatter has no default constructor", itsConfig.Formatter.FullName);
                    continue;
                }

                // Now we want to create the formatter
                IStructureFormatter formatter = ciFormatter.Invoke(null) as IStructureFormatter;
                if (itsConfig.GraphAide != null)
                {
                    ciFormatter = itsConfig.GraphAide.GetConstructor(Type.EmptyTypes);
                    if (ciFormatter == null)
                    {
                        Trace.TraceError("Cannot create listener for {0} as the formatter has no default constructor", itsConfig.GraphAide.FullName);
                        continue;
                    }

                    formatter.GraphAides.Add(ciFormatter.Invoke(null) as IStructureFormatter);
                }

                // Build the type cache
                if (formatter is ICodeDomStructureFormatter && itsConfig.CacheTypes.Count > 0)
                {
                    Trace.TraceInformation("Creating type cache for {0} ({1} types)", itsConfig.Formatter, itsConfig.CacheTypes.Count);
                    (formatter as ICodeDomStructureFormatter).BuildCache(itsConfig.CacheTypes.ToArray());
                    Trace.TraceInformation("{0} CLR types created", (formatter as ICodeDomStructureFormatter).GeneratedAssemblies[0].GetTypes().Length);
                }
                if (formatter is IValidatingStructureFormatter)
                    (formatter as IValidatingStructureFormatter).ValidateConformance = itsConfig.ValidateInstances;

                // Message handlers
                foreach(var mh in itsConfig.MessageHandlers)
                    mh.Handler.Context = this.Context;

                // Iterate through listeners and start em up
                foreach (var listenerConfig in itsConfig.Listeners)
                {

                    ConstructorInfo ciListener = listenerConfig.ConnectorType.GetConstructor(Type.EmptyTypes);
                    if (ciListener == null) // sanity checks
                    {
                        Trace.TraceError("Cannot create listener for {0} as the connector {1} has no default constructor", itsConfig.Formatter.FullName, listenerConfig.ConnectorType.FullName);
                        continue;
                    }
                    else if (listenerConfig.ConnectorType.GetInterface(typeof(IFormattedConnector).FullName) == null)
                    {
                        Trace.TraceError("Cannot append formatter {0} to connector {1} as the connector doesn't implement IFormattedConnector", itsConfig.Formatter.FullName, listenerConfig.ConnectorType.FullName);
                        continue;
                    }
                    else if (listenerConfig.ConnectorType.GetInterface(typeof(IListenWaitConnector).FullName) == null)
                    {
                        Trace.TraceError("Cannot append formatter {0} to connector {1} as the connector doesn't implement IListenWaitConnector", itsConfig.Formatter.FullName, listenerConfig.ConnectorType.FullName);
                        continue;
                    }

                    // Now create the connector
                    IListenWaitConnector connector = ciListener.Invoke(null) as IListenWaitConnector;
                    
                    try
                    {
                        // Set the connector up
                        (connector as IFormattedConnector).Formatter = formatter;
                        connector.ConnectionString = listenerConfig.ConnectionString;
                        connector.MessageAvailable += new EventHandler<UnsolicitedDataEventArgs>(connector_MessageAvailable);

                        Trace.TraceInformation("Starting listener {0}", connector.ConnectionString);
                        connector.Open();
                        connector.Start();
                        m_activeConnectors.Add(connector);
                    }
                    catch (ConnectorException e)
                    {
                        Trace.TraceError("Could not start listener {0}", connector.ConnectionString);
                        Trace.Indent();
                        Trace.WriteLine(e.Message);
                        Trace.Indent();
                        foreach (var detail in e.Details ?? new IResultDetail[0])
                            Trace.WriteLine(String.Format("{0}: {1}", detail.Type, detail.Message));
                        Trace.Unindent();
                        Trace.Unindent();
                    }
                    catch (System.Exception e)
                    {
                        Trace.TraceError("Could not start listener {0}", connector.ConnectionString);
                        Trace.Indent();
                        Trace.WriteLine(e.ToString());
                        Trace.Unindent();
                    }
                }
                

            }

            return m_activeConnectors.Count > 0;
        }

        /// <summary>
        /// Write the specified message to the memory stream
        /// </summary>
        /// <param name="conn">The connector that received the message</param>
        /// <param name="ms">The memory stream</param>
        /// <param name="msg">The message</param>
        void WriteMessageToStream(IFormattedConnector conn, IGraphable msg, MemoryStream ms)
        {
            //var fmtr = conn.Formatter.Clone() as IStructureFormatter;
            conn.Formatter.GraphObject(ms, msg);
            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
        }

        /// <summary>
        /// Fired whenever a message is available for processing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void connector_MessageAvailable(object sender, UnsolicitedDataEventArgs e)
        {
            // Audit service
            IAuditorService auditService = Context.GetService(typeof(IAuditorService)) as IAuditorService;

            #region Setup Audit
            AuditData audit = new AuditData(
                    DateTime.Now, ActionType.Execute,
                    OutcomeIndicator.Success,
                    EventIdentifierType.ApplicationActivity,
                    new CodeValue("GEN")
                );
            audit.Actors.Add(new AuditActorData()
            {
                NetworkAccessPointId = Environment.MachineName,
                NetworkAccessPointType = NetworkAccessPointType.MachineName
            });
            audit.Actors.Add(new AuditActorData()
            {
                UserIsRequestor = true,
                NetworkAccessPointId = e.SolicitorEndpoint.ToString(),
                NetworkAccessPointType = NetworkAccessPointType.IPAddress
            });
            audit.AuditableObjects.Add(new AuditableObject()
            {
                IDTypeCode = AuditableObjectIdType.Uri,
                LifecycleType = AuditableObjectLifecycle.Access,
                Role = AuditableObjectRole.DataRepository,
                ObjectId = e.ReceiveEndpoint.ToString(),
                Type = AuditableObjectType.SystemObject
            });
            #endregion

            try
            {
                // Find a receiver capable of processing this
                IReceiveResult rcvResult = (sender as IListenWaitConnector).Receive();

                // get the persistence service from the context
                IMessagePersistenceService persistenceService = Context.GetService(typeof(IMessagePersistenceService)) as IMessagePersistenceService;

                // Were we able to process the message
                Assembly messageTypeAssembly = null;
                if (rcvResult.Structure != null)
                    messageTypeAssembly = rcvResult.Structure.GetType().Assembly;

                // Find the configuration section that handles the specified revision
                var curRevision = m_configuration.Revisions.Find(o => o.Assembly.FullName.Equals(messageTypeAssembly.FullName));
                if (curRevision == null)
                {
                    Trace.TraceError("This service does not seem to have support for the version of message being used");
                    throw new UninterpretableMessageException("This service doesn't support this standard", rcvResult);
                }

                // Do we have a handler for this message interaction? Cast as an interaction
                // and attempt to find the handler configuration
                IInteraction interactionStructure = rcvResult.Structure as IInteraction;
                MessageHandlerConfiguration receiverConfig = null;
                if (interactionStructure != null && interactionStructure.InteractionId != null &&
                    !String.IsNullOrEmpty(interactionStructure.InteractionId.Extension))
                    receiverConfig = curRevision.MessageHandlers.Find(o => o.Interactions.Contains(interactionStructure.InteractionId.Extension));

                IEverestMessageReceiver currentHandler = null, defaultHandler = null;
                
                var defaultHandlerConfig = curRevision.MessageHandlers.Find(o => o.Interactions.Contains("*"));
                receiverConfig = receiverConfig ?? defaultHandlerConfig; // find a handler

                // Receiver configuration
                if (receiverConfig == null)
                    throw new InvalidOperationException("Cannot find appropriate handler this message");
                else
                {
                    
                    var messageState = MessageState.New;
                    IInteraction response = null;

                    // check with persistence
                    if (persistenceService != null)
                    {
                        messageState = persistenceService.GetMessageState(String.Format(curRevision.MessageIdentifierFormat, interactionStructure.Id.Root, interactionStructure.Id.Extension));
                    }

                    switch (messageState)
                    {
                        case MessageState.New:

                            // Persist the message 
                            if (persistenceService != null)
                            {
                                MemoryStream ms = new MemoryStream();
                                try
                                {
                                    WriteMessageToStream(sender as IFormattedConnector, interactionStructure, ms);
                                    persistenceService.PersistMessage(String.Format(curRevision.MessageIdentifierFormat, interactionStructure.Id.Root, interactionStructure.Id.Extension), ms);
                                }
                                finally
                                {
                                    ms.Dispose();
                                }
                            }
                                
                            currentHandler = receiverConfig.Handler;
                            defaultHandler = defaultHandlerConfig.Handler;
                            response = currentHandler.HandleMessageReceived(sender, e, rcvResult) as IInteraction;
                            if (persistenceService != null)
                            {
                                MemoryStream ms = new MemoryStream();
                                try
                                {
                                    WriteMessageToStream(sender as IFormattedConnector, response, ms);
                                    persistenceService.PersistResultMessage(String.Format(curRevision.MessageIdentifierFormat, response.Id.Root, response.Id.Extension), String.Format(curRevision.MessageIdentifierFormat, interactionStructure.Id.Root, interactionStructure.Id.Extension), ms);
                                }
                                finally
                                {
                                    ms.Dispose();
                                }
                            }

                            break;
                        case MessageState.Complete:
                            var rms = persistenceService.GetMessageResponseMessage(String.Format(curRevision.MessageIdentifierFormat, interactionStructure.Id.Root, interactionStructure.Id.Extension));
                            response = (sender as IFormattedConnector).Formatter.ParseObject(rms) as IInteraction;
                            break;
                        case MessageState.Active:
                            throw new ApplicationException("Message is already being processed");
                    }
                    // Send back
                    IListenWaitRespondConnector ilwConnector = sender as IListenWaitRespondConnector;
                    if (ilwConnector == null) // no need to send something back
                    {
                        auditService.SendAudit(audit);
                        return;
                    }
                    else
                    {

                        // Invalid message delegate
                        var invalidMessageDelegate = new EventHandler<MessageEventArgs>(delegate(object sndr, MessageEventArgs mea)
                            {
                                audit.Outcome = OutcomeIndicator.MinorFail;
                                InvalidMessageResult res = new InvalidMessageResult()
                                {
                                    Code = mea.Code,
                                    Details = mea.Details,
                                    Structure = rcvResult.Structure
                                };
                                if (defaultHandler != null)
                                    mea.Alternate = response;
                                Trace.TraceWarning("Returning a default message because Everest was unable to serialize the response correctly. Error was {0}", mea.Code);
                                Trace.Indent();
                                foreach (IResultDetail dtl in mea.Details)
                                    Trace.TraceWarning("{0} : {1} : {2}", dtl.Type, dtl.Message, dtl.Location);
                                Trace.Unindent();

                            });
                        ilwConnector.InvalidResponse += invalidMessageDelegate;

                        try
                        {
                            ISendResult sndResult = ilwConnector.Send(response, rcvResult);
                            if (sndResult.Code != ResultCode.Accepted &&
                                sndResult.Code != ResultCode.AcceptedNonConformant)
                            {
                                Trace.TraceError("Cannot send response back to the solicitor : {0}", sndResult.Code);
                                Trace.Indent();
                                foreach (IResultDetail dtl in sndResult.Details ?? new IResultDetail[0])
                                    Trace.TraceError("{0}: {1} : {2}", dtl.Type, dtl.Message, dtl.Location);
                                Trace.Unindent();
                            }
                        }
                        finally
                        {
                            if(auditService != null)
                                auditService.SendAudit(audit);
                            // Remove the invalid message delegate
                            ilwConnector.InvalidResponse -= invalidMessageDelegate;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {

                #region Audit Failure
                audit.Outcome = OutcomeIndicator.EpicFail;

                if(auditService != null)
                    auditService.SendAudit(audit);
                #endregion

                Trace.TraceError(ex.ToString());
                throw;
            }
        }

        public bool Stop()
        {
            foreach (var connector in m_activeConnectors)
            {
                Trace.TraceInformation("Stopping listener {0}", connector.ConnectionString);
                connector.Close();
            }

            return true;
        }

        #endregion

        #region IUsesHostContext Members

        public HostContext Context
        {
            get;
            set; 
        }

        #endregion
    }
}
