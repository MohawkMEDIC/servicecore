using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MARC.HI.EHRS.SVC.Core.Plugins
{
    /// <summary>
    /// This interface is insert into an assembly manifest via [assembly:PluginAssembly()] 
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class AssemblyPluginAttribute : Attribute
    {
    }
}
