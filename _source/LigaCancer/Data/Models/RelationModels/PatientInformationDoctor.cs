// <copyright file="PatientInformationDoctor.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using RVCC.Data.Models.Domain;

namespace RVCC.Data.Models.RelationModels
{
    public class PatientInformationDoctor : IPatientInformationDoctor
    {
        public PatientInformationDoctor()
        {
        }

        public PatientInformationDoctor(Doctor doctor)
        {
            Doctor = doctor;
        }

        public int PatientInformationId { get; set; }

        public PatientInformation PatientInformation { get; set; }

        public int DoctorId { get; set; }

        public Doctor Doctor { get; set; }
    }
}
