/**
 * Copyright 2012-2013 Mohawk College of Applied Arts and Technology
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
using System.ComponentModel;
using MARC.HI.EHRS.SVC.Core.Data;
using MARC.HI.EHRS.SVC.Core.Event;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Principal;

namespace MARC.HI.EHRS.SVC.Core.Services
{

    /// <summary>
    /// Data persistence modes
    /// </summary>
    public enum TransactionMode
    {
        /// <summary>
        /// Inherit the persistence mode from a parent context
        /// </summary>
        None,
        /// <summary>
        /// Debug mode, this means nothing is actually committed to the database
        /// </summary>
        Rollback,
        /// <summary>
        /// Production, everything is for reals
        /// </summary>
        Commit
    }
    
    /// <summary>
    /// Interface that defines a data persistence service which is used to 
    /// store, query, update and list data
    /// </summary>
    public interface IDataPersistenceService<TData>
    {

        /// <summary>
        /// Store the specified <see cref="T:System.ComponentModel.IContainer"/> into
        /// the perminant data store. 
        /// </summary>
        /// <param name="storageData">The container data to store</param>
        /// <returns>The identifiers representing the identifier of the stored container object</returns>
        /// <exception cref="System.ArgumentException">Thrown when the storage data container is of an unknown type</exception>
        /// <exception cref="System.InvalidOperationException">Thrown when there is not sufficient data known to store the container</exception>
        TData Insert(TData storageData, IPrincipal principal, TransactionMode mode);

        /// <summary>
        /// Update the specified <see cref="T:System.ComponentModel.IContainer"/> into the
        /// perminent data store. 
        /// </summary>
        /// <remarks>The <paramref name="storageData"/> container should have some form of identification to permit the update process</remarks>
        /// <param name="storageData">The data that is to be updated. Should have some form of identification</param>
        /// <returns>The </returns>
        /// <exception cref="System.KeyNotFoundException">Thrown when the persistence service cannot determine the record to update</exception>
        /// <exception cref="System.ArgumentException">Thrown when the container is of an unknown type</exception>
        TData Update(TData storageData, IPrincipal principal, TransactionMode mode);
        
        /// <summary>
        /// Obsoletes a particular container object
        /// </summary>
        TData Obsolete(TData storageData, IPrincipal principal, TransactionMode mode);

        /// <summary>
        /// Get the object represention of the specified container as specified by <paramref name="containerId"/>
        /// </summary>
        /// <param name="containerId">The versioned domain identifier of the container to retrieve</param>
        /// <returns>An IContainer object that represents the stored container</returns>
        /// <exception cref="System.KeyNotFoundException">Thrown when the <paramref name="containerId"/> is not present in the database</exception>
        TData Get<TIdentifier>(Identifier<TIdentifier> containerId, IPrincipal principal, bool loadFast);

        /// <summary>
        /// Counts the number of records which would be returned by the specified query
        /// </summary>
        int Count(Expression<Func<TData, bool>> query, IPrincipal authContext);

        /// <summary>
        /// Query the data persistence store for data
        /// </summary>
        IEnumerable<TData> Query(Expression<Func<TData, bool>> query, IPrincipal authContext);

        /// <summary>
        /// Query the data persistence store for data
        /// </summary>
        IEnumerable<TData> Query(Expression<Func<TData, bool>> query, int offset, int? count, IPrincipal authContext, out int totalCount);

        /// <summary>
        /// Fired prior to an insertion into the database
        /// </summary>
        event EventHandler<PrePersistenceEventArgs<TData>> Inserting;
        /// <summary>
        /// Fired after an insertion to the database is completed
        /// </summary>
        event EventHandler<PostPersistenceEventArgs<TData>> Inserted;
        /// <summary>
        /// Fired prior to an update occurring
        /// </summary>
        event EventHandler<PrePersistenceEventArgs<TData>> Updating;
        /// <summary>
        /// Fired after an update has completed
        /// </summary>
        event EventHandler<PostPersistenceEventArgs<TData>> Updated;
        /// <summary>
        /// Fired prior to a record being obsoleted
        /// </summary>
        event EventHandler<PrePersistenceEventArgs<TData>> Obsoleting;
        /// <summary>
        /// Fired after a record has been obsoleted
        /// </summary>
        event EventHandler<PostPersistenceEventArgs<TData>> Obsoleted;
        /// <summary>
        /// Fired prior to a record being retrieved
        /// </summary>
        event EventHandler<PreRetrievalEventArgs<TData>> Retrieving;
        /// <summary>
        /// Fired after a record has been retrieved
        /// </summary>
        event EventHandler<PostRetrievalEventArgs<TData>> Retrieved;
        /// <summary>
        /// Fired prior to a record being queried
        /// </summary>
        event EventHandler<PreQueryEventArgs<TData>> Querying;
        /// <summary>
        /// Fired after a record has been retrieved
        /// </summary>
        event EventHandler<PostQueryEventArgs<TData>> Queried;
    }
}
