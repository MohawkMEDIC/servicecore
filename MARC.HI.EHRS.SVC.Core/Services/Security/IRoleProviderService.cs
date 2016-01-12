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
        /// User user in the specified role
        /// </summary>
        bool IsUserInRole(String userName, String roleName);

        /// <summary>
        /// Is user in the specified role
        /// </summary>
        bool IsUserInRole(IPrincipal principal, String roleName);
    }
}
