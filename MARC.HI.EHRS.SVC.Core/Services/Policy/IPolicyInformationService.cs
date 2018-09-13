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
using System.Text;
using System.Threading.Tasks;

namespace MARC.HI.EHRS.SVC.Core.Services.Policy
{

    /// <summary>
    /// Represents a policy
    /// </summary>
    public interface IPolicy
    {
        /// <summary>
        /// Gets the unique object identifier for the policy
        /// </summary>
        String Oid { get; }

        /// <summary>
        /// Gets the name of the policy
        /// </summary>
        String Name { get; }

        /// <summary>
        /// True whether the policy can be overridden
        /// </summary>
        bool CanOverride { get; }

        /// <summary>
        /// True if the policy is actively enforced
        /// </summary>
        bool IsActive { get; }
    }

    /// <summary>
    /// Represents a policy instance
    /// </summary>
    public interface IPolicyInstance
    {
        /// <summary>
        /// The policy 
        /// </summary>
        IPolicy Policy { get; }
        /// <summary>
        /// The rule
        /// </summary>
        PolicyDecisionOutcomeType Rule { get; }
        /// <summary>
        /// The securable
        /// </summary>
        Object Securable { get; }
    }
    /// <summary>
    /// Represents a policy information service
    /// </summary>
    public interface IPolicyInformationService
    {

        /// <summary>
        /// Gets a list of all policies
        /// </summary>
        IEnumerable<IPolicy> GetPolicies();

        /// <summary>
        /// Get active policies for the specified securable
        /// </summary>
        /// <param name="securable">The object for which policies should be retrieved. Examples: A role, a user, a document, etc.</param>
        IEnumerable<IPolicyInstance> GetActivePolicies(Object securable);

        /// <summary>
        /// Gets the policy by policy OID
        /// </summary>
        IPolicy GetPolicy(String policyOid);

        /// <summary>
        /// Adds the specified policies to the specified securable object
        /// </summary>
        /// <param name="securable">The object to which policies should be added</param>
        /// <param name="rule">The rule to be applied to the securable</param>
        /// <param name="policyOids">The oids of the policies to add</param>
        void AddPolicies(Object securable, PolicyDecisionOutcomeType rule, params String[] policyOids);
    }
}
