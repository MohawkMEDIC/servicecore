/*
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
 * Date: 30-7-2013
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.Timer;
using System.Diagnostics;
using System.Data;
using MARC.HI.EHRS.QM.Persistence.Data.Configuration;
using System.Configuration;
using MARC.HI.EHRS.SVC.Core;
using MARC.HI.EHRS.SVC.Core.Services;

namespace MARC.HI.EHRS.QM.Persistence.Data
{
    /// <summary>
    /// Timer job
    /// </summary>
    public class QueryPersistenceCleanJob : ITimerJob, IDaemonService
    {

         /// <summary>
        /// Configuration handler
        /// </summary>
        private static ConfigurationHandler m_configuration;

        public event EventHandler Starting;
        public event EventHandler Stopping;
        public event EventHandler Started;
        public event EventHandler Stopped;

        /// <summary>
        /// Ado Query Persistence Service
        /// </summary>
        static QueryPersistenceCleanJob()
        {
            m_configuration = ApplicationContext.Current.GetService<IConfigurationManager>().GetSection("marc.hi.ehrs.qm.persistence.data") as ConfigurationHandler;
        }

        #region ITimerJob Members

        /// <summary>
        /// When the timer elapses
        /// </summary>
        public void Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            #if DEBUG
            Trace.TraceInformation("Cleaning stale queries from database...");
            #endif 

            IDbConnection connection = m_configuration.CreateConnection();
            try
            {
                connection.Open();

                // Clean the query database
                IDbCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "qry_cln";
                IDataParameter ageParm = cmd.CreateParameter();
                ageParm.DbType = DbType.String;
                ageParm.Direction = ParameterDirection.Input;
                ageParm.ParameterName = "max_age_in";
                ageParm.Value = String.Format("{0} minutes", m_configuration.MaxQueryAge);
                cmd.Parameters.Add(ageParm);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

        }

        /// <summary>
        /// Startup and add thyself to the timer job if we don't exist already in the timer job
        /// </summary>
        public bool Start()
        {
            this.Starting?.Invoke(this, EventArgs.Empty);

            ApplicationContext.Current.Started += (o, e) =>
            {
                var timerService = ApplicationContext.Current.GetService<ITimerService>();
                if(!timerService.IsJobRegistered(typeof(QueryPersistenceCleanJob)))
                    timerService.AddJob(this, new TimeSpan(4, 0, 0));
            };

            this.Started?.Invoke(this, EventArgs.Empty);
            return true;
        }

        /// <summary>
        /// Stop the service
        /// </summary>
        public bool Stop()
        {
            this.Stopping?.Invoke(this, EventArgs.Empty);
            this.Stopped?.Invoke(this, EventArgs.Empty);
            return true;
        }

        #endregion

        #region IUsesHostContext Members

        /// <summary>
        /// True if the job is running
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return ApplicationContext.Current.GetService<ITimerService>()?.IsJobRegistered(typeof(QueryPersistenceCleanJob)) == true;
            }
        }

        #endregion
    }
}
