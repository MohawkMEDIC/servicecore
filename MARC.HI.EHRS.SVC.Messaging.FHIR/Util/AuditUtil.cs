using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.DataTypes;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;
using System.Net;
using System.ComponentModel;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Resources;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Util
{
    /// <summary>
    /// Auditing utility
    /// </summary>
    public static class AuditUtil
    {

        /// <summary>
        /// Create audit data
        /// </summary>
        public static AuditData CreateAuditData(IEnumerable<ResourceBase> records)
        {
            // Audit data
            AuditData retVal = null;

            AuditableObjectLifecycle lifecycle = AuditableObjectLifecycle.Access;

            // Get the actor information
            string userId = String.Empty;
            if (OperationContext.Current.Channel.RemoteAddress != null && OperationContext.Current.Channel.RemoteAddress.Uri != null)
                userId = OperationContext.Current.Channel.RemoteAddress.Uri.OriginalString;
            else if (OperationContext.Current.ServiceSecurityContext != null && OperationContext.Current.ServiceSecurityContext.PrimaryIdentity != null)
                userId = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;
            

            MessageProperties properties = OperationContext.Current.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string remoteEndpoint = "http://anonymous";
            if(endpoint != null)
                remoteEndpoint = endpoint.Address;

            switch (WebOperationContext.Current.IncomingRequest.Method)
            {
                case "GET":
                    {
                        retVal = new AuditData(DateTime.Now, ActionType.Read, OutcomeIndicator.Success, EventIdentifierType.Query, new CodeValue(
                            "GET", "urn:ietf:rfc:2616"));

                        // Audit actor for Patient Identity Source
                        retVal.Actors.Add(new AuditActorData()
                        {
                            UserIsRequestor = true,
                            UserIdentifier = userId,
                            ActorRoleCode = new List<CodeValue>() {
                            new  CodeValue("110153", "DCM") { DisplayName = "Source" }
                        },
                            NetworkAccessPointId = remoteEndpoint,
                            NetworkAccessPointType = NetworkAccessPointType.IPAddress,
                            UserName = userId
                        });
                        // Audit actor for FHIR service
                        retVal.Actors.Add(new AuditActorData()
                        {
                            UserIdentifier = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri.ToString(),
                            UserIsRequestor = false,
                            ActorRoleCode = new List<CodeValue>() { new CodeValue("110152", "DCM") { DisplayName = "Destination" } },
                            NetworkAccessPointType = NetworkAccessPointType.MachineName,
                            NetworkAccessPointId = Dns.GetHostName(),
                            UserName = Environment.UserName
                        });

                        // Serialize the query
                        retVal.AuditableObjects.Add(new AuditableObject()
                        {
                            Type = AuditableObjectType.SystemObject,
                            Role = AuditableObjectRole.Query,
                            IDTypeCode = AuditableObjectIdType.SearchCritereon,
                            QueryData = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.Query))
                        });

                        break;
                    }
                case "POST":
                    {
                        retVal = new AuditData(DateTime.Now, ActionType.Create, OutcomeIndicator.Success, EventIdentifierType.Import, new CodeValue(
                            "POST", "urn:ietf:rfc:2616"));

                        // Audit actor for Patient Identity Source
                        retVal.Actors.Add(new AuditActorData()
                        {
                            UserIsRequestor = true,
                            UserIdentifier = userId,
                            ActorRoleCode = new List<CodeValue>() {
                            new  CodeValue("110153", "DCM") { DisplayName = "Source" }
                        },
                            NetworkAccessPointId = remoteEndpoint,
                            NetworkAccessPointType = NetworkAccessPointType.IPAddress,
                            UserName = userId
                        });
                        // Audit actor for FHIR service
                        retVal.Actors.Add(new AuditActorData()
                        {
                            UserIdentifier = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri.ToString(),
                            UserIsRequestor = false,
                            ActorRoleCode = new List<CodeValue>() { new CodeValue("110152", "DCM") { DisplayName = "Destination" } },
                            NetworkAccessPointType = NetworkAccessPointType.MachineName,
                            NetworkAccessPointId = Dns.GetHostName(),
                            UserName = Environment.UserName
                        });

                        break;
                    }
                case "PUT":
                    {
                        retVal = new AuditData(DateTime.Now, ActionType.Update, OutcomeIndicator.Success, EventIdentifierType.Import, new CodeValue(
                            "POST", "urn:ietf:rfc:2616"));

                        // Audit actor for Patient Identity Source
                        retVal.Actors.Add(new AuditActorData()
                        {
                            UserIsRequestor = true,
                            UserIdentifier = userId,
                            ActorRoleCode = new List<CodeValue>() {
                            new  CodeValue("110153", "DCM") { DisplayName = "Source" }
                        },
                            NetworkAccessPointId = remoteEndpoint,
                            NetworkAccessPointType = NetworkAccessPointType.IPAddress,
                            UserName = userId
                        });
                        // Audit actor for FHIR service
                        retVal.Actors.Add(new AuditActorData()
                        {
                            UserIdentifier = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri.ToString(),
                            UserIsRequestor = false,
                            ActorRoleCode = new List<CodeValue>() { new CodeValue("110152", "DCM") { DisplayName = "Destination" } },
                            NetworkAccessPointType = NetworkAccessPointType.MachineName,
                            NetworkAccessPointId = Dns.GetHostName(),
                            UserName = Environment.UserName
                        });

                        break;
                    }
                default:
                    {
                        retVal = new AuditData(DateTime.Now, ActionType.Execute, OutcomeIndicator.Success, EventIdentifierType.ApplicationActivity, new CodeValue(
                            String.Format("GET {0}", WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.OriginalString), "http://marc-hi.ca/fhir/actions"));

                        break;
                    }
            }


            if(records != null)
                foreach (ResourceBase pat in records)
                {
                    // TODO: Make this more generic
                    AuditableObject aud = null;

                    var ptcptObjMap = pat.GetType().GetCustomAttributes(typeof(ParticipantObjectMapAttribute), true);
                    string domain = String.Empty;
                    if(ptcptObjMap.Length > 0)
                    {
                        var mapAttribute = ptcptObjMap[0] as ParticipantObjectMapAttribute;
                        domain = ApplicationContext.ConfigurationService.OidRegistrar.GetOid(mapAttribute.OidName).Oid;
                        aud = new AuditableObject() {
                            IDTypeCode = mapAttribute.IdType,
                            Role = mapAttribute.Role,
                            Type = mapAttribute.Type
                        };
                    }
                    else
                        continue;

                    // Lifecycle
                    switch (retVal.ActionCode.Value)
                    {
                        case ActionType.Create:
                            aud.LifecycleType = AuditableObjectLifecycle.Creation;
                            break;
                        case ActionType.Delete:
                            aud.LifecycleType = AuditableObjectLifecycle.LogicalDeletion;
                            break;
                        case ActionType.Execute:
                            aud.LifecycleType = AuditableObjectLifecycle.Access;
                            break;
                        case ActionType.Read:
                            aud.LifecycleType = AuditableObjectLifecycle.Disclosure;
                            break;
                        case ActionType.Update:
                            aud.LifecycleType = AuditableObjectLifecycle.Amendment;
                            break;
                    }

                    aud.ObjectId = String.Format("{1}^^^&{0}&ISO", domain, pat.Id);
                    retVal.AuditableObjects.Add(aud);

                }
            return retVal;
        }

    }
}
