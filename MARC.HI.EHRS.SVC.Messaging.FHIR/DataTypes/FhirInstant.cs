﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes
{
    /// <summary>
    /// Instant in time
    /// </summary>
    [XmlType("instant", Namespace = "http://hl7.org/fhir")]
    public class FhirInstant : FhirDateTime
    {
    }
}
