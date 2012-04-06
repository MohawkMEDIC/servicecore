using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;
using MARC.HI.EHRS.SVC.Core.DataTypes;
using MARC.HI.EHRS.SVC.Core.Services;
using MARC.HI.EHRS.SVC.Core.ComponentModel.Components;

namespace MARC.HI.EHRS.SVC.Subscription.Core.Services
{
    /// <summary>
    /// Represents a subscription service
    /// </summary>
    public interface ISubscriptionManagementService : IUsesHostContext
    {

        /// <summary>
        /// Publish a record to the subscription service
        /// </summary>
        void PublishContainer(IContainer record);

        /// <summary>
        /// Registers the subscription
        /// </summary>
        /// <param name="subscription">The subscription predicate</param>
        /// <returns>A unique identifier for the registered subscription</returns>
        bool RegisterSubscription(Guid subscriptionId, FilterExpression subscriptionDefinition, HealthcareParticipant recipientOfDisclosure);

        /// <summary>
        /// Check the specified subscription id for any new records added since sinceDate
        /// </summary>
        /// <param name="subscriptionId">The id of the subscription</param>
        /// <param name="sinceDate">The date to check since</param>
        /// <returns>A list of versioned domain identifiers matching the subscription results</returns>
        SubscriptionResult[] CheckSubscription(Guid subscriptionId, bool newOnly);

        /// <summary>
        /// Get the subscription
        /// </summary>
        Subscription GetSubscription(Guid subscriptionId);
    }
}
