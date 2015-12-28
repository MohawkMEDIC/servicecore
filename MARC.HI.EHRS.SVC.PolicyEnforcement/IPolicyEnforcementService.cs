/**
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

using System.Collections.Generic;
using System.ComponentModel;
using MARC.HI.EHRS.SVC.Core.Issues;
using MARC.HI.EHRS.SVC.Core.Services;
using MARC.HI.EHRS.SVC.Core.Authorization;

namespace MARC.HI.EHRS.SVC.PolicyEnforcement
{
    /// <summary>
    /// An interface that represents a policy enforcement target
    /// </summary>
    public interface IPolicyEnforcementService<TContainer>
    {

        /// <summary>
        /// Apply policies agains a health service record component
        /// </summary>
        /// <param name="comp">The component record that is to have policies applied</param>
        /// <param name="reqContainer">The original request container that was used to query the data</param>
        /// <param name="issues">Any detected issues</param>
        TContainer ApplyPolicies(AuthorizationContext reqContainer, TContainer comp, List<DetectedIssue> issues);

    }
}
