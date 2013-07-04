using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes;
using System.ComponentModel;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{
    /// <summary>
    /// Operation outcome
    /// </summary>
    [XmlType("OperationOutcome", Namespace="http://hl7.org/fhir")]
    [XmlRoot("OperationOutcome", Namespace = "http://hl7.org/fhir")]
    [Profile(ProfileId = "svccore")]
    [ResourceProfile(Name = "ServiceCore Resource - Operation Outcome")]
    public class OperationOutcome : ResourceBase
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public OperationOutcome()
        {
            this.Issue = new List<Issue>();
        }

        /// <summary>
        /// Gets or sets a list of issues 
        /// </summary>
        [XmlElement("issue")]
        [Description("A list of issues related to the operation")]
        [ElementProfile(MinOccurs = 1)]
        public List<Issue> Issue { get; set; }

    }
}
