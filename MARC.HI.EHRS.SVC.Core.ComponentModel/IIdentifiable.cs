using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MARC.HI.EHRS.SVC.Core.ComponentModel
{
    /// <summary>
    /// Represents a container that has an 
    /// </summary>
    public interface IIdentifiable
    {
        /// <summary>
        /// Identifier of the object
        /// </summary>
        decimal Identifier { get; set; }

        /// <summary>
        /// The version identifier of the object
        /// </summary>
        decimal VersionIdentifier { get; set; }
    }
}
