using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes
{
    /// <summary>
    /// Identifies a profile
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ProfileAttribute : Attribute
    {

        /// <summary>
        /// Gets or sets the profile identifier
        /// </summary>
        public string ProfileId { get; set; }

        /// <summary>
        /// Gets or sets the name of the profile
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Import for the profile
        /// </summary>
        public string Import { get; set; }

    }
}
