using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.DataTypes;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Core.ComponentModel.Components
{
    /// <summary>
    /// Health service record component reference
    /// </summary>'
    [Serializable][XmlType("HealthServiceRecordComponentRef")]
    public class HealthServiceRecordComponentRef : HealthServiceRecordContainer
    {

        /// <summary>
        /// Gets or sets the alternate identifier
        /// </summary>
        [XmlElement("altId")]
        public DomainIdentifier AlternateIdentifier { get; set; }

    }
}
