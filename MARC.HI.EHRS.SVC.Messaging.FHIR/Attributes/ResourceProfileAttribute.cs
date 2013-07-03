using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes
{
    /// <summary>
    /// Identifies that a class is a profile for a FHIR resource
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ResourceProfileAttribute : System.Attribute
    {


        /// <summary>
        /// Identifies the name of the resource profile
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Identifies the resource this profile definition profiles
        /// </summary>
        public Type Resource { get; set; }

    }
}
