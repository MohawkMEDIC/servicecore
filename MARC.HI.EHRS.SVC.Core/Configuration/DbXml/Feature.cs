/*
 * Copyright 2010-2018 Mohawk College of Applied Arts and Technology
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you 
 * may not use this file except in compliance with the License. You may 
 * obtain a copy of the License at 
 * 
 * http://www.apache.org/licenses/LICENSE-2.0 
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations under 
 * the License.
 * 
 * User: fyfej
 * Date: 1-9-2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Core.Configuration.DbXml
{
    [XmlType(Namespace = "http://marc-hi.ca/plugin/dbxml", TypeName = "Feature")]
    [XmlRoot(Namespace = "http://marc-hi.ca/plugin/dbxml", ElementName = "Feature")]
    public class Feature : DbXmlBase
    {
        /// <summary>
        /// The version of the feature
        /// </summary>
        [XmlAttribute("version")]
        public String Version { get; set; }

        /// <summary>
        /// The list of commands used to install the feature
        /// </summary>
        [XmlArray("install")]
        [XmlArrayItem("sqlCommand", Type = typeof(SqlCommand))]
        [XmlArrayItem("sqlScript", Type = typeof(SqlScript))]
        public List<Command> Install { get; set; }

        /// <summary>
        /// The list of commands used to install the feature
        /// </summary>
        [XmlArray("uninstall")]
        [XmlArrayItem("sqlCommand", Type = typeof(SqlCommand))]
        [XmlArrayItem("sqlScript", Type=typeof(SqlScript))]
        public List<Command> UnInstall { get; set; }

    }
}
