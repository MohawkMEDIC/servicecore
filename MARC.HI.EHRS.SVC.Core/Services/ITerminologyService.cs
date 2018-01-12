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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.Terminology;
using System.Workflow.Activities;
using MARC.HI.EHRS.SVC.Core.Data;

namespace MARC.HI.EHRS.SVC.Core.Services
{
   
    /// <summary>
    /// Identifies a service that can validate and/or correct terminology problems
    /// </summary>
    public interface ITerminologyService
    {

        /// <summary>
        /// Validate the specified code
        /// </summary>
        ConceptValidationResult Validate(CodeValue code);

        /// <summary>
        /// Validate a code value
        /// </summary>
        ConceptValidationResult ValidateEx(string code, string displayName, String codeSystem);

        /// <summary>
        /// Translate the specified code into a code within the specified
        /// target domain
        /// </summary>
        CodeValue Translate(CodeValue code, string targetDomain);

        /// <summary>
        /// Fill in code details
        /// </summary>
        CodeValue FillInDetails(CodeValue codeValue);
    }
}
