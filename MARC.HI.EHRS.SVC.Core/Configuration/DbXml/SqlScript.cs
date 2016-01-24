using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Core.Configuration.DbXml
{
    /// <summary>
    /// SQL Command 
    /// </summary>
    [XmlType("SqlScript", Namespace = "http://marc-hi.ca/plugin/dbxml")]
    public class SqlScript : Command
    {

        /// <summary>
        /// Gets the resource name
        /// </summary>
        [XmlAttribute("resourceName")]
        public string ResourceName { get; set; }
        
        /// <summary>
        /// Gets or sets the assembly name
        /// </summary>
        [XmlAttribute("assemblyName")]
        public string AssemblyName { get; set; }

        /// <summary>
        /// Gets or sets s file reference
        /// </summary>
        [XmlAttribute("fileName")]
        public string FileName { get; set; }
    }
}