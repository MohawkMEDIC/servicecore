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
 * Date: 7-5-2012
 */

using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Auditing.Data
{
	/// <summary>
	/// Identifies roles of objects in the audit event
	/// </summary>
	[XmlType(nameof(AuditableObjectRole), Namespace = "http://marc-hi.ca/svc/audit")]
	public enum AuditableObjectRole
	{
		/// <remarks>Use with object type Person</remarks>
		[XmlEnum("pat")]
		Patient = 0x01,

		/// <remarks>Use with object type Organization</remarks>
		[XmlEnum("loc")]
		Location = 0x02,

		/// <remarks>Use with object type SysObject</remarks>
		[XmlEnum("rpt")]
		Report = 0x03,

		/// <remarks>Use with object type Person or Organization</remarks>
		[XmlEnum("res")]
		Resource = 0x04,

		/// <remarks>Use with object type SysObject</remarks>
		[XmlEnum("mf")]
		MasterFile = 0x05,

		/// <remarks>Use with object type Person, SysObject</remarks>
		[XmlEnum("usr")]
		User = 0x06,

		/// <remarks>Use with object type SysObject</remarks>
		[XmlEnum("lst")]
		List = 0x07,

		/// <remarks>Use with object type Person</remarks>
		[XmlEnum("doc")]
		Doctor = 0x08,

		/// <remarks>Use with object type Organization</remarks>
		[XmlEnum("sub")]
		Subscriber = 0x09,

		/// <remarks>Use with object type Person, Organization</remarks>
		[XmlEnum("guar")]
		Guarantor = 0x0a,

		/// <remarks>Use with object type SyOBject</remarks>
		[XmlEnum("usr")]
		SecurityUser = 0x0b,

		/// <remarks>Use with object type SysObject</remarks>
		[XmlEnum("rol")]
		SecurityGroup = 0x0c,

		/// <remarks>Use with object type SysObject</remarks>
		[XmlEnum("secres")]
		SecurityResource = 0x0d,

		/// <remarks>Use with object type SysObject</remarks>
		[XmlEnum("secdef")]
		SecurityGranularityDefinition = 0x0e,

		/// <remarks>Use with object type Person or Organization</remarks>
		[XmlEnum("pvdr")]
		Provider = 0x0f,

		/// <remarks>Use with object type SysObject</remarks>
		[XmlEnum("dest")]
		DataDestination = 0x10,

		/// <remarks>Use with object type SysObject</remarks>
		[XmlEnum("repo")]
		DataRepository = 0x11,

		/// <remarks>Use with object type SysObject</remarks>
		[XmlEnum("sched")]
		Schedule = 0x12,

		/// <remarks>Use with object type Person</remarks>
		[XmlEnum("cust")]
		Customer = 0x13,

		/// <remarks>Use with object type SysObject</remarks>
		[XmlEnum("job")]
		Job = 0x14,

		/// <remarks>Use with object type SysObject</remarks>
		[XmlEnum("jobstr")]
		JobStream = 0x15,

		/// <remarks>Use with object type SysObject</remarks>
		[XmlEnum("tbl")]
		Table = 0x16,

		/// <remarks>Use with object type SysObject</remarks>
		[XmlEnum("route")]
		RoutingCriteria = 0x17,

		/// <remarks>Use with object type SysObject</remarks>
		[XmlEnum("qry")]
		Query = 0x18
	}
}