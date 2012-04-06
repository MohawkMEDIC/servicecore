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
