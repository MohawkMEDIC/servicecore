using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MARC.HI.EHRS.SVC.Core.Plugins
{
    /// <summary>
    /// Indicates a GUID identified dependency on another assembly
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class AssemblyPluginDependencyAttribute : Attribute
    {

        /// <summary>
        /// The assembly's guid for dependency resolution
        /// </summary>
        public Guid DependentAssemblyGuid { get; set; }

        /// <summary>
        /// The dependent version of the assembly
        /// </summary>
        public String DependentVersion { get; set; }

        /// <summary>
        /// Create a new dependency attribute
        /// </summary>
        public AssemblyPluginDependencyAttribute(String guid, String dependentVersion)
        {
            this.DependentAssemblyGuid = Guid.Parse(guid);
            this.DependentVersion = dependentVersion;
        }
    }
}
