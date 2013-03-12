/**
 * Copyright 2012-2013 Mohawk College of Applied Arts and Technology
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
using System.ComponentModel;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Core.ComponentModel.Components;
using MARC.HI.EHRS.SVC.Core.DataTypes;

namespace MARC.HI.EHRS.SVC.Core.ComponentModel
{
    /// <summary>
    /// Represents a health service component that is also a component container
    /// </summary>
    [Serializable][XmlType("HealthServiceRecordContainer")]
    public abstract class HealthServiceRecordContainer : HealthServiceRecordComponent, IContainer
    {
        #region IContainer Members

        /// <summary>
        /// Identifier of the component
        /// </summary>
        [XmlElement("id")]
        public Decimal Id { get; set; }

        // Supported components
        private readonly Type[] m_supportedTypes = new Type[] {
            typeof(Annotation),
            typeof(Client),
            typeof(ChangeSummary),
            typeof(HealthcareParticipant),
            typeof(HealthServiceRecordComponentRef),
            typeof(MaskingIndicator),
            typeof(PersonalRelationship),
            typeof(QueryRestriction),
            typeof(ServiceDeliveryLocation)
        };

        /// <summary>
        /// Components
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("annotation", typeof(Annotation))]
        [XmlElement("client", typeof(Client))]
        [XmlElement("changeSummary", typeof(ChangeSummary))]
        [XmlElement("healthcareParticipant", typeof(HealthcareParticipant))]
        [XmlElement("healthServiceRecordComponentRef", typeof(HealthServiceRecordComponentRef))]
        [XmlElement("mask", typeof(MaskingIndicator))]
        [XmlElement("personalRelationship", typeof(PersonalRelationship))]
        [XmlElement("queryRestriction", typeof(QueryRestriction))]
        [XmlElement("serviceDeliveryLocation", typeof(ServiceDeliveryLocation))]
        public virtual List<HealthServiceRecordComponent> XmlComponents
        {
            get
            {
                var retVal = new List<HealthServiceRecordComponent>(m_components.Count);
                foreach (var mc in m_components)
                    if (mc is HealthServiceRecordComponent && 
                        Array.Exists(m_supportedTypes, o=>o.Equals(mc.GetType())))
                        retVal.Add(mc as HealthServiceRecordComponent);
                return retVal;
            }
            set
            {
                if(m_components == null)
                    m_components = new List<IComponent>(value.Count);
                foreach (var mv in value)
                {
                    (mv.Site as HealthServiceRecordSite).Container = this;
                    (mv.Site as HealthServiceRecordSite).Component = mv;
                    (mv.Site as HealthServiceRecordSite).Context = this.Context;
                    m_components.Add(mv);
                }
            }
        }

        /// <summary>
        /// Gets a list of components that are members of this container
        /// </summary>
        protected List<IComponent> m_components = new List<IComponent>();

        /// <summary>
        /// Add a component to this container
        /// </summary>
        public void Add(IComponent component, string name)
        {
            // See if the name already exists
            if (m_components.Exists(o => o.Site != null && o.Site.Name == name))
                throw new InvalidOperationException("Duplicate component name has already been added");

            // add the component site
            if (component != null)
            {
                component.Site = new HealthServiceRecordSite(component, this, Context);
                component.Site.Name = name;
                m_components.Add(component);
            }
        }

        /// <summary>
        /// Add a component to this container
        /// </summary>
        /// <param name="component">The component being added</param>
        /// <param name="name">The unique name for the component</param>
        /// <param name="siteType">The type of component being added</param>
        public void Add(IComponent component, string name, HealthServiceRecordSiteRoleType siteType, List<DomainIdentifier> originalIdentifiers)
        {
            Add(component, name, siteType, originalIdentifiers, true);
        }

        /// <summary>
        /// Add a range of components
        /// </summary>
        public void AddRange(IEnumerable<IComponent> components, HealthServiceRecordSiteRoleType siteType)
        {
            foreach (var c in components)
                this.Add(c, Guid.NewGuid().ToString(), siteType, null);
        }

        /// <summary>
        /// Add a component
        /// </summary>
        public void Add(IComponent component, string name, HealthServiceRecordSiteRoleType siteType, List<DomainIdentifier> originalIdentifiers, bool contextConduction)
        {
            Add(component, name);
            (component.Site as HealthServiceRecordSite).SiteRoleType = siteType;
            (component.Site as HealthServiceRecordSite).OriginalIdentifier = originalIdentifiers;
            (component.Site as HealthServiceRecordSite).ContextConduction = contextConduction;
        }

        /// <summary>
        /// Add a component to this container
        /// </summary>
        public virtual void Add(IComponent component)
        {
            Add(component, Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Gets a collection of all components within this container
        /// </summary>
        [XmlIgnore]
        public ComponentCollection Components
        {
            get
            {
                return new ComponentCollection(m_components.ToArray());
            }
        }

        /// <summary>
        /// Remove a component from this container
        /// </summary>
        public void Remove(IComponent component)
        {
            m_components.Remove(component);
        }

        /// <summary>
        /// Remove all child components with the specified role
        /// </summary>
        public void RemoveAllFromRole(HealthServiceRecordSiteRoleType role)
        {
            m_components.RemoveAll(o => o.Site is HealthServiceRecordSite &&
                (o.Site as HealthServiceRecordSite).SiteRoleType.Equals(role));

        }
        #endregion

        /// <summary>
        /// Find component in the container
        /// </summary>
        public IComponent FindComponent(HealthServiceRecordSiteRoleType healthServiceRecordSiteRoleType)
        {
            return m_components.Find(o => (o.Site is HealthServiceRecordSite) && (o.Site as HealthServiceRecordSite).SiteRoleType == healthServiceRecordSiteRoleType);
        }

        /// <summary>
        /// Find all components that match the given role type
        /// </summary>
        /// <param name="healthServiceRecordSiteRoleType"></param>
        /// <returns></returns>
        public List<IComponent> FindAllComponents(HealthServiceRecordSiteRoleType healthServiceRecordSiteRoleType)
        {
            return m_components.FindAll(o => (o.Site is HealthServiceRecordSite) && (o.Site as HealthServiceRecordSite).SiteRoleType.Equals(healthServiceRecordSiteRoleType));
        }

        /// <summary>
        /// Clone the object
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            var retVal = base.Clone();
            (retVal as HealthServiceRecordContainer).m_components = new List<IComponent>(this.m_components);
            (retVal as IComponent).Site = null;
            return retVal;
        }

        /// <summary>
        /// Sort components by role
        /// </summary>
        public void SortComponentsByRole()
        {
            this.m_components.Sort((a, b) => ((a as HealthServiceRecordComponent).Site as HealthServiceRecordSite).SiteRoleType.CompareTo(((b as HealthServiceRecordComponent).Site as HealthServiceRecordSite).SiteRoleType));
        }
    }
}
