// <copyright file="PatientAuxiliarAccessoryTypeSearchModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;

namespace RVCC.Models.SearchModel
{
    public class PatientAuxiliarAccessoryTypeSearchModel
    {
        public PatientAuxiliarAccessoryTypeSearchModel()
        {
        }

        public PatientAuxiliarAccessoryTypeSearchModel(string patientId)
        {
            PatientId = patientId;
        }

        [HiddenInput]
        public string PatientId { get; set; }
    }
}
