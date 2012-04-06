using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Subscription.Core
{
    /// <summary>
    /// Operator type
    /// </summary>
    [XmlType("OperatorType", Namespace = "urn:marc-hi:ehrs:subscription")]
    public enum OperatorType
    {
        [XmlEnum("eq")]
        EqualTo,
        [XmlEnum("lt")]
        LessThan,
        [XmlEnum("le")]
        LessThanEqualTo,
        [XmlEnum("gt")]
        GreaterThan,
        [XmlEnum("ge")]
        GreaterThanEqualTo,
        [XmlEnum("ne")]
        NotEqualTo
    }
}
