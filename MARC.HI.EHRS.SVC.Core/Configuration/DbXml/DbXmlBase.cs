using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Core.Configuration.DbXml
{
    /// <summary>
    /// Base type for DB-XML stuff
    /// </summary>
    [XmlType("DbXmlBase", Namespace = "http://openiz.org/plugin/dbxml")]
    public abstract class DbXmlBase 
    {
        /// <summary>
        /// The database management system on which to run the command
        /// </summary>
        [XmlAttribute("dbms")]
        public string Dbms { get; set; }
        /// <summary>
        /// The identifier of the command (for execute once)
        /// </summary>
        [XmlAttribute("id")]
        public string Id { get; set; }

    }
}