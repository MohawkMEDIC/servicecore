using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MARC.HI.EHRS.SVC.Core.Services.Policy
{

    /// <summary>
    /// Outcome of a policy decision
    /// </summary>
    public enum PolicyDecisionOutcomeType
    {
        Deny,
        Elevate,
        Grant
    }

    /// <summary>
    /// Policy decision
    /// </summary>
    public class PolicyDecision
    {

        /// <summary>
        /// Creates a new policy decision
        /// </summary>
        public PolicyDecision()
        {
            this.Details = new List<PolicyDecisionDetail>();
        }

        /// <summary>
        /// Details of the policy decision
        /// </summary>
        public List<PolicyDecisionDetail> Details { get; private set; }
        /// <summary>
        /// Gets the outcome of the poilcy decision taking into account all triggered policies
        /// </summary>
        public PolicyDecisionOutcomeType Outcome {
            get
            {
                PolicyDecisionOutcomeType restrictive = PolicyDecisionOutcomeType.Grant;
                foreach (var dtl in this.Details)
                    if (dtl.Outcome < restrictive)
                        restrictive = dtl.Outcome;
                return restrictive;
            }
        }
    }

    /// <summary>
    /// Represents a decision on a single policy element
    /// </summary>
    public struct PolicyDecisionDetail
    {
        /// <summary>
        /// Creates a new policy decision outcome
        /// </summary>
        public PolicyDecisionDetail(String policyId, PolicyDecisionOutcomeType outcome)
        {
            this.PolicyId = policyId;
            this.Outcome = outcome;
        }

        /// <summary>
        /// Gets the policy identifier
        /// </summary>
        public String PolicyId { get; private set; }

        /// <summary>
        /// Gets the policy decision outcome
        /// </summary>
        public PolicyDecisionOutcomeType Outcome { get; private set; }
    }

    /// <summary>
    /// Represents a policy decision service
    /// </summary>
    public interface IPolicyDecisionService
    {
        /// <summary>
        /// Make a simple policy decision for a specific securable
        /// </summary>
        PolicyDecision GetPolicyDecision(ClaimsPrincipal principal, Object securable);

        /// <summary>
        /// Get a policy decision for a specific policy
        /// </summary>
        PolicyDecisionOutcomeType GetPolicyOutcome(ClaimsPrincipal principal, String policyId);
        
    }
}
