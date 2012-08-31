/* 
 * Copyright 2008-2011 Mohawk College of Applied Arts and Technology
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
 * User: Justin Fyfe
 * Date: 08-24-2011
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MARC.HI.EHRS.SVC.Core.DataTypes;

namespace MARC.HI.EHRS.SVC.Core.Services
{
    /// <summary>
    /// Defines a structure for query persistence services
    /// </summary>
    public interface IQueryPersistenceService : IUsesHostContext
    {

        /// <summary>
        /// Register a query set 
        /// </summary>
        /// <param name="queryId">The unique identifier for the query</param>
        /// <param name="results">The results to be stored in the query</param>
        /// <param name="tag">A user tag for the query result set. Can be used to determine
        /// the type of data being returned</param>
        bool RegisterQuerySet(string queryId, VersionedDomainIdentifier[] results, object tag);

        /// <summary>
        /// Returns true if the query identifier is already registered
        /// </summary>
        /// <param name="queryId"></param>
        /// <returns></returns>
        bool IsRegistered(string queryId);

        /// <summary>
        /// Get query results from the query set result store
        /// </summary>
        /// <param name="queryId">The identifier for the query</param>
        /// <param name="nRecords">The number of records to pop</param>
        VersionedDomainIdentifier[] GetQueryResults(string queryId, int startRecord, int nRecords);

        /// <summary>
        /// Get the query tag value from the result store
        /// </summary>
        object GetQueryTag(string queryId);

        /// <summary>
        /// Count the number of remaining query results
        /// </summary>
        /// <param name="queryId">Unique identifier for the query to count remaining results</param>
        long QueryResultTotalQuantity(string queryId);


    }
}
