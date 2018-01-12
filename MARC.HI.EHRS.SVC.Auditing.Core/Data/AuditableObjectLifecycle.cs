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