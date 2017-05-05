using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MARC.HI.EHRS.SVC.Core.Configuration.UI
{
    /// <summary>
    /// Represents a trace source attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TraceSourceAttribute : Attribute
    {

        /// <summary>
        /// Trace source attribute
        /// </summary>
        public TraceSourceAttribute(String sourceName)
        {
            this.SourceName = sourceName;
        }

        /// <summary>
        /// Gets or sets the source name
        /// </summary>
        public String SourceName { get; set; }
        

        /// <summary>
        /// Represent this as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.SourceName;
        }

        /// <summary>
        /// Get hash code
        /// </summary>
        public override int GetHashCode()
        {
            return this.SourceName.GetHashCode();
        }
    }
}
