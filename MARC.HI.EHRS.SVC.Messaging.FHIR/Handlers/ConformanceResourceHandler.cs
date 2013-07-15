using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Handlers;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Util;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Handlers
{
    /// <summary>
    /// Conformance resource handler
    /// </summary>
    public class ConformanceResourceHandler : IFhirResourceHandler
    {
        #region IFhirResourceHandler Members

        /// <summary>
        /// Gets the resource name
        /// </summary>
        public string ResourceName
        {
            get { return "Conformance"; }
        }

        /// <summary>
        /// Read the conformance
        /// </summary>
        public FhirOperationResult Read(string id, string versionId)
        {
            return new FhirOperationResult()
            {
                Outcome = Everest.Connectors.ResultCode.Accepted,
                Results = new List<Resources.ResourceBase>() { ConformanceUtil.GetConformanceStatement() }
            };
        }

        /// <summary>
        /// Update
        /// </summary>
        public FhirOperationResult Update(string id, Resources.ResourceBase target, Core.Services.DataPersistenceMode mode)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Delete
        /// </summary>
        public FhirOperationResult Delete(string id, Core.Services.DataPersistenceMode mode)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Create 
        /// </summary>
        public FhirOperationResult Create(Resources.ResourceBase target, Core.Services.DataPersistenceMode mode)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Validate
        /// </summary>
        public FhirOperationResult Validate(string id, Resources.ResourceBase target)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Query
        /// </summary>
        public FhirQueryResult Query(System.Collections.Specialized.NameValueCollection parameters)
        {
            return new FhirQueryResult()
            {
                Outcome = Everest.Connectors.ResultCode.Accepted,
                Results = this.Read(null, null).Results,
                Query = new FhirQuery()
                {
                    ActualParameters = new System.Collections.Specialized.NameValueCollection(),
                    IncludeHistory = false,
                    MinimumDegreeMatch = 100,
                    Start = 0
                }
            };
                    
        }

        #endregion
    }
}
