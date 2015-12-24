using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MARC.HI.EHRS.SVC.Core.Data
{
    /// <summary>
    /// Represents an identifier used for storing / retrieving data
    /// </summary>
    public class Identifier<TIdentifier>
    {
        /// <summary>
        /// Get the assigning authority of the identifier
        /// </summary>
        public OidData AssigningAuthority { get; set; }
        /// <summary>
        /// Represents the identifier of the object
        /// </summary>
        public TIdentifier Id { get; set; }
        /// <summary>
        /// Represents the version of the object
        /// </summary>
        public TIdentifier VersionId { get; set; }
    }
}
