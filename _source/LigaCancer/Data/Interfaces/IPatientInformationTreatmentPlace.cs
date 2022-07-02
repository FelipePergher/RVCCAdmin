// <copyright file="IPatientInformationTreatmentPlace.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

namespace RVCC.Data.Interfaces
{
    public interface IPatientInformationTreatmentPlace
    {
        public int PatientInformationId { get; set; }

        public int TreatmentPlaceId { get; set; }
    }
}