﻿/**
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
using System.ComponentModel;

namespace MARC.HI.EHRS.SVC.Auditing.Atna.Format
{
    /// <summary>
    /// Audit source type
    /// </summary>
    public enum AuditSourceType
    {
        [XmlEnum("1")]
        [Category("RFC-3881")]
        EndUserInterface = 1,
        [XmlEnum("2")]
        [Category("RFC-3881")]
        DeviceOrInstrument = 2,
        [XmlEnum("3")]
        [Category("RFC-3881")]
        WebServerProcess = 3,
        [XmlEnum("4")]
        [Category("RFC-3881")]
        ApplicationServerProcess = 4,
        [XmlEnum("5")]
        [Category("RFC-3881")]
        DatabaseServerProcess = 5,
        [XmlEnum("6")]
        [Category("RFC-3881")]
        SecurityServerProcess = 6,
        [XmlEnum("7")]
        [Category("RFC-3881")]
        ISOLevel1or3Component = 7,
        [XmlEnum("8")]
        [Category("RFC-3881")]
        ISOLevel4or6Software = 8,
        [XmlEnum("9")]
        [Category("RFC-3881")]
        Other = 9
    }
}
