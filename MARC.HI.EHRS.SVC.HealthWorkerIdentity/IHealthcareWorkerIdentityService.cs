/**
 * Copyright 2012-2012 Mohawk College of Applied Arts and Technology
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
using MARC.HI.EHRS.SVC.Core.Services;
using MARC.HI.EHRS.SVC.Core.ComponentModel.Components;
using MARC.HI.EHRS.SVC.Core.DataTypes;

namespace MARC.HI.EHRS.SVC.HealthWorkerIdentity
{
    /// <summary>
    /// Identifies a service that provides health care participant lookup 
    /// functionality
    /// </summary>
    public interface IHealthcareWorkerIdentityService : IUsesHostContext
    {

        /// <summary>
        /// Lookup a participant by their identifier
        /// </summary>
        HealthcareParticipant FindParticipant(DomainIdentifier identifier);

        /// <summary>
        /// Register the participant
        /// </summary>
        void Register(HealthcareParticipant ptcpt);
    }
}
