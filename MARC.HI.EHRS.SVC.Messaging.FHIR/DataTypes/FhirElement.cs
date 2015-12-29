﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Reflection;
using System.Collections;
using System.Xml;
using System.Runtime.Serialization;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Resources;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes
{
    /// <summary>
    /// Represents a value that can be referenced using IDREF
    /// </summary>
    [XmlType("Shareable", Namespace = "http://hl7.org/fhir")]
    [Serializable]
    public class FhirElement 
    {

        // XHTML
        public const string NS_XHTML = "http://www.w3.org/1999/xhtml";

        [NonSerialized]
        private List<Extension> m_extensions;

        /// <summary>
        /// Represents a referencable class
        /// </summary>
        public FhirElement()
        {
            this.Extension = new List<Extension>();
        }


        /// <summary>
        /// Represents the ID of the object via XS:ID
        /// </summary> 
        [XmlAttribute("id")]
        [DataMember(Name = "id")]
        public string XmlId { get; set; }

        /// <summary>
        /// Identifier reference
        /// </summary>
        [XmlAttribute("idref")]
        [DataMember(Name = "idref")]
        public string IdRef { get; set; }

        /// <summary>
        /// Extension
        /// </summary>
        [XmlElement("extension")]
        [DataMember(Name = "extension")]
        public List<Extension> Extension { get { return this.m_extensions; } set { this.m_extensions = value; } }


        /// <summary>
        /// Make this a reference type
        /// </summary>
        public FhirElement MakeReference()
        {
            if(String.IsNullOrEmpty(this.XmlId))
                this.XmlId = String.Format("objid{0}", this.GetHashCode().ToString());
            
            return new FhirElement()
            {
                IdRef = String.Format("#{0}", this.XmlId)
            };
        }

        /// <summary>
        /// Make an IDref
        /// </summary>
        public IdRef MakeIdRef()
        {
            return new IdRef() { Value = this.MakeReference().IdRef };
        }

        /// <summary>
        /// Make an IDref
        /// </summary>
        public static FhirElement ResolveReference(IdRef idRef, FhirElement context)
        {
            return new FhirElement() { IdRef = idRef.Value }.ResolveReference(context);
        }

        /// <summary>
        /// Resolve the IDRef in this object to an identified object
        /// </summary>
        public FhirElement ResolveReference(FhirElement context)
        {
            
            // Check "this"
            if (context == null)
                return null;
            else if (String.Format("#{0}", context.XmlId) == this.IdRef)
                return context;
            else if (context.XmlId == this.IdRef)
                return context;

            // Check each property
            foreach (var pi in context.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                object value = pi.GetValue(context, null);
                if (value is FhirElement) // Referencable
                {
                    var refValue = value as FhirElement;
                    if (String.Format("#{0}", refValue.XmlId) == this.IdRef)
                        return refValue;
                    else
                    {
                        refValue = this.ResolveReference(refValue);
                        if (refValue != null) return refValue;
                    }
                }
                else if (value is IEnumerable)
                    foreach (var val in value as IEnumerable)
                    {
                        var refValue = this.ResolveReference(val as FhirElement);
                        if (refValue != null) return refValue;
                    }
            }
            return null;
        }

        /// <summary>
        /// Write text
        /// </summary>
        internal virtual void WriteText(System.Xml.XmlWriter w)
        {
            w.WriteString(this.ToString());
        }

        /// <summary>
        /// Write a table header
        /// </summary>
        protected void WriteTableHeader(XmlWriter w, FhirElement value)
        {
            this.WriteTableCellInternal(w, value, 1, 1, "th", null);
        }

        /// <summary>
        /// Write a table header
        /// </summary>
        protected void WriteTableCell(XmlWriter w, FhirElement value)
        {
            this.WriteTableCellInternal(w, value, 1, 1, "td", null);
        }

        /// <summary>
        /// Write a table header
        /// </summary>
        protected void WriteTableHeader(XmlWriter w, FhirElement value, int colspan, int rowspan)
        {
            this.WriteTableCellInternal(w, value, colspan, rowspan, "th", null);
        }

        /// <summary>
        /// Write a table header using the specified style
        /// </summary>
        protected void WriteTableHeader(XmlWriter w, FhirElement value, int colspan, int rowspan, string style)
        {
            this.WriteTableCellInternal(w, value, colspan, rowspan, "th", style);
        }

        /// <summary>
        /// Write a table row
        /// </summary>
        protected void WriteTableRows(XmlWriter w, FhirString key, params FhirElement[] values)
        {
            // Span calc
            if (values == null || values.Length == 0) return;
            int tSpan = values.Length;

            w.WriteStartElement("tr", NS_XHTML);
            this.WriteTableCell(w, key, 0, tSpan);
            w.WriteStartElement("td", NS_XHTML);
            if (values[0] == null)
                w.WriteString("N/A");
            else
                values[0].WriteText(w);
            w.WriteEndElement(); // tr
            w.WriteEndElement(); // tr
            for (int i = 1; i < values.Length; i++)
            {
                w.WriteStartElement("tr", NS_XHTML);
                w.WriteStartElement("td", NS_XHTML);
                values[i].WriteText(w);
                w.WriteEndElement();
                w.WriteEndElement();
            }
        }

        /// <summary>
        /// Write a table cell
        /// </summary>
        protected void WriteTableCell(XmlWriter w, FhirElement value, int colspan, int rowspan)
        {
            this.WriteTableCellInternal(w, value, colspan, rowspan, "td", null);
        }

        /// <summary>
        /// Write a table cell using the specified style
        /// </summary>
        protected void WriteTableCell(XmlWriter w, FhirElement value, int colspan, int rowspan, string style)
        {
            this.WriteTableCellInternal(w, value, colspan, rowspan, "td", style);
        }

        /// <summary>
        /// Write table cell utility
        /// </summary>
        private void WriteTableCellInternal(XmlWriter w, FhirElement value, int colspan, int rowspan, string tagName, string style)
        {
            w.WriteStartElement(tagName, NS_XHTML);

            // CSS
            if(!String.IsNullOrEmpty(style))
                w.WriteAttributeString("style", style);
            if (colspan > 1)
                w.WriteAttributeString("colspan", colspan.ToString());
            if (rowspan > 1)
                w.WriteAttributeString("rowspan", rowspan.ToString());

            if(value != null)
                value.WriteText(w);
            w.WriteEndElement(); // td
        }
    }
}
