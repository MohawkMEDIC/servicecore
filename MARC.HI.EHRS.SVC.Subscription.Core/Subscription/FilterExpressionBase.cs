using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace MARC.HI.EHRS.SVC.Subscription.Core
{
    /// <summary>
    /// Filter expression base
    /// </summary>
    [XmlType("FilterExpressionBase", Namespace = "urn:marc-hi:ehrs:subscription")]
    public abstract class FilterExpressionBase
    {
        /// <summary>
        /// Represents the expression tree XML
        /// </summary>
        [XmlElement("where")]
        public FilterExpression Where { get; set; }

        /// <summary>
        /// When true, will report the matching parameter back to in the syndication feed
        /// </summary>
        [XmlAttribute("report")]
        public bool ReportMatch { get; set; }

        /// <summary>
        /// Gets a list of match data that matched the query
        /// </summary>
        public List<FilterExpressionBase> MatchedData(object component)
        {

            var retVal = new List<FilterExpressionBase>();

            if (Where == null) return null;

            // Already processed?
            foreach (var term in Where.Terms)
            {
                bool isTermMatch = term.Match(component);
                if (isTermMatch && term.ReportMatch)
                    retVal.Add(term.MakeReportable());
            }

            return retVal;

        }

        /// <summary>
        /// Make the item reportable
        /// </summary>
        /// <returns></returns>
        public virtual FilterExpressionBase MakeReportable()
        {
            return this;
        }

        /// <summary>
        /// True if the container matches
        /// </summary>
        public virtual bool Match(object component)
        {

            bool isMatch = true;

            if (Where == null) return true;

            // Already processed?
            List<FilterExpressionBase> processed = new List<FilterExpressionBase>(Where.Terms.Count);

            foreach (var term in Where.Terms)
            {
                bool isTermMatch = term.Match(component);
                if (processed.Exists(o => o.IsSameAs(term)))
                    isMatch |= isTermMatch;
                else
                    isMatch &= isTermMatch;
                processed.Add(term);
            }

            return isMatch;
        }

        /// <summary>
        /// Determine if a filter expression is the same as another
        /// </summary>
        public virtual bool IsSameAs(FilterExpressionBase other)
        {
            return this.Equals(other);
        }

        /// <summary>
        /// Validate the filter expression
        /// </summary>
        public virtual bool Validate()
        {
            return true;
        }
    }
}
