using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.Everest.Connectors;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR
{
    /// <summary>
    /// Result detail related to a persistence problem
    /// </summary>
    public class PersistenceResultDetail : ResultDetail
    {
        /// <summary>
        /// Create a new instance of the invalid state transition detail
        /// </summary>
        public PersistenceResultDetail(ResultDetailType type, string message, Exception innerException)
            : base(type, message, innerException)
        { }
    }
}
