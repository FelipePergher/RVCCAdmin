// <copyright file="PatientPrintViewModel.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using RVCC.Models.FormModel;

namespace RVCC.Models.ViewModel
{
    public class PatientPrintViewModel
    {
        public string PatientId { get; set; }

        public string PatientInformationId { get; set; }

        public string NaturalityId { get; set; }

        public PatientProfileFormModel PatientProfile { get; set; }

        public PatientInformationFormModel PatientInformation { get; set; }

        public NaturalityFormModel Naturality { get; set; }
    }
}
