using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MARC.HI.EHRS.SVC.Core.Attributes
{
    /// <summary>
    /// Allows a class to describe the trace source that it uses
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TraceSourceAttribute : Attribute
    {

        /// <summary>
        /// Constructor which initializes the trace source name attribute
        /// </summary>
        public TraceSourceAttribute(String traceSourceName)
        {
            this.TraceSourceName = traceSourceName;
        }

        /// <summary>
        /// Gets the trace source name
        /// </summary>
        public String TraceSourceName { get; private set; }
    }
}
