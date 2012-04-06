using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.Services;
using MARC.HI.EHRS.SVC.Core.ComponentModel.Components;
using MARC.HI.EHRS.SVC.Core.DataTypes;

namespace MARC.HI.EHRS.SVC.HealthWorkerIdentity
{
    /// <summary>
    /// Identifies a service that provides health care participant lookup 
    /// functionality
    /// </summary>
    public interface IHealthcareWorkerIdentityService : IUsesHostContext
    {

        /// <summary>
        /// Lookup a participant by their identifier
        /// </summary>
        HealthcareParticipant FindParticipant(DomainIdentifier identifier);

    }
}
