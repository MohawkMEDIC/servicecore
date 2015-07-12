using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Core.Configuration.Update
{


    /// <summary>
    /// Script file
    /// </summary>
    [XmlType("DbSchemaScriptFile", Namespace = "urn:marc-hi:svc:configuration")]
    public class DbSchemaScriptFile
    {
        /// <summary>
        /// The file which this script is contained in
        /// </summary>
        [XmlAttribute("file")]
        public String File { get; set; }
    }

    /// <summary>
    /// Represents a single update to a database
    /// </summary>
    [XmlType("DbSchemaUpdate", Namespace = "urn:marc-hi:svc:configuration")]
    [XmlRoot("update", Namespace = "urn:marc-hi:svc:configuration")]
    public class DbSchemaUpdate
    {
        /// <summary>
        /// The identifier of the update
        /// </summary>
        [XmlAttribute("id")]
        public String Id { get; set; }

        /// <summary>
        /// The function which returns the current version
        /// </summary>
        [XmlAttribute("verFunction")]
        public String VersionFunction { get; set; }

        /// <summary>
        /// The lower version of the db that this update applies to
        /// </summary>
        [XmlAttribute("from")]
        public String FromVersion { get; set; }

        /// <summary>
        /// The version that this update brings the database to
        /// </summary>
        [XmlAttribute("to")]
        public String ToVersion { get; set; }

        /// <summary>
        /// The list of installation scripts to update the database
        /// </summary>
        [XmlElement("install")]
        public List<DbSchemaScriptFile> InstallScript { get; set; }

        /// <summary>
        /// The list of uninstallation scripts to un-update the database
        /// </summary>
        [XmlElement("uninstall")]
        public List<DbSchemaScriptFile> UninstallScript { get; set; }

        /// <summary>
        /// The description of the update
        /// </summary>
        [XmlElement("description")]
        public String Description { get; set; }

        /// <summary>
        /// Load the dbschema udpate file
        /// </summary>
        public static DbSchemaUpdate Load(string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException("Specified update file not found");
            using(XmlReader rdr = XmlReader.Create(fileName))
            {
                XmlSerializer xsz = new XmlSerializer(typeof(DbSchemaUpdate));
                if (!xsz.CanDeserialize(rdr))
                    return null;
                DbSchemaUpdate retVal = xsz.Deserialize(rdr) as DbSchemaUpdate;
                
                // Install script correction
                if (retVal.InstallScript != null)
                    for (int i = 0; i < retVal.InstallScript.Count; i++)
                        retVal.InstallScript[i].File = Path.Combine(Path.GetDirectoryName(fileName), retVal.InstallScript[i].File);
                // UnInstall script correction
                if (retVal.InstallScript != null)
                    for (int i = 0; i < retVal.UninstallScript.Count; i++)
                        retVal.UninstallScript[i].File = Path.Combine(Path.GetDirectoryName(fileName), retVal.UninstallScript[i].File);

                return retVal;
            }
        }

        /// <summary>
        /// The description of the update
        /// </summary>
        public override string ToString()
        {
            return String.Format("{0}: {1} (v:{2} -> v:{3})", this.Id, this.Description, this.FromVersion, this.ToVersion);
        }
    }
}
