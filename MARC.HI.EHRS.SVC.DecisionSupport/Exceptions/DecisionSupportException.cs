/**
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
