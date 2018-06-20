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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MARC.HI.EHRS.SVC.Core.Data
{
    /// <summary>
    /// Identifier
    /// </summary>
    public class Identifier
    {

    }

    /// <summary>
    /// Represents an identifier used for storing / retrieving data
    /// </summary>
    public class Identifier<TIdentifier> : Identifier
    {

        /// <summary>
        /// Creates a new empty identifier
        /// </summary>
        public Identifier()
        {

        }

        /// <summary>
        /// Creates a new identifier with the specified identifier
        /// </summary>
        public Identifier(TIdentifier id)
        {
            this.Id = id;
        }

        /// <summary>
        /// Creates a new identifier with specified identifier and version
        /// </summary>
        public Identifier(TIdentifier id, TIdentifier versionId) : this(id)
        {
            this.VersionId = versionId;
        }

        /// <summary>
        /// Create a new identifier with specified id and AA
        /// </summary>
        public Identifier(TIdentifier id, OidData assigningAuthority) : this(id)
        {
            this.AssigningAuthority = assigningAuthority;
        }

        /// <summary>
        /// Get the assigning authority of the identifier
        /// </summary>
        public OidData AssigningAuthority { get; set; }
        /// <summary>
        /// Represents the identifier of the object
        /// </summary>
        public TIdentifier Id { get; set; }
        /// <summary>
        /// Represents the version of the object
        /// </summary>
        public TIdentifier VersionId { get; set; }
    }
}
