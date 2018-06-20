using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Auditing.Data
{
	/// <summary>
	/// Represents types of actions
	/// </summary>
	[XmlType(nameof(ActionType), Namespace = "http://marc-hi.ca/svc/audit")]
	public enum ActionType
	{
		/// <summary>
		/// Data was created in the system
		/// </summary>
		[XmlEnum("c")]
		Create,

		/// <summary>
		/// Data was viewed, printed, displayed, etc...
		/// </summary>
		[XmlEnum("r")]
		Read,

		/// <summary>
		/// Data was revised in the system
		/// </summary>
		[XmlEnum("u")]
		Update,

		/// <summary>
		/// Data was removed from the system
		/// </summary>
		[XmlEnum("d")]
		Delete,

		/// <summary>
		/// A system, or application function was performed
		/// </summary>
		[XmlEnum("x")]
		Execute
	}
}