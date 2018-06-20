/*
 * Copyright 2010-2018 Mohawk College of Applied Arts and Technology
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
 * Date: 1-9-2017
 */

using MARC.HI.EHRS.SVC.Core.Configuration.DbXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MARC.HI.EHRS.SVC.Core.Services
{
    /// <summary>
    /// Represents a service which is responsible for maintaining feature sets installed
    /// </summary>
    public interface IDbFeatureService
    {

        /// <summary>
        /// Determines whether a feature is installed
        /// </summary>
        bool IsFeatureInstalled(Feature feature);

        /// <summary>
        /// Get the installed features
        /// </summary>
        IEnumerable<Feature> GetInstalledFeature();

        /// <summary>
        /// Register that a feature has been installed
        /// </summary>
        void RegisterFeature(Feature feature);

        /// <summary>
        /// Deletes a feature from the feature store
        /// </summary>
        void DeleteFeature(Feature feature);

    }
}
