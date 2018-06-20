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

using System.Collections.Generic;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Core.Configuration.DbXml
{
    /// <summary>
    /// Represents an abstract command to do something against the database
    /// </summary>
    [XmlType("Command", Namespace = "http://marc-hi.ca/plugin/dbxml")]
    public abstract class Command : DbXmlBase
    {
        /// <summary>
        /// A series of "where" clauses which when all execute to "TRUE" instruct the
        /// command text to be executed
        /// </summary>
        [XmlArray("condition")]
        [XmlArrayItem("sqlCommand", Type = typeof(SqlCommand))]
        public List<Command> Condition { get; set; }

    }
}