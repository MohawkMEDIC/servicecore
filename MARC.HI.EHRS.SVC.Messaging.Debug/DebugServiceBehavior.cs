using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.Services;
using System.ServiceModel.Web;

namespace MARC.HI.EHRS.SVC.Messaging.Debug
{
    /// <summary>
    /// Debug service behavior
    /// </summary>
    public class DebugServiceBehavior : IDebugServiceContract
    {
        #region IDebugServiceContract Members

        /// <summary>
        /// Get a message from the message persistence provider
        /// </summary>
        public System.IO.Stream GetMessage(string messageId)
        {
            var imps = ApplicationContext.CurrentContext.GetService(typeof(IMessagePersistenceService)) as IMessagePersistenceService;
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/vnd.marc.message";
            if (imps != null)
                return imps.GetMessage(messageId);
            else
                throw new WebFaultException(System.Net.HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// Get the response message from the message persistence provider
        /// </summary>
        public System.IO.Stream GetResponseMessage(string messageId)
        {
            var imps = ApplicationContext.CurrentContext.GetService(typeof(IMessagePersistenceService)) as IMessagePersistenceService;
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/vnd.marc.message";
            if (imps != null)
                return imps.GetMessageResponseMessage(messageId);
            else
                throw new WebFaultException(System.Net.HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// Get the current state of a message
        /// </summary>
        public Core.DataTypes.MessageState GetMessageState(string messageId)
        {
            var imps = ApplicationContext.CurrentContext.GetService(typeof(IMessagePersistenceService)) as IMessagePersistenceService;
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/vnd.marc.message";
            if (imps != null)
                return imps.GetMessageState(messageId);
            else
                throw new WebFaultException(System.Net.HttpStatusCode.InternalServerError);
        }


        /// <summary>
        /// Find all messages
        /// </summary>
        public List<string> FindMessages(string from, string to)
        {
            var imps = ApplicationContext.CurrentContext.GetService(typeof(IMessagePersistenceService)) as IMessagePersistenceService;
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
            if (imps != null)
            {
                List<String> retVal = new List<string>();
                foreach (var itm in imps.GetMessageIds(DateTime.Parse(from), DateTime.Parse(to)))
                    retVal.Add(String.Format("{0}/message/{1}", WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri, itm));
                return retVal;
            }
            else
                throw new WebFaultException(System.Net.HttpStatusCode.InternalServerError);
        }

        #endregion
    }
}
