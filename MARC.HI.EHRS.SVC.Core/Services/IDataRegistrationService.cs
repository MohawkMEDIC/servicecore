using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MARC.HI.EHRS.SVC.Core.DataTypes;
using MARC.HI.EHRS.SVC.Core.Services;

namespace MARC.HI.EHRS.SVC.Core.Services
{
    /// <summary>
    /// Framework for classes that update record registries. A record regsitry is a system
    /// that registers document meta-data in a manner that allows it to be queried.
    /// </summary>
    public interface IDataRegistrationService : IUsesHostContext
    {

        /// <summary>
        /// Register a record with the registration service
        /// </summary>
        bool RegisterRecord(IComponent recordComponent, DataPersistenceMode mode);

        /// <summary>
        /// Query records from the registration service
        /// </summary>
        /// <param name="queryParameters">A series of parameters that represent the filter parameters</param>
        VersionedDomainIdentifier[] QueryRecord(IComponent queryParameters);

    }
}
