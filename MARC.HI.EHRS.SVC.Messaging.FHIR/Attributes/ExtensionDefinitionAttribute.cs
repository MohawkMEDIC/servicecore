using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes
{
    /// <summary>
    /// Identifies an extension definition
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true)]
    public class ExtensionDefinitionAttribute : ElementProfileAttribute
    {

        /// <summary>
        /// Gets or sets the name of the extension
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Identifies the type that carries the value
        /// </summary>
        public Type ValueType { get; set; }



    }
}
