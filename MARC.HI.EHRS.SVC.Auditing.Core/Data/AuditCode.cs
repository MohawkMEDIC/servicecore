using Newtonsoft.Json;
using System;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Auditing.Data
{
	/// <summary>
	/// Represents an audit
	/// </summary>
	[XmlType(nameof(AuditCode), Namespace = "http://marc-hi.ca/svc/audit")]
	[JsonObject(nameof(AuditCode))]
	public class AuditCode
	{
		/// <summary>
		/// Serialization ctor
		/// </summary>
		public AuditCode()
		{
		}

		/// <summary>
		/// Construct a new code
		/// </summary>
		public AuditCode(string code, string codeSystem)
		{
			this.Code = code;
			this.CodeSystem = codeSystem;
		}

		/// <summary>
		/// Gets or sets the code of the code value
		/// </summary>
		[XmlElement("code"), JsonProperty("code")]
		public String Code { get; set; }

		/// <summary>
		/// Gets or sets the system in which the code value is drawn
		/// </summary>
		[XmlElement("system"), JsonProperty("system")]
		public String CodeSystem { get; set; }

		/// <summary>
		/// Gets or sets the human readable name of the code sytsem
		/// </summary>
		[XmlElement("systemName"), JsonProperty("systemName")]
		public string CodeSystemName { get; set; }

		/// <summary>
		/// Gets or sets the version of the code system
		/// </summary>
		[XmlElement("systemVersion"), JsonProperty("systemVersion")]
		public string CodeSystemVersion { get; set; }

		/// <summary>
		/// Gets or sets the display name
		/// </summary>
		[XmlElement("display"), JsonProperty("display")]
		public String DisplayName { get; set; }
	}
}