using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MARC.HI.EHRS.SVC.Core.ComponentModel
{
    /// <summary>
    /// Identifies the record site role
    /// </summary>
    [Flags]
    public enum HealthServiceRecordSiteRoleType
    {
        /// <summary>
        /// Identifies the role as inversed
        /// </summary>
        Inverse = 0x0000001, 
        /// <summary>
        /// The component is logically linked as a subordinate contained 
        /// within the container
        /// </summary>
        ComponentOf = 0x0000002,
        /// <summary>
        /// The component participates within the container
        /// </summary>
        ParticipantIn = 0x0000004,
        /// <summary>
        /// The component is represents a replacement of the container
        /// </summary>
        ReplacementOf = 0x0000008,
        /// <summary>
        /// The component reports on the container
        /// </summary>
        ReportsOn = 0x0000010,
        /// <summary>
        /// The component fulfills the request made in the container
        /// </summary>
        Fulfills = 0x0000020,
        /// <summary>
        /// The component is the author of the container
        /// </summary>
        AuthorOf = 0x0000040,
        /// <summary>
        /// The component is responsible for the execution of the container act
        /// </summary>
        ResponsibleFor = 0x0000080,
        /// <summary>
        /// The component is the target of the container act
        /// </summary>
        TargetOf = 0x0000100,
        /// <summary>
        /// The component is to be notified of the container act
        /// </summary>
        NotifiedOf = 0x0000200,
        /// <summary>
        /// The component is an informant to the container act
        /// </summary>
        InformantTo = 0x0000400,
        /// <summary>
        /// The component is an enterer of the container act
        /// </summary>
        EntererOf = 0x0000800,
        /// <summary>
        /// The component is the occurence location of the act
        /// </summary>
        PlaceOfOccurence = 0x0001000,
        /// <summary>
        /// The component is the place of entry for the container act
        /// </summary>
        PlaceOfEntry = 0x0002000,
        /// <summary>
        /// The component is the subject of the act
        /// </summary>
        SubjectOf = 0x0004000,
        /// <summary>
        /// The component is the attestor of the act
        /// </summary>
        Attestor = 0x0008000,
        /// <summary>
        /// The component is an official representation of the container
        /// </summary>
        RepresentitiveOf = 0x0010000,
        /// <summary>
        /// The component represents a reference for the container
        /// </summary>
        ReferenceFor = 0x0020000,
        /// <summary>
        /// The component represents the discharger for the container
        /// </summary>
        Discharger = 0x0040000,
        /// <summary>
        /// The component represents an outcome of the container
        /// </summary>
        OutcomeOf = 0x0080000,
        /// <summary>
        /// The component represents a query filter condition
        /// </summary>
        FilterOf = 0x0100000,
        /// <summary>
        /// The component represents an older version of the container
        /// </summary>
        OlderVersionOf = 0x0200000,
        /// <summary>
        /// The component is a comment about the container
        /// </summary>
        CommentOn = 0x0400000,
        /// <summary>
        /// The component represents the place of record for the container
        /// </summary>
        PlaceOfRecord = 0x0800000,
        /// <summary>
        /// The component is a reason for the container occuring
        /// </summary>
        ReasonFor = 0x1000000,
        /// <summary>
        /// The component is a shadow (or alternate to) the container
        /// </summary>
        /// <remarks>Used when querying</remarks>
        AlternateTo = 0x8000000,
        /// <summary>
        /// The component is the performer of
        /// </summary>
        PerformerOf = ResponsibleFor | ParticipantIn,
        /// <summary>
        /// The admitter of the 
        /// </summary>
        AttenderFor = ResponsibleFor | ParticipantIn,
        /// <summary>
        /// Consent override
        /// </summary>
        ConsentOverrideFor = Inverse | FilterOf
    }
}
