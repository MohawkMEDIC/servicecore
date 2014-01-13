﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources.Attributes
{
    /// <summary>
    /// Represents the confidence of a result
    /// </summary>
    public class ConfidenceAttribute : ResourceAttributeBase
    {
        /// <summary>
        /// Represents the confidence of a result
        /// </summary>
        public decimal Confidence { get; set; }
    }
}
