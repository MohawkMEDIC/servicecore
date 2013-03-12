/**
 * Copyright 2012-2012 Mohawk College of Applied Arts and Technology
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
 * Date: 7-5-2012
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MARC.HI.EHRS.SVC.Core.Services;
using MARC.HI.EHRS.SVC.Core;
using MARC.HI.EHRS.SVC.Core.DataTypes;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Core.ComponentModel
{
    /// <summary>
    /// Identifies an ISite for health service records
    /// </summary>
    [Serializable][XmlType("HealthServiceRecordSite")]
    public class HealthServiceRecordSite : ISite, IUsesHostContext
    {

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public HealthServiceRecordSite() { }

        /// <summary>
        /// Identifies how the component relates to the container
        /// </summary>
        [XmlAttribute("roleType")]
        public HealthServiceRecordSiteRoleType SiteRoleType { get; set; }
        
        /// <summary>
        /// Identifies the original identifier that was used to establish the link
        /// </summary>
        [XmlElement("originalIdentifier")]
        public List<DomainIdentifier> OriginalIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the update mode
        /// </summary>
        [XmlAttribute("updateMode")]
        public UpdateMode UpdateMode { get; set; }

        /// <summary>
        /// True if context conduction should be used
        /// </summary>
        [XmlAttribute("contextConduction")]
        public bool ContextConduction { get; set; }


        /// <summary>
        /// If true, then this reference represents a symbolic link and should not be recorded
        /// in any tables that represent hard links. Examples of symbolic linkages are a link 
        /// from an event to a control act event whereby the linked record (the CACT) has no 
        /// bearing on the data of the parent.
        /// </summary>
        [XmlAttribute("symbolic")]
        public bool IsSymbolic { get; set; }

        /// <summary>
        /// Creates a new instance of Health service record site
        /// </summary>
        public HealthServiceRecordSite(IComponent component, IContainer container)
        {
            this.Component = component;
            this.Container = container;
        }

        /// <summary>
        /// Creates a new instance of Health Service Record site
        /// </summary>
        public HealthServiceRecordSite(IComponent component, IContainer container, IServiceProvider context)
            : this(component, container)
        {
            this.Context = context;
        }

        #region ISite Members

        /// <summary>
        /// Gets or sets the component that is contained in the site
        /// </summary>
        [XmlIgnore]
        public IComponent Component
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the container in which the component is contained
        /// </summary>
        [XmlIgnore]
        public IContainer Container
        {
            get;
            set; 
        }

        /// <summary>
        /// Design mode is not supported
        /// </summary>
        public bool DesignMode
        {
            get { return false; }
        }

        /// <summary>
        /// Gets or sets the name of the site
        /// </summary>
        [XmlAttribute("name")]
        public string Name
        {
            get;
            set;
        }

        #endregion

        #region IServiceProvider Members

        /// <summary>
        /// Get a service from the context
        /// </summary>
        public object GetService(Type serviceType)
        {
            if (Context != null)
                return Context.GetService(serviceType);
            return null;
        }

        #endregion

        #region ISharedHealthRecordService Members

        [NonSerialized]
        private IServiceProvider m_context = null;

        /// <summary>
        /// Gets or sets the context of the site
        /// </summary>
        [XmlIgnore]
        public IServiceProvider Context
        {
            get { return this.m_context; }
            set { this.m_context = value; }
        }

        #endregion
    }
}
