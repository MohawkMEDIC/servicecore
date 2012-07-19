/* 
 * Copyright 2008-2011 Mohawk College of Applied Arts and Technology
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
 * User: Justin Fyfe
 * Date: 08-24-2011
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MARC.HI.EHRS.SVC.Core.DataTypes
{
    /// <summary>
    /// Provides a mechanism for locating OIDs by friendly name
    /// </summary>
    public class OidRegistrar : IEnumerable<MARC.HI.EHRS.SVC.Core.DataTypes.OidRegistrar.OidData>
    {
        /// <summary>
        /// OID Data
        /// </summary>
        public class OidData
        {
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
        }

        /// <summary>
        /// Registered OIDs
        /// </summary>
        private List<OidData> m_registerOids = new List<OidData>();

        /// <summary>
        /// Register an oid
        /// </summary>
        /// <param name="name">The friendly name of the OID</param>
        /// <param name="oid">The OID itself</param>
        /// <param name="desc">A description for the OID</param>
        public void Register(string name, string oid, string desc, string uri)
        {
            // is there another oid with this name
            if (m_registerOids.Count(o => o.Name.Equals(name)) > 0)
                throw new InvalidOperationException("Cannot register a duplicate named OID");

            m_registerOids.Add(new OidData()
            {
                Name = name,
                Description = desc,
                Oid = oid,
                Ref = new Uri(uri ?? string.Format("oid:{0}", oid))
            });
        }

        /// <summary>
        /// Lookup an oid by its name
        /// </summary>
        public OidData GetOid(string oidName)
        {
            return m_registerOids.Find(o => o.Name.Equals(oidName));
        }

        /// <summary>
        /// Find OID data by OID
        /// </summary>
        public OidData FindData(string oid)
        {
            return m_registerOids.Find(o => o.Oid.Equals(oid));
        }

        /// <summary>
        /// Find an OID by its reference
        /// </summary>
        public OidData FindData(Uri reference)
        {
            return this.m_registerOids.Find(o => o.Ref.Equals(reference));
        }

        /// <summary>
        /// Delete an oid
        /// </summary>
        /// <param name="oidName"></param>
        public void DeleteOid(string oidName)
        {
            m_registerOids.RemoveAll(o => o.Name.Equals(oidName));
        }

        #region IEnumerable<OidData> Members

        /// <summary>
        /// Get the OID enumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<OidRegistrar.OidData>  GetEnumerator()
        {
            m_registerOids.Sort((a, b) => a.Name.CompareTo(b.Name));
            return m_registerOids.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Get the OID Enumerator
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator  System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
}
}
