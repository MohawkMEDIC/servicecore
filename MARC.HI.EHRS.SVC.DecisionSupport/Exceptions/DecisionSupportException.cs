using System;
using MARC.HI.EHRS.SVC.Core.Issues;

namespace MARC.HI.EHRS.SVC.DecisionSupport.Exceptions
{
    /// <summary>
    /// Identifies exceptions that occur due to decision support issues.
    /// </summary>
    /// <remarks>
    /// This exception can be used to HALT the current operation
    /// </remarks>
    public class DecisionSupportException : Exception
    {
        /// <summary>
        /// Gets the issue that caused the exception
        /// </summary>
        public DetectedIssue Issue { get; private set; }


        public DecisionSupportException(DetectedIssue issue) { this.Issue = issue; }
        public DecisionSupportException(DetectedIssue issue, Exception innerException)
            : base(null, innerException)
        { this.Issue = issue; }
    }
}
