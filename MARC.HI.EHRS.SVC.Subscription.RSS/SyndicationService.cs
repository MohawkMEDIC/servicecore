/**
 * Copyright 2013-2013 Mohawk College of Applied Arts and Technology
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
 * Date: 12-3-2013
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using MARC.HI.EHRS.SVC.Subscription.Data.Messaging;
using System.Security.Cryptography;
using System.IO;
using MARC.HI.EHRS.SVC.Core.Issues;
using MARC.HI.EHRS.SVC.Core.Services;
using MARC.HI.EHRS.SVC.HealthWorkerIdentity;
using System.Data;
using MARC.HI.EHRS.SVC.Subscription.Core.Services;
using MARC.HI.EHRS.SVC.Core.DataTypes;
using MARC.HI.EHRS.SVC.Core.ComponentModel.Components;
using MARC.HI.EHRS.SVC.Subscription.Data.Configuration;
using System.Configuration;
using System.ServiceModel.Syndication;
using System.Reflection;
using MARC.HI.EHRS.SVC.Subscription.Core;
using System.Xml.Linq;
using System.ServiceModel.Channels;
using System.Xml.Serialization;
using System.Diagnostics;

namespace MARC.HI.EHRS.SVC.Subscription.Data
{
    /// <summary>
    /// Syndication service
    /// </summary>
    /// <remarks>Most of this code was written after several beers. It is for reference only and really shouldn't be considered the final solution</remarks>
    [ServiceBehavior]
    public class SyndicationService : ISyndicationContract
    {

        // Load config
        internal static ConfigurationSectionHandler s_configuration;

        /// <summary>
        /// Get database key
        /// </summary>
        /// TODO: This is really insecure but I'm just trying to get RSS working
        private Guid GetDatabaseKey(Guid id, string pin)
        {
            // The PIN becomes the salt
            string hash = id.ToString() + pin;
            MD5Cng md5 = new MD5Cng();
            byte[] b = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(hash));

            // Now parse the hash back into a guid
            return new Guid(b);

        }

        #region ISyndicationContract Members

        /// <summary>
        /// Get the records created since subscription was formed
        /// </summary>
        public System.ServiceModel.Syndication.Atom10FeedFormatter GetSubscription(string id, string pin)
        {

            // Localization service
            ILocalizationService localeService = ApplicationContext.CurrentContext.GetService(typeof(ILocalizationService)) as ILocalizationService;

            if (String.IsNullOrEmpty(id))
                throw new ArgumentNullException("id");
            else if (String.IsNullOrEmpty(pin))
                throw new ArgumentNullException("pin");

            if(s_configuration == null)
                lock(s_configuration)
                    s_configuration = ConfigurationManager.GetSection("marc.hi.ehrs.svc.subscription") as ConfigurationSectionHandler;

            // Get the db id
            Guid dbId = GetDatabaseKey(new Guid(id), pin);

            // Register a subscription
            ISubscriptionManagementService mgrSvc = ApplicationContext.CurrentContext.GetService(typeof(ISubscriptionManagementService)) as ISubscriptionManagementService;
            if (mgrSvc == null)
                throw new InvalidOperationException(localeService.GetString("SBSE002"));
            
            var subscriptionData = mgrSvc.GetSubscription(dbId);
            //subscriptionData.SubscriptionId = id;
            MessageProperties properties = OperationContext.Current.IncomingMessageProperties; 
            HttpRequestMessageProperty requestProperty = (HttpRequestMessageProperty)properties[HttpRequestMessageProperty.Name]; 
            string queryString = requestProperty.QueryString;

            bool newOnly = queryString.Contains("new");
            var subscriptionResults = mgrSvc.CheckSubscription(dbId, newOnly);

            // Construct the feed
            return new Atom10FeedFormatter(GenerateFeed(subscriptionData, id, pin, subscriptionResults));
        }

        
        /// <summary>
        /// Generate a feed
        /// </summary>
        private SyndicationFeed GenerateFeed(Core.Subscription subscriptionData, String id, String pin, SubscriptionResult[] subscriptionResults)
        {
            // Localization service
            ILocalizationService localeService = ApplicationContext.CurrentContext.GetService(typeof(ILocalizationService)) as ILocalizationService;

            SyndicationFeed feed = new SyndicationFeed();
            feed.Generator = String.Format("{0} v{1}", ApplicationContext.ConfigurationService.Custodianship.Name, Assembly.GetEntryAssembly().GetName().Version);
            feed.Id = subscriptionData.SubscriptionId;
            feed.Title = new TextSyndicationContent(localeService.GetString("SBSI001"), TextSyndicationContentKind.Plaintext);
            feed.Language = ApplicationContext.ConfigurationService.JurisdictionData.DefaultLanguageCode;

            if (subscriptionData == null)
                feed.Items = new List<SyndicationItem>() { 
                    new SyndicationItem(
                        localeService.GetString("SBSE001"), 
                        localeService.GetString("SBSE001.a"),
                        null
                        )
                };
            else
            {
                feed.LastUpdatedTime = subscriptionData.LastUpdate;
               
                XNamespace xn = "urn:marc-hi:ehrs:subscription";
                feed.ElementExtensions.Add(
                    new XElement(xn + "intendedFor",
                        new XElement(xn + "id",
                            new XAttribute("domain", subscriptionData.ParticipantId.Domain),
                            new XAttribute("uid", subscriptionData.ParticipantId.Identifier)
                        )
                    ).CreateReader());

                //feed.ElementExtensions.Add(
                //      new SubscriptionDisclosure() { ParticipantId = subscriptionData.ParticipantId }, new System.Xml.Serialization.XmlSerializer(typeof(HealthcareParticipant))
                //);

                // Add items
                List<SyndicationItem> items = new List<SyndicationItem>((int)s_configuration.MaximumRecords);
                foreach (var itm in subscriptionResults.Take((int)s_configuration.MaximumRecords))
                {
                    SyndicationItem feedItem = new SyndicationItem();
                    feedItem.Title = new TextSyndicationContent(String.Format("{1}^^^&{0}&ISO", itm.Id.Domain, itm.Id.Identifier), TextSyndicationContentKind.Plaintext);
                    feedItem.Content = new TextSyndicationContent(localeService.GetString("SBSI004"), TextSyndicationContentKind.Plaintext);
                    feedItem.Id = itm.FeedItemId.ToString();
                    feedItem.PublishDate = itm.Created;
                    feedItem.LastUpdatedTime = itm.Published;
                    feedItem.Links.Add(
                        new SyndicationLink(
                            new Uri(String.Format("{0}/{1}?pin={2}", OperationContext.Current.IncomingMessageHeaders.To.ToString().Replace( OperationContext.Current.IncomingMessageHeaders.To.Query, ""), itm.FeedItemId, pin)
                            )
                        ));
                    // Add match
                    feedItem.ElementExtensions.Add(CreateMatchXml(itm.Match).CreateReader());

                    items.Add(feedItem);
                }
                feed.Items = items;
            }
            return feed;
        }

        /// <summary>
        /// Create match element
        /// </summary>
        private XElement CreateMatchXml(FilterExpression filterExpression)
        {
            XNamespace xn = "urn:marc-hi:ehrs:subscription";
            XElement retVal = new XElement(xn + "matched");
            foreach (FilterExpressionBase term in filterExpression.Terms)
            {
                var cfe = term as ComponentFilterExpression;
                var pfe = term as PropertyFilterExpression;
                XElement child = null;
                if (cfe != null)
                    child = new XElement(xn + "hasComponent",
                        new XAttribute("type", cfe.TypeName),
                        new XAttribute("withRole", cfe.Role));
                else if (pfe != null)
                {
                    child = new XElement(xn + "hasProperty",
                        new XAttribute("name", pfe.Name));
                    if (pfe.Value != null)
                    {
                        child.Add(new XAttribute("value", pfe.Value));
                        child.Add(new XAttribute("operator", OperatorString(pfe.Operator)));
                    }
                }
                // Add terms
                if (term.Where != null)
                    child.Add(CreateMatchXml(term.Where));
                retVal.Add(child);
            }
            return retVal;
        }

        /// <summary>
        /// Get the operator string since the XMLSerializer extension thing 
        /// doesn't work and we need to use XElement
        /// </summary>
        private string OperatorString(OperatorType operatorType)
        {
            switch (operatorType)
            {
                case OperatorType.EqualTo:
                    return "eq";
                case OperatorType.GreaterThan:
                    return "gt";
                case OperatorType.GreaterThanEqualTo:
                    return "ge";
                case OperatorType.LessThan:
                    return "lt";
                case OperatorType.LessThanEqualTo:
                    return "le";
                case OperatorType.NotEqualTo:
                    return "ne";
                default:
                    return String.Empty;
            }
        }

       

        /// <summary>
        /// Registers a subscription for the specified user
        /// </summary>
        public Messaging.RegisterSubscriptionResponse RegisterSubscription(Messaging.RegisterSubscriptionRequest filter)
        {

            // Construct the identifiers
            Guid id = Guid.NewGuid();
            Guid dbKey = GetDatabaseKey(id, filter.Pin);

            try
            {

                // Validate the message
                if (filter.Where == null || !filter.Where.Validate())
                    throw new ArgumentException("where", "Missing 'where' clause of subscription");
                else if (filter.ResponsiblePerson == null)
                    throw new ArgumentNullException("author","Missing 'author' of subscription");
                else if (filter.Pin == null)
                    throw new ArgumentNullException("pin", "Missing 'pin' for subscription");

                // Get the healthcare worker & validate
                IHealthcareWorkerIdentityService identityProvider = ApplicationContext.CurrentContext.GetService(typeof(IHealthcareWorkerIdentityService)) as IHealthcareWorkerIdentityService;
                if (identityProvider == null)
                    throw new ConstraintException("Cannot locate the identity provider to validate the requesting provider");
                HealthcareParticipant hcp = null;
                foreach (var altId in filter.ResponsiblePerson.AlternateIdentifiers)
                {
                    hcp = identityProvider.FindParticipant(altId);
                    if (hcp != null)
                        break;
                }

                if (hcp == null)
                    throw new ConstraintException("Cannot locate the provider specified in this message");

                // Validate the HCP
                if (filter.ResponsiblePerson.LegalName.SimilarityTo(hcp.LegalName) < 0.5f)
                    throw new ConstraintException("The name that provided for the 'author' node does not appear to match the name of the provider located. Are you sure you have the correct data?");
                filter.ResponsiblePerson.Id = hcp.Id;

                // Register a subscription
                ISubscriptionManagementService mgrSvc = ApplicationContext.CurrentContext.GetService(typeof(ISubscriptionManagementService)) as ISubscriptionManagementService;
                if (mgrSvc == null)
                    throw new InvalidOperationException("Cannot find the subscription management service!");
                mgrSvc.RegisterSubscription(dbKey, filter.Where, hcp);

                // Register a subscription response
                return new RegisterSubscriptionResponse()
                {
                    SubscriptionId = new List<Identifier>()
                    {
                        new Identifier()
                        {
                            Identifier = id.ToString()
                        }
                    },
                    Issues = new List<DetectedIssue>() { 
                        new DetectedIssue() {
                            Priority = IssuePriorityType.Informational,
                            Severity = IssueSeverityType.Low,
                            Text = String.Format("Subscription '{0}' created successfully. Get url: {1}/subscription/{0}?pin=xxxx",
                                id.ToString(), OperationContext.Current.Channel.LocalAddress)
                        }
                    }
                };
            }
            catch (ConstraintException e)
            {

                Trace.TraceError(e.ToString());
                // Return the error
                return new RegisterSubscriptionResponse()
                {
                    Issues = new List<DetectedIssue>() {
                        new DetectedIssue() {
                            Priority = IssuePriorityType.Error,
                            Severity = IssueSeverityType.Moderate,
                            Text = e.Message,
                            Type = IssueType.BusinessConstraintViolation
                        }
                    }
                };

            }
            catch (Exception e)
            {

                Trace.TraceError(e.ToString());
                // Return the error
                return new RegisterSubscriptionResponse()
                {
                    Issues = new List<DetectedIssue>() {
                        new DetectedIssue() {
                            Priority = IssuePriorityType.Error,
                            Severity = IssueSeverityType.Moderate,
                            Text = e.Message,
                            Type = IssueType.DetectedIssue
                        }
                    }
                };
            }

        }

        /// <summary>
        /// Delete a subscription
        /// </summary>
        public RegisterSubscriptionResponse DeleteSubscription(string id, string pin)
        {
            return new Messaging.RegisterSubscriptionResponse()
            {
                SubscriptionId = new List<Identifier>() {
                    new Identifier()
                    {
                        Identifier = id
                    }
                }
            };
        }

        /// <summary>
        /// Get all subscriptions that match the query
        /// </summary>
        public RegisterSubscriptionResponse GetAllSubscriptions(string oid, string id, string pin)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ISyndicationContract Members

        /// <summary>
        /// Get a subscription item
        /// </summary>
        public Stream GetSubscriptionItem(string subid, string pin, string id)
        {

            try
            {
                // Get the subscription item
                // Localization service
                ILocalizationService localeService = ApplicationContext.CurrentContext.GetService(typeof(ILocalizationService)) as ILocalizationService;

                if (String.IsNullOrEmpty(subid))
                    throw new ArgumentNullException("subid");
                else if (String.IsNullOrEmpty(pin))
                    throw new ArgumentNullException("pin");

                if (s_configuration == null)
                    lock (s_configuration)
                        s_configuration = ConfigurationManager.GetSection("marc.hi.ehrs.svc.subscription") as ConfigurationSectionHandler;

                // Get the db id
                Guid dbId = GetDatabaseKey(new Guid(subid), pin);

                // Register a subscription
                ISubscriptionManagementService mgrSvc = ApplicationContext.CurrentContext.GetService(typeof(ISubscriptionManagementService)) as ISubscriptionManagementService;
                if (mgrSvc == null)
                    throw new InvalidOperationException(localeService.GetString("SBSE002"));

                var subscriptionResults = mgrSvc.GetSubscriptionItem(dbId, Decimal.Parse(id));

                if (subscriptionResults == null)
                {
                    return null;
                }
                else
                {
                    // Get the identifier
                    IDataPersistenceService persistence = ApplicationContext.CurrentContext.GetService(typeof(IDataPersistenceService)) as IDataPersistenceService;
                    // De-persist
                    var resultData = persistence.GetContainer(subscriptionResults.Id, true);
                    if (resultData != null)
                    {
                        XmlSerializer xsz = new XmlSerializer(resultData.GetType());
                        MemoryStream retVal = new MemoryStream();
                        xsz.Serialize(retVal, resultData);
                        retVal.Seek(0, SeekOrigin.Begin);
                        return retVal;
                    }
                    else
                        return null;
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                return null;
            }
        }

        #endregion
    }
}
