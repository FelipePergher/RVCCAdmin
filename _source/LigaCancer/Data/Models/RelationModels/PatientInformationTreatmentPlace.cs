// <copyright file="PatientInformationTreatmentPlace.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

namespace RVCC.Data.Models.RelationModels
{
    public class PatientInformationTreatmentPlace
    {
        public PatientInformationTreatmentPlace()
        {
        }

        public PatientInformationTreatmentPlace(TreatmentPlace treatmentPlace)
        {
            TreatmentPlace = treatmentPlace;
        }

        public int PatientInformationId { get; set; }

        public PatientInformation PatientInformation { get; set; }

        public int TreatmentPlaceId { get; set; }

        public TreatmentPlace TreatmentPlace { get; set; }
    }
}
