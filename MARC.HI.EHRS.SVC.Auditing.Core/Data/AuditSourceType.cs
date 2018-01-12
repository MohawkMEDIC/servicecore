using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Auditing.Data
{
	/// <summary>
	/// Audit source type
	/// </summary>
	[XmlType(nameof(AuditSourceType), Namespace = "http://marc-hi.ca/svc/audit")]
	public enum AuditSourceType
	{
		/// <summary>
		/// Represents the end user interface.
		/// </summary>
		[XmlEnum("ui")]
		EndUserInterface = 1,

		/// <summary>
		/// Represents a device or instrument.
		/// </summary>
		[XmlEnum("dev")]
		DeviceOrInstrument = 2,

		/// <summary>
		/// Represents a web server process.
		/// </summary>
		[XmlEnum("web")]
		WebServerProcess = 3,

		/// <summary>
		/// Represents an application server process.
		/// </summary>
		[XmlEnum("app")]
		ApplicationServerProcess = 4,

		/// <summary>
		/// Represents a database server process.
		/// </summary>
		[XmlEnum("db")]
		DatabaseServerProcess = 5,

		/// <summary>
		/// Represents a security server process.
		/// </summary>
		[XmlEnum("sec")]
		SecurityServerProcess = 6,

		/// <summary>
		/// Represents an ISO level 1 or level 3 component.
		/// </summary>
		[XmlEnum("isol1")]
		ISOLevel1or3Component = 7,

		/// <summary>
		/// Represents an ISO level 4 or 6 software.
		/// </summary>
		[XmlEnum("isol4")]
		ISOLevel4or6Software = 8,

		/// <summary>
		/// Represents an other audit source type.
		/// </summary>
		[XmlEnum("other")]
		Other = 9
	}
}