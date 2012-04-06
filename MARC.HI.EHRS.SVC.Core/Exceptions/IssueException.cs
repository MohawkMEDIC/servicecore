using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.Issues;

namespace MARC.HI.EHRS.SVC.Core.Exceptions
{
    /// <summary>
    /// Represents an exception that was thrown as a result of violation of 
    /// a business constraint
    /// </summary>
    public class IssueException : System.Exception
    {
        /// <summary>
        /// Identifies the issue that occured
        /// </summary>
        public DetectedIssue Issue { get; set; }

        /// <summary>
        /// Creates a new instance of issueException
        /// </summary>
        public IssueException()
        {
        }

        /// <summary>
        /// Creates a new instance of an IssueException with the specified issue
        /// </summary>
        public IssueException(DetectedIssue issue)
        {
            this.Issue = issue;
        }

        /// <summary>
        /// Issue exception
        /// </summary>
        public IssueException(IssueType type, ManagementType mitigation, IssuePriorityType priority, IssueSeverityType severity, String text) : this(
            new DetectedIssue()
            {
                Type = type,
                MitigatedBy = mitigation,
                Priority = priority,
                Severity = severity,
                Text = text
            })
        {}
    }
}
