using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MARC.HI.EHRS.SVC.Core.Services.Security
{
    /// <summary>
    /// Password hashing service
    /// </summary>
    public interface IPasswordHashingService
    {

        /// <summary>
        /// Encode a password
        /// </summary>
        String EncodePassword(String password);

    }
}
