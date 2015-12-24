
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
    }
}