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

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{
    /// <summary>
    /// Element definition
    /// </summary>
    [XmlType("ElementDefinition", Namespace = "http://hl7.org/fhir")]
    public class ElementDefinition : Shareable
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
                this.MustUnderstand = false;
            }

            // Next description attribute
            if (description != null)
            {
                this.ShortDefinition = this.ShortDefinition ?? description.Description;
                this.FormalDefinition = this.FormalDefinition ?? description.Description;
            }

            this.Type.Add(new TypeRef() { Code = new PrimitiveCode<string>("Resource") });
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
            this.FormalDefinition = profile.FormalDefinition;
            this.MaxOccurs = profile.MaxOccurs == -1 ? "*" : profile.MaxOccurs.ToString();
            this.MinOccurs = profile.MinOccurs;
            this.MustSupport = profile.MustSupport;
            this.MustUnderstand = profile.MustUnderstand;
            this.Binding = profile.Binding != null ? profile.Binding.GetValueSetDefinition().ToString() : null;

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

            var tr = TypeRef.MakeTypeRef(travType);
            if (tr != null)
                this.Type.Add(tr);
        }

        /// <summary>
        /// Short definition of the extension
        /// </summary>
        [XmlElement("short")]
        [Description("A concise definition that is shown in the concise XML format that summarized profiles")]
        public FhirString ShortDefinition { get; set; }

        /// <summary>
        /// Formal definition of the extension
        /// </summary>
        [XmlElement("formal")]
        [Description("The definition must be consistent with the base definition but convey the meaning of the element")]
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
        public FhirInt MinOccurs { get; set; }
        /// <summary>
        /// Maximum occurance
        /// </summary>
        [XmlElement("max")]
        [Description("The maximum number of times this element can appear in the instance")]
        public FhirString MaxOccurs { get; set; }
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
        [XmlElement("mustUnderstand")]
        public FhirBoolean MustUnderstand { get; set; }
        /// <summary>
        /// The external binding if applicable
        /// </summary>
        [Description("Identifies the set of codes that applies to this element if a data type supporting codes is used")]
        [XmlElement("binding")]
        public FhirString Binding { get; set; }

        /// <summary>
        /// The type of the element
        /// </summary>
        [Description("The type of the element")]
        [XmlElement("type")]
        public List<TypeRef> Type { get; set; }

        /// <summary>
        /// Write text utility
        /// </summary>
        internal override void WriteText(System.Xml.XmlWriter w)
        {

            base.WriteTableCell(w, (FhirString)String.Format("{0}..{1} ", this.MinOccurs, this.MaxOccurs));
            
            if(this.Type != null && this.Type.Count > 0)
                base.WriteTableCell(w, this.Type[0]);

            w.WriteStartElement("td");

            (this.Comments ?? this.FormalDefinition ?? this.ShortDefinition ?? new FhirString()).WriteText(w);
            if (this.Binding != null)
            {
                w.WriteStartElement("em");
                w.WriteString("Note: This value is bound to ");
                w.WriteStartElement("a");
                w.WriteAttributeString("href", this.Binding);
                this.Binding.WriteText(w);
                w.WriteEndElement(); // a
                w.WriteEndElement(); // wm
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
