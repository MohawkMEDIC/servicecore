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
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Core.DataTypes
{
    /// <summary>
    /// Event Identifier
    /// </summary>
    public enum EventIdentifierType
    {
        ProvisioningEvent,
        MedicationEvent,
        ResourceAssignment,
        CareEpisode,
        CareProtocol,
        ProcedureRecord,
        Query,
        PatientRecord,
        OrderRecord,
        NetowrkEntry,
        Import,
        Export,
        ApplicationActivity
    }

    /// <summary>
    /// Represents data related to an audit event
    /// </summary>
    [Serializable]
    public class AuditData
    {
        /// <summary>
        /// Event timestamp
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Identifies the action code
        /// </summary>
        public ActionType? ActionCode { get; set; }

        /// <summary>
        /// Identifies the outcome of the event
        /// </summary>
        public OutcomeIndicator? Outcome { get; set; }

        /// <summary>
        /// Identifies the event
        /// </summary>
        public EventIdentifierType EventIdentifier { get; set; }

        /// <summary>
        /// Identifies the type of event
        /// </summary>
        public CodeValue EventTypeCode { get; set; }

        /// <summary>
        /// Represents the actors within the audit event
        /// </summary>
        public List<AuditActorData> Actors { get; set; }

        /// <summary>
        /// Represents other objects of interest
        /// </summary>
        public List<AuditableObject> AuditableObjects { get; set; }

        /// <summary>
        /// Default CTOR
        /// </summary>
        public AuditData()
        {
            this.Timestamp = DateTime.Now;
            this.Actors = new List<AuditActorData>();
            this.AuditableObjects = new List<AuditableObject>();
        }

        /// <summary>
        /// Create a new instance of the AuditData class
        /// </summary>
        public AuditData(DateTime timeStamp, ActionType actionCode, OutcomeIndicator outcome, 
            EventIdentifierType eventIdentifier, CodeValue eventTypeCode)
            : this()
        {
            this.Timestamp = timeStamp;
            this.ActionCode = actionCode;
            this.Outcome = outcome;
            this.EventIdentifier = eventIdentifier;
            this.EventTypeCode = eventTypeCode;
            this.Actors = new List<AuditActorData>();
            this.AuditableObjects = new List<AuditableObject>();
        }
    }
}
