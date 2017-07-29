using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MARC.HI.EHRS.SVC.Core.Services
{

    /// <summary>
    /// Event information related to authorization events
    /// </summary>
    public class AuthorizationEventArgs : EventArgs
    {

    }

    /// <summary>
    /// Contract which describes a service which is capable of exposing authorization information services
    /// </summary>
    public interface IAuthorizationService : IMessageHandlerService
    {

        event EventHandler<AuthorizationEventArgs> Authorizing;

        event EventHandler<AuthorizationEventArgs> AuthorizeSuccess;

        event EventHandler<AuthorizationEventArgs> AuthorizeDeny;

        event EventHandler<ServiceStatusEventArgs> StatusChanged;

    }
}
