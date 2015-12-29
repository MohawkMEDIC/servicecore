using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using System.ComponentModel;
using System.Reflection;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Attributes;
using System.Collections;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Backbone
{
    /// <summary>
    /// Element definition
    /// </summary>
    [XmlType("ElementDefinition", Namespace = "http://hl7.org/fhir")]
    public class ElementDefinition : BackboneElement
    {

        /// <summary>
        /// Element definition
        /// </summary>
        public ElementDefinition()
        {
            this.Type = new List<TypeRef>();
        }

        /// <summary>
        /// Root type
        /// </summary>
        public ElementDefinition(Type rootType)
            : this()
        {
            var description = rootType.GetCustomAttribute<DescriptionAttribute>();
            var profile = rootType.GetCustomAttribute<ResourceProfileAttribute>();

            // Now populate!
            // First, populate what we can
            if (profile != null)
            {
                this.ShortDefinition = profile.Name;
                this.MaxOccurs = "1";
                this.MinOccurs = 1;
                this.MustSupport = false;
                this.IsModifier = false;
            }

            // Next description attribute
            if (description != null)
            {
                this.ShortDefinition = this.ShortDefinition ?? description.Description;
                this.FormalDefinition = this.FormalDefinition ?? description.Description;
            }

            this.Type.Add(new TypeRef() { Code = new FhirCode<string>("Resource") });
        }

        /// <summary>
        /// Create a new element definition from a property
        /// </summary>
        public ElementDefinition(PropertyInfo property) : this()
        {
            var description = property.GetCustomAttribute<DescriptionAttribute>();
            var profile = property.GetCustomAttribute<ElementProfileAttribute>();

            if (profile == null)
            {
                profile = new ElementProfileAttribute(); // defaults
                // List ?
                if (property.PropertyType.GetInterface(typeof(IEnumerable).FullName) != null)
                    profile.MaxOccurs = -1;
            }

            // First, populate what we can
            this.ShortDefinition = profile.ShortDescription;
            this.FormalDefinition = profile.FormalDefinition ?? profile.ShortDescription;
            this.MaxOccurs = profile.MaxOccurs == -1 ? "*" : profile.MaxOccurs.ToString();
            this.MinOccurs = profile.MinOccurs;
            this.MustSupport = profile.MustSupport;
            this.IsModifier = profile.IsModifier;

            if(profile.Binding != null || profile.RemoteBinding != null)
                this.Binding = new BindingDefinition(profile);

            // Next description attribute
            if (description != null)
            {
                this.ShortDefinition = this.ShortDefinition ?? description.Description;
                this.FormalDefinition = this.FormalDefinition ?? description.Description;
            }

            // Types?
            var traversals = property.GetCustomAttributes<XmlElementAttribute>();
            Type[] travType = null;
            if (traversals.Length == 1)
                travType = new Type[] { traversals[0].Type ?? property.PropertyType };
            else
            {
                List<Type> travTypes = new List<Type>();
                foreach (var trav in traversals)
                    travTypes.Add(trav.Type);
                travType = travTypes.ToArray();
            }

            foreach (var t in travType)
            {
                var tr = TypeRef.MakeTypeRef(t);
                if (tr != null)
                    this.Type.Add(tr);
            }
        }

        /// <summary>
        /// Short definition of the extension
        /// </summary>
        [XmlElement("short")]
        [Description("A concise definition that is shown in the concise XML format that summarized profiles")]
        [ElementProfile(MinOccurs = 1)]
        public FhirString ShortDefinition { get; set; }

        /// <summary>
        /// Formal definition of the extension
        /// </summary>
        [XmlElement("formal")]
        [Description("The definition must be consistent with the base definition but convey the meaning of the element")]
        [ElementProfile(MinOccurs = 1)]
        public FhirString FormalDefinition { get; set; }

        /// <summary>
        /// Comments
        /// </summary>
        [XmlElement("comments")]
        [Description("Comments about the particular element")]
        public FhirString Comments { get; set; }

        /// <summary>
        /// Minimum occurance
        /// </summary>
        [XmlElement("min")]
        [Description("The minimum number of times this element must appear in the instance")]
        [ElementProfile(MinOccurs = 1)]
        public FhirInt MinOccurs { get; set; }
        /// <summary>
        /// Maximum occurance
        /// </summary>
        [XmlElement("max")]
        [Description("The maximum number of times this element can appear in the instance")]
        [ElementProfile(MinOccurs = 1)]
        public FhirString MaxOccurs { get; set; }

        /// <summary>
        /// The type of the element
        /// </summary>
        [Description("The type of the element")]
        [XmlElement("type")]
        public List<TypeRef> Type { get; set; }

        /// <summary>
        /// True if the object must be supported
        /// </summary>
        [XmlElement("mustSupport")]
        [Description("If true, conformant resource authors must be capable of providing a value for the element and resource consumers must be capable of extracting and doing something useful with the data element. If false, the element may be ignored and not supported")]
        public FhirBoolean MustSupport { get; set; }

        /// <summary>
        /// True if the object must be understood
        /// </summary>
        [Description("If true, the element cannot be ignored by systems unless they recognize the element and a pre-determination has been made that it is not relevant to their particular system")]
        [XmlElement("isModifier")]
        public FhirBoolean IsModifier { get; set; }

        /// <summary>
        /// The external binding if applicable
        /// </summary>
        [Description("Identifies the set of codes that applies to this element if a data type supporting codes is used")]
        [XmlElement("binding")]
        public BindingDefinition Binding { get; set; }

        /// <summary>
        /// Write text utility
        /// </summary>
        internal override void WriteText(System.Xml.XmlWriter w)
        {

            // Style for minoccurs
            string style = String.Empty;
            if (this.MaxOccurs == "0")
                style = "text-decoration:line-through";

            base.WriteTableCell(w, (FhirString)String.Format("{0}..{1} ", this.MinOccurs, this.MaxOccurs), 1, 1, style);

            if (this.Type != null && this.Type.Count > 0)
            {
                w.WriteStartElement("td");
                foreach (var t in this.Type)
                {
                    t.WriteText(w);

                    if (t != this.Type.Last())
                        w.WriteString(" | ");
                }
                w.WriteEndElement(); // td
            }

            w.WriteStartElement("td");


            // Default content
            w.WriteStartElement("span");
            if (!String.IsNullOrEmpty(style))
                w.WriteAttributeString("style", style);
                        (this.FormalDefinition ?? this.ShortDefinition ?? new FhirString()).WriteText(w);
            if (this.Binding != null)
            {
                w.WriteStartElement("br");
                w.WriteEndElement();
                this.Binding.WriteText(w);
            }
            w.WriteEndElement(); // span

            // Additional comments
            if (this.Comments != null)
            {
                w.WriteStartElement("br");
                w.WriteEndElement(); w.WriteStartElement("span"); // comments
                w.WriteAttributeString("style", "color:red; font-style:italic");
                w.WriteStartElement("strong");
                w.WriteString("Additional Comments: ");
                w.WriteEndElement(); // strong
                w.WriteString(this.Comments);
                w.WriteEndElement(); // span
            }            
            w.WriteEndElement(); // td

            //base.WriteTableRows(w, "Definition", this.FormalDefinition);
            //base.WriteTableRows(w, "Multiplicity", (FhirString)String.Format("{0} .. {1}", this.MinOccurs, this.MaxOccurs));
            
            //String flags = ( this.MustUnderstand.Value.Value ? "MustUnderstand " : "" ) + (this.MustSupport.Value.Value ? "MustSupport " : "");
            //base.WriteTableRows(w, "Flags", (FhirString)flags);
            
            //if(this.Type != null)
            //    base.WriteTableRows(w, "Type", this.Type.ToArray());

            //if (this.Comments != null)
            //    base.WriteTableRows(w, "Comments", this.Comments);
            

        }
    }
}
