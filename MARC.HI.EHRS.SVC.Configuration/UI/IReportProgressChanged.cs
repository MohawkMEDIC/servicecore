using System;
using System.ComponentModel;

namespace MARC.HI.EHRS.SVC.Configuration.UI
{
    /// <summary>
    /// Represents an interface that reports its progress to the ui
    /// </summary>
    public interface IReportProgressChanged
    {

        /// <summary>
        /// Progress has changed
        /// </summary>
        event EventHandler<ProgressChangedEventArgs> ProgressChanged;

    }
}