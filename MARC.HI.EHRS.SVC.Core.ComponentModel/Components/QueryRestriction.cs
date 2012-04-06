using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.DataTypes;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Core.ComponentModel.Components
{
    /// <summary>
    /// Query restriction component is used to filter additional 
    /// data from a query match object
    /// </summary>
    [Serializable][XmlType("QueryRestriction")]
    public class QueryRestriction : HealthServiceRecordComponent
    {

        /// <summary>
        /// Signals that the SHR should filter on records that have
        /// been ammended or created since the date specified
        /// </summary>
        [XmlElement("amendDate")]
        public TimestampPart AmmendedDate { get; set; }

        /// <summary>
        /// Signals that the SHR should filter to only the most
        /// recent record of each type
        /// </summary>
        [XmlAttribute("mostRecentByType")]
        public bool MostRecentByType { get; set; }

    }
}
