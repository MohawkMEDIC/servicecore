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

namespace MARC.HI.EHRS.SVC.Core.Services
{
    /// <summary>
    /// Represents the base service interface
    /// </summary>
    public interface IDaemonService
    {
        /// <summary>
        /// Start the service and any necessary functions
        /// </summary>
        bool Start();
        /// <summary>
        /// Stop the service and any necessary functions
        /// </summary>
        bool Stop();

        /// <summary>
        /// Returns an indicator whether the service is running
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Fired when the daemon service is starting
        /// </summary>
        event EventHandler Starting;
        /// <summary>
        /// Fired when the daemon service is stopping
        /// </summary>
        event EventHandler Stopping;
        /// <summary>
        /// Fired when the daemon service has finished starting
        /// </summary>
        event EventHandler Started;
        /// <summary>
        /// Fired when the daemon service has finished stopping
        /// </summary>
        event EventHandler Stopped;
    }
}