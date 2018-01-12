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

namespace MARC.HI.EHRS.SVC.Core.Data
{
    /// <summary>
    /// Represents a basic codified value
    /// </summary>
    public class CodeValue
    {
        /// <summary>
        /// Construct a new code
        /// </summary>
        public CodeValue(string code, string codeSystem)
        {
            this.Code = code;
            this.CodeSystem = codeSystem;
        }

        /// <summary>
        /// Gets or sets the code of the code value
        /// </summary>
        public String Code { get; set; }
        /// <summary>
        /// Gets or sets the system in which the code value is drawn
        /// </summary>
        public String CodeSystem { get; set; }

        /// <summary>
        /// Gets or sets the human readable name of the code sytsem
        /// </summary>
        public string CodeSystemName { get; set; }

        /// <summary>
        /// Gets or sets the version of the code system
        /// </summary>
        public string CodeSystemVersion { get; set; }

        /// <summary>
        /// Gets or sets the display name
        /// </summary>
        public String DisplayName { get; set; }

    }
}
