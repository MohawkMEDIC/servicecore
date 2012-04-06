using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MARC.HI.EHRS.SVC.Core.ComponentModel.Components;

namespace MARC.HI.EHRS.SVC.Core.PatientDataProvider
{
    /// <summary>
    /// Represents a provider that has the ability to query for patient records of a particular patient
    /// and return the results as a series of components
    /// </summary>
    public interface IPatientClinicalSummaryProvider
    {

        /// <summary>
        /// Queries for the specified client summary
        /// </summary>
        IContainer[] Query(Client target);

    }
}
