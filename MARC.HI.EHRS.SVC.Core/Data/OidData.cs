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
