/**
 * Copyright 2012-2013 Mohawk College of Applied Arts and Technology
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
using MARC.HI.EHRS.SVC.Core.Configuration;
using System.Reflection;

namespace MARC.HI.EHRS.SVC.Core
{
    /// <summary>
    /// Provides a context for components. 
    /// </summary>
    /// <remarks>Allows components to be communicate with each other via a loosely coupled
    /// broker system.</remarks>
    public class ApplicationContext : IServiceProvider, IDisposable
    {

        // Lock object
        private static Object s_lockObject = new object();
        // Context
        private static ApplicationContext s_context = null;

        /// <summary>
        /// Singleton accessor
        /// </summary>
        public static ApplicationContext Current
        {
            get
            {
                if(s_context == null)
                    lock(s_lockObject)
                        if(s_context == null)
                            s_context = new ApplicationContext();
                return s_context;
            }
        }

        /// <summary>
        /// Get the host configuration
        /// </summary>
        public HostConfiguration Configuration { get { return this.m_configuration; } }

        /// <summary>
        /// Gets the identifier for this context
        /// </summary>
        public Guid ContextId { get; private set; }

        /// <summary>
        /// Gets whether the domain is running
        /// </summary>
        public bool IsRunning { get { return this.m_running; } }

        // Message handler service
        private IMessageHandlerService m_messageHandler = null;

        /// <summary>
        /// Configuration
        /// </summary>
        private HostConfiguration m_configuration;

        // True with the object has been disposed
        private bool m_disposed = false;

        // Running?
        private bool m_running = false;

        /// <summary>
        /// Cached services
        /// </summary>
        private Dictionary<Type, object> m_cachedServices = new Dictionary<Type, object>();

        /// <summary>
        /// Creates a new instance of the host context
        /// </summary>
        private ApplicationContext()
        {
            ContextId = Guid.NewGuid();
            this.m_configuration = ConfigurationManager.GetSection("marc.hi.ehrs.svc.core") as HostConfiguration;
        }

        #region IServiceProvider Members

        /// <summary>
        /// Fired when the application context starting
        /// </summary>
        public event EventHandler Starting;
        /// <summary>
        /// Fired after application startup is complete
        /// </summary>
        public event EventHandler Started;
        /// <summary>
        /// Fired wehn the application context commences stop
        /// </summary>
        public event EventHandler Stopping;
        /// <summary>
        /// Fired after the appplication context is stopped
        /// </summary>
        public event EventHandler Stopped;

        /// <summary>
        /// Start the application context
        /// </summary>
        public bool Start()
        {
            if (!this.m_running)
            {

                if (this.Starting != null)
                    this.Starting(this, null);

                m_configuration = ConfigurationManager.GetSection("marc.hi.ehrs.svc.core") as HostConfiguration;

                // If there is no configuration manager then add the local
                if (this.GetService(typeof(IConfigurationManager)) == null)
                    this.m_configuration.ServiceProviders.Add(typeof(LocalConfigurationManager));

                Trace.TraceInformation("Starting all daemon services");
                foreach (var svc in this.m_configuration.ServiceProviders.Where(t=>t.GetInterface(typeof(IDaemonService).FullName) != null).ToList())
                {
                    Trace.TraceInformation("Starting daemon service {0}...", svc.Name);
                    IDaemonService instance = this.GetService(svc) as IDaemonService;
                    instance.Start();
                }


                if (this.Started != null)
                    this.Started(this, null);
                this.m_running = true;

            }

            return true;
        }

        /// <summary>
        /// Stop the application context
        /// </summary>
        public void Stop()
        {

            if (this.Stopping != null)
                this.Stopping(this, null);

            this.m_messageHandler?.Stop();

            this.m_running = false;
            foreach (var svc in this.m_configuration.ServiceProviders.OfType<IDaemonService>())
            {
                Trace.TraceInformation("Stopping daemon service {0}...", svc.GetType().Name);
                svc.Stop();
            }

            if (this.Stopped != null)
                this.Stopped(this, null);

        }

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
                    List<Type> candidateServices = m_configuration.ServiceProviders.FindAll(o => o == serviceType || serviceType.IsAssignableFrom(o));
                    if (candidateServices.Count > 1)
                        Trace.TraceWarning("More than one service implementation for {0} found, using {1} as default", serviceType.FullName, candidateServices[0].GetType().FullName);

                    if (candidateServices.Count != 0) // found
                    {
                        var candidateServiceType = candidateServices[0]; // take the first one
                        // The type of instantiation
                        ServiceInstantiationType type = ServiceInstantiationType.Singleton;
                        var serviceAttribute = candidateServiceType.GetCustomAttributes(typeof(ServiceAttribute), true);
                        if (serviceAttribute.Length > 0)
                            type = (serviceAttribute[0] as ServiceAttribute).Type;

                        // Service is a singleton
                        if (type == ServiceInstantiationType.Singleton)
                        {
                            lock (m_cachedServices)
                            {
                                candidateService = Activator.CreateInstance(candidateServiceType);
                                m_cachedServices.Add(serviceType, candidateService);
                            }
                        }
                        else
                        {
                            candidateService = Activator.CreateInstance(candidateServiceType);
                        }
                    }
                    else
                        #if DEBUG
                        Trace.TraceWarning("Could not locate service implementation for {0}", serviceType.FullName);
                        #else
                        ;
                        #endif
                }

            return candidateService;
        }

        /// <summary>
        /// Get strongly typed service
        /// </summary>
        public T GetService<T>() where T : class
        {
            return this.GetService(typeof(T)) as T;
        }

        /// <summary>
        /// Throw if disposed
        /// </summary>
        private void ThrowIfDisposed()
        {
            if (this.m_disposed)
                throw new ObjectDisposedException(nameof(ApplicationContext));
        }


        /// <summary>
        /// Add service provider type
        /// </summary>
        public void AddServiceProvider(Type serviceType)
        {
            this.m_configuration.ServiceProviders.Add(serviceType);
        }

        /// <summary>
        /// Remove service provider
        /// </summary>
        public void RemoveServiceProvider(Type serviceType)
        {
            this.m_configuration.ServiceProviders.Remove(serviceType);
            if (this.m_cachedServices.ContainsKey(serviceType))
                this.m_cachedServices.Remove(serviceType);

        }

        #endregion


        #region IDisposable Members


        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            foreach (var kv in this.m_configuration.ServiceProviders)
                if (kv is IDisposable)
                    (kv as IDisposable).Dispose();
            this.m_disposed = true;
        }

        #endregion

    }
}
