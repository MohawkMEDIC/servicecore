using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MARC.HI.EHRS.SVC.Core.Services
{

    /// <summary>
    /// Service instantiation type
    /// </summary>
    public enum ServiceInstantiationType
    {
        /// <summary>
        /// The service class is constructed once and one instance is shared in the entire application domain
        /// </summary>
        Singleton,
        /// <summary>
        /// The service class is instantiated for each call of GetService()
        /// </summary>
        Instance
    }

    /// <summary>
    /// Identifies the manner in which a service is executed
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : Attribute
    {

        /// <summary>
        /// Create a new service attibute
        /// </summary>
        public ServiceAttribute(ServiceInstantiationType type)
        {
            this.Type = type;
        }

        /// <summary>
        /// Service type
        /// </summary>
        public ServiceInstantiationType Type { get; set; }
    }
}
