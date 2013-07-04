﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes
{
    /// <summary>
    /// Represents a profile on a resource property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true)]
    public class ElementProfileAttribute : Attribute
    {

        /// <summary>
        /// Profile attribute
        /// </summary>
        public ElementProfileAttribute()
        {
            this.MaxOccurs = 1;
            this.MinOccurs = 0;
            this.MustSupport = true;
            this.MustUnderstand = true;
        }

        /// <summary>
        /// Gets the type which hosts the attribute
        /// </summary>
        public Type HostType { get; set; }

        /// <summary>
        /// The property being profiled
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// Short description
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// Format definition
        /// </summary>
        public string FormalDefinition { get; set; }

        /// <summary>
        /// Comment
        /// </summary>
        public string Comment { get; set; }

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

        /// <summary>
        /// Min-occurs
        /// </summary>
        public int MinOccurs { get; set; }

        /// <summary>
        /// Max-occurs
        /// </summary>
        public int MaxOccurs { get; set; }

    }
}
