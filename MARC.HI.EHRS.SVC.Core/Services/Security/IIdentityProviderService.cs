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

using MARC.HI.EHRS.SVC.Core.Event;
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
    /// Identifies a class which can generate TFA secrets
    /// </summary>
    public interface ITwoFactorSecretGenerator
    {
        /// <summary>
        /// Gets the name of the TFA generator
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Generate a TFA secret
        /// </summary>
        String GenerateTfaSecret();

        /// <summary>
        /// Validates the secret 
        /// </summary>
        bool Validate(String secret);
    }

    /// <summary>
    /// Identity provider service
    /// </summary>
    public interface IIdentityProviderService 
    {

        /// <summary>
        /// Fired prior to an authentication event
        /// </summary>
        event EventHandler<AuthenticatingEventArgs> Authenticating;

        /// <summary>
        /// Fired after an authentication decision being made
        /// </summary>
        event EventHandler<AuthenticatedEventArgs> Authenticated;

        /// <summary>
        /// Retrieves an identity from the object
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        IIdentity GetIdentity(String userName);

        /// <summary>
        /// Create a basic identity in the provider
        /// </summary>
        /// <param name="userName">The username of the identity</param>
        /// <param name="password">The intitial password of the identity</param>
        /// <returns>The created identity</returns>
        IIdentity CreateIdentity(String userName, String password, IPrincipal authContext);

        /// <summary>
        /// Authenticate the user creating an identity
        /// </summary>
        /// <returns></returns>
        IPrincipal Authenticate(String userName, String password);

        /// <summary>
        /// Authenticate the user using two factor authentication
        /// </summary>
        IPrincipal Authenticate(String userName, String password, String tfaSecret);

        /// <summary>
        /// Change user password
        /// </summary>
        void ChangePassword(String userName, String newPassword, IPrincipal authContext);

        /// <summary>
        /// Set the user's two factor authentication secret
        /// </summary>
        String GenerateTfaSecret(String userName);

        /// <summary>
        /// Delete an identity
        /// </summary>
        void DeleteIdentity(String userName, IPrincipal authContext);

        /// <summary>
        /// Set lockout
        /// </summary>
        void SetLockout(String userName, bool lockout, IPrincipal authContext);

        /// <summary>
        /// Adds a claim to the specified user account
        /// </summary>
        void AddClaim(String userName, Claim claim);

        /// <summary>
        /// Removes a claim from the specified user account
        /// </summary>
        void RemoveClaim(String userName, String claimType);

    }
}
