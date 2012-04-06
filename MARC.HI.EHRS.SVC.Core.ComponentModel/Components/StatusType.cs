using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MARC.HI.EHRS.SVC.Core.ComponentModel.Components
{
    /// <summary>
    /// Identifies the status codes that an object can be "in"
    /// </summary>
    [Flags]
    public enum StatusType
    {
        Unknown = 0x0,
        New = 0x01, 
        Active = 0x02,
        Cancelled = 0x04,
        Completed = 0x08,
        Obsolete = 0x10,
        Aborted = 0x20,
        Nullified = 0x40
    }
}
