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
 * Date: 9-7-2012
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.ClientIdentity.pCS.Configuration;
using System.Configuration;
using MARC.Everest.Connectors.WCF;
using MARC.Everest.Formatters.XML.ITS1;
using MARC.Everest.Formatters.XML.Datatypes.R1;
using MARC.Everest.RMIM.CA.R020402.Interactions;
using MARC.HI.EHRS.SVC.Core.Services;
using MARC.Everest.RMIM.CA.R020402.Vocabulary;
using MARC.Everest.DataTypes;
using MARC.Everest.Exceptions;
using System.Diagnostics;
using MARC.HI.EHRS.SVC.Core.DataTypes;
using MARC.HI.EHRS.SHR.Messaging;
using MARC.Everest.Connectors;
using MARC.HI.EHRS.SVC.Core.ComponentModel.Components;

namespace MARC.HI.EHRS.SVC.ClientIdentity.pCS
{
    /// <summary>
    /// Implementation of the IClientIdentityService that provides identity
    /// services for pCS v3
    /// </summary>
    public class CanadianClientIdentityProvider : IClientIdentityService
    {

       
        // Configuration for the object
        private static ConfigurationSectionHandler s_configuration = null;

        /// <summary>
        /// Static constructor for the 
        /// </summary>
        static CanadianClientIdentityProvider()
        {
            s_configuration = ConfigurationManager.GetSection("marc.hi.ehrs.svc.clientid.pcs") as ConfigurationSectionHandler;
        }

        #region IClientIdentityService Members

        /// <summary>
        /// Find a client by identifier
        /// </summary>
        public Core.ComponentModel.Components.Client FindClient(Core.DataTypes.DomainIdentifier identifier)
        {

            // System Configuration 
            ISystemConfigurationService config = this.Context.GetService(typeof(ISystemConfigurationService)) as ISystemConfigurationService;
            ILocalizationService locale = this.Context.GetService(typeof(ILocalizationService)) as ILocalizationService;

            // Find the client in the registry system
            WcfClientConnector client = new WcfClientConnector(s_configuration.WcfConnectionString);
            client.Formatter = new XmlIts1Formatter();
            client.Formatter.GraphAides.Add(new DatatypeFormatter());

            // Construct the request
            PRPA_IN101101CA request = new PRPA_IN101101CA(
                Guid.NewGuid(),
                DateTime.Now,
                ResponseMode.Immediate,
                PRPA_IN101101CA.GetInteractionId(),
                PRPA_IN101001CA.GetProfileId(),
                ProcessingID.Production,
                AcknowledgementCondition.Always,
                new Everest.RMIM.CA.R020402.MCCI_MT002200CA.Receiver(
                    new TEL() { NullFlavor = NullFlavor.NoInformation },
                    new Everest.RMIM.CA.R020402.MCCI_MT002200CA.Device2(
                        new II(s_configuration.DeviceId)
                    )
                ),
                new Everest.RMIM.CA.R020402.MCCI_MT002200CA.Sender(
                    new TEL() { NullFlavor = NullFlavor.NoInformation },
                    new Everest.RMIM.CA.R020402.MCCI_MT002200CA.Device1(
                        new II(config.DeviceIdentifier, config.DeviceName)
                    )
                ),
                new Everest.RMIM.CA.R020402.MFMI_MT700751CA.ControlActEvent<Everest.RMIM.CA.R020402.PRPA_MT101101CA.ParameterList>(
                    Guid.NewGuid(),
                    PRPA_IN101103CA.GetTriggerEvent(),
                    new Everest.RMIM.CA.R020402.MFMI_MT700711CA.Author(
                        DateTime.Now
                    ),
                    new Everest.RMIM.CA.R020402.QUQI_MT120008CA.QueryByParameter<Everest.RMIM.CA.R020402.PRPA_MT101101CA.ParameterList>(
                        Guid.NewGuid(),
                        ResponseModality.RealTime,
                        1,
                        QueryRequestLimit.Record,
                        new Everest.RMIM.CA.R020402.PRPA_MT101101CA.ParameterList()
                    )
                ));

            request.controlActEvent.Author.SetAuthorPerson(new Everest.RMIM.CA.R020402.MCAI_MT700210CA.AuthorRole());
            request.controlActEvent.QueryByParameter.parameterList.ClientIDBus = new Everest.RMIM.CA.R020402.PRPA_MT101101CA.ClientIDBus(
                new II(identifier.Domain, identifier.Identifier)
            );

             
            // Send message
            try
            {
                client.Open();
                var sendResult = client.Send(request);
                if (sendResult.Code != Everest.Connectors.ResultCode.Accepted &&
                    sendResult.Code != Everest.Connectors.ResultCode.AcceptedNonConformant)
                {
                    foreach (var dtls in sendResult.Details)
                        Trace.TraceError("pCS Client Registry : {0} : {1}", dtls.Type, dtls.Message);
                    throw new ConnectorException("Could not send client registry request", ConnectorException.ReasonType.MessageError);
                }
                var receiveResult = client.Receive(sendResult);

                var result = receiveResult.Structure as PRPA_IN101102CA;
                // Get the result
                if (result == null ||
                    result.Acknowledgement.TypeCode != AcknowledgementType.ApplicationAcknowledgementAccept ||
                    result.controlActEvent == null ||
                    result.controlActEvent.Subject == null ||
                    result.controlActEvent.QueryAck.QueryResponseCode != QueryResponse.DataFound)
                    return null;
                else
                {
                    var clnt = result.controlActEvent.Subject[0].RegistrationEvent.Subject.registeredRole;
                    ComponentUtil cu = new ComponentUtil() { Context = this.Context };

                    // Create the return value
                    var retVal = new Core.ComponentModel.Components.Client()
                    {
                        AlternateIdentifiers = cu.CreateDomainIdentifierList(clnt.Id),
                        TelecomAddresses = new List<TelecommunicationsAddress>(),
                        BirthTime = cu.CreateTimestamp(new IVL<TS>(clnt.IdentifiedPerson.BirthTime), new List<Everest.Connectors.IResultDetail>()).Parts[0],
                        GenderCode = Util.ToWireFormat(clnt.IdentifiedPerson.AdministrativeGenderCode),
                        LegalName = cu.CreateNameSet(clnt.IdentifiedPerson.Name.Find(o => o.Use.Contains(EntityNameUse.Legal)) ?? clnt.IdentifiedPerson.Name[0], new List<IResultDetail>()),
                        PerminantAddress = cu.CreateAddressSet(clnt.IdentifiedPerson.Addr.Find(o => o.Use.Contains(PostalAddressUse.PrimaryHome)) ?? clnt.IdentifiedPerson.Addr[0], new List<IResultDetail>())
                    };

                    // Telecom addresses
                    foreach (var tel in clnt.IdentifiedPerson.Telecom)
                        retVal.TelecomAddresses.Add(new TelecommunicationsAddress()
                        {
                            Use = Util.ToWireFormat(tel.Use),
                            Value = tel.Value
                        });

                    return retVal;
                }
            }
            finally
            {
                client.Dispose();
            }

        }

        /// <summary>
        /// Find a client based on the supplied parameters
        /// </summary>
        public Core.ComponentModel.Components.Client[] FindClient(Core.DataTypes.NameSet name, string genderCode, Core.DataTypes.TimestampPart birthTime)
        {
            // System Configuration 
            ISystemConfigurationService config = this.Context.GetService(typeof(ISystemConfigurationService)) as ISystemConfigurationService;
            ILocalizationService locale = this.Context.GetService(typeof(ILocalizationService)) as ILocalizationService;

            // Find the client in the registry system
            WcfClientConnector client = new WcfClientConnector(s_configuration.WcfConnectionString);
            client.Formatter = new XmlIts1Formatter();
            client.Formatter.GraphAides.Add(new DatatypeFormatter());

            // Construct the request
            PRPA_IN101103CA request = new PRPA_IN101103CA(
                Guid.NewGuid(),
                DateTime.Now,
                ResponseMode.Immediate,
                PRPA_IN101103CA.GetInteractionId(),
                PRPA_IN101103CA.GetProfileId(),
                ProcessingID.Production,
                AcknowledgementCondition.Always,
                new Everest.RMIM.CA.R020402.MCCI_MT002200CA.Receiver(
                    new TEL() { NullFlavor = NullFlavor.NoInformation },
                    new Everest.RMIM.CA.R020402.MCCI_MT002200CA.Device2(
                        new II(s_configuration.DeviceId)
                    )
                ),
                new Everest.RMIM.CA.R020402.MCCI_MT002200CA.Sender(
                    new TEL() { NullFlavor = NullFlavor.NoInformation },
                    new Everest.RMIM.CA.R020402.MCCI_MT002200CA.Device1(
                        new II(config.DeviceIdentifier, config.DeviceName)
                    )
                ),
                new Everest.RMIM.CA.R020402.MFMI_MT700751CA.ControlActEvent<Everest.RMIM.CA.R020402.PRPA_MT101103CA.ParameterList>(
                    Guid.NewGuid(),
                    PRPA_IN101103CA.GetTriggerEvent(),
                    new Everest.RMIM.CA.R020402.MFMI_MT700711CA.Author(
                        DateTime.Now
                    ),
                    new Everest.RMIM.CA.R020402.QUQI_MT120008CA.QueryByParameter<Everest.RMIM.CA.R020402.PRPA_MT101103CA.ParameterList>(
                        Guid.NewGuid(),
                        ResponseModality.RealTime,
                        1,
                        QueryRequestLimit.Record,
                        new Everest.RMIM.CA.R020402.PRPA_MT101103CA.ParameterList()
                    )
                ));

            request.controlActEvent.Author.SetAuthorPerson(new Everest.RMIM.CA.R020402.MCAI_MT700210CA.AuthorRole());

            // Set the query parameters
            
            // Send message
            try
            {
                client.Open();
                var sendResult = client.Send(request);
                if (sendResult.Code != Everest.Connectors.ResultCode.Accepted &&
                    sendResult.Code != Everest.Connectors.ResultCode.AcceptedNonConformant)
                {
                    foreach (var dtls in sendResult.Details)
                        Trace.TraceError("pCS Client Registry : {0} : {1}", dtls.Type, dtls.Message);
                    throw new ConnectorException("Could not send client registry request", ConnectorException.ReasonType.MessageError);
                }
                var receiveResult = client.Receive(sendResult);

                var result = receiveResult.Structure as PRPA_IN101104CA;
                // Get the result
                if (result == null ||
                    result.Acknowledgement.TypeCode != AcknowledgementType.ApplicationAcknowledgementAccept ||
                    result.controlActEvent == null ||
                    result.controlActEvent.Subject == null ||
                    result.controlActEvent.QueryAck.QueryResponseCode != QueryResponse.DataFound)
                    return null;
                else
                {
                    List<Client> retVal = new List<Client>();

                    foreach (var subj in result.controlActEvent.Subject)
                    {
                        var clnt = subj.RegistrationEvent.Subject.registeredRole;
                        ComponentUtil cu = new ComponentUtil() { Context = this.Context };

                        // Create the return value
                        var tmpClient = new Core.ComponentModel.Components.Client()
                        {
                            AlternateIdentifiers = cu.CreateDomainIdentifierList(clnt.Id),
                            TelecomAddresses = new List<TelecommunicationsAddress>(),
                            BirthTime = cu.CreateTimestamp(new IVL<TS>(clnt.IdentifiedPerson.BirthTime), new List<Everest.Connectors.IResultDetail>()).Parts[0],
                            GenderCode = Util.ToWireFormat(clnt.IdentifiedPerson.AdministrativeGenderCode),
                            LegalName = cu.CreateNameSet(clnt.IdentifiedPerson.Name.Find(o => o.Use.Contains(EntityNameUse.Legal)) ?? clnt.IdentifiedPerson.Name[0], new List<IResultDetail>()),
                            PerminantAddress = cu.CreateAddressSet(clnt.IdentifiedPerson.Addr.Find(o => o.Use.Contains(PostalAddressUse.PrimaryHome)) ?? clnt.IdentifiedPerson.Addr[0], new List<IResultDetail>())
                        };

                        // Telecom addresses
                        foreach (var tel in clnt.IdentifiedPerson.Telecom)
                            tmpClient.TelecomAddresses.Add(new TelecommunicationsAddress()
                            {
                                Use = Util.ToWireFormat(tel.Use),
                                Value = tel.Value
                            });

                        retVal.Add(tmpClient);
                    }
                    return retVal.ToArray();
                }
            }
            finally
            {
                client.Dispose();
            }

        }

        #endregion

        #region IUsesHostContext Members

        /// <summary>
        /// Gets or sets the context of the provider
        /// </summary>
        public IServiceProvider Context { get; set; }
        #endregion
    }
}
