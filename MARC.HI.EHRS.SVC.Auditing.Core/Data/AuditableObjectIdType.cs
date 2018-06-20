using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Auditing.Data
{
	/// <summary>
	/// Classifies the type of identifier that a auditable object may have
	/// </summary>
	[XmlType(nameof(AuditableObjectIdType), Namespace = "http://marc-hi.ca/svc/audit")]
	public enum AuditableObjectIdType
	{
		/// <remarks>Use with object type code Person</remarks>
		[XmlEnum("mrn")]
		MedicalRecord = 0x01,

		/// <remarks>Use with object type code Person</remarks>
		[XmlEnum("pid")]
		PatientNumber = 0x02,

		/// <remarks>Use with object type code Person</remarks>
		[XmlEnum("ern")]
		EncounterNumber = 0x03,

		/// <remarks>Use with object type code Person</remarks>
		[XmlEnum("enrl")]
		EnrolleeNumber = 0x04,

		/// <remarks>Use with object type code Person</remarks>
		[XmlEnum("ssn")]
		SocialSecurityNumber = 0x05,

		/// <remarks>Use with object type code Person</remarks>
		[XmlEnum("acct")]
		AccountNumber = 0x06,

		/// <remarks>Use with object type code Person or Organization</remarks>
		[XmlEnum("guar")]
		GuarantorNumber = 0x07,

		/// <remarks>Use with object type code SystemObject</remarks>
		[XmlEnum("rpt")]
		ReportName = 0x08,

		/// <remarks>Use with object type code SystemObject</remarks>
		[XmlEnum("rpn")]
		ReportNumber = 0x09,

		/// <remarks>Use with object type code SystemObject</remarks>
		[XmlEnum("srch")]
		SearchCritereon = 0x0a,

		/// <remarks>Use with object type code Person or SystemObject</remarks>
		[XmlEnum("uid")]
		UserIdentifier = 0x0b,

		/// <remarks>Use with object type code SystemObject</remarks>
		[XmlEnum("uri")]
		Uri = 0x0c,

		/// <summary>
		/// Custom code
		/// </summary>
		[XmlEnum("ext")]
		Custom = 0x0d
	}
}