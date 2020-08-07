// <copyright file="PatientDetailsViewModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Models.FormModel;

namespace RVCC.Models.ViewModel
{
    public class PatientDetailsViewModel
    {
        public string PatientId { get; set; }

        public string PatientInformationId { get; set; }

        public string NaturalityId { get; set; }

        public PatientProfileFormModel PatientProfile { get; set; }

        public PatientInformationFormModel PatientInformation { get; set; }

        public NaturalityFormModel Naturality { get; set; }
    }
}
