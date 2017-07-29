using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MARC.HI.EHRS.SVC.Core.Data
{
    /// <summary>
    /// Represents a user in the service core context
    /// </summary>
    public abstract class User
    {
        /// <summary>
        /// Gets or sets the user's name
        /// </summary>
        public string Name { get; set; }

        
    }
}
