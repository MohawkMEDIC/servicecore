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
using MARC.Everest.Connectors;
using System.ComponentModel;
using System.Collections;

namespace MARC.HI.EHRS.SVC.Subscription.Core
{
    /// <summary>
    /// Property based filter expression
    /// </summary>
    [XmlType("PropertyFilterExpression", Namespace = "urn:marc-hi:ehrs:subscription")]
    public class PropertyFilterExpression : FilterExpressionBase
    {

        /// <summary>
        /// The name of the property
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// The equality of the property
        /// </summary>
        [XmlAttribute("value")]
        public string Value { get; set; }

        /// <summary>
        /// Operator
        /// </summary>
        [XmlAttribute("operator")]
        public OperatorType Operator { get; set; }

        /// <summary>
        /// Match this filter expression to a component
        /// </summary>
        public override bool Match(Object component)
        {
            var pi = component.GetType().GetProperty(this.Name, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            if (pi == null)
                return false; // no such property
            var value = pi.GetValue(component, null);

            // Match any value in a collection
            ICollection filterValues = null;
            if (value is ICollection)
                filterValues = value as ICollection;
            else
                filterValues = new List<Object>() { value };
            bool isMatch = false;
            foreach (var val in filterValues)
            {

                // Value compare?
                if (!String.IsNullOrEmpty(this.Value))
                {
                    object compareToValue = Util.FromWireFormat(this.Value, pi.PropertyType);
                    int compare = ((IComparable)val).CompareTo(compareToValue);
                    switch (this.Operator)
                    {
                        case OperatorType.EqualTo:
                            isMatch |= compare == 0;
                            break;
                        case OperatorType.GreaterThan:
                            isMatch |= compare > 0;
                            break;
                        case OperatorType.GreaterThanEqualTo:
                            isMatch |= compare >= 0;
                            break;
                        case OperatorType.LessThan:
                            isMatch |= compare < 0;
                            break;
                        case OperatorType.LessThanEqualTo:
                            isMatch |= compare <= 0;
                            break;
                        case OperatorType.NotEqualTo:
                            isMatch |= compare != 0;
                            break;
                        default:
                            throw new InvalidOperationException("Cannot determine hasProperty outcome as the operator is invalid");
                    }
                }
                else
                    isMatch |= base.Match(val);
            }
            return isMatch;

        }

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
                {
                    if (term.ReportMatch)
                        retVal.Where.Terms.Add(term.MakeReportable());
                }
            }
            return retVal;

        }
        /// <summary>
        /// Determine if this is the same as the other
        /// </summary>
        public override bool IsSameAs(FilterExpressionBase other)
        {
            return other is PropertyFilterExpression && (other as PropertyFilterExpression).Name == this.Name &&
                (other as PropertyFilterExpression).Operator == this.Operator;
        }

        /// <summary>
        /// Validate
        /// </summary>
        /// <returns></returns>
        public override bool Validate()
        {
            return !String.IsNullOrEmpty(Name) && ((!String.IsNullOrEmpty(Value)) ^ (this.Where != null && this.Where.Validate() && !this.Where.Terms.Exists(o=>!(o is PropertyFilterExpression))));
        }

    }
}
