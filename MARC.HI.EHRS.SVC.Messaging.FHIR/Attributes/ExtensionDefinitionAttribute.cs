﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes
{
    /// <summary>
    /// Identifies an extension definition
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true)]
    public class ExtensionDefinitionAttribute : Attribute
    {

        /// <summary>
        /// Gets or sets the name of the extension
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Identifies the type that carries the value
        /// </summary>
        public Type ValueType { get; set; }


        /// <summary>
        /// Gets the type which hosts the attribute
        /// </summary>
        public Type HostType { get; set; }

        /// <summary>
        /// Short description
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// Format definition
        /// </summary>
        public string FormalDefinition { get; set; }

        /// <summary>
        /// Gets or sets the property to which the extension applies
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// True if the implementer must support
        /// </summary>
        public bool MustSupport { get; set; }

        /// <summary>
        /// True if the implementer must understand
        /// </summary>
        public bool MustUnderstand { get; set; }

        /// <summary>
        /// Identifies the binding (value set)
        /// </summary>
        public Type Binding { get; set; }


    }
}
