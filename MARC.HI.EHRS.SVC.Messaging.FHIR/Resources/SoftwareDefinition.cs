using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using System.Reflection;
using System.IO;
using System.ComponentModel;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{
    /// <summary>
    /// Software definition
    /// </summary>
    [XmlType("Software", Namespace = "http://hl7.org/fhir")]
    public class SoftwareDefinition : Shareable
    {

        /// <summary>
        /// From assembly information
        /// </summary>
        public static SoftwareDefinition FromAssemblyInfo()
        {
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            var retVal = new SoftwareDefinition();
            
            // Get assembly attributes
            var title = entryAssembly.GetCustomAttribute<AssemblyTitleAttribute>();
            var version = entryAssembly.GetName().Version.ToString();
            var releaseDate = new FileInfo(Assembly.GetEntryAssembly().Location).LastWriteTime;

            if (title != null)
                retVal.Name = title.Title;
            else
                retVal.Name = entryAssembly.FullName;

            retVal.Version = version;
            retVal.ReleaseDate = releaseDate;

            return retVal;
        }

        /// <summary>
        /// Name of the software
        /// </summary>
        [XmlElement("name")]
        [Description("Name the software is known by")]
        [ElementProfile(MinOccurs = 1)]
        public FhirString Name { get; set; }

        /// <summary>
        /// Version of the software
        /// </summary>
        [XmlElement("version")]
        [Description("Version covered by this statement")]
        public FhirString Version { get; set; }

        /// <summary>
        /// Release date of the software
        /// </summary>
        [XmlElement("releaseDate")]
        [Description("The date this version was released")]
        public Date ReleaseDate { get; set; }

    }
}
