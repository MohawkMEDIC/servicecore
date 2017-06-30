using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Resources;
using MARC.HI.EHRS.SVC.Core.Services;
using System.Collections.Specialized;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Handlers
{
    /// <summary>
    /// Represents a class that can handle a FHIR resource query request
    /// </summary>
    public interface IFhirResourceHandler
    {

        /// <summary>
        /// Gets the type of resource this handler can perform operations on
        /// </summary>
        string ResourceName { get; }

        /// <summary>
        /// Read a specific version of a resource
        /// </summary>
        FhirOperationResult Read(string id, string versionId);

        /// <summary>
        /// Update a resource
        /// </summary>
        FhirOperationResult Update(string id, DomainResourceBase target, TransactionMode mode);

        /// <summary>
        /// Delete a resource
        /// </summary>
        FhirOperationResult Delete(string id, TransactionMode mode);

        /// <summary>
        /// Create a resource
        /// </summary>
        FhirOperationResult Create(DomainResourceBase target, TransactionMode mode);

        /// <summary>
        /// Query a FHIR resource
        /// </summary>
        FhirQueryResult Query(NameValueCollection parameters);

    }
}
