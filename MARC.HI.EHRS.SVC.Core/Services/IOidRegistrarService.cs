using MARC.HI.EHRS.SVC.Core.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MARC.HI.EHRS.SVC.Core.Services
{
    /// <summary>
    /// Oid Registrar Service - A service that is responsible for storing data related to registered object identifiers
    /// </summary>
    public interface IOidRegistrarService//<MARC.HI.EHRS.SVC.Core.DataTypes.OidRegistrar.OidData>
    {
        /// <summary>
        /// Register an identifier type
        /// </summary>
        OidData Register(string name, string oid, string desc, string uri);
        /// <summary>
        /// Get an OID by name
        /// </summary>
        /// <param name="oidName"></param>
        /// <returns></returns>
        OidData GetOid(string oidName);
        /// <summary>
        /// Find data based on the OID
        /// </summary>
        OidData FindData(string oid);
        /// <summary>
        /// Find data based on attribute name / value
        /// </summary>
        OidData FindData(string attributeName, string attributeValue);
        /// <summary>
        /// Find an OID based on Uri
        /// </summary>
        OidData FindData(Uri reference);
        /// <summary>
        /// Delete an OID
        /// </summary>
        /// <param name="oidName"></param>
        void DeleteOid(string oidName);
        /// <summary>
        /// Register an OID
        /// </summary>
        /// <param name="oidData"></param>
        void Register(OidData oidData);
        /// <summary>
        /// Remove an OID from the data
        /// </summary>
        /// <param name="oidData"></param>
        void Remove(OidData oidData);
        /// <summary>
        /// Get extended attributes supported by this OID registrar
        /// </summary>
        Dictionary<String, Type> ExtendedAttributes { get; }
    }
}
