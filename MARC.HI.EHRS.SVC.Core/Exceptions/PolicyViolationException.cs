using MARC.HI.EHRS.SVC.Core.Services.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MARC.HI.EHRS.SVC.Core.Exceptions
{
    /// <summary>
    /// Represents a policy violation
    /// </summary>
    public class PolicyViolationException : SecurityException
    {

        /// <summary>
        /// Gets the policy that was violated
        /// </summary>
        public IPolicy Policy { get; private set; }

        /// <summary>
        /// Gets the policy id that was violed
        /// </summary>
        public String PolicyId { get; private set; }

        /// <summary>
        /// Gets the policy outcome
        /// </summary>
        public PolicyDecisionOutcomeType PolicyDecision { get; private set; }

        /// <summary>
        /// Creates a new instance of the policy violation
        /// </summary>
        public PolicyViolationException(String policyId, PolicyDecisionOutcomeType outcome)
        {
            if (policyId == null)
                throw new ArgumentNullException(nameof(policyId));

            this.PolicyId = policyId;
            this.PolicyDecision = outcome;
        }

        /// <summary>
        /// Creates a new instance of the policy violation exception
        /// </summary>
        public PolicyViolationException(IPolicy policy, PolicyDecisionOutcomeType outcome)
        {
            if (policy == null)
                throw new ArgumentNullException(nameof(policy));
            
            this.Policy = policy;
            this.PolicyId = policy.Oid;
            this.PolicyDecision = outcome;
        }

        /// <summary>
        /// The message
        /// </summary>
        public override string Message
        {
            get
            {
                return string.Format("Policy {0} was violated. Enforcement decision was {1}", this.PolicyId, this.PolicyDecision);
            }
        }
    }
}
