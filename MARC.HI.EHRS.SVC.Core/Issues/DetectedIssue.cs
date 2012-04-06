using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Core.Issues
{

    /// <summary>
    /// Identifies the priority of an issue
    /// </summary>
    public enum IssuePriorityType
    {
        [XmlEnum("E")]
        Error,
        [XmlEnum("I")]
        Informational,
        [XmlEnum("W")]
        Warning
    }

    /// <summary>
    /// Identifies the severity of an issue
    /// </summary>
    public enum IssueSeverityType
    {
        [XmlEnum("M")]
        Moderate,
        [XmlEnum("H")]
        High,
        [XmlEnum("L")]
        Low
    }

    /// <summary>
    /// Identifies the type of management that can be used
    /// to mitigate the issue
    /// </summary>
    public enum ManagementType
    {
        TherapyAppropriate,
        ProvidedPatientEducation,
        AddedConcurrentTherapy,
        TermporarilySuspendedTherapy,
        StoppedTherapy,
        SupplyAppropriate,
        Replacement,
        VacationSupply,
        WeekendSupply,
        LeaveOfAbsence,
        ConsultedSupplier,
        AssessedPatient,
        AdditionalQuantityOnSeparateDispense,
        AuthorizationConfirmed,
        AppropriateIndicationDiagnosis,
        PriorTherapyDocumented,
        AugmentCurrentSupplyOnHand,
        PatientExplanation,
        ConsultedOtherSource,
        ConsultedProvider,
        PrescriberDeclinedChange,
        IteractingTherapyNoLongActive,
        OtherActionTaken,
        InstitutedOngionMonitoring,
        EmergencyAuthorizationOverride
    }

    /// <summary>
    /// Identifies types of issues
    /// </summary>
    public enum IssueType
    {
        CommonlyAbused,
        AgeAlert,
        AllergyAlert,
        AlreadyPerformed,
        BusinessConstraintViolation,
        ComplianceAlert,
        ConditionAlert,
        ReactionAlert,
        DuplicateTherapyAlert,
        DuplicateGenericAlert,
        FoodInteractionAlert,
        PotentialFraud,
        GeneticAlert,
        GenderAlert,
        HeldOrSuspended,
        NotLegal,
        IntoleranceAlert,
        DetectedIssue,
        LactationAlert,
        InsufficientAuthorization,
        NoLongerActionable,
        ObservationAlert,
        PregnancyAlert,
        PreviouslyIneffective,
        RelatedAllergyAlert,
        WeightAlert
    }
    /// <summary>
    /// A class representing a business issue
    /// </summary>
    [Serializable][XmlType("DetectedIssue")]
    public class DetectedIssue
    {

        /// <summary>
        /// Rule violation type
        /// </summary>
        [XmlAttribute("type")]
        public IssueType Type { get; set; }

        /// <summary>
        /// Textual information about the issue
        /// </summary>
        [XmlText]
        public string Text { get; set; }

        /// <summary>
        /// Identifies the priority of the issue
        /// </summary>
        [XmlAttribute("priority")]
        public IssuePriorityType Priority { get; set; }

        /// <summary>
        /// Identifies mitigation for this issue
        /// </summary>
        [XmlIgnore]
        public ManagementType? MitigatedBy { get; set; }

        /// <summary>
        /// Severity of the issue
        /// </summary>
        [XmlAttribute("severity")]
        public IssueSeverityType Severity { get; set; }
    }
}
