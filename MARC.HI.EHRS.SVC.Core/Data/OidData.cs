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

namespace MARC.HI.EHRS.SVC.Core.Data
{
    /// <summary>
    /// OID Data
    /// </summary>
    public class OidData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OidData()
        {
            this.Attributes = new List<KeyValuePair<string, string>>();

        }

        /// <summary>
        /// The name of the OID
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The OID Value
        /// </summary>
        public string Oid { get; set; }

        /// <summary>
        /// The description for the oid
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The local mnemonic for the oid
        /// </summary>
        public string Mnemonic { get; set; }

        /// <summary>
        /// Extended attributes
        /// </summary>
        public List<KeyValuePair<String, String>> Attributes { get; set; }

        /// <summary>
        /// Reference to the OID
        /// </summary>
        public Uri Ref { get; set; }

        /// <summary>
        /// Represent this as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} ({1})", Oid, Description);
        }

        /// <summary>
        /// Clone this OID
        /// </summary>
        internal OidData Clone()
        {
            OidData retVal = new OidData();
            retVal.Name = this.Name;
            retVal.Description = this.Description;
            retVal.Oid = this.Oid;
            retVal.Ref = new Uri(this.Ref.ToString());
            retVal.Attributes = new List<KeyValuePair<string, string>>();
            foreach (var kv in this.Attributes)
                retVal.Attributes.Add(new KeyValuePair<string, string>(kv.Key, kv.Value));
            return retVal;
        }
    }
}
