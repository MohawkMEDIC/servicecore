/*
 * Copyright 2010-2018 Mohawk College of Applied Arts and Technology
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
 * Date: 1-9-2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
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
        public PolicyDecision(Object securable)
        {
            this.Details = new List<PolicyDecisionDetail>();
            this.Securable = securable;

        }

        /// <summary>
        /// Details of the policy decision
        /// </summary>
        public List<PolicyDecisionDetail> Details { get; private set; }


        /// <summary>
        /// The securable that this policy outcome is made against
        /// </summary>
        public Object Securable { get; private set; }

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
        PolicyDecision GetPolicyDecision(IPrincipal principal, Object securable);

        /// <summary>
        /// Get a policy decision for a specific policy
        /// </summary>
        PolicyDecisionOutcomeType GetPolicyOutcome(IPrincipal principal, String policyId);
        
    }
}
