using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Resources;
using System.ServiceModel.Syndication;
using System.Xml.Serialization;
using System.ServiceModel.Web;
using MARC.Everest.Connectors;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Resources.Attributes;
using MARC.HI.EHRS.SVC.Messaging.FHIR.WcfCore;
using MARC.HI.EHRS.SVC.Core.Data;
using MARC.HI.EHRS.SVC.Core;
using MARC.HI.EHRS.SVC.Core.Services;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Util
{
    /// <summary>
    /// Represents a series of message processing utilities
    /// </summary>
    public static class MessageUtil
    {

        // Escape characters
        private static readonly Dictionary<String, String> s_escapeChars = new Dictionary<string, string>()
        {
            { "\\,", "\\#002C" },
            { "\\$", "\\#0024" },
            { "\\|", "\\#007C" },
            { "\\\\", "\\#005C" },
        };

        /// <summary>
        /// Escape a string
        /// </summary>
        public static String Escape(String str)
        {
            string retVal = str;
            foreach (var itm in s_escapeChars)
                retVal = retVal.Replace(itm.Key, itm.Value);
            return retVal;
        }

        /// <summary>
        /// Un-escape a string
        /// </summary>
        public static string UnEscape(String str)
        {
            string retVal = str;
            foreach (var itm in s_escapeChars)
                retVal = retVal.Replace(itm.Value, itm.Key);
            return retVal;
        }

        /// <summary>
        /// Populate a domain identifier from a FHIR token
        /// </summary>
        public static Identifier<String> IdentifierFromToken(string token)
        {
            string[] tokens = token.Split('|');
            if (tokens.Length == 1)
                return new Identifier<String>() { Id = MessageUtil.UnEscape(tokens[0]) };
            else
                return new Identifier<String>()
                {
                    AssigningAuthority = TranslateFhirDomain(MessageUtil.UnEscape(tokens[0])),
                    Id = MessageUtil.UnEscape(tokens[1])
                };
        }

        /// <summary>
        /// Attempt to translate fhir domain
        /// </summary>
        public static OidData TranslateFhirDomain(string fhirDomain)
        {
            var oidService = ApplicationContext.Current.GetService<IOidRegistrarService>();
            if (oidService == null)
                throw new InvalidOperationException("No OID Registrar service has been registered");

            if (String.IsNullOrEmpty(fhirDomain))
                return null;
            Uri fhirDomainUri = null;
            if (fhirDomain.StartsWith("urn:oid:"))
                return oidService.FindData("urn:oid:", "");
            else if (fhirDomain.StartsWith("urn:ietf:rfc:3986"))
                return oidService.GetOid("UUID");

            else if (Uri.TryCreate(fhirDomain, UriKind.Absolute, out fhirDomainUri))
            {
                var oid = oidService.FindData(fhirDomainUri);
                if (oid == null)
                    throw new InvalidOperationException(String.Format("Could not locate identity system '{0}'", fhirDomain));
                return oid;
            }
            else if (MARC.Everest.DataTypes.II.IsValidOidFlavor(new MARC.Everest.DataTypes.II(fhirDomain)))
                return oidService.FindData(fhirDomain);
            else
                return null;
        }

        /// <summary>
        /// Attempt to translate fhir domain
        /// </summary>
        public static string TranslateDomain(string crDomain)
        {
            // Attempt to lookup the OID
            var oidService = ApplicationContext.Current.GetService<IOidRegistrarService>();
            if (oidService == null)
                throw new InvalidOperationException("No OID Registrar service has been registered");

            var oid = oidService.FindData(crDomain);
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
            string[] tokens = token.Split('|');
            if (tokens.Length == 1)
                return new CodeValue(MessageUtil.UnEscape(tokens[0]), null);
            else
                return new CodeValue(MessageUtil.UnEscape(tokens[1]), MessageUtil.UnEscape(tokens[0]));
        }


        /// <summary>
        /// Create a feed
        /// </summary>
        internal static SyndicationFeed CreateFeed(FhirOperationResult result)
        {

            SyndicationFeed retVal = new SyndicationFeed();
            FhirQueryResult queryResult = result as FhirQueryResult;

            int pageNo = queryResult == null || queryResult.Query.Quantity == 0 ? 0 : queryResult.Query.Start / queryResult.Query.Quantity,
                nPages = queryResult == null || queryResult.Query.Quantity == 0 ? 1 : (queryResult.TotalResults / queryResult.Query.Quantity);
            
            if (result.Details.Exists(o => o.Type == ResultDetailType.Error))
                retVal.Title = new TextSyndicationContent(String.Format("Error", pageNo));
            else
                retVal.Title = new TextSyndicationContent(String.Format("Results Page {0}", pageNo));
            retVal.Id = String.Format("urn:uuid:{0}", Guid.NewGuid());
            retVal.Authors.Add(new SyndicationPerson(null, Environment.MachineName, null));
            // Make the Self uri
            String baseUri = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.AbsoluteUri;
            if (baseUri.Contains("?"))
                baseUri = baseUri.Substring(0, baseUri.IndexOf("?") + 1);
            else
                baseUri += "?";

            // Self uri
            if (queryResult != null)
            {
                for (int i = 0; i < queryResult.Query.ActualParameters.Count; i++)
                    foreach (var itm in queryResult.Query.ActualParameters.GetValues(i))
                        switch(queryResult.Query.ActualParameters.GetKey(i))
                        {
                            case "stateid":
                            case "page":
                                break;
                            default:
                                baseUri += string.Format("{0}={1}&", queryResult.Query.ActualParameters.GetKey(i), itm);
                                break;
                        }

                if(!baseUri.Contains("stateid=") && queryResult.Query.QueryId != Guid.Empty)
                    baseUri += String.Format("stateid={0}&", queryResult.Query.QueryId);
            }

            // Format
            string format = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters["_format"];
            if (String.IsNullOrEmpty(format))
                format = "xml";
            else if (format == "application/xml+fhir")
                format = "xml";
            else if (format == "application/json+fhir")
                format = "json";

            if (!baseUri.Contains("_format"))
                baseUri += String.Format("_format={0}&", format);

            var localizationService = ApplicationContext.Current.GetService<ILocalizationService>();

            // Self URI
            if (queryResult != null && queryResult.TotalResults > queryResult.Results.Count)
            {
                retVal.Links.Add(new SyndicationLink(new Uri(String.Format("{0}page={1}", baseUri, pageNo)), "self", null, null, 0));
                if (pageNo > 0)
                {
                    retVal.Links.Add(new SyndicationLink(new Uri(String.Format("{0}page=0", baseUri)), "first", localizationService.GetString("FHIR001"), null, 0));
                    retVal.Links.Add(new SyndicationLink(new Uri(String.Format("{0}page={1}", baseUri, pageNo - 1)), "previous", localizationService.GetString("FHIR002"), null, 0));
                }
                if (pageNo <= nPages)
                {
                    retVal.Links.Add(new SyndicationLink(new Uri(String.Format("{0}page={1}", baseUri, pageNo + 1)), "next", localizationService.GetString("FHIR003"), null, 0));
                    retVal.Links.Add(new SyndicationLink(new Uri(String.Format("{0}page={1}", baseUri, nPages + 1)), "last", localizationService.GetString("FHIR004"), null, 0));
                }
            }
            else
                retVal.Links.Add(new SyndicationLink(new Uri(baseUri), "self", null, null, 0));

            // Updated
            retVal.LastUpdatedTime = DateTime.Now;
            //retVal.Generator = "MARC-HI Service Core Framework";

            // HACK: Remove me
            if(queryResult != null)
                retVal.ElementExtensions.Add("totalResults", "http://a9.com/-/spec/opensearch/1.1/", queryResult.TotalResults);

            //retVal.
            // Results
            if (result.Results != null)
            {
                var feedItems = new List<SyndicationItem>();
                foreach (ResourceBase itm in result.Results)
                {
                    Uri resourceUrl = new Uri(String.Format("{0}/{1}?_format={2}", WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri, String.Format("{0}/{1}/_history/{2}", itm.GetType().Name, itm.Id, itm.VersionId), format));
                    SyndicationItem feedResult = new SyndicationItem(String.Format("{0} id {1} version {2}", itm.GetType().Name, itm.Id, itm.VersionId), null ,resourceUrl);
                    feedResult.Links.Add(new SyndicationLink(resourceUrl, "self", null, null, 0));

                    string summary = "<div xmlns=\"http://www.w3.org/1999/xhtml\">" + itm.Text.ToString() + "</div>";
                    feedResult.Summary = new TextSyndicationContent(summary, TextSyndicationContentKind.XHtml);
                    feedResult.Content = new XmlSyndicationContent("text/xml", new SyndicationElementExtension(itm, new FhirXmlObjectSerializer()));
                    feedResult.LastUpdatedTime = itm.Timestamp;
                    feedResult.PublishDate = DateTime.Now;
                    feedResult.Authors.Add(new SyndicationPerson(null, Environment.MachineName, null));

                    // Add confidence if the attribute permits
                    ConfidenceAttribute confidence = itm.Attributes.Find(a => a is ConfidenceAttribute) as ConfidenceAttribute;
                    if(confidence != null)
                        feedResult.ElementExtensions.Add("score", "http://a9.com/-/opensearch/extensions/relevance/1.0/", confidence.Confidence);
                    feedItems.Add(feedResult);
                }
                retVal.Items = feedItems;
            }

            // Outcome
            //if (result.Details.Count > 0 || result.Issues != null && result.Issues.Count > 0)
            //{
            //    var outcome = CreateOutcomeResource(result);
            //    retVal.ElementExtensions.Add(outcome, new XmlSerializer(typeof(OperationOutcome)));
            //    retVal.Description = new TextSyndicationContent(outcome.Text.ToString(), TextSyndicationContentKind.Html);
            //}
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
                    Severity = new DataTypes.FhirCode<string>(dtl.Type.ToString().ToLower())
                };

                if (!String.IsNullOrEmpty(dtl.Location))
                    issue.Location.Add(new DataTypes.FhirString(dtl.Location));

                // Type
                if (dtl.Exception is TimeoutException)
                    issue.Type = new DataTypes.FhirCoding(fhirIssue, "timeout");
                else if (dtl is FixedValueMisMatchedResultDetail)
                    issue.Type = new DataTypes.FhirCoding(fhirIssue, "value");
                else if (dtl is PersistenceResultDetail)
                    issue.Type = new DataTypes.FhirCoding(fhirIssue, "no-store");
                else
                    issue.Type = new DataTypes.FhirCoding(fhirIssue, "exception");

                retVal.Issue.Add(issue);
            }

            // Add detected issues
            if (result.Issues != null)
                foreach (var iss in result.Issues)
                    retVal.Issue.Add(new Issue()
                    {
                        Details = new DataTypes.FhirString(iss.Text),
                        Severity = new DataTypes.FhirCode<string>(iss.Severity.ToString().ToLower()),
                        Type = new DataTypes.FhirCoding(fhirIssue, "business-rule")
                    });

            return retVal;
        }

    }
}
