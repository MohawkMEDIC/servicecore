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
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MARC.HI.EHRS.SVC.Core.Event
{
    /// <summary>
    /// Identity event arguments
    /// </summary>
    public class AuthenticatingEventArgs : EventArgs
    {

        /// <summary>
        /// Creates new instance of authenticating event args
        /// </summary>
        public AuthenticatingEventArgs(String userName)
        {
            this.UserName = userName;
        }

        /// <summary>
        /// Gets the name of the user being authenticated
        /// </summary>
        public String UserName { get; private set; }

        /// <summary>
        /// True if the operation should be cancelled
        /// </summary>
        public bool Cancel { get; set; }

    }

    /// <summary>
    /// For events fired after authentication decision has been made
    /// </summary>
    public class AuthenticatedEventArgs : EventArgs
    {

        /// <summary>
        /// Creates a new instance of the authenticated event args
        /// </summary>
        public AuthenticatedEventArgs(String userName, IPrincipal principal, bool success)
        {
            this.UserName = userName;
            this.Principal = principal;
            this.Success = success;
        }

        /// <summary>
        /// The identity of the user
        /// </summary>
        public String UserName { get; private set; }

        /// <summary>
        /// The principal issued. Null if none was issued
        /// </summary>
        public IPrincipal Principal { get; private set; }

        /// <summary>
        /// Whether the authentication was successful
        /// </summary>
        public bool Success { get; private set; }
    }

}
