using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace MARC.HI.EHRS.SVC.Core.Authorization
{
    /// <summary>
    /// The authorization context represents the context under which an action has been authorized to occur
    /// </summary>
    public class AuthorizationContext
    {
        /// <summary>
        /// Represents the claims identity
        /// </summary>
        /// <param name="identity"></param>
        public AuthorizationContext(ClaimsIdentity identity)
        {
            this.Identity = identity;
            
        }

        /// <summary>
        /// Gets or sets the identity used in the auth context
        /// </summary>
        public ClaimsIdentity Identity { get; private set; }
        
    }
}
