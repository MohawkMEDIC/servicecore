using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MARC.HI.EHRS.SVC.Core.Configuration
{
    /// <summary>
    /// Represents a configuration panel that is always deployed (is not enabled or disabled, but always present)
    /// </summary>
    public interface IAlwaysDeployedConfigurationPanel : IConfigurationPanel
    {
    }
}
