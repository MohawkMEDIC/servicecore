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
using System.Data;

namespace MARC.HI.EHRS.QM.Core.Exception
{
    /// <summary>
    /// Represents a query persistence exception
    /// </summary>
    public class QueryPersistenceException : DataException
    {
        /// <summary>
        /// Creates a new instance of the query persistence exception
        /// </summary>
        public QueryPersistenceException() : base() { }

        /// <summary>
        /// Creates a new instance of the query persistence exception
        /// </summary>
        public QueryPersistenceException(string message) : base(message) { }

        /// <summary>
        /// Creates a new instance of the query persistence exception
        /// </summary>
        public QueryPersistenceException(string message, System.Exception causedBy) : base(message, causedBy) { }
    }
}
