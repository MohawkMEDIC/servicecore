using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Auditing.Data
{
	/// <summary>
	/// Represents potential outcomes.
	/// </summary>
	[XmlType(nameof(OutcomeIndicator), Namespace = "http://marc-hi.ca/svc/audit")]
	public enum OutcomeIndicator
	{
		/// <summary>
		/// Successful operation.
		/// </summary>
		[XmlEnum("ok")]
		Success = 0x00,

		/// <summary>
		/// Minor failure, action should be restarted.
		/// </summary>
		[XmlEnum("fail.minor")]
		MinorFail = 0x04,

		/// <summary>
		/// Action was terminated.
		/// </summary>
		[XmlEnum("fail.major")]
		SeriousFail = 0x08,

		/// <summary>
		/// Major failure, action is made unavailable.
		/// </summary>
		[XmlEnum("fail.epic")]
		EpicFail = 0x0C
	}
}