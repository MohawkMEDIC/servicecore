using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Core.DataTypes
{

    /// <summary>
    /// Update mode of the data
    /// </summary>
    public enum UpdateModeType
    {
        /// <summary>
        /// Add the data no matter what
        /// </summary>
        Add,
        /// <summary>
        /// Add the data or update if it is already present
        /// </summary>
        AddOrUpdate,
        /// <summary>
        /// Perform an update only
        /// </summary>
        Update,
        /// <summary>
        /// Remove the data
        /// </summary>
        Remove
    }

    /// <summary>
    /// Represents the base of all data types
    /// </summary>
    [Serializable][XmlType("Datatype")]
    public abstract class Datatype
    {

        /// <summary>
        /// Represents the key for the items
        /// </summary>
        [XmlAttribute("key")]
        public decimal Key { get; set; }

        /// <summary>
        /// Update mode of the data
        /// </summary>
        [XmlAttribute("updateMode")]
        public UpdateModeType UpdateMode { get; set; }

    }
}
