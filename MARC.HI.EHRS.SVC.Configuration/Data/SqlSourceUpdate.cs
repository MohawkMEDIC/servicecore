using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace MARC.HI.EHRS.SVC.Configuration.Data
{
    /// <summary>
    /// Represents an IDataUpdate drawn from a SQL file
    /// </summary>
    public class SqlSourceUpdate : IDataUpdate
    {

        // Metadata regex
        private static Regex m_metaRegx = new Regex(@"\/\*\*(.*?)\*\/");

        // Deploy sql
        private string m_deploySql;

        // Check SQL
        private string m_checkRange;

        // Invariant name
        private string m_invariant;

        /// <summary>
        /// Load the specified stream
        /// </summary>
        public static SqlSourceUpdate Load(Stream source)
        {

            var retVal = new SqlSourceUpdate();

            // Get deployment sql
            using(var sr = new StreamReader(source))
                retVal.m_deploySql = sr.ReadToEnd();

            var xmlSql = m_metaRegx.Match(retVal.m_deploySql.Replace("\r\n",""));
            if (xmlSql.Success)
            {
                var xmlText = xmlSql.Groups[1].Value.Replace("*", "");
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(xmlText);
                retVal.Name = xd.SelectSingleNode("/update/@id")?.Value ?? "other update";
                retVal.Description = xd.SelectSingleNode("/update/summary/text()")?.Value ?? "other update";

                retVal.m_checkRange = xd.SelectSingleNode("/update/@applyRange")?.Value;
                retVal.m_invariant = xd.SelectSingleNode("/update/@invariant")?.Value;

            }
            else
                throw new InvalidOperationException("Invalid SQL update file");

            return retVal;
        }

        /// <summary>
        /// Gets the description of the update
        /// </summary>
        public string Description
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the name of the update
        /// </summary>
        public string Name
        {
            get; private set;
        }

        /// <summary>
        /// Gets the check sql
        /// </summary>
        public string GetCheckSql(string invariantName)
        {
            switch(invariantName.ToLower())
            {
                case "npgsql":
                    var updateRange = this.m_checkRange.Split('-');
                    return $"select string_to_array(get_sch_vrsn(), '.')::int[] between string_to_array('{updateRange[0]}','.')::int[] and string_to_array('{updateRange[1]}', '.')::int[]";
                default:
                    throw new InvalidOperationException($"This update provider does not support {invariantName}");
            }
        }

        /// <summary>
        /// Get the deployment sql
        /// </summary>
        public string GetDeploySql(string invariantName)
        {
            if (this.m_invariant == invariantName)
                return this.m_deploySql;
            else
                return null;
        }
    }
}
