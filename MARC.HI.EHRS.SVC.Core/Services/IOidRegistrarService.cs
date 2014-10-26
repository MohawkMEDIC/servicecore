using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MARC.HI.EHRS.SVC.Core.Services
{
    /// <summary>
    /// Oid Registrar Service
    /// </summary>
    public interface IOidRegistrarService : IEnumerable<MARC.HI.EHRS.SVC.Core.DataTypes.OidRegistrar.OidData>
    {
        MARC.HI.EHRS.SVC.Core.DataTypes.OidRegistrar.OidData Register(string name, string oid, string desc, string uri);
        MARC.HI.EHRS.SVC.Core.DataTypes.OidRegistrar.OidData GetOid(string oidName);
        MARC.HI.EHRS.SVC.Core.DataTypes.OidRegistrar.OidData FindData(string oid);
        MARC.HI.EHRS.SVC.Core.DataTypes.OidRegistrar.OidData FindData(Predicate<MARC.HI.EHRS.SVC.Core.DataTypes.OidRegistrar.OidData> match);
        MARC.HI.EHRS.SVC.Core.DataTypes.OidRegistrar.OidData FindData(Uri reference);
        void DeleteOid(string oidName);
        void Register(MARC.HI.EHRS.SVC.Core.DataTypes.OidRegistrar.OidData oidData);
        void Remove(MARC.HI.EHRS.SVC.Core.DataTypes.OidRegistrar.OidData oidData);
    }
}
