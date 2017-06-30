using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Core.Configuration.DbXml
{
    /// <summary>
    /// SQL Command 
    /// </summary>
    [XmlType("SqlCommand", Namespace = "http://marc-hi.ca/plugin/dbxml")]
    public class SqlCommand : Command
    {
        /// <summary>
        /// The body of the SQL
        /// </summary>
        [XmlText]
        public string Sql { get; set; }

    }
}