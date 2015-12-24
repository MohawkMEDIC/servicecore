using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MARC.HI.EHRS.SVC.Core.Services.Security
{

    /// <summary>
    /// Represents a service contract for role providers
    /// </summary>
    public interface IRoleProviderService
    {

        void AddUserToRole();

    }
}
