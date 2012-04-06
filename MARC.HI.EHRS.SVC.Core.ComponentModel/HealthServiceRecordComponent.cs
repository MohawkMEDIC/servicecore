using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MARC.HI.EHRS.SVC.Core.Services;
using MARC.HI.EHRS.SVC.Core;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Core.ComponentModel
{
    /// <summary>
    /// Identifies 
    /// </summary>
    [Serializable][XmlType("HealthServiceRecordComponent")]
    public abstract class HealthServiceRecordComponent : IComponent, IUsesHostContext, ICloneable
    {

        /// <summary>
        /// Create a new instance of the health service record component
        /// </summary>
        public HealthServiceRecordComponent()
        {
            Timestamp = DateTime.Now;
        }

        /// <summary>
        /// True if the data in this HSRC was masked
        /// </summary>
        [XmlAttribute("isMasked")]
        public bool IsMasked { get; set; }

        /// <summary>
        /// The time that the item as authored 
        /// </summary>
        [XmlAttribute("timestamp")]
        public DateTime Timestamp { get; set; }

        #region IComponent Members

        /// <summary>
        /// True if the HSR component has been disposed
        /// </summary>
        bool m_disposed = false;

        /// <summary>
        /// Fired when this object is disposed
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// Gets or sets the site of this HSR component
        /// </summary>
        [XmlIgnore]
        public ISite Site
        {
            get;
            set;
        }

        /// <summary>
        /// Facilitates XML Serialization
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("hsrSite")]
        public HealthServiceRecordSite XmlSite
        {
            get
            {
                return Site as HealthServiceRecordSite;
            }
            set
            {
                this.Site = value;
            }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Dispose of this item
        /// </summary>
        public void Dispose()
        {
            if (!m_disposed)
            {
                // todo: dispose children here
                if (Disposed != null)
                    Disposed(this, EventArgs.Empty);
                return;
            }
            m_disposed = true;
        }

        #endregion

        #region IUsesHostContext Members

        [NonSerialized]
        private HostContext m_context;

        /// <summary>
        /// Get or set the context of this component
        /// </summary>
        [XmlIgnore]
        public HostContext Context
        {
            get { return this.m_context; }
            set { this.m_context = value; }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone the object
        /// </summary>
        public virtual object Clone()
        {
            var clone = this.MemberwiseClone();
            (clone as IComponent).Site = null; // new site
            return clone;
        }

        #endregion
    }
}
