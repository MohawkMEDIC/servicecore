using System.Collections.Generic;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Core.Configuration.DbXml
{
    /// <summary>
    /// Represents an abstract command to do something against the database
    /// </summary>
    [XmlType("Command", Namespace = "http://openiz.org/plugin/dbxml")]
    public abstract class Command : DbXmlBase
    {
        /// <summary>
        /// A series of "where" clauses which when all execute to "TRUE" instruct the
        /// command text to be executed
        /// </summary>
        [XmlArray("condition")]
        [XmlArrayItem("sqlCommand", Type = typeof(SqlCommand))]
        public List<Command> Condition { get; set; }

    }
}