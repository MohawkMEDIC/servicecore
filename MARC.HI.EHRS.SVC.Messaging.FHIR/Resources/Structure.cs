using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using System.Xml.Serialization;
using System.ComponentModel;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{
    /// <summary>
    /// Represents a structure definition
    /// </summary>
    [XmlType("Structure", Namespace = "http://hl7.org/fhir")]
    public class Structure 
    {

        /// <summary>
        /// Gets or sets the resouce type
        /// </summary>
        [XmlIgnore]
        public Type ResouceType { get; set; }

        /// <summary>
        /// Gets or sets the type of the structure
        /// </summary>
        [XmlElement("type")]
        [Description("The Resource or Data type being described")]
        public PrimitiveCode<String> Type { 
            get
            {
                object[] typeName = this.ResouceType.GetCustomAttributes(typeof(XmlTypeAttribute), true);
                if(typeName == null)
                    return null;
                else
                    return new PrimitiveCode<String>((typeName[0] as XmlTypeAttribute).TypeName);
            }
            set
            {
                if(value == null)
                    return;

                // Find the value type
                this.ResouceType = this.GetType().Assembly.GetTypes().FirstOrDefault((t)=>{
                     object[] typeName = this.ResouceType.GetCustomAttributes(typeof(XmlTypeAttribute), true);
                    if(typeName == null)
                        return false;
                    else
                        return (typeName[0] as XmlTypeAttribute).TypeName == value;
                });


            }
        }

        /// <summary>
        /// Gets or sets the name of the structure
        /// </summary>
        [Description("The name of this resource constraint statement (to refer to it from other resource constraints)")]
        [XmlElement("name")]
        public FhirString Name { get; set; }
        /// <summary>
        /// Gets or sets the purpose
        /// </summary>
        [Description("Human summary: why describe this resource?")]
        [XmlElement("purpose")]
        public FhirString Purpose { get; set; }
        /// <summary>
        /// Gets or sets the profile to which the structure belongs
        /// </summary>
        [Description("Reference to a resource profile that includes the constraint statement that applies to this resource")]
        [XmlElement("profile")]
        public FhirString Profile { get; set; }
        /// <summary>
        /// Gets or sets the search parameters supported on the structure
        /// </summary>
        [XmlElement("searchParam")]
        [Description("Defines additional search parameters for implementations to support and/or make use of")]
        public List<SearchParam> SearchParams { get; set; }
        /// <summary>
        /// Gets or sets the elements that are supported in the resource
        /// </summary>
        [XmlElement("element")]
        [Description("Captures constraints on each element within the resource")]
        public List<Element> Elements { get; set; }
    }
}
