/**
 * Copyright 2013-2013 Mohawk College of Applied Arts and Technology
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
 * Date: 12-3-2013
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MARC.HI.EHRS.SVC.Core.Services
{
    /// <summary>
    /// Represents a service which executes timer jobs
    /// </summary>
    public interface ITimerService : IDaemonService
    {

        /// <summary>
        /// Add a job to the timer
        /// </summary>
        void AddJob(object jobObject, TimeSpan elapseTime);

        /// <summary>
        /// Gets the execution state
        /// </summary>
        /// <returns></returns>
        List<KeyValuePair<object, DateTime>> GetState();

    }
}
