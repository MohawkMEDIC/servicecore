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
 * Date: 4-1-2013
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.Services;
using MARC.HI.EHRS.SVC.Core.Timer.Configuration;
using System.Configuration;
using System.Threading;
using System.Diagnostics;
using System.Timers;

namespace MARC.HI.EHRS.SVC.Core.Timer
{
    /// <summary>
    /// Represents the default implementation of the timer
    /// </summary>
    public class TimerService : ITimerService
    {
        // Host context
        private IServiceProvider m_hostContext;

        /// <summary>
        /// Timer configuration
        /// </summary>
        private TimerConfiguration m_configuration;

        /// <summary>
        /// Timer thread
        /// </summary>
        private System.Timers.Timer[] m_timers;

        /// <summary>
        /// Creates a new instance of the timer
        /// </summary>
        public TimerService()
        {
            this.m_configuration = ConfigurationManager.GetSection("marc.hi.svc.core.timer") as TimerConfiguration;
            
        }

        #region ITimerService Members

        /// <summary>
        /// Start the timer
        /// </summary>
        public void Start()
        {
            if (this.m_timers != null)
                this.Stop();

            Trace.TraceInformation("Starting timer service...");

            // Setup timers based on the jobs
            this.m_timers = new System.Timers.Timer[this.m_configuration.Jobs.Count];
            int i = 0;
            foreach (var job in this.m_configuration.Jobs)
            {
                // Timer setup
                var timer = new System.Timers.Timer(job.Timeout.TotalMilliseconds)
                {
                    AutoReset = true,
                    Enabled = true
                };
                timer.Elapsed += new System.Timers.ElapsedEventHandler(job.Job.Elapsed);
                timer.Start();
                this.m_timers[i++] = timer;

                // Fire the job at startup
                job.Job.Elapsed(timer, null);
            }

            Trace.TraceInformation("Timer service started successfully");
        }

        /// <summary>
        /// Stops the timer
        /// </summary>
        public void Stop()
        {
            // Stop all timers
            Trace.TraceInformation("Stopping timer service...");

            if(this.m_timers != null)
                foreach (var timer in this.m_timers)
                {
                    timer.Stop();
                    timer.Dispose();
                }
            this.m_timers = null;
            Trace.TraceInformation("Timer service stopped successfully");
        }

        #endregion

        #region IUsesHostContext Members

        /// <summary>
        /// Gets or sets the host context
        /// </summary>
        public IServiceProvider Context
        {
            get { return this.m_hostContext; }
            set
            {
                this.m_hostContext = value;
                this.m_configuration.Context = value;
            }
        }

        #endregion
    }
}
