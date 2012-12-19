/**
 * Copyright 2012-2012 Mohawk College of Applied Arts and Technology
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you 
 * may not use this file except in compliance with the License. You may 
 * obtain a copy of the License at 
 * 
 * http://www.apache.org/licenses/LICENSE-2.0 
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations under 
 * the License.
 * 
 * User: fyfej
 * Date: 7-5-2012
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.Everest.Connectors;
using MARC.Everest.Interfaces;

namespace MARC.HI.EHRS.SVC.Messaging.Everest.Exception
{
    /// <summary>
    /// Represents an exception that indicates that a message is not interpretable by
    /// the system
    /// </summary>
    public class UninterpretableMessageException : ApplicationException, IGraphable
    {

        /// <summary>
        /// Gets or sets the details of the uninterpratbable message graph operation
        /// </summary>
        public IReceiveResult Details { get; set; }

        public UninterpretableMessageException () : base() { }
        public UninterpretableMessageException(string message) : base(message) { }
        public UninterpretableMessageException(string message, System.Exception innerException) : base(message, innerException) { }
        public UninterpretableMessageException(string message, IReceiveResult details)
            : base(message)
        {
            this.Details = details;
        }
        public UninterpretableMessageException(string message, IReceiveResult details, System.Exception innerException)
            : base(message, innerException)
        {
            this.Details = details;
        }

        /// <summary>
        /// Represents the exception as a string
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("MARC.HI.SHRS.SVC.Messaging.Everest.Exception.UninterpretableMessageException : ");
            sb.AppendFormat("{0}\r\n", Message);
            if (Details != null)
            {
                sb.AppendFormat("\t-------- Caused by -------------\r\n\r\nCode: {0}\r\n", Details.Code);
                foreach (IResultDetail dtl in Details.Details ?? new IResultDetail[0])
                    sb.AppendFormat("\t{0} : {1}\r\n\t{2}", dtl.Type, dtl.Message, dtl.Exception);
            }
            if(InnerException != null)
                sb.AppendFormat("\t------- Inner Exception ---------\r\n\t{0}", InnerException);
            return sb.ToString();
        }
    }
}
