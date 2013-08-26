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
        public StoredMessageCollection FindMessages(string from, string to)
        {
            
            var imps = ApplicationContext.CurrentContext.GetService(typeof(IMessagePersistenceService)) as IMessagePersistenceService;
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";

            // Now sanity check
            DateTime dtFrom = DateTime.Parse(from),
                dtTo = DateTime.Parse(to);
            if (dtTo.Subtract(dtFrom).TotalMinutes > 60)
                throw new InvalidOperationException("Cannot query more than 60 minutes worth of messages");

            if (imps != null)
            {
                StoredMessageCollection retVal = new StoredMessageCollection();
                foreach (var itm in imps.GetMessageIds(dtFrom, dtTo))
                {
                    var mi = imps.GetMessageInfo(itm);
                    var smi = new StoredMessageInfo(mi);
                    smi.Response = new StoredMessageInfo(imps.GetMessageInfo(mi.Response));
                    retVal.Messages.Add(smi);
                }
                return retVal;
            }
            else
                throw new WebFaultException(System.Net.HttpStatusCode.InternalServerError);
        }

        #endregion
    }
}
