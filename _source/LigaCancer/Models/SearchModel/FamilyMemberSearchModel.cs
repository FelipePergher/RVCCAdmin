// <copyright file="FamilyMemberSearchModel.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;

namespace RVCC.Models.SearchModel
{
    public class FamilyMemberSearchModel
    {
        public FamilyMemberSearchModel()
        {
        }

        public FamilyMemberSearchModel(string patientId)
        {
            PatientId = patientId;
        }

        [HiddenInput]
        public string PatientId { get; set; }
    }
}
