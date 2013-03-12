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

namespace MARC.HI.EHRS.SVC.Subscription.Core
{

    /// <summary>
    /// Filter expression
    /// </summary>
    [XmlType("FilterExpression", Namespace = "urn:marc-hi:ehrs:subscription")]
    [XmlRoot("expression", Namespace = "urn:marc-hi:ehrs:subscription")]
    public class FilterExpression
    {

        /// <summary>
        /// Filter expression
        /// </summary>
        public FilterExpression()
        {
            this.Terms = new List<FilterExpressionBase>();
        }

        /// <summary>
        /// Filter expression terms
        /// </summary>
        [XmlElement("containsComponent", typeof(ComponentFilterExpression))]
        [XmlElement("hasProperty", typeof(PropertyFilterExpression))]
        public List<FilterExpressionBase> Terms { get; set; }

        /// <summary>
        /// Validate
        /// </summary>
        public bool Validate()
        {
            bool valid = true;
            foreach (var term in this.Terms)
                valid &= term.Validate();
            return valid;
        }
    }
}
