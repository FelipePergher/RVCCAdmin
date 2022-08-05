// <copyright file="PatientInformationDoctor.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using RVCC.Data.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;

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

        public int DoctorId { get; set; }

        #region Relation

        [ForeignKey(nameof(PatientInformationId))]
        public PatientInformation PatientInformation { get; set; }

        [ForeignKey(nameof(PatientInformationId))]
        public Doctor Doctor { get; set; }

        #endregion
    }
}
