using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.DataTypes;

namespace MARC.HI.EHRS.SVC.Subscription.Core
{
    /// <summary>
    /// Subscription result
    /// </summary>
    public class SubscriptionResult
    {

        /// <summary>
        /// Gets the feed item identifier
        /// </summary>
        public decimal FeedItemId { get; set; }

        /// <summary>
        /// The identifier of the subscription result
        /// </summary>
        public VersionedDomainIdentifier Id { get; set; }

        /// <summary>
        /// The date the result was created
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// The date the result was published
        /// </summary>
        public DateTime Published { get; set; }

        /// <summary>
        /// Gets or sets the match
        /// </summary>
        public FilterExpression Match { get; set; }
    }
}
