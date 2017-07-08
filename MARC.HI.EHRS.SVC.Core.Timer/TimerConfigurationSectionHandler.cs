/*
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
 * Date: 2-1-2013
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Reflection;

namespace MARC.HI.EHRS.SVC.Core.Timer.Configuration
{
    /// <summary>
    /// A configuration section handler for the timer job service
    /// </summary>
    public class TimerConfigurationSectionHandler : IConfigurationSectionHandler
    {
        
        #region IConfigurationSectionHandler Members

        /// <summary>
        /// Create the configuration object
        /// </summary>
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            
            // Create the return value and populate each job based on the contents of the config section
            TimerConfiguration retVal = new TimerConfiguration();

            var jobs = section.SelectNodes(".//*[local-name() = 'job']");
            if (jobs != null)
                foreach (XmlElement job in jobs)
                {
                    TimerJobConfiguration jobConfig = new TimerJobConfiguration();
                    TimeSpan tParsedTimespan = TimeSpan.MinValue;
                    // If either the timer parameter is missing or cannot be parsed into a timespan then bail
                    if (job.Attributes["timer"] == null || job.Attributes["timer"] != null && !TimeSpan.TryParse(job.Attributes["timer"].Value, out tParsedTimespan))
                        throw new ConfigurationErrorsException("Timer attribute on timer job is invalid or missing");
                    if (job.Attributes["type"] == null)
                        throw new ConfigurationErrorsException("Type attribute on timer job is missing");
                    Type jobType = Type.GetType(job.Attributes["type"].Value);
                    // could not find type?
                    if (jobType == null)
                        throw new ConfigurationErrorsException(String.Format("Could not find type described by '{0}'", job.Attributes["type"].Value));
                    ConstructorInfo jobCi = jobType.GetConstructor(Type.EmptyTypes);
                    if (jobCi == null)
                        throw new ConfigurationErrorsException(String.Format("Could not find default constructor for '{0}'", jobType));
                    // invoke
                    var jobInstance = jobCi.Invoke(null) as ITimerJob;
                    if (jobInstance == null)
                        throw new ConfigurationErrorsException(String.Format("Type '{0}' does not implement ITimerJob", jobType));
                    jobConfig.Job = jobInstance;
                    jobConfig.Timeout = tParsedTimespan;
                    retVal.Jobs.Add(jobConfig);
                }

            return retVal;

        }

        #endregion
    }
}
