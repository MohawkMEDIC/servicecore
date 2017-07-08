/*
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
 * Date: 17-10-2012
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MARC.HI.EHRS.SVC.Messaging.Everest.Configuration
{
    /// <summary>
    /// Interaction configuration
    /// </summary>
    public class InteractionConfiguration
    {

        /// <summary>
        /// Gets or sets the identifier for the interaction
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// True if the interaction results in disclosure
        /// </summary>
        /// <remarks>Alters the manner in which the interaction message is persisted</remarks>
        public bool Disclosure { get; set; }
        /// <summary>
        /// Response headers
        /// </summary>
        public XmlNodeList ResponseHeaders { get; set; }
    }
}
