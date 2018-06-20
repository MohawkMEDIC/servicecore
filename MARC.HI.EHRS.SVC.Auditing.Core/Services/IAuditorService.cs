using MARC.HI.EHRS.SVC.Auditing.Data;

namespace MARC.HI.EHRS.SVC.Auditing.Services
{
	/// <summary>
	/// This interface defines a framework for implementing a auditing service
	/// </summary>
	public interface IAuditorService
	{
		/// <summary>
		/// Send an audit
		/// </summary>
		bool SendAudit(AuditData ad);
	}
}