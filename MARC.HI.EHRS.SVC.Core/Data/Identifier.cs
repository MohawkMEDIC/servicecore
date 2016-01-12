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
        /// Creates a new empty identifier
        /// </summary>
        public Identifier()
        {

        }

        /// <summary>
        /// Creates a new identifier with the specified identifier
        /// </summary>
        public Identifier(TIdentifier id)
        {
            this.Id = id;
        }

        /// <summary>
        /// Creates a new identifier with specified identifier and version
        /// </summary>
        public Identifier(TIdentifier id, TIdentifier versionId) : this(id)
        {
            this.VersionId = versionId;
        }

        /// <summary>
        /// Create a new identifier with specified id and AA
        /// </summary>
        public Identifier(TIdentifier id, OidData assigningAuthority) : this(id)
        {
            this.AssigningAuthority = assigningAuthority;
        }

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
