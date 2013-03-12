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
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Core.ComponentModel;
using System.ComponentModel;

namespace MARC.HI.EHRS.SVC.Subscription.Core
{
    /// <summary>
    /// Component filter expression
    /// </summary>
    [XmlType("ComponentFilterExpression", Namespace = "urn:marc-hi:ehrs:subscription")]
    public class ComponentFilterExpression : FilterExpressionBase
    {

        /// <summary>
        /// The name of the component
        /// </summary>
        [XmlAttribute("type")]
        public string TypeName { get; set; }

        /// <summary>
        /// The role that the component must fill
        /// </summary>
        [XmlAttribute("withRole")]
        public HealthServiceRecordSiteRoleType Role { get; set; }

        /// <summary>
        /// Make a reportable copy of this
        /// </summary>
        public override FilterExpressionBase MakeReportable()
        {
            var retVal = this.MemberwiseClone() as FilterExpressionBase;
            if (this.Where != null)
            {
                retVal.Where = new FilterExpression();
                foreach (var term in this.Where.Terms)
                    if (term.ReportMatch)
                        retVal.Where.Terms.Add(term.MakeReportable());
            }
            return retVal;
        }

        /// <summary>
        /// Match the component filter to the component
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public override bool Match(object component)
        {
            if (!(component is IContainer))
                return false; // only containers can contain components

            foreach (IComponent comp in (component as IContainer).Components)
            {
                var hsrSite = comp.Site as HealthServiceRecordSite;
                if (comp.GetType().Name == TypeName && hsrSite != null && hsrSite.SiteRoleType == Role) // We has a match!
                {
                    var isMatch = base.Match(comp);
                    if(isMatch)
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determine if two components are the same as one another
        /// </summary>
        public override bool IsSameAs(FilterExpressionBase other)
        {
            return other is ComponentFilterExpression && (other as ComponentFilterExpression).TypeName == this.TypeName &&
                (other as ComponentFilterExpression).Role == this.Role;
        }

        /// <summary>
        /// Validate the filter expression
        /// </summary>
        public override bool Validate()
        {
            
            return !String.IsNullOrEmpty(TypeName) && (!ReportMatch || ReportMatch && Role != HealthServiceRecordSiteRoleType.SubjectOf && Role != HealthServiceRecordSiteRoleType.OutcomeOf && Role != HealthServiceRecordSiteRoleType.OlderVersionOf && Role != HealthServiceRecordSiteRoleType.AlternateTo);
        }
    }
}
