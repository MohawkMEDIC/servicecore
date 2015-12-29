﻿using MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Backbone;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{
    /// <summary>
    /// Identifies the type of bundles 
    /// </summary>
    [XmlType("BundleType", Namespace = "http://hl7.org/fhir")]
    [FhirValueSet(Uri = "http://hl7.org/fhir/ValueSet/bundle-type")]
    public enum BundleType
    {
        [XmlEnum("document")]
        Document,
        [XmlEnum("message")]
        Message,
        [XmlEnum("transaction")]
        Transaction,
        [XmlEnum("transaction-response")]
        TransactionResponse,
        [XmlEnum("batch")]
        Batch,
        [XmlEnum("batch-response")]
        BatchResponse,
        [XmlEnum("history")]
        HistoryList,
        [XmlEnum("searchset")]
        SearchResults,
        [XmlEnum("collection")]
        Collection
    }

    /// <summary>
    /// Represents a bundle of resources. DSTU2 replacement for feeds
    /// </summary>
    [XmlType("Bundle", Namespace = "http://hl7.org/fhir")]
    public class Bundle : ResourceBase
    {
        /// <summary>
        /// Creates a new bundle
        /// </summary>
        public Bundle()
        {
            this.Link = new List<BundleLink>();
            this.Entry = new List<BundleEntry>();
        }

        /// <summary>
        /// Gets or sets the type of the bundle
        /// </summary>
        [XmlElement("type")]
        [ElementProfile(Binding = typeof(BundleType), Comment = "The type of bundle", MaxOccurs = 1, MinOccurs = 1, MustSupport = true)]
        [Description("Identifies the type of bundle")]
        public FhirCode<BundleType> Type { get; set; }

        /// <summary>
        /// Gets or sets the total number of search results
        /// </summary>
        [XmlElement("total")]
        [Description("If search, the total number of search results")]
        public FhirInt Total { get; set; }

        /// <summary>
        /// Gets or sets a series of links related to the bundle
        /// </summary>
        [XmlElement("link")]
        [Description("Links related to this bundle")]
        public List<BundleLink> Link { get; set; }

        /// <summary>
        /// Gets or sets a list of bundle entries
        /// </summary>
        [XmlElement("entry")]
        [Description("Entry in the bundle")]
        public List<BundleEntry> Entry { get; set; }

        /// <summary>
        /// Gets or sets the signature of the bundle
        /// </summary>
        [XmlElement("signature")]
        [Description("Digital signature for the bundle contents")]
        public FhirSignature Signature { get; set; }

        /// <summary>
        /// Write the text of the bundle
        /// </summary>
        internal override void WriteText(XmlWriter w)
        {
            w.WriteStartElement("div", NS_XHTML);
            
            // Links
            if(this.Link?.Count > 0)
            {
                w.WriteStartElement("div", NS_XHTML);
                w.WriteAttributeString("class", "bundle_Link");
                w.WriteString("Links:");
                w.WriteStartElement("ul", NS_XHTML);
                w.WriteAttributeString("class", "bundle_Link_List");
                foreach (var lnk in this.Link)
                {
                    w.WriteStartElement("li");
                    lnk.WriteText(w);
                    w.WriteEndElement();
                }
                w.WriteEndElement(); // ul
                w.WriteEndElement(); // div
            }

            // Items
            if(this.Entry?.Count > 0)
            {
                w.WriteStartElement("div", NS_XHTML);
                w.WriteAttributeString("class", "bundle_Entry");
                w.WriteString(String.Format("Entries ({0} total)", this.Total.Value));
                w.WriteStartElement("ul", NS_XHTML);
                w.WriteAttributeString("class", "bundle_Entry_List");
                foreach (var ent in this.Entry)
                {
                    w.WriteStartElement("li");
                    ent.WriteText(w);
                    w.WriteEndElement();
                }
                w.WriteEndElement(); // ul
                w.WriteEndElement(); // li
            }

            w.WriteEndElement(); // ul
        }
    }
}
