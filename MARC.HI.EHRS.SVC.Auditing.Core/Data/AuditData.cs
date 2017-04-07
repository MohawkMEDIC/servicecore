
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

namespace MARC.HI.EHRS.SVC.Auditing.Data
{
    /// <summary>
    /// Event Identifier
    /// </summary>
    [XmlType(nameof(EventIdentifierType), Namespace = "http://marc-hi.ca/svc/audit")]
    public enum EventIdentifierType
    {
        [XmlEnum("provision")]
        ProvisioningEvent,
        [XmlEnum("medication")]
        MedicationEvent,
        [XmlEnum("resource")]
        ResourceAssignment,
        [XmlEnum("careep")]
        CareEpisode,
        [XmlEnum("careprotocol")]
        CareProtocol,
        [XmlEnum("procedure")]
        ProcedureRecord,
        [XmlEnum("query")]
        Query,
        [XmlEnum("patient")]
        PatientRecord,
        [XmlEnum("order")]
        OrderRecord,
        [XmlEnum("network")]
        NetowrkEntry,
        [XmlEnum("import")]
        Import,
        [XmlEnum("export")]
        Export,
        [XmlEnum("application")]
        ApplicationActivity,
        [XmlEnum("security")]
        SecurityAlert,
        [XmlEnum("auth")]
        UserAuthentication,
        [XmlEnum("btg")]
        EmergencyOverrideStarted,
        [XmlEnum("restrictedFn")]
        UseOfRestrictedFunction,
        [XmlEnum("login")]
        Login,
        [XmlEnum("logout")]
        Logout
    }

    /// <summary>
    /// Represents data related to an audit event
    /// </summary>
    [XmlType(nameof(AuditData), Namespace = "http://marc-hi.ca/svc/audit")]
    [XmlRoot("audit", Namespace = "http://marc-hi.ca/audit")]
    public class AuditData
    {
        /// <summary>
        /// Event timestamp
        /// </summary>
        [XmlAttribute("timestamp")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Identifies the action code
        /// </summary>
        [XmlAttribute("action")]
        public ActionType? ActionCode { get; set; }

        /// <summary>
        /// Identifies the outcome of the event
        /// </summary>
        [XmlAttribute("outcome")]
        public OutcomeIndicator? Outcome { get; set; }

        /// <summary>
        /// Identifies the event
        /// </summary>
        [XmlAttribute("event")]
        public EventIdentifierType EventIdentifier { get; set; }

        /// <summary>
        /// Identifies the type of event
        /// </summary>
        [XmlElement("type")]
        public AuditCode EventTypeCode { get; set; }

        /// <summary>
        /// Represents the actors within the audit event
        /// </summary>
        [XmlElement("actor")]
        public List<AuditActorData> Actors { get; set; }

        /// <summary>
        /// Represents other objects of interest
        /// </summary>
        [XmlElement("object")]
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
            EventIdentifierType eventIdentifier, AuditCode eventTypeCode)
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
