

using MARC.HI.EHRS.SVC.Configuration.Data;
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
* Date: 5-12-2012
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace MARC.HI.EHRS.SVC.Configuration.UI
{
    /// <summary>
    /// Configuration application context
    /// </summary>
    public static class ConfigurationApplicationContext
    {

        // Configuration panels
        public static List<IConfigurableFeature> s_configurationPanels = new List<IConfigurableFeature>(10);
                
        // Configuration File
        public static string s_configFile = null;

        // Fired when the configuration has been applied
        public static event EventHandler ConfigurationApplied;

        /// <summary>
        /// Configuration has been applied
        /// </summary>
        internal static void OnConfigurationApplied()
        {
            if (ConfigurationApplied != null)
                ConfigurationApplied(null, EventArgs.Empty);
        }

        /// <summary>
        /// Scan and load plugin files for configuration
        /// </summary>
        public static void Initialize()
        {
            //ConfigurationApplicationContext.s_configurationPanels.Add(new ClientRegistryAboutPanel());

            // Load DB providers
            foreach (var file in Directory.GetFiles(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "*.dll"))
            {
                try
                {
                    Application.DoEvents();
                    Assembly asm = Assembly.LoadFrom(file);
                    // Scan assembly for database configurators
                    foreach (var typ in asm.GetTypes())
                    {
                        ConstructorInfo ci = typ.GetConstructor(Type.EmptyTypes);
                        if (ci != null)
                        {
                            if (typeof(IDatabaseProvider).IsAssignableFrom(typ))
                                DatabaseConfiguratorRegistrar.Configurators.Add(ci.Invoke(null) as IDatabaseProvider);
                            else if(typeof(IDataFeature).IsAssignableFrom(typ))
                                DatabaseConfiguratorRegistrar.Features.Add(ci.Invoke(null) as IDataFeature);
                            else if (typeof(IDataUpdate).IsAssignableFrom(typ))
                                DatabaseConfiguratorRegistrar.Updates.Add(ci.Invoke(null) as IDataUpdate);

                        }
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR: {0} : {1}", file, e.ToString());
                }
            }

            // Load Panels
            foreach (var file in Directory.GetFiles(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "*.dll"))
            {
                try
                {
                    Application.DoEvents();
                    Assembly asm = Assembly.LoadFrom(file);
                    // Scan assembly for configuration panels
                    foreach (var typ in Array.FindAll<Type>(asm.GetTypes(), o => o.GetInterface(typeof(IConfigurableFeature).FullName) != null))
                    {
                        ConstructorInfo ci = typ.GetConstructor(Type.EmptyTypes);
                        if (ci != null)
                        {
                            try
                            {
                                var config = ci.Invoke(null);
                                Console.WriteLine("Adding panel {0}...", config.ToString());
                                ConfigurationApplicationContext.s_configurationPanels.Add(config as IConfigurableFeature);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("ERROR: {0} : {1}", file, e.ToString());
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR: {0} : {1}", file, e.ToString());
                }
            }
        }
    }
}
