/**
 * Copyright 2012-2013 Mohawk College of Applied Arts and Technology
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
 * Date: 1-8-2012
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Core.DataTypes
{
    /// <summary>
    /// Groups one or more name components into a set
    /// </summary>
    [Serializable]
    [XmlType("NameSet", Namespace = "urn:marc-hi:svc:componentModel")]
    public class NameSet : Datatype
    {
        public enum NameSetUse
        {
            /// <summary>
            /// A name to be used for searching only
            /// </summary>
            Search = 0x0,
            /// <summary>
            /// The name represents the legal name of the entity
            /// </summary>
            Legal = 0x1,
            /// <summary>
            /// The name is an official name for the entity
            /// </summary>
            OfficialRecord = 0x2,
            /// <summary>
            /// The name is used for licensing purposes
            /// </summary>
            License = 0x3,
            /// <summary>
            /// The name is a tribal name
            /// </summary>
            Indigenous = 0x4,
            /// <summary>
            /// The name is an alternate name (ie: AKA)
            /// </summary>
            Pseudonym = 0x5,
            /// <summary>
            /// The name is an artistic name (ie: Prince)
            /// </summary>
            Artist = 0x6,
            /// <summary>
            /// The name is used in a religious context (ie: Baptismal name)
            /// </summary>
            Religious = 0x7,
            /// <summary>
            /// The name has been assigned by an authority
            /// </summary>
            Assigned = 0x8,
            /// <summary>
            /// The name is a phonetic representation of the name
            /// </summary>
            Phonetic = 0x9,
            /// <summary>
            /// Maiden name
            /// </summary>
            MaidenName = 0xa
        }

        /// <summary>
        /// Gets the list of name parts
        /// </summary>
        [XmlElement("part")]
        public List<NamePart> Parts { get; set; }

        /// <summary>
        /// Gets or sets the conditions under which the name can be used
        /// </summary>
        [XmlAttribute("use")]
        public NameSetUse Use { get; set; }

        /// <summary>
        /// Creates a new instance of the name set
        /// </summary>
        public NameSet()
        {
            this.Parts = new List<NamePart>();
        }

        /// <summary>
        /// Represent the name part as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var retStr = new StringBuilder();
            foreach (var part in this.Parts)
                retStr.AppendFormat("({1}){0} ", part.Value, part.Type);
            return retStr.ToString();
        }

        /// <summary>
        /// Return the similarity of the names to one another
        /// </summary>
        public float SimilarityTo(NameSet other)
        {
            int nMatched = 0;
            foreach (var part in this.Parts)
                nMatched += other.Parts.Count(o => o.Type == part.Type && o.Value.ToLower()== part.Value.ToLower());

            if (this.Parts.Count == 0 && other.Parts.Count == 0)
                return 1;
            return (float)nMatched / this.Parts.Count;
        }
    }
}
