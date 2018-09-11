/*
 * Copyright 2010-2018 Mohawk College of Applied Arts and Technology
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
 * Date: 1-9-2017
 */

using MARC.HI.EHRS.SVC.Core.Data;
using MARC.HI.EHRS.SVC.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MARC.HI.EHRS.SVC.Core.Event
{

    /// <summary>
    /// Secure access to data event arguments
    /// </summary>
    public abstract class SecureAccessEventArgs : EventArgs
    {

        /// <summary>
        /// Creates a new secured access event args
        /// </summary>
        public SecureAccessEventArgs(IPrincipal claimsPrincipal)
        {
            this.Principal = claimsPrincipal;
        }

        /// <summary>
        /// Gets the authorization context (claims, users, etc.) associated with the event
        /// </summary>
        public IPrincipal Principal { get; private set; }
    }

    /// <summary>
    /// Represents a generic structure for a pre-persistence event 
    /// </summary>
    /// <typeparam name="TData">The type of data being persisted</typeparam>
    public class PrePersistenceEventArgs<TData> : SecureAccessEventArgs
    {

        /// <summary>
        /// Creates a new persistence event args instance
        /// </summary>
        /// <param name="data"></param>
        public PrePersistenceEventArgs(TData data, IPrincipal authContext = null) : base(authContext)
        {
            this.Data = data;
        }
        /// <summary>
        /// True if the handler deems the persistence event should be cancelled
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Sets the mode of deta persistence
        /// </summary>
        public TransactionMode Mode { get; set; }

        /// <summary>
        /// The data being stored
        /// </summary>
        public TData Data { get; private set; }
    }

    /// <summary>
    /// Represents an event argument class used for post-persistence events
    /// </summary>
    public class PostPersistenceEventArgs<TData> : SecureAccessEventArgs
    {
        /// <summary>
        /// Creates a new persistence event args instance
        /// </summary>
        /// <param name="data"></param>
        public PostPersistenceEventArgs(TData data, IPrincipal authContext = null) : base(authContext)
        {
            this.Data = data;
        }

        /// <summary>
        /// Sets the mode of deta persistence
        /// </summary>
        public TransactionMode Mode { get; set; }

        /// <summary>
        /// The data being stored
        /// </summary>
        public TData Data { get; private set; }

    }

    /// <summary>
    /// Represents event data associated with a data retrieval operation
    /// </summary>
    public class PreRetrievalEventArgs<TData> : SecureAccessEventArgs
    {

        /// <summary>
        /// Creates a new pre-retrieval event args object
        /// </summary>
        public PreRetrievalEventArgs(Object identifier, IPrincipal authContext = null) : base(authContext)
        {
            this.Identifier = identifier;
        }

        /// <summary>
        /// The identifier
        /// </summary>
        public Object Identifier { get; set; }

        /// <summary>
        /// Allows the handler to cancel the operation
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets the data retrieved
        /// </summary>
        public TData OverrideResult { get; set; }
    }

    /// <summary>
    /// A class used to store event information related to post-retrieval events
    /// </summary>
    public class PostRetrievalEventArgs<TData> : SecureAccessEventArgs
    {

        /// <summary>
        /// Post retrieval data
        /// </summary>
        public PostRetrievalEventArgs(TData data, IPrincipal authContext = null) : base(authContext)
        {
            this.Data = data;
        }

        /// <summary>
        /// Gets the data retrieved
        /// </summary>
        public TData Data { get; set; }
    } 

    /// <summary>
    /// Stores data related to the events and query
    /// </summary>
    public class PreQueryEventArgs<TData> : SecureAccessEventArgs
    {
        /// <summary>
        /// Creates new pre-query event args
        /// </summary>
        public PreQueryEventArgs(Expression<Func<TData, bool>> predicate, Guid? queryId = null, int offset = 0, int? count = null, IPrincipal authContext = null) : base(authContext)
        {
            this.Query = predicate;
            this.Offset = offset;
            this.Count = count;
            this.QueryId = queryId;
        }

        /// <summary>
        /// When cancelling the action allows the callee to override the results returned to the service
        /// </summary>
        public IEnumerable<TData> OverrideResults { get; set; }

        /// <summary>
        /// Gets or sets the supplied query id
        /// </summary>
        public Guid? QueryId { get; set; }

        /// <summary>
        /// When specified, overrides the total results output parameters
        /// </summary>
        public int? OverrideTotalResults { get; set; }

        /// <summary>
        /// The expression tree representing the query parameters
        /// </summary>
        public Expression<Func<TData, bool>> Query { get; set; }

        /// <summary>
        /// Gets the offset
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Get sor sets the count of objects
        /// </summary>
        public int? Count { get; set; }

        /// <summary>
        /// True if the callee wishes the caller to cancel the operation
        /// </summary>
        public bool Cancel { get; set; }
    }

    /// <summary>
    /// Fired for all post-query events
    /// </summary>
    public class PostQueryEventArgs<TData> : SecureAccessEventArgs
    {

        /// <summary>
        /// Creates a new post query event args
        /// </summary>
        public PostQueryEventArgs(Expression<Func<TData, bool>> query, IQueryable<TData> results, IPrincipal authContext = null) : base(authContext)
        {
            this.Query = query;
            this.Results = results;
        }
        
        /// <summary>
        /// Except results
        /// </summary>
        public void Except(IEnumerable<TData> results)
        {
            this.Results = this.Results.Except(results);
        }        

        /// <summary>
        /// Gets the actual query used for the event
        /// </summary>
        public Expression<Func<TData, bool>> Query { get; private set; }

        /// <summary>
        /// Gets the results of the query
        /// </summary>
        public IQueryable<TData> Results { get; set; }
    }
}
