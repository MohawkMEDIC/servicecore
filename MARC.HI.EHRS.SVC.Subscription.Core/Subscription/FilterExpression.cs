using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Subscription.Core
{

    /// <summary>
    /// Filter expression
    /// </summary>
    [XmlType("FilterExpression", Namespace = "urn:marc-hi:ehrs:subscription")]
    [XmlRoot("expression", Namespace = "urn:marc-hi:ehrs:subscription")]
    public class FilterExpression
    {

        /// <summary>
        /// Filter expression
        /// </summary>
        public FilterExpression()
        {
            this.Terms = new List<FilterExpressionBase>();
        }

        /// <summary>
        /// Filter expression terms
        /// </summary>
        [XmlElement("containsComponent", typeof(ComponentFilterExpression))]
        [XmlElement("hasProperty", typeof(PropertyFilterExpression))]
        public List<FilterExpressionBase> Terms { get; set; }

        /// <summary>
        /// Validate
        /// </summary>
        public bool Validate()
        {
            bool valid = true;
            foreach (var term in this.Terms)
                valid &= term.Validate();
            return valid;
        }
    }
}
