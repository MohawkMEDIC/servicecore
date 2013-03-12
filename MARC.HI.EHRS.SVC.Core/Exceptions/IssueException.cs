﻿/**
 * Copyright 2012-2013 Mohawk College of Applied Arts and Technology
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
 * Date: 7-5-2012
 */

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
