using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MARC.HI.EHRS.SVC.Core.Data
{
    /// <summary>
    /// Represents any model class which is identified
    /// </summary>
    public interface IIdentified<TIdentifier>
    {
        /// <summary>
        /// Gets the identifier of the IIdentified object
        /// </summary>
        Identifier<TIdentifier> Id { get; }
    }
}
