// <copyright file="PatientTreatmentTypeSearchModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;

namespace RVCC.Models.SearchModel
{
    public class PatientTreatmentTypeSearchModel
    {
        public PatientTreatmentTypeSearchModel()
        {
        }

        public PatientTreatmentTypeSearchModel(string patientId)
        {
            PatientId = patientId;
        }

        [HiddenInput]
        public string PatientId { get; set; }
    }
}
