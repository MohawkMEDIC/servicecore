﻿/**
 * Copyright 2013-2013 Mohawk College of Applied Arts and Technology
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
 * Date: 25-2-2013
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core;
using MARC.HI.EHRS.SVC.Core.Services;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR
{
    /// <summary>
    /// Handlers for application context
    /// </summary>
    public static class ApplicationContext
    {

        /// <summary>
        /// Host context backing field for singleton
        /// </summary>
        private static IServiceProvider s_hostContext;
        /// <summary>
        /// Sync-lock
        /// </summary>
        private static Object s_syncLock = new object();
        /// <summary>
        /// System configuration service
        /// </summary>
        private static ISystemConfigurationService s_sysConfigService = null;
        /// <summary>
        /// Localization service
        /// </summary>
        private static ILocalizationService s_localeService = null;

        /// <summary>
        /// Gets the configuration service
        /// </summary>
        public static ISystemConfigurationService ConfigurationService
        {
            get { return s_sysConfigService; }
        }

        /// <summary>
        /// gets the localization service
        /// </summary>
        public static ILocalizationService LocalizationService
        {
            get { return s_localeService; }
        }

        /// <summary>
        /// Gets or sets the current host context
        /// </summary>
        public static IServiceProvider CurrentContext
        {
            get
            {
                return s_hostContext;
            }
            set
            {
                if (s_hostContext == null)
                    lock (s_syncLock)
                    {
                        s_hostContext = value;
                        s_sysConfigService = s_hostContext.GetService(typeof(ISystemConfigurationService)) as ISystemConfigurationService;
                        s_localeService = s_hostContext.GetService(typeof(ILocalizationService)) as ILocalizationService;
                    }
            }
        }
    }
}
