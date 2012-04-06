using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.DataTypes;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Core.ComponentModel.Components
{
    /// <summary>
    /// Represents a masking indicator
    /// </summary>
    [Serializable][XmlType("MaskingIndicator")]
    public class MaskingIndicator : HealthServiceRecordComponent
    {
        /// <summary>
        /// Identifies the code of masking
        /// </summary>
        [XmlElement("code")]
        public CodeValue MaskingCode { get; set; }

    }
}
