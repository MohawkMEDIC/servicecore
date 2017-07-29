
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