using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Auditing.Data
{
    /// <summary>
    /// Represents an audit 
    /// </summary>
    [XmlType(nameof(AuditCode), Namespace = "http://marc-hi.ca/svc/audit")]
    public class AuditCode
    {
        /// <summary>
        /// Construct a new code
        /// </summary>
        public AuditCode(string code, string codeSystem)
        {
            this.Code = code;
            this.CodeSystem = codeSystem;
        }

        /// <summary>
        /// Gets or sets the code of the code value
        /// </summary>
        [XmlAttribute("code")]
        public String Code { get; set; }
        /// <summary>
        /// Gets or sets the system in which the code value is drawn
        /// </summary>
        [XmlAttribute("system")]
        public String CodeSystem { get; set; }

        /// <summary>
        /// Gets or sets the human readable name of the code sytsem
        /// </summary>
        [XmlAttribute("systemName")]
        public string CodeSystemName { get; set; }

        /// <summary>
        /// Gets or sets the version of the code system
        /// </summary>
        [XmlAttribute("systemVersion")]
        public string CodeSystemVersion { get; set; }

        /// <summary>
        /// Gets or sets the display name
        /// </summary>
        [XmlAttribute("display")]
        public String DisplayName { get; set; }

    }
}
