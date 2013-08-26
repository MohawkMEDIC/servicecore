using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;
using MARC.HI.EHRS.SVC.Core.DataTypes;
using MARC.HI.EHRS.SVC.Core.Services;

namespace MARC.HI.EHRS.SVC.Messaging.Debug
{
    /// <summary>
    /// Debug service contract
    /// </summary>
    [ServiceContract]
    [XmlSerializerFormat]
    public interface IDebugServiceContract
    {

        /// <summary>
        /// Get a particular stored message 
        /// </summary>
        [WebGet(UriTemplate = "/message/{messageId}")]
        Stream GetMessage(String messageId);

        /// <summary>
        /// Get the response message
        /// </summary>
        [WebGet(UriTemplate = "/message/{messageId}/response")]
        Stream GetResponseMessage(String messageId);

        /// <summary>
        /// Get the response message
        /// </summary>
        [WebGet(UriTemplate = "/message/{messageId}/state")]
        MessageState GetMessageState(String messageId);

        /// <summary>
        /// Find messages
        /// </summary>
        [WebGet(UriTemplate = "/message?from={from}&to={to}")]
        StoredMessageCollection FindMessages(String from, String to);
    }
}
