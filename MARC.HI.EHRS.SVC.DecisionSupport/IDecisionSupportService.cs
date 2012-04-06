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
        List<DetectedIssue> RetrievingRecord(DomainIdentifier recordId);

        /// <summary>
        /// Record has been retrieved
        /// </summary>
        /// <remarks>This function is called after a record has been retrieved from the database</remarks>
        List<DetectedIssue> RetrievedRecord(HealthServiceRecordComponent hsr);
    }
}
