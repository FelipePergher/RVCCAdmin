// <copyright file="PatientInformationTreatmentPlace.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using RVCC.Data.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace RVCC.Data.Models.RelationModels
{
    public class PatientInformationTreatmentPlace : IPatientInformationTreatmentPlace
    {
        public PatientInformationTreatmentPlace()
        {
        }

        public PatientInformationTreatmentPlace(TreatmentPlace treatmentPlace)
        {
            TreatmentPlace = treatmentPlace;
        }

        public int PatientInformationId { get; set; }

        public int TreatmentPlaceId { get; set; }

        #region Relation

        [ForeignKey(nameof(PatientInformationId))]
        public PatientInformation PatientInformation { get; set; }

        [ForeignKey(nameof(TreatmentPlaceId))]
        public TreatmentPlace TreatmentPlace { get; set; }

        #endregion
    }
}
