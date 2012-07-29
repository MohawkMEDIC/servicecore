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

            // Now parse the has back into a guid
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
            var subscriptionResults = mgrSvc.CheckSubscription(dbId, false);

            // Construct the feed
           

            return new Atom10FeedFormatter(GenerateFeed(subscriptionData, subscriptionResults));
        }

        /// <summary>
        /// Generate a feed
        /// </summary>
        private SyndicationFeed GenerateFeed(Core.Subscription subscriptionData, SubscriptionResult[] subscriptionResults)
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
                feed.Links.Add(
                    new SyndicationLink(
                        new Uri(String.Format("{0}/subscription/{1}?new&pin={2}", OperationContext.Current.Channel.LocalAddress, subscriptionData.SubscriptionId, "pin")),
                        "related",
                        localeService.GetString("SBSI002"),
                        "application/xml",
                        0
                    )
                );
                feed.Links.Add(
                    new SyndicationLink(
                        new Uri(String.Format("{0}/subscription/{1}?all&pin={2}", OperationContext.Current.Channel.LocalAddress, subscriptionData.SubscriptionId, "pin")),
                        "related",
                        localeService.GetString("SBSI003"),
                        "application/xml",
                        0
                    )
                );
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
                            new Uri(String.Format("{0}/subscription/{1}/{2}?pin={3}", OperationContext.Current.Channel.LocalAddress, subscriptionData.SubscriptionId, itm.FeedItemId, "pin")
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

        public System.ServiceModel.Syndication.Atom10FeedFormatter GetSubscriptionNewOnly(string id, string pin)
        {
            throw new NotImplementedException();
        }

        public System.ServiceModel.Syndication.Atom10FeedFormatter GetSubscriptionAll(string id, string pin)
        {
            throw new NotImplementedException();
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
                    SubscriptionId = new List<DomainIdentifier>()
                    {
                        new DomainIdentifier()
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
                SubscriptionId = new List<DomainIdentifier>() {
                    new DomainIdentifier()
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
    }
}
