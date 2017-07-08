/*
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

namespace MARC.HI.EHRS.SVC.Core.Exceptions
{
    /// <summary>
    /// Represents an invalid state transition
    /// </summary>
    public class InvalidStateTransitionException : ApplicationException
    {
        /// <summary>
        /// Identifies the state that is being exited
        /// </summary>
        public string FromState { get; set; }
        /// <summary>
        /// Identifies the state that is being entered
        /// </summary>
        public string ToState { get; set; }

        /// <summary>
        /// Create a new instance of the InvalidStateTransitionException object
        /// </summary>
        /// <param name="fromState">The state that is being exited</param>
        /// <param name="toState">The state that is being entered</param>
        public InvalidStateTransitionException(string fromState, string toState)
            : base(String.Format("Cannot transform state from {0} to {1}", fromState, toState))
        {
            this.FromState = fromState;
            this.ToState = toState;
        }
    }
}
