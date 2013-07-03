using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes
{
    /// <summary>
    /// Represents a profile on a search parameter
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true)]
    public class SearchParameterProfileAttribute : Attribute
    {

        /// <summary>
        /// Name of the parameter
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Search type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Description of the search item
        /// </summary>
        public string Description { get; set; }
    }
}
