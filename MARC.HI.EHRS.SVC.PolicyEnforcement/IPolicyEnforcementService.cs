using System.Collections.Generic;
using System.ComponentModel;
using MARC.HI.EHRS.SVC.Core.ComponentModel;
using MARC.HI.EHRS.SVC.Core.Issues;
using MARC.HI.EHRS.SVC.Core.Services;

namespace MARC.HI.EHRS.SVC.PolicyEnforcement
{
    /// <summary>
    /// An interface that represents a policy enforcement target
    /// </summary>
    public interface IPolicyEnforcementService : IUsesHostContext
    {

        /// <summary>
        /// Apply policies agains a health service record component
        /// </summary>
        /// <param name="comp">The component record that is to have policies applied</param>
        /// <param name="reqContainer">The original request container that was used to query the data</param>
        /// <param name="issues">Any detected issues</param>
        HealthServiceRecordComponent ApplyPolicies(IContainer reqContainer, HealthServiceRecordComponent comp, List<DetectedIssue> issues);

    }
}
