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
 * Date: 2-1-2013
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.Services;
using MARC.HI.EHRS.SVC.Core;

namespace MARC.HI.EHRS.SVC.Core.Timer.Configuration
{
    /// <summary>
    /// Data class for timer configuration
    /// </summary>
    public class TimerConfiguration : IUsesHostContext
    {

        // Host context
        private IServiceProvider m_hostContext = null;

        /// <summary>
        /// Creates a new instance of the timer configuration
        /// </summary>
        internal TimerConfiguration()
        {
            this.Jobs = new List<TimerJobConfiguration>();
        }

        /// <summary>
        /// Gets a list of job configurations
        /// </summary>
        public List<TimerJobConfiguration> Jobs { get; private set; }

        #region IUsesHostContext Members

        /// <summary>
        /// Gets or sets the host context that owns this configuration
        /// </summary>
        public IServiceProvider Context
        {
            get
            {
                return this.m_hostContext;
            }
            set
            {
                this.m_hostContext = value;
                // Cascade the host context
                foreach (var job in this.Jobs)
                    job.Job.Context = this.m_hostContext;
            }
        }

        #endregion
    }
}
