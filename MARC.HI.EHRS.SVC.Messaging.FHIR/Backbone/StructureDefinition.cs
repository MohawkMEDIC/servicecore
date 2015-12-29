using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using System.Xml.Serialization;
using System.ComponentModel;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Backbone
{
    /// <summary>
    /// Represents a structure definition
    /// </summary>
    [XmlType("Structure", Namespace = "http://hl7.org/fhir")]
    public class StructureDefinition : BackboneElement
    {
        /// <summary>
        /// Sharable structure
        /// </summary>
        public StructureDefinition() : base()
        {
            this.Elements = new List<Element>();
            this.SearchParams = new List<SearchParamDefinition>();
        }

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
        [ElementProfile(MinOccurs = 1)]
        public FhirCode<String> Type { 
            get
            {
                object[] typeName = this.ResouceType.GetCustomAttributes(typeof(XmlTypeAttribute), true);
                if(typeName == null)
                    return null;
                else
                    return new FhirCode<String>((typeName[0] as XmlTypeAttribute).TypeName);
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
        /// Gets or sets the elements that are supported in the resource
        /// </summary>
        [XmlElement("element")]
        [Description("Captures constraints on each element within the resource")]
        public List<Element> Elements { get; set; }
        
        /// <summary>
        /// Gets or sets the search parameters supported on the structure
        /// </summary>
        [XmlElement("searchParam")]
        [Description("Defines additional search parameters for implementations to support and/or make use of")]
        public List<SearchParamDefinition> SearchParams { get; set; }

        /// <summary>
        /// Write text
        /// </summary>
        internal override void WriteText(System.Xml.XmlWriter w)
        {
            w.WriteStartElement("table");
            w.WriteAttributeString("border", "1");
            w.WriteElementString("caption", this.Name);
            w.WriteStartElement("tbody");

            w.WriteStartElement("tr");
            w.WriteStartElement("td");

            w.WriteStartElement("table");
            w.WriteAttributeString("border", "1");
            w.WriteElementString("caption", String.Format("Definition",  this.Name));
            w.WriteStartElement("tbody");

            // Write header rows 
            w.WriteStartElement("tr");
            base.WriteTableHeader(w, (FhirString)"Path");
            base.WriteTableHeader(w, (FhirString)"Multiplicity");
            base.WriteTableHeader(w, (FhirString)"Type");
            base.WriteTableHeader(w, (FhirString)"Comments");
            w.WriteEndElement(); // tr

            foreach (var element in this.Elements)
                element.WriteText(w);

            // end
            w.WriteEndElement(); // tbody
            w.WriteEndElement(); // table
            w.WriteEndElement(); // td
            w.WriteEndElement(); // tr

            // next row
            w.WriteStartElement("tr");
            w.WriteStartElement("td");


            w.WriteStartElement("table");
            w.WriteAttributeString("border", "1");
            w.WriteElementString("caption", String.Format("Search Parameters", this.Name));
            w.WriteStartElement("tbody");
            // Write header rows 
            w.WriteStartElement("tr");
            base.WriteTableHeader(w, (FhirString)"Parameter");
            base.WriteTableHeader(w, (FhirString)"Type");
            base.WriteTableHeader(w, (FhirString)"Definition");
            w.WriteEndElement(); // tr

            // Write each of the profile elements
            foreach (var search in this.SearchParams)
                search.WriteText(w);

            w.WriteEndElement(); // tbody
            w.WriteEndElement(); // table
            w.WriteEndElement(); // td
            w.WriteEndElement(); // tr
            w.WriteEndElement(); // tbody
            w.WriteEndElement(); // table

            //w.WriteStartElement("div");
            //w.WriteAttributeString("class", "resource");

            //// Output the name of the resource
            //w.WriteStartElement("a");
            //w.WriteAttributeString("href", String.Format("#{0}", this.Name));
            //w.WriteAttributeString("class", "h2");
            //this.Name.WriteText(w);
            //w.WriteEndElement(); // span

            //// Elements
            //w.WriteStartElement("table");
            //w.WriteElementString("caption", "Elements");
            //w.WriteStartElement("tbody");

            //// Write each of the profile elements
            //foreach (var element in this.Elements)
            //    element.WriteText(w);

            //w.WriteEndElement(); // tbody
            //w.WriteEndElement(); // table


            //w.WriteStartElement("table");
            //w.WriteElementString("caption", "Search Parameters");
            //w.WriteStartElement("tbody");

            //// Write each of the profile elements
            //foreach (var search in this.SearchParams)
            //    search.WriteText(w);

            //w.WriteEndElement(); // tbody
            //w.WriteEndElement(); // table

            //w.WriteEndElement(); // div
        }
    }
}
