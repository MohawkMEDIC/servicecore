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

namespace MARC.HI.EHRS.SVC.Messaging.Everest
{
    /// <summary>
    /// Error tasks
    /// </summary>
    internal static class ErrorDescriptions
    {
        public const string ERR_MISSING_TYPE_ATTRIBUTE = "The 'type' attribute must be supplied and must point to a valid type in memory";
        public const string ERR_MISSING_ASM_ATTRIBUTE = "The 'assembly' attribute must be supplied";
        public const string ERR_CANT_FIND_TYPE = "Can't find type '{0}'";
        public const string ERR_MISSING_NS_ATTRIBUTE = "The 'namespace' attribute must be supplied";
        public const string ERR_MISSING_FMTR_ATTRIBUTE = "The 'formatter' attribute must be supplied and must point to a valid type in memory";
        public const string ERR_CANT_FIND_CTOR = "No appropriate constructor could be found for type '{0}'";
        public const string ERR_INVALID_INTERFACE = "Type '{0}' does not implement interface '{1}'";
        

    }
}
