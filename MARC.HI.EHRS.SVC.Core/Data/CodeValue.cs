using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MARC.HI.EHRS.SVC.Core.Data
{
    /// <summary>
    /// Represents a basic codified value
    /// </summary>
    public class CodeValue
    {
        /// <summary>
        /// Construct a new code
        /// </summary>
        public CodeValue(string code, string codeSystem)
        {
            this.Code = code;
            this.CodeSystem = codeSystem;
        }

        /// <summary>
        /// Gets or sets the code of the code value
        /// </summary>
        public String Code { get; set; }
        /// <summary>
        /// Gets or sets the system in which the code value is drawn
        /// </summary>
        public String CodeSystem { get; set; }
        /// <summary>
        /// Gets or sets the display name
        /// </summary>
        public String DisplayName { get; set; }

    }
}
