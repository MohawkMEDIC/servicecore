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
	/// Auditable object lifecycle
	/// </summary>
	[XmlType(nameof(AuditableObjectLifecycle), Namespace = "http://marc-hi.ca/svc/audit")]
	public enum AuditableObjectLifecycle
	{
		[XmlEnum("create")]
		Creation = 0x01,

		[XmlEnum("import")]
		Import = 0x02,

		[XmlEnum("amend")]
		Amendment = 0x03,

		[XmlEnum("verif")]
		Verification = 0x04,

		[XmlEnum("xfrm")]
		Translation = 0x05,

		[XmlEnum("access")]
		Access = 0x06,

		[XmlEnum("deid")]
		Deidentification = 0x07,

		[XmlEnum("agg")]
		Aggregation = 0x08,

		[XmlEnum("rpt")]
		Report = 0x09,

		[XmlEnum("export")]
		Export = 0x0a,

		[XmlEnum("disclose")]
		Disclosure = 0x0b,

		[XmlEnum("rcpdisclose")]
		ReceiptOfDisclosure = 0x0c,

		[XmlEnum("arch")]
		Archiving = 0x0d,

		[XmlEnum("obsolete")]
		LogicalDeletion = 0x0e,

		[XmlEnum("delete")]
		PermanentErasure = 0x0f
	}
}