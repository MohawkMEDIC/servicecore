using Newtonsoft.Json;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Auditing.Data
{
    /// <summary>
    /// Represents metadata keys
    /// </summary>
    [XmlType(nameof(AuditMetadataKey), Namespace = "http://marc-hi.ca/svc/audit")]
    public enum AuditMetadataKey
    {
        [XmlEnum("pid")]
        PID,
        [XmlEnum("process")]
        ProcessName,
        [XmlEnum("remoteHost")]
        RemoteHost,
        [XmlEnum("remoteEp")]
        RemoteEndpoint,
        [XmlEnum("localEp")]
        LocalEndpoint,
        [XmlEnum("submission_time")]
        SubmissionTime,
        [XmlEnum("format")]
        OriginalFormat,
        [XmlEnum("status")]
        SubmissionStatus,
        [XmlEnum("priority")]
        Priority,
        [XmlEnum("classification")]
        Classification,
        [XmlEnum("sessionId")]
        SessionId,
        [XmlEnum("enterpriseSiteId")]
        EnterpriseSiteID,
        [XmlEnum("auditSourceId")]
        AuditSourceID,
        [XmlEnum("auditSourceType")]
        AuditSourceType
    }

    /// <summary>
    /// Represents audit metadata such as submission time, submission sequence, etc.
    /// </summary>
    [XmlType(nameof(AuditMetadata), Namespace = "http://marc-hi.ca/svc/audit")]
    [JsonObject(nameof(AuditMetadata))]
    public class AuditMetadata 
    {

        /// <summary>
        /// Default ctor
        /// </summary>
        public AuditMetadata()
        {

        }

        /// <summary>
        /// Create new audit metadata with specified key and value
        /// </summary>
        public AuditMetadata(AuditMetadataKey key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the metadata key
        /// </summary>
        [XmlAttribute("key"), JsonProperty("key")]
        public AuditMetadataKey Key { get; set; }

        /// <summary>
        /// Gets or sets the process name
        /// </summary>
        [XmlAttribute("value"), JsonProperty("value")]
        public string Value { get; set; }

    }
}