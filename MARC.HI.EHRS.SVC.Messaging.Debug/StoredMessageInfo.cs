using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Core.Services;

namespace MARC.HI.EHRS.SVC.Messaging.Debug
{
    /// <summary>
    /// Stored message collection
    /// </summary>
    [XmlType("MessageCollection", Namespace = "http://marc-hi.ca/svccore/message")]
    [XmlRoot("messages", Namespace = "http://marc-hi.ca/svccore/message")]
    public class StoredMessageCollection
    {

        /// <summary>
        /// Stored message collection
        /// </summary>
        public StoredMessageCollection()
        {
            this.Messages = new List<StoredMessageInfo>();
        }

        /// <summary>
        /// Messages
        /// </summary>
        [XmlElement("message")]
        public List<StoredMessageInfo> Messages { get; set; }

    }

    /// <summary>
    /// Stored message information
    /// </summary>
    [XmlType("MessageInfo", Namespace = "http://marc-hi.ca/svccore/message")]
    [XmlRoot("messageInfo", Namespace = "http://marc-hi.ca/svccore/message")]
    public class StoredMessageInfo
    {

        /// <summary>
        /// For serialization
        /// </summary>
        public StoredMessageInfo()
        {

        }

        /// <summary>
        /// Constructs a new stored message info copying data from the parameter
        /// </summary>
        public StoredMessageInfo(MessageInfo copy)
        {
            this.Id = copy.Id;
            this.Timestamp = copy.Timestamp;

            if(copy.Source != null)
                this.From = copy.Source.ToString();
            if(copy.Destination != null)
                this.To = copy.Destination.ToString();
        }

        /// <summary>
        /// The id of the message
        /// </summary>
        [XmlElement("id")]
        public String Id { get; set; }
        /// <summary>
        /// The destination address of the message
        /// </summary>
        [XmlElement("to")]
        public String To { get; set; }
        /// <summary>
        /// The source address of the message
        /// </summary>
        [XmlElement("from")]
        public String From { get; set; }
        /// <summary>
        /// The date the message was received
        /// </summary>
        [XmlElement("date")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// The identifier of the message this message responds to
        /// </summary>
        [XmlElement("response")]
        public StoredMessageInfo Response { get; set; }

    }
}
