/*
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

namespace MARC.HI.EHRS.SVC.Core.Issues
{

    /// <summary>
    /// Identifies the priority of an issue
    /// </summary>
    [Flags]
    public enum IssuePriorityType
    {
        [XmlEnum("E")]
        Error = 0x1,
        [XmlEnum("I")]
        Informational = 0x2,
        [XmlEnum("W")]
        Warning = 0x4
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
    [Serializable]
    [XmlType("DetectedIssue", Namespace = "urn:marc-hi:svc:componentModel")]
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
