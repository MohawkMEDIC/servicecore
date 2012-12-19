/**
 * Copyright 2012-2012 Mohawk College of Applied Arts and Technology
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
 * Date: 7-5-2012
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Syndication;
using System.ServiceModel;
using System.ServiceModel.Web;
using MARC.HI.EHRS.SVC.Subscription.Data.Messaging;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Subscription.Data
{
    /// <summary>
    /// Represents the WCF service contract for subscriptions
    /// </summary>
    [XmlSerializerFormat]
    [ServiceContract]
    public interface ISyndicationContract
    {

        /// <summary>
        /// Get all subscription data for the specified subscription
        /// </summary>
        [OperationContract]
        [WebGet(UriTemplate = "/subscription/{id}?pin={pin}", ResponseFormat = WebMessageFormat.Xml)]
        Atom10FeedFormatter GetSubscription(string id, string pin);

        /// <summary>
        /// Get subscription data since the specified date
        /// </summary>
        [OperationContract]
        [WebGet(UriTemplate = "/subscription/{id}?new&pin={pin}", ResponseFormat = WebMessageFormat.Xml)]
        Atom10FeedFormatter GetSubscriptionNewOnly(string id, string pin);

        /// <summary>
        /// Get subscription data since the specified date
        /// </summary>
        [OperationContract]
        [WebGet(UriTemplate = "/subscription/{id}?all&pin={pin}", ResponseFormat = WebMessageFormat.Xml)]
        Atom10FeedFormatter GetSubscriptionAll(string id, string pin);

        /// <summary>
        /// Register a subscription
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "/subscription", RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml)]
        RegisterSubscriptionResponse RegisterSubscription(RegisterSubscriptionRequest filter);

        /// <summary>
        /// Get all subscriptions for a particular user
        /// </summary>
        [OperationContract]
        [WebGet(UriTemplate = "/author/{oid}@{id}/subscriptions?pin={pin}", ResponseFormat = WebMessageFormat.Xml)]
        RegisterSubscriptionResponse GetAllSubscriptions(string oid, string id, string pin);

        /// <summary>
        /// Delete a subscription
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "/subscription/{id}?pin={pin}", ResponseFormat = WebMessageFormat.Xml)]
        RegisterSubscriptionResponse DeleteSubscription(string id, string pin);
        

    }
}
