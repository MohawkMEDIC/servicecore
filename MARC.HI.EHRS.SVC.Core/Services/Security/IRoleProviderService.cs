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
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MARC.HI.EHRS.SVC.Core.Services.Security
{
    /// <summary>
    /// Represents a role provider service
    /// </summary>
    public interface IRoleProviderService
    {
        /// <summary>
        /// Creates a role
        /// </summary>
        void CreateRole(String roleName, IPrincipal authPrincipal);

        /// <summary>
        /// Add users to roles
        /// </summary>
        void AddUsersToRoles(String[] users, String[] roles, IPrincipal authPrincipal);

        /// <summary>
        /// Remove users from specified roles
        /// </summary>
        void RemoveUsersFromRoles(String[] users, String[] roles, IPrincipal authPrincipal);

        /// <summary>
        /// Find all users in a specified role
        /// </summary>
        String[] FindUsersInRole(String role);

        /// <summary>
        /// Get all roles
        /// </summary>
        String[] GetAllRoles();

        /// <summary>
        /// Get all roles
        /// </summary>
        String[] GetAllRoles(string userName);

        /// <summary>
        /// User user in the specified role
        /// </summary>
        bool IsUserInRole(String userName, String roleName);

        /// <summary>
        /// Is user in the specified role
        /// </summary>
        bool IsUserInRole(IPrincipal principal, String roleName);
    }
}
