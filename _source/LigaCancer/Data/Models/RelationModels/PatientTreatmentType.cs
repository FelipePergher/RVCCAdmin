// <copyright file="PatientTreatmentType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using RVCC.Data.Models.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RVCC.Data.Models.RelationModels
{
    public class PatientTreatmentType : IPatientTreatmentType
    {
        public PatientTreatmentType()
        {
        }

        public PatientTreatmentType(TreatmentType treatmentType, Patient patient, DateTime? startDate, DateTime? endDate, string note)
        {
            Patient = patient;
            TreatmentType = treatmentType;
            StartDate = startDate;
            EndDate = endDate;
            Note = note;
        }

        [Key]
        public int PatientTreatmentTypeId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Note { get; set; }

        public int PatientId { get; set; }

        public int TreatmentTypeId { get; set; }

        #region Relation

        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; }

        [ForeignKey(nameof(TreatmentTypeId))]
        public TreatmentType TreatmentType { get; set; }

        #endregion
    }
}
