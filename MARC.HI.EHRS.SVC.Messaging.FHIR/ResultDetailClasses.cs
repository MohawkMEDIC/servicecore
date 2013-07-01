﻿/**
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
 * Date: 4-9-2012
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.Everest.Connectors;
using MARC.HI.EHRS.SVC.Core.Services;
using MARC.Everest.DataTypes;
using System.Net;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR
{
    /// <summary>
    /// Unrecognized sender
    /// </summary>
    public class UnrecognizedSenderResultDetail : ResultDetail
    {

        public UnrecognizedSenderResultDetail(EndPoint sender) :
            base(ResultDetailType.Error, String.Format("'{0}' was not a valid solicitor", sender.ToString()), null, null)
        { }

    }

    /// <summary>
    /// Detected issue event detail
    /// </summary>
    public class DetectedIssueResultDetail : ResultDetail
    {
        public DetectedIssueResultDetail(string message) :
            base(message) { }

        public DetectedIssueResultDetail(ResultDetailType type, string message, string location)
            : base(type, message, location) { }

        public DetectedIssueResultDetail(ResultDetailType type, string message, Exception exception)
            : base(type, message, exception) { }
    }

    /// <summary>
    /// Patient was not found result detail
    /// </summary>
    public class PatientNotFoundResultDetail : ResultDetail
    {
        public PatientNotFoundResultDetail(ILocalizationService locale) : base(ResultDetailType.Warning, locale.GetString("DTPE006"), "//urn:hl7-org:v3#controlActProcess/urn:hl7-org:v3#queryByParameter/urn:hl7-org:v3#parameterList/urn:hl7-org:v3#patientIdentifier/urn:hl7-org:v3#value", null) { }
    }

    /// <summary>
    /// Patient was not found result detail
    /// </summary>
    public class UnrecognizedPatientDomainResultDetail : ResultDetail
    {
        public UnrecognizedPatientDomainResultDetail(ILocalizationService locale, string domain) : base(ResultDetailType.Error, locale.GetString("DBCF00C"), String.Format("//urn:hl7-org:v3#controlActProcess/urn:hl7-org:v3#queryByParameter/urn:hl7-org:v3#parameterList/urn:hl7-org:v3#patientIdentifier/urn:hl7-org:v3#value[@root='{0}']", domain), null) { }
    }

    /// <summary>
    /// Patient was not found result detail
    /// </summary>
    public class UnrecognizedTargetDomainResultDetail : ResultDetail
    {
        public UnrecognizedTargetDomainResultDetail(ILocalizationService locale, string domain) : base(ResultDetailType.Error, locale.GetString("DBCF00C"), String.Format("//urn:hl7-org:v3#controlActProcess/urn:hl7-org:v3#queryByParameter/urn:hl7-org:v3#parameterList/urn:hl7-org:v3#patientIdentifier/urn:hl7-org:v3#value[@root='{0}']", domain), null) { }
    }
}
