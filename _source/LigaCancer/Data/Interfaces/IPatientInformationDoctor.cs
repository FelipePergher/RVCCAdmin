// <copyright file="IPatientInformationDoctor.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

namespace RVCC.Data.Interfaces
{
    public interface IPatientInformationDoctor
    {
        public int PatientInformationId { get; set; }

        public int DoctorId { get; set; }
    }
}