/*
 * Copyright 2010-2018 Mohawk College of Applied Arts and Technology
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
 * Date: 1-9-2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MARC.HI.EHRS.SVC.Core.Attributes
{
    /// <summary>
    /// Indicates a GUID identified dependency on another assembly
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class AssemblyPluginDependencyAttribute : Attribute
    {

        /// <summary>
        /// The assembly's guid for dependency resolution
        /// </summary>
        public Guid DependentAssemblyGuid { get; set; }

        /// <summary>
        /// The dependent version of the assembly
        /// </summary>
        public String DependentVersion { get; set; }

        /// <summary>
        /// Create a new dependency attribute
        /// </summary>
        public AssemblyPluginDependencyAttribute(String guid, String dependentVersion)
        {
            this.DependentAssemblyGuid = Guid.Parse(guid);
            this.DependentVersion = dependentVersion;
        }
    }
}
