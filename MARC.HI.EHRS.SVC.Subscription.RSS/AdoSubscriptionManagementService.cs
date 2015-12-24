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
using MARC.HI.EHRS.SVC.Core.Services;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using MARC.HI.EHRS.SVC.Subscription.Data.Configuration;
using System.Configuration;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Core.ComponentModel;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data;
using MARC.HI.EHRS.SVC.HealthWorkerIdentity;
using MARC.HI.EHRS.SVC.Core;
using MARC.HI.EHRS.SVC.PolicyEnforcement;
using System.ServiceModel.Web;
using System.ServiceModel;
using System.ServiceModel.Description;
using MARC.HI.EHRS.SVC.Subscription.Core.Services;
using MARC.HI.EHRS.SVC.Core.DataTypes;
using MARC.HI.EHRS.SVC.Core.Issues;
using MARC.HI.EHRS.SVC.Core.ComponentModel.Components;
using MARC.Everest.Threading;
using MARC.HI.EHRS.SVC.Subscription.Core;
using System.ComponentModel;

namespace MARC.HI.EHRS.SVC.Subscription.Data
{
    /// <summary>
    /// Subscription management service
    /// </summary>
    [Description("ADO.NET RSS Subscription Service")]
    public class AdoSubscriptionManagementService : ISubscriptionManagementService, IDisposable
    {

        // Cached subscriptions (saves some compute power)
        public static List<Subscription.Core.Subscription> s_cachedSubscriptions = new List<Subscription.Core.Subscription>();

        /// <summary>
        /// Load subscription data
        /// </summary>
        private static Subscription.Core.Subscription LoadSubscription(IDbConnection conn, IDbTransaction tx, Guid subscriptionId)
        {

            // Look in the cache
            var sub = s_cachedSubscriptions.Find(o => o.SubscriptionId == subscriptionId.ToString());

            if(sub == null)
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    // Setup
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "get_sub";
                    cmd.Connection = conn;
                    cmd.Transaction = tx;

                    // Parameter
                    IDataParameter parm = cmd.CreateParameter();
                    parm.ParameterName = "sub_id_in";
                    parm.DbType = DbType.String;
                    parm.Direction = ParameterDirection.Input;
                    parm.Value = subscriptionId.ToString();
                    cmd.Parameters.Add(parm);

                    // Load each one 
                    using (IDataReader rdr = cmd.ExecuteReader())
                        if (rdr.Read())
                            if (!AdoSubscriptionManagementService.s_cachedSubscriptions.Exists(o => o.SubscriptionId == rdr["sub_id"].ToString()))
                                lock (AdoSubscriptionManagementService.s_cachedSubscriptions)
                                {
                                    sub = AdoSubscriptionManagementService.LoadSubscription(rdr);
                                    AdoSubscriptionManagementService.s_cachedSubscriptions.Add(sub);
                                }
                }
            return sub;
        }

        /// <summary>
        /// Load a subscription from the specified reader
        /// </summary>
        private static Subscription.Core.Subscription LoadSubscription(IDataReader rdr)
        {
            XmlSerializer xsz = new XmlSerializer(typeof(Subscription.Core.Subscription));
            using (MemoryStream ms = new MemoryStream((byte[])rdr["fltr"]))
            {
                var sub = xsz.Deserialize(ms) as Subscription.Core.Subscription;
                sub.SubscriptionId = rdr["sub_id"].ToString();
                sub.ParticipantId = new Identifier()
                {
                    Domain = ApplicationContext.ConfigurationService.OidRegistrar.GetOid("SUBSC_PID").Oid,
                    Identifier = rdr["aut_ent_id"].ToString()
                };
                sub.LastUpdate = Convert.ToDateTime(rdr["bld_utc"]);
                return sub;
            }
            
        }

        /// <summary>
        /// Represents a work item that is to be performed for subscription sync
        /// </summary>
        [Serializable]
        public class WorkItem
        {
            // Sync object
            [NonSerialized]
            private static object s_syncObject = new object();
            
            /// <summary>
            /// The id of the work item
            /// </summary>
            public string Id { get; set; }

            /// <summary>
            /// The event to be scanned
            /// </summary>
            public HealthServiceRecordContainer Event { get; set; }

            /// <summary>
            /// Do work
            /// </summary>
            public void DoWork(object state)
            {

                IDbConnection conn = state as IDbConnection;
                IDbTransaction tx = null;
                try
                {
                    
                    // Event is identifiable
                    if (!(Event is IIdentifiable))
                        throw new InvalidOperationException("Event must be identifiable");
                    else if (Event.IsMasked)
                        throw new InvalidOperationException("Will not publish a masked event");

                    // Get services
                    ISystemConfigurationService configService = ApplicationContext.ConfigurationService;
                    IPolicyEnforcementService policyService = ApplicationContext.CurrentContext.GetService(typeof(IPolicyEnforcementService)) as IPolicyEnforcementService;
                    IHealthcareWorkerIdentityService identityService = ApplicationContext.CurrentContext.GetService(typeof(IHealthcareWorkerIdentityService)) as IHealthcareWorkerIdentityService;

                    conn.Open();

                    tx = conn.BeginTransaction();

                    // First step ... we need to get all the active subscriptions if it is empty
                    if(AdoSubscriptionManagementService.s_cachedSubscriptions.Count == 0)
                        lock (s_syncObject)
                        {
                            // Check again
                            if(AdoSubscriptionManagementService.s_cachedSubscriptions.Count == 0)
                                using (IDbCommand cmd = conn.CreateCommand())
                                {
                                    // Setup
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.CommandText = "get_actv_sub";
                                    cmd.Connection = conn;
                                    cmd.Transaction = tx;

                                    // Load each one 
                                    using (IDataReader rdr = cmd.ExecuteReader())
                                        while (rdr.Read())
                                            if (!AdoSubscriptionManagementService.s_cachedSubscriptions.Exists(o => o.SubscriptionId == rdr["sub_id"].ToString()))
                                                lock (AdoSubscriptionManagementService.s_cachedSubscriptions)
                                                {
                                                    var sub = AdoSubscriptionManagementService.LoadSubscription(rdr);
                                                    AdoSubscriptionManagementService.s_cachedSubscriptions.Add(sub);
                                                }

                                }
                        }

                    // Next we want to loop through each of the filter expressions and evaluate them
                    List<Subscription.Core.Subscription> matchingSubscriptions;
                    lock(AdoSubscriptionManagementService.s_cachedSubscriptions)
                        matchingSubscriptions = AdoSubscriptionManagementService.s_cachedSubscriptions.FindAll(o => o.Match(Event));

                    // Next we want to register the new record with the constructed subscritpion
                    foreach (var ms in matchingSubscriptions)
                    {

                        // Apply policy if applicable and there are filters (masking indicators) that need to be applied
                        if (policyService != null)
                        {
                            // A dummy request container
                            SubscriptionQueryContainer rqo = new SubscriptionQueryContainer();
                            var hcId = identityService.FindParticipant(ms.ParticipantId); // get the hc participant for the subscription
                            // Add the components to the request filter
                            rqo.Add(hcId, "AUT", HealthServiceRecordSiteRoleType.AuthorOf, new List<Identifier>() { ms.ParticipantId });
                            var tEvent = policyService.ApplyPolicies(rqo, Event, new List<DetectedIssue>());
                            if (tEvent == null || tEvent.IsMasked) // the event is masked for this subscription, so don't include it
                            {
                                Trace.TraceInformation("Record '{0}' will not be published as the policy service has marked it masked", Event.Id);
                                continue;
                            }
                        }

                        // Create a command and add the record to the subscription list
                        using (IDbCommand cmd = conn.CreateCommand())
                        {
                            // Data
                            FilterExpression sub = new FilterExpression()
                            {
                                Terms = ms.MatchedData(Event)
                            };
                            MemoryStream stream = new MemoryStream();
                            XmlSerializer xsz = new XmlSerializer(typeof(FilterExpression));
                            xsz.Serialize(stream, sub);
                            stream.Flush();
                            stream.Seek(0, SeekOrigin.Begin);
                            byte[] dataBuffer = new byte[stream.Length];
                            stream.Read(dataBuffer, 0, (int)stream.Length);

                            // Command
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "add_sub_rec";
                            cmd.Connection = conn;
                            cmd.Transaction = tx;

                            // Parameters
                            IDataParameter pId = cmd.CreateParameter(),
                                pEvent = cmd.CreateParameter(),
                                pTime = cmd.CreateParameter(),
                                pMatch = cmd.CreateParameter();
                            pId.DbType = DbType.String;
                            pEvent.DbType = DbType.String;
                            pTime.DbType = DbType.DateTime;
                            pMatch.DbType = DbType.Binary;
                            pMatch.Direction = pTime.Direction = pId.Direction = pEvent.Direction = ParameterDirection.Input;
                            pId.ParameterName = "sub_id_in";
                            pEvent.ParameterName = "ent_id_in";
                            pTime.ParameterName = "crt_utc_in";
                            pMatch.ParameterName = "matched_in";
                            pId.Value = ms.SubscriptionId;
                            pEvent.Value = (Event as IIdentifiable).Identifier;
                            pTime.Value = this.Event.Timestamp;
                            pMatch.Value = dataBuffer;
                            cmd.Parameters.Add(pId);
                            cmd.Parameters.Add(pEvent);
                            cmd.Parameters.Add(pTime);
                            cmd.Parameters.Add(pMatch);

                            // Insert 
                            cmd.ExecuteNonQuery();

                        }
                    }

                    // Commit the transaction
                    tx.Commit();
                }
                catch (Exception e)
                {
                    if(tx != null) tx.Rollback();
                    Trace.TraceError("Error occurred while processing the subscription event");
                    Trace.TraceError(e.ToString());
                }
                finally
                {
                    conn.Close();
                    if (tx != null) tx.Dispose();
                    conn.Dispose();
                    
                    // Remove the work item
                    AdoSubscriptionManagementService.CleanWorkItem(this);
                    
                }
            }
        }

        // True when this object is disposed
        private bool m_disposed = false;

        // Service host
        private WebServiceHost m_svcHost = null;

        // Wait thread pool
        private WaitThreadPool m_threadPool = new WaitThreadPool();

        // Folder that will be used for commit 
        private static readonly string s_workItemBackupFolder = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Assembly.GetEntryAssembly().GetName().Name), typeof(AdoSubscriptionManagementService).Assembly.GetName().Name);

        // The host context
        private IServiceProvider m_context;

        // Configuration
        private ConfigurationSectionHandler m_configuration;

        /// <summary>
        /// Constructor
        /// </summary>
        public AdoSubscriptionManagementService()
        {
            // Create the commit folder
            if (!Directory.Exists(s_workItemBackupFolder))
                try
                {
                    Directory.CreateDirectory(s_workItemBackupFolder);
                }
                catch (Exception e)
                {
                    Trace.TraceError("Cannot create commit log directory, if the service is interrupted abruptly then syndicated items may not be synchronized");
                    Trace.TraceError(e.ToString());
                }

            // Serialization
            this.m_configuration = ConfigurationManager.GetSection("marc.hi.ehrs.svc.subscription") as ConfigurationSectionHandler;
            SyndicationService.s_configuration = this.m_configuration;

            // Service host
            this.m_svcHost = new WebServiceHost(typeof(SyndicationService), this.m_configuration.Address);
            
            #if DEBUG
            (this.m_svcHost.Description.Behaviors[typeof(ServiceDebugBehavior)] as ServiceDebugBehavior).IncludeExceptionDetailInFaults = true;
            #endif
            this.m_svcHost.Open();
        }

        /// <summary>
        /// Resume the queue
        /// </summary>
        private void ResumeQueue()
        {
            Trace.TraceInformation("Resuming queue backup...");
            foreach (var fil in Directory.GetFiles(s_workItemBackupFolder))
            {
                WorkItem wi = LoadWorkItem(fil);
                if(wi != null)
                    this.m_threadPool.QueueUserWorkItem(wi.DoWork, this.m_configuration.CreateConnection());
            }
        }

        /// <summary>
        /// Loads a work item from the filename
        /// </summary>
        private static WorkItem LoadWorkItem(string fileName)
        {
            // Save the item
            FileStream fs = null;
            try
            {
                fs = File.OpenRead(fileName);
                BinaryFormatter bf = new BinaryFormatter();
                return bf.Deserialize(fs) as WorkItem;
            }
            catch (Exception e)
            {
                Trace.TraceError("Could not load a backup of syndication work item '{0}'", fileName);
                Trace.TraceError(e.ToString());
                return null;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        /// <summary>
        /// Clean a work item
        /// </summary>
        private static void CleanWorkItem(WorkItem item)
        {
            try
            {

                File.Delete(Path.Combine(s_workItemBackupFolder, item.Id));
            }
            catch (Exception e)
            {
                Trace.TraceError("Could not remove backup of syndication work item '{0}'", item.Id);
                Trace.TraceError(e.ToString());
            }
        }

        /// <summary>
        /// Save a work item
        /// </summary>
        private static void SaveWorkItem(WorkItem item)
        {
            item.Id = Guid.NewGuid().ToString();
            // Save the item
            FileStream fs = null;
            try
            {
                fs = File.Create(Path.Combine(s_workItemBackupFolder, item.Id));
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, item);
            }
            catch (Exception e)
            {
                Trace.TraceError("Could not create a backup of syndication work item '{0}'", item.Id);
                Trace.TraceError(e.ToString());
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        #region ISubscriptionManagementService Members

        /// <summary>
        /// Publish a container
        /// </summary>
        public void PublishContainer(System.ComponentModel.IContainer record)
        {

            // validation
            if (record == null)
            {
                Trace.TraceError("Cannot publish a null container");
                return;
            }
            else if (!(record is HealthServiceRecordContainer))
            {
                Trace.TraceError("Cannot publish container of type '{0}'", record.GetType().FullName);
                return;
            }

            // Create work item
            WorkItem wi = new WorkItem() { Event = record as HealthServiceRecordContainer };
            // Save work item in case the service goes down
            SaveWorkItem(wi);

            // Now queue the work item
            this.m_threadPool.QueueUserWorkItem(wi.DoWork, this.m_configuration.CreateConnection());
        }

        #endregion

        #region IUsesHostContext Members

        /// <summary>
        /// Gets or sets the host context
        /// </summary>
        public IServiceProvider Context
        {
            get { return this.m_context; }
            set
            {
                if (this.m_context != value && value != null)
                {
                    this.m_context = value;
                    ApplicationContext.CurrentContext = value;
                    ResumeQueue();
                }
            }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Dispose of the object
        /// </summary>
        public void Dispose()
        {
            this.m_disposed = true;
            this.m_svcHost.Close();
            this.m_threadPool.Dispose();
        }

        #endregion


        #region ISubscriptionManagementService Members

        /// <summary>
        /// Register a subscription
        /// </summary>
        public bool RegisterSubscription(Guid subscriptionId, Core.FilterExpression subscriptionDefinition, HealthcareParticipant recipientOfDisclosure)
        {
            if(subscriptionDefinition == null || !subscriptionDefinition.Validate())
                throw new ArgumentNullException();
            else if(recipientOfDisclosure == null)
                throw new ArgumentNullException();

            var configService = Context.GetService(typeof(ISystemConfigurationService)) as ISystemConfigurationService;

            // Preferred identifier
            string provId = String.Empty;

            // Alternate identifiers
            foreach (var altId in recipientOfDisclosure.AlternateIdentifiers)
                if (altId.Domain == configService.OidRegistrar.GetOid("SUBSC_PID").Oid)
                    provId = altId.Identifier;

            // TODO: Create and register a provider identifier
            if (String.IsNullOrEmpty(provId))
                throw new ArgumentException("Cannot locate the recipientOfDisclosure");

            // Now register
            using(IDbConnection conn = this.m_configuration.CreateConnection())
                try
                {

                    // Serialize the "where clause"
                    Subscription.Core.Subscription sub = new Core.Subscription()
                    {
                        Where = subscriptionDefinition,
                        ParticipantId = new Identifier() { Identifier = provId, Domain = configService.OidRegistrar.GetOid("SUBSC_PID").Oid },
                        SubscriptionId = subscriptionId.ToString()
                    };
                    MemoryStream ms = new MemoryStream();
                    XmlSerializer xsz = new XmlSerializer(typeof(Subscription.Core.Subscription));
                    xsz.Serialize(ms, sub);
                    ms.Flush();
                    ms.Seek(0, SeekOrigin.Begin);
                    byte[] dataBuffer = new byte[ms.Length];
                    ms.Read(dataBuffer, 0, (int)ms.Length);

                    // Open the connection
                    conn.Open();

                    // Create a command
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "crt_sub";
                        cmd.Connection = conn;

                        // parameters
                        IDataParameter pSubId = cmd.CreateParameter(),
                            pFltr = cmd.CreateParameter(),
                            pAut = cmd.CreateParameter();
                        pSubId.Direction = pFltr.Direction = pAut.Direction = ParameterDirection.Input;
                        pSubId.DbType = pAut.DbType = DbType.String;
                        pFltr.DbType = DbType.Binary;
                        pSubId.ParameterName = "sub_id_in";
                        pFltr.ParameterName = "fltr_in";
                        pAut.ParameterName = "aut_ent_id_in";
                        pSubId.Value = subscriptionId;
                        pFltr.Value = dataBuffer;
                        pAut.Value = provId;
                        cmd.Parameters.Add(pSubId);
                        cmd.Parameters.Add(pFltr);
                        cmd.Parameters.Add(pAut);

                        // Now execute
                        cmd.ExecuteNonQuery();
                        
                        // Cache
                        lock (s_cachedSubscriptions)
                            s_cachedSubscriptions.Add(sub);

                        return true;
                    }
                }
                catch (Exception e)
                {
                    #if DEBUG
                    Trace.TraceError(e.ToString());
                    #else
                    Trace.TraceError(e.Message);
                    #endif

                    throw;
                }
                finally
                {
                    conn.Close();
                }
        }

        /// <summary>
        /// Get subscription
        /// </summary>
        public Subscription.Core.Subscription GetSubscription(Guid subscriptionId)
        {
            var configService = Context.GetService(typeof(ISystemConfigurationService)) as ISystemConfigurationService;

            // Now register
            using (IDbConnection conn = this.m_configuration.CreateConnection())
                try
                {

                    // Open the connection
                    conn.Open();

                    return LoadSubscription(conn, null, subscriptionId);
                }
                catch (Exception e)
                {
                    #if DEBUG
                    Trace.TraceError(e.ToString());
                    #else
                    Trace.TraceError(e.Message);
                    #endif

                    throw;
                }
                finally
                {
                    conn.Close();
                }
        }

        /// <summary>
        /// Subscription results
        /// </summary>
        public SubscriptionResult[] CheckSubscription(Guid subscriptionId, bool newOnly)
        {
            List<SubscriptionResult> result = new List<SubscriptionResult>();

            var configService = Context.GetService(typeof(ISystemConfigurationService)) as ISystemConfigurationService;
            
            // Now register
            using(IDbConnection conn = this.m_configuration.CreateConnection())
                try
                {

                    // Open the connection
                    conn.Open();
                   
                    // Create a command
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = newOnly ? "get_sub_rec_new" : "get_sub_rec";
                        cmd.Connection = conn;

                        // parameters
                        IDataParameter pSubId = cmd.CreateParameter(),
                            pMax = cmd.CreateParameter();
                        pSubId.Direction = pMax.Direction = ParameterDirection.Input;
                        pSubId.DbType = DbType.String;
                        pMax.DbType = DbType.Decimal;
                        pSubId.ParameterName = "sub_id_in";
                        pMax.ParameterName = "limit_in";
                        pSubId.Value = subscriptionId;
                        pMax.Value = Int32.MaxValue;
                        cmd.Parameters.Add(pSubId);

                        if(!newOnly)
                            cmd.Parameters.Add(pMax);

                        // Now execute
                        using (IDataReader rdr = cmd.ExecuteReader())
                            while (rdr.Read())
                            {
                                var r = new SubscriptionResult()
                                {
                                    Id = new VersionedDomainIdentifier()
                                    {
                                        Domain = configService.OidRegistrar.GetOid("SUBSC_RID").Oid,
                                        Identifier = rdr["ent_id"].ToString()
                                    },
                                    Created = Convert.ToDateTime(rdr["crt_utc"]),
                                    Published = Convert.ToDateTime(rdr["pub_utc"]),
                                    FeedItemId = Convert.ToDecimal(rdr["rec_id"])
                                };

                                // Deserialize the matched parameters
                                byte[] match = (byte[])rdr["matched"];
                                using (var ms = new MemoryStream(match))
                                {
                                    XmlSerializer xsz = new XmlSerializer(typeof(FilterExpression));
                                    r.Match = xsz.Deserialize(ms) as FilterExpression;
                                }
                                result.Add(r);
                            }
                    }
                    return result.ToArray();
                }
                catch (Exception e)
                {
                    #if DEBUG
                    Trace.TraceError(e.ToString());
                    #else
                    Trace.TraceError(e.Message);
                    #endif

                    throw;
                }
                finally
                {
                    conn.Close();
                }
        }

        #endregion

        #region ISubscriptionManagementService Members

        /// <summary>
        /// Get subscription item
        /// </summary>
        public SubscriptionResult GetSubscriptionItem(Guid subscriptionId, decimal feedItemId)
        {
            SubscriptionResult result = null;

            var configService = Context.GetService(typeof(ISystemConfigurationService)) as ISystemConfigurationService;

            // Now register
            using (IDbConnection conn = this.m_configuration.CreateConnection())
                try
                {

                    // Open the connection
                    conn.Open();

                    // Create a command
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "get_sub_itm";
                        cmd.Connection = conn;

                        // parameters
                        IDataParameter pRecId = cmd.CreateParameter(),
                            pSubId = cmd.CreateParameter();
                        pSubId.Direction = pRecId.Direction = ParameterDirection.Input;
                        pSubId.DbType = DbType.String;
                        pRecId.DbType = DbType.Decimal;
                        pSubId.ParameterName = "sub_id_in";
                        pRecId.ParameterName = "rec_id_in";
                        pSubId.Value = subscriptionId;
                        pRecId.Value = feedItemId;
                        cmd.Parameters.Add(pSubId);
                        cmd.Parameters.Add(pRecId);


                        // Now execute
                        using (IDataReader rdr = cmd.ExecuteReader())
                            if(rdr.Read())
                            {
                                result = new SubscriptionResult()
                                {
                                    Id = new VersionedDomainIdentifier()
                                    {
                                        Domain = configService.OidRegistrar.GetOid("SUBSC_RID").Oid,
                                        Identifier = rdr["ent_id"].ToString()
                                    },
                                    Created = Convert.ToDateTime(rdr["crt_utc"]),
                                    Published = Convert.ToDateTime(rdr["pub_utc"]),
                                    FeedItemId = Convert.ToDecimal(rdr["rec_id"])
                                };

                            }
                    }
                    return result;
                }
                catch (Exception e)
                {
#if DEBUG
                    Trace.TraceError(e.ToString());
#else
                    Trace.TraceError(e.Message);
#endif

                    throw;
                }
                finally
                {
                    conn.Close();
                }
        }

        #endregion
    }
}
