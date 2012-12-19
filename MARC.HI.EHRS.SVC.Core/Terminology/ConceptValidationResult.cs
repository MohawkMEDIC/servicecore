/**
 * Copyright 2012-2012 Mohawk College of Applied Arts and Technology
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

namespace MARC.HI.EHRS.SVC.Core.Terminology
{
    /// <summary>
    /// Identifies the overall result of a terminology resolution
    /// </summary>
    [Serializable]
    public class ConceptValidationResult
    {
        /// <summary>
        /// Validation result detail
        /// </summary>
        [Serializable]
        public struct ConceptValidationResultDetail
        {
            /// <summary>
            /// If TRUE, identifies that the detail line is an error
            /// </summary>
            public bool IsError { get; set; }

            /// <summary>
            /// A textual message describing the error
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// A codified representation of the error
            /// </summary>
            public string Code { get; set; }
        }

        /// <summary>
        /// Identifies a list of details related to the validation of a concept
        /// </summary>
        private List<ConceptValidationResultDetail> m_details = new List<ConceptValidationResultDetail>();

        /// <summary>
        /// Gets or sets the outcome of validation
        /// </summary>
        public ValidationOutcome Outcome { get; set; }

        /// <summary>
        /// Gets a list of validation result details
        /// </summary>
        public List<ConceptValidationResultDetail> Details
        {
            get
            {
                return m_details;
            }
        }

        /// <summary>
        /// Add a validatoin detail to the message
        /// </summary>
        /// <param name="isError">True if the detail is an error</param>
        /// <param name="message">The message of the error</param>
        /// <param name="code">A codified representation of the error</param>
        public void AddDetail(bool isError, string message, string code)
        {
            Details.Add(new ConceptValidationResultDetail()
            {
                Code = code,
                IsError = isError,
                Message = message
            });
        }
    }
}
