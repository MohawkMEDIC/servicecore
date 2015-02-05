using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes
{
    /// <summary>
    /// Extension profile attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ExtensionProfileAttribute : Attribute
    {
        /// <summary>
        /// The class which contains extension defns
        /// </summary>
        public Type ExtensionClass { get; set; }
    }
}
