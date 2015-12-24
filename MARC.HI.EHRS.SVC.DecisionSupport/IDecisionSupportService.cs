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

using System.Collections.Generic;
using MARC.HI.EHRS.SVC.Core.ComponentModel;
using MARC.HI.EHRS.SVC.Core.DataTypes;
using MARC.HI.EHRS.SVC.Core.Issues;

namespace MARC.HI.EHRS.SVC.DecisionSupport
{
    /// <summary>
    /// Decision support service
    /// </summary>
    public interface IDecisionSupportService
    {
        /// <summary>
        /// Called prior to a message being persisted to the database
        /// </summary>
        /// <remarks>This hook would most likely be used in business rule validation prior to the message
        /// being written to the database. To halt the storage procedure throw an exception from this function</remarks>
        List<DetectedIssue> RecordPersisting(HealthServiceRecordComponent hsr);

        /// <summary>
        /// Called after the record has been persisted
        /// </summary>
        /// <remarks>This hook would most likely be used in business roles that result in decision support and
        /// can determing the "big-picture" of the particular record</remarks>
        List<DetectedIssue> RecordPersisted(HealthServiceRecordComponent hsr);

        /// <summary>
        /// Record is being retrieved
        /// </summary>
        /// <remarks>This function is called prior to connecting to the database
        /// and de-persisting a record</remarks>
        List<DetectedIssue> RetrievingRecord(Identifier recordId);

        /// <summary>
        /// Record has been retrieved
        /// </summary>
        /// <remarks>This function is called after a record has been retrieved from the database</remarks>
        List<DetectedIssue> RetrievedRecord(HealthServiceRecordComponent hsr);
    }
}
