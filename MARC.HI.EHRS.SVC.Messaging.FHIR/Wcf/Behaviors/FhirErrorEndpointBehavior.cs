﻿using MARC.HI.EHRS.SVC.Messaging.FHIR.Wcf.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Wcf.Behavior
{
    /// <summary>
    /// Service behavior
    /// </summary>
    public class FhirErrorEndpointBehavior : WebHttpBehavior
    {

        /// <summary>
        /// Error handlers
        /// </summary>
        protected override void AddServerErrorHandlers(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            base.AddServerErrorHandlers(endpoint, endpointDispatcher);

            //Remove all other error handlers
            endpointDispatcher.ChannelDispatcher.ErrorHandlers.Clear();
            //Add our own
            endpointDispatcher.ChannelDispatcher.ErrorHandlers.Add(new FhirErrorHandler());

        }
    }
}
