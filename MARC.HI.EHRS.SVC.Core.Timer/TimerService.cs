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
using System.ComponentModel;

namespace MARC.HI.EHRS.SVC.Core.Timer
{
    /// <summary>
    /// Represents the default implementation of the timer
    /// </summary>
    [Description("Default Timer Service")]
    public class TimerService : ITimerService
    {
        /// <summary>
        /// Timer configuration
        /// </summary>
        private TimerConfiguration m_configuration;

        /// <summary>
        /// Timer thread
        /// </summary>
        private System.Timers.Timer[] m_timers;

        /// <summary>
        /// Timer service is starting
        /// </summary>
        public event EventHandler Starting;
        /// <summary>
        /// Timer service is stopping
        /// </summary>
        public event EventHandler Stopping;
        /// <summary>
        /// Timer service is started
        /// </summary>
        public event EventHandler Started;
        /// <summary>
        /// Timer service is stopped
        /// </summary>
        public event EventHandler Stopped;

        /// <summary>
        /// Log of timers
        /// </summary>
        private Dictionary<ITimerJob, DateTime> m_log = new Dictionary<ITimerJob, DateTime>();

        /// <summary>
        /// Creates a new instance of the timer
        /// </summary>
        public TimerService()
        {
            this.m_configuration = ApplicationContext.Current.GetService<IConfigurationManager>().GetSection("marc.hi.svc.core.timer") as TimerConfiguration;
            
        }

        #region ITimerService Members

        /// <summary>
        /// Start the timer
        /// </summary>
        public bool Start()
        {

            if (this.m_timers != null)
                this.Stop();

            Trace.TraceInformation("Starting timer service...");

            // Invoke the starting event handler
            this.Starting?.Invoke(this, EventArgs.Empty);

            // Setup timers based on the jobs
            this.m_timers = new System.Timers.Timer[this.m_configuration.Jobs.Count];
            int i = 0;
            foreach (var job in this.m_configuration.Jobs)
            {
                lock (this.m_log)
                    this.m_log.Add(job.Job as ITimerJob, DateTime.MinValue);

                // Timer setup
                var timer = new System.Timers.Timer(job.Timeout.TotalMilliseconds)
                {
                    AutoReset = true,
                    Enabled = true
                };
                timer.Elapsed += this.CreateElapseHandler(job.Job);
                timer.Start();
                this.m_timers[i++] = timer;

                // Fire the job at startup
                job.Job.Elapsed(timer, null);
            }

            this.Started?.Invoke(this, EventArgs.Empty);

            Trace.TraceInformation("Timer service started successfully");
            return true;
        }

        /// <summary>
        /// Create a time elapsed handler
        /// </summary>
        private ElapsedEventHandler CreateElapseHandler(ITimerJob job)
        {
            return new System.Timers.ElapsedEventHandler((o, e) =>
            {

                // Log that the timer fired
                if (this.m_log.ContainsKey(job))
                    this.m_log[job] = e.SignalTime;
                else
                    lock (this.m_log)
                        this.m_log.Add(job, e.SignalTime);

                job.Elapsed(o, e);

            });
        }

        /// <summary>
        /// Stops the timer
        /// </summary>
        public bool Stop()
        {
            // Stop all timers
            Trace.TraceInformation("Stopping timer service...");
            this.Stopping?.Invoke(this, EventArgs.Empty);

            if(this.m_timers != null)
                foreach (var timer in this.m_timers)
                {
                    timer.Stop();
                    timer.Dispose();
                }
            this.m_timers = null;

            this.Stopped?.Invoke(this, EventArgs.Empty);

            Trace.TraceInformation("Timer service stopped successfully");
            return true;
        }

        /// <summary>
        /// Add a job
        /// </summary>
        public void AddJob(object jobObject, TimeSpan elapseTime)
        {
            if (!(jobObject is ITimerJob))
                throw new ArgumentOutOfRangeException(nameof(jobObject));

            lock (this.m_log)
                this.m_log.Add(jobObject as ITimerJob, DateTime.MinValue);

            // Resize the timer array
            lock (this.m_timers)
            {
                Array.Resize(ref this.m_timers, this.m_timers.Length + 1);
                var timer = new System.Timers.Timer(elapseTime.TotalMilliseconds)
                {
                    AutoReset = true,
                    Enabled = true
                };
                timer.Elapsed += this.CreateElapseHandler(jobObject as ITimerJob);
                timer.Start();
                this.m_timers[this.m_timers.Length - 1] = timer;
            }
            (jobObject as ITimerJob).Elapsed(this, null);

        }

        /// <summary>
        /// Get the state
        /// </summary>
        public List<KeyValuePair<object, DateTime>> GetState()
        {
            return this.m_log.Select(o => new KeyValuePair<Object, DateTime>(o.Key, o.Value)).ToList();
        }

        /// <summary>
        /// Return true if job object is registered
        /// </summary>
        public bool IsJobRegistered(Type jobObject)
        {
            return this.m_log.Keys.Any(o => o.GetType() == jobObject);
        }

        /// <summary>
        /// Returns true when the service is running
        /// </summary>
        public bool IsRunning { get { return this.m_timers != null; } }
        #endregion
    }
}
