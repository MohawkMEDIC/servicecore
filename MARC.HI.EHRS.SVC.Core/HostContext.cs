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
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using MARC.HI.EHRS.SVC.Core.Services;
using System.Runtime.InteropServices;

namespace MARC.HI.EHRS.SVC.Core
{
    /// <summary>
    /// Provides a context for components. 
    /// </summary>
    /// <remarks>Allows components to be communicate with each other via a loosely coupled
    /// broker system.</remarks>
    public class HostContext : IServiceProvider, IDisposable
    {

        /// <summary>
        /// Gets the identifier for this context
        /// </summary>
        public Guid ContextId { get; private set; }

        /// <summary>
        /// Configuration
        /// </summary>
        private HostConfigurationSectionHandler m_configuration;

        // True with the object has been disposed
        private bool m_disposed = false;

        /// <summary>
        /// Cached services
        /// </summary>
        private Dictionary<Type, object> m_cachedServices = new Dictionary<Type, object>();

        /// <summary>
        /// Creates a new instance of the host context
        /// </summary>
        public HostContext()
        {
            m_configuration = ConfigurationManager.GetSection("marc.hi.ehrs.svc.core") as HostConfigurationSectionHandler;
            ContextId = Guid.NewGuid();
            foreach (var svc in m_configuration.ServiceProviders)
                if (svc is IUsesHostContext)
                    (svc as IUsesHostContext).Context = this;
        }

        #region IServiceProvider Members

        /// <summary>
        /// Get a service from this host context
        /// </summary>
        public object GetService(Type serviceType)
        {
            ThrowIfDisposed();
            object candidateService = null;
            lock(m_cachedServices)
                if (!m_cachedServices.TryGetValue(serviceType, out candidateService))
                {
                    List<object> candidateServices = m_configuration.ServiceProviders.FindAll(o => o.GetType().GetInterface(serviceType.FullName) != null);
                    if (candidateServices.Count > 1)
                        Trace.TraceWarning("More than one service implementation for {0} found, using {1} as default", serviceType.FullName, candidateServices[0].GetType().FullName);

                    if (candidateServices.Count != 0) // found
                    {
                        candidateService = candidateServices[0]; // take the first one
                        lock(m_cachedServices)
                            m_cachedServices.Add(serviceType, candidateService);
                    }
                    else
                        #if DEBUG
                        Trace.TraceWarning("Could not locate service implementation for {0}", serviceType.FullName);
                        #else
                        ;
                        #endif
                }
            if (candidateService is IUsesHostContext)
                (candidateService as IUsesHostContext).Context = this;

            return candidateService;
        }

        /// <summary>
        /// Throw if disposed
        /// </summary>
        private void ThrowIfDisposed()
        {
            if (this.m_disposed)
                throw new ObjectDisposedException("HostContext");
        }

        #endregion


        #region IDisposable Members


        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            this.m_disposed = true;
            foreach (var kv in this.m_configuration.ServiceProviders)
                if (kv is IDisposable)
                    (kv as IDisposable).Dispose();
        }

        #endregion
    }
}
