using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Resources;
using System.ServiceModel.Syndication;
using System.Xml.Serialization;
using System.ServiceModel.Web;
using MARC.HI.EHRS.SVC.Core.DataTypes;
using MARC.Everest.Connectors;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Util
{
    /// <summary>
    /// Represents a series of message processing utilities
    /// </summary>
    public static class MessageUtil
    {
        /// <summary>
        /// Populate a domain identifier from a FHIR token
        /// </summary>
        public static DomainIdentifier IdentifierFromToken(string token)
        {
            string[] tokens = token.Split('!');
            if (tokens.Length == 1)
                return new DomainIdentifier() { Identifier = tokens[0] };
            else
                return new DomainIdentifier()
                {
                    Domain = TranslateFhirDomain(tokens[0]),
                    Identifier = tokens[1]
                };
        }

        /// <summary>
        /// Attempt to translate fhir domain
        /// </summary>
        public static string TranslateFhirDomain(string fhirDomain)
        {
            Uri fhirDomainUri = null;
            if (MARC.Everest.DataTypes.II.IsValidOidFlavor(new MARC.Everest.DataTypes.II(fhirDomain)))
                return fhirDomain;
            else if (fhirDomain.StartsWith("urn:oid:"))
                return fhirDomain.Replace("urn:oid:", "");
            else if (fhirDomain.StartsWith("urn:ietf:rfc:3986"))
                return fhirDomain;
            else if (Uri.TryCreate(fhirDomain, UriKind.Absolute, out fhirDomainUri))
            {
                var oid = ApplicationContext.ConfigurationService.OidRegistrar.FindData(fhirDomainUri);
                if (oid == null)
                    throw new InvalidOperationException(String.Format("Could not locate the specified domain '{0}'", fhirDomain));
                return oid.Oid;
            }
            else
                return fhirDomain;
        }

        /// <summary>
        /// Attempt to translate fhir domain
        /// </summary>
        public static string TranslateDomain(string crDomain)
        {
            // Attempt to lookup the OID
            var oid = ApplicationContext.ConfigurationService.OidRegistrar.FindData(crDomain);
            if (oid == null)
                return String.Format("urn:oid:{0}", crDomain);
            else if (crDomain == "urn:ietf:rfc:3986")
                return crDomain;
            else
                return oid.Ref != null ? oid.Ref.ToString() : string.Format("urn:oid:{0}", crDomain);
        }

        /// <summary>
        /// Populate a domain identifier from a FHIR token
        /// </summary>
        public static CodeValue CodeFromToken(string token)
        {
            string[] tokens = token.Split('!');
            if (tokens.Length == 1)
                return new CodeValue() { Code = tokens[0] };
            else
                return new CodeValue()
                {
                    CodeSystem = tokens[0],
                    Code = tokens[1]
                };
        }


        /// <summary>
        /// Create a feed
        /// </summary>
        internal static SyndicationFeed CreateFeed(FhirOperationResult result)
        {

            SyndicationFeed retVal = new SyndicationFeed();
            FhirQueryResult queryResult = result as FhirQueryResult;

            int pageNo = queryResult == null || queryResult.Query.Quantity == 0 ? 0 : queryResult.Query.Start / queryResult.Query.Quantity,
                nPages = queryResult == null || queryResult.Query.Quantity == 0 ? 1 : (queryResult.TotalResults / queryResult.Query.Quantity) + 1;

            if (result.Details.Exists(o => o.Type == ResultDetailType.Error))
                retVal.Title = new TextSyndicationContent(String.Format("Error", pageNo));
            else
                retVal.Title = new TextSyndicationContent(String.Format("Results Page {0}", pageNo));
            retVal.Id = String.Format("urn:uuid:{0}", Guid.NewGuid());

            // Make the Self uri
            String baseUri = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.AbsoluteUri;
            if (baseUri.Contains("?"))
                baseUri = baseUri.Substring(0, baseUri.IndexOf("?") + 1);

            // Self uri
            if (queryResult != null)
            {
                for (int i = 0; i < queryResult.Query.ActualParameters.Count; i++)
                    foreach (var itm in queryResult.Query.ActualParameters.GetValues(i))
                        baseUri += string.Format("{0}={1}&", queryResult.Query.ActualParameters.GetKey(i), itm);
                baseUri += String.Format("stateid={0}&", queryResult.Query.QueryId);
            }

            // Self URI
            if (nPages > 1)
            {
                retVal.Links.Add(new SyndicationLink(new Uri(String.Format("{0}&page={1}", baseUri, pageNo)), "self", null, null, 0));
                if (pageNo > 0)
                {
                    retVal.Links.Add(new SyndicationLink(new Uri(String.Format("{0}&page=0", baseUri)), "first", ApplicationContext.LocalizationService.GetString("FHIR001"), null, 0));
                    retVal.Links.Add(new SyndicationLink(new Uri(String.Format("{0}&page={1}", baseUri, pageNo - 1)), "previous", ApplicationContext.LocalizationService.GetString("FHIR002"), null, 0));
                }
                if (pageNo < nPages - 1)
                {
                    retVal.Links.Add(new SyndicationLink(new Uri(String.Format("{0}&page={1}", baseUri, pageNo + 1)), "next", ApplicationContext.LocalizationService.GetString("FHIR003"), null, 0));
                    retVal.Links.Add(new SyndicationLink(new Uri(String.Format("{0}&page={1}", baseUri, nPages)), "last", ApplicationContext.LocalizationService.GetString("FHIR004"), null, 0));
                }
            }
            else
                retVal.Links.Add(new SyndicationLink(new Uri(baseUri), "self", null, null, 0));

            // Updated
            retVal.LastUpdatedTime = DateTime.Now;
            retVal.Generator = "http://te.marc-hi.ca";

            //retVal.
            // Results
            if (result.Results != null)
            {
                var feedItems = new List<SyndicationItem>();
                foreach (ResourceBase itm in result.Results)
                {
                    Uri resourceUrl = new Uri(String.Format("{0}/{1}", WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri, String.Format("{0}/@{1}/history/@{2}", itm.GetType().Name, itm.Id, itm.VersionId)));
                    SyndicationItem feedResult = new SyndicationItem(String.Format("{0} id {1} version {2}", itm.GetType().Name, itm.Id, itm.VersionId), null, resourceUrl);

                    feedResult.Summary = new TextSyndicationContent(itm.Text.ToString(), TextSyndicationContentKind.Html);
                    feedResult.Content = new XmlSyndicationContent("application/fhir+xml", new SyndicationElementExtension(itm, new XmlSerializer(itm.GetType())));
                    feedResult.LastUpdatedTime = itm.Timestamp;
                    feedResult.PublishDate = DateTime.Now;
                    // TODO: author
                    feedItems.Add(feedResult);
                }
                retVal.Items = feedItems;
            }

            // Outcome
            if (result.Details.Count > 0 || result.Issues != null && result.Issues.Count > 0)
            {
                var outcome = CreateOutcomeResource(result);
                retVal.ElementExtensions.Add(outcome, new XmlSerializer(typeof(OperationOutcome)));
                retVal.Description = new TextSyndicationContent(outcome.Text.ToString(), TextSyndicationContentKind.Html);
            }
            return retVal;

        }



        /// <summary>
        /// Create an operation outcome resource
        /// </summary>
        public static OperationOutcome CreateOutcomeResource(FhirOperationResult result)
        {
            var retVal = new OperationOutcome();

            Uri fhirIssue = new Uri("http://hl7.org/fhir/issue-type");

            // Add issues for each of the details
            foreach (var dtl in result.Details)
            {
                Issue issue = new Issue()
                {
                    Details = new DataTypes.FhirString(dtl.Message),
                    Severity = new DataTypes.PrimitiveCode<string>(dtl.Type.ToString().ToLower())
                };

                if (!String.IsNullOrEmpty(dtl.Location))
                    issue.Location.Add(new DataTypes.FhirString(dtl.Location));

                // Type
                if (dtl.Exception is TimeoutException)
                    issue.Type = new DataTypes.Coding(fhirIssue, "timeout");
                else if (dtl is FixedValueMisMatchedResultDetail)
                    issue.Type = new DataTypes.Coding(fhirIssue, "value");
                else if (dtl is PersistenceResultDetail)
                    issue.Type = new DataTypes.Coding(fhirIssue, "no-store");
                else
                    issue.Type = new DataTypes.Coding(fhirIssue, "exception");

                retVal.Issue.Add(issue);
            }

            // Add detected issues
            if (result.Issues != null)
                foreach (var iss in result.Issues)
                    retVal.Issue.Add(new Issue()
                    {
                        Details = new DataTypes.FhirString(iss.Text),
                        Severity = new DataTypes.PrimitiveCode<string>(iss.Severity.ToString().ToLower()),
                        Type = new DataTypes.Coding(fhirIssue, "business-rule")
                    });

            return retVal;
        }

    }
}
