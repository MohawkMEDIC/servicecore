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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.Services;
using MARC.HI.EHRS.SVC.Core.ComponentModel.Components;
using MARC.HI.EHRS.SVC.Core.DataTypes;

namespace MARC.HI.EHRS.SVC.ClientIdentity
{
    /// <summary>
    /// Can be used by providers to gather client lookup information
    /// </summary>
    public interface IClientIdentityService : IUsesHostContext
    {
        /// <summary>
        /// Lookup a client by their domain identifier
        /// </summary>
        Client FindClient(DomainIdentifier identifier);

        /// <summary>
        /// Find client by their name
        /// </summary>
        Client[] FindClient(NameSet name, string genderCode, TimestampPart birthTime);
    }
}
