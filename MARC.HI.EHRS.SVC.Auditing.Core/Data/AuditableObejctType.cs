﻿/*
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

using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Auditing.Data
{
	/// <summary>
	/// Identifies the type of auditable objects in the system
	/// </summary>
	[XmlType(nameof(AuditableObjectType), Namespace = "http://marc-hi.ca/svc/audit")]
	public enum AuditableObjectType
	{
		[XmlEnum("p")]
		Person = 1,

		[XmlEnum("s")]
		SystemObject = 2,

		[XmlEnum("o")]
		Organization = 3,

		[XmlEnum("x")]
		Other = 4
	}
}