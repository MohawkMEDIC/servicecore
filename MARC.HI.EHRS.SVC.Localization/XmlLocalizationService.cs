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
using MARC.HI.EHRS.SVC.Core.Services;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace MARC.HI.EHRS.SVC.Localization
{
    /// <summary>
    /// Xml Localization service
    /// </summary>
    public class XmlLocalizationService : ILocalizationService
    {

        // Localization data
        [NonSerialized]
        private Dictionary<String, LocalizationData> m_localeData = new Dictionary<string, LocalizationData>();

        #region ILocalizationService Members

        /// <summary>
        /// Get a string from the default locale 
        /// </summary>
        public string GetString(string identifier)
        {
            ISystemConfigurationService sysConfigSvc = this.Context.GetService(typeof(ISystemConfigurationService)) as ISystemConfigurationService;
            return GetStringEx(identifier, sysConfigSvc.JurisdictionData.DefaultLanguageCode);
        }

        /// <summary>
        /// Get a string extended, get specified identifier from the locale file
        /// </summary>
        public string GetStringEx(string identifier, string locale)
        {
            LocalizationData ld = null;
            if (!this.m_localeData.TryGetValue(locale, out ld)) // Load locale data
            {
                Stream input = null;
                try
                {
                    string localeFile = Path.ChangeExtension(Assembly.GetEntryAssembly().Location, locale) + ".xml";
                    input = File.OpenRead(localeFile);
                    XmlSerializer xsz = new XmlSerializer(typeof(LocalizationData));
                    ld = xsz.Deserialize(input) as LocalizationData;
                    m_localeData.Add(locale, ld);
                }
                catch { }
                finally
                {
                    if (input != null)
                        input.Close();
                }
            }
            if (ld != null) // Get string
            {
                var sd = ld.Strings.Find(o => o.Name.Equals(identifier));
                if (sd != null)
                    return sd.Value;
            }
            return identifier;

        }

        #endregion

        #region IUsesHostContext Members

        /// <summary>
        /// Gets or sets the host context
        /// </summary>
        public IServiceProvider Context { get; set; }

        #endregion
    }
}
