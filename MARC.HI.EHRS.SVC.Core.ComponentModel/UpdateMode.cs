using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MARC.HI.EHRS.SVC.Core.ComponentModel
{
    /// <summary>
    /// Update mode
    /// </summary>
    public enum UpdateMode
    {
        /// <summary>
        /// Add the component if it doesn't exist
        /// </summary>
        Add,
        /// <summary>
        /// Remove the component if it exists
        /// </summary>
        Remove
    }
}
