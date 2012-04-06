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
using MARC.HI.EHRS.SVC.Core.DataTypes;
using System.ComponentModel;

namespace MARC.HI.EHRS.SVC.Core.Services
{

    /// <summary>
    /// Data persistence modes
    /// </summary>
    public enum DataPersistenceMode
    {
        /// <summary>
        /// Debug mode, this means nothing is actually committed to the database
        /// </summary>
        Debugging,
        /// <summary>
        /// Production, everything is for reals
        /// </summary>
        Production
    }

    /// <summary>
    /// Interface that defines a data persistence service which is used to 
    /// store, query, update and list data
    /// </summary>
    public interface IDataPersistenceService : IUsesHostContext
    {

        /// <summary>
        /// Store the specified <see cref="T:System.ComponentModel.IContainer"/> into
        /// the perminant data store. 
        /// </summary>
        /// <param name="storageData">The container data to store</param>
        /// <returns>The identifiers representing the identifier of the stored container object</returns>
        /// <exception cref="System.ArgumentException">Thrown when the storage data container is of an unknown type</exception>
        /// <exception cref="System.InvalidOperationException">Thrown when there is not sufficient data known to store the container</exception>
        VersionedDomainIdentifier StoreContainer(IContainer storageData, DataPersistenceMode mode);

        /// <summary>
        /// Update the specified <see cref="T:System.ComponentModel.IContainer"/> into the
        /// perminent data store. 
        /// </summary>
        /// <remarks>The <paramref name="storageData"/> container should have some form of identification to permit the update process</remarks>
        /// <param name="storageData">The data that is to be updated. Should have some form of identification</param>
        /// <returns>The </returns>
        /// <exception cref="System.KeyNotFoundException">Thrown when the persistence service cannot determine the record to update</exception>
        /// <exception cref="System.ArgumentException">Thrown when the container is of an unknown type</exception>
        VersionedDomainIdentifier UpdateContainer(IContainer storageData, DataPersistenceMode mode);

        /// <summary>
        /// Get the object represention of the specified container as specified by <paramref name="containerId"/>
        /// </summary>
        /// <param name="containerId">The versioned domain identifier of the container to retrieve</param>
        /// <returns>An IContainer object that represents the stored container</returns>
        /// <exception cref="System.KeyNotFoundException">Thrown when the <paramref name="containerId"/> is not present in the database</exception>
        IContainer GetContainer(VersionedDomainIdentifier containerId, bool loadFast);

       }
}
