// <copyright file="PatientAuxiliarAccessoryType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using RVCC.Data.Models.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static RVCC.Business.Enums;

namespace RVCC.Data.Models.RelationModels
{
    public class PatientAuxiliarAccessoryType : IPatientAuxiliarAccessoryType
    {
        public PatientAuxiliarAccessoryType()
        {
        }

        public PatientAuxiliarAccessoryType(AuxiliarAccessoryType auxiliarAccessoryType, Patient patient, AuxiliarAccessoryTypeTime auxiliarAccessoryTypeTime, string note, DateTime duoDate)
        {
            Patient = patient;
            AuxiliarAccessoryType = auxiliarAccessoryType;
            AuxiliarAccessoryTypeTime = auxiliarAccessoryTypeTime;
            Note = note;
            DuoDate = duoDate;
        }

        [Key]
        public int PatientAuxiliarAccessoryTypeId { get; set; }

        public AuxiliarAccessoryTypeTime AuxiliarAccessoryTypeTime { get; set; }

        public DateTime DuoDate { get; set; }

        public string Note { get; set; }

        public int PatientId { get; set; }

        public int AuxiliarAccessoryTypeId { get; set; }

        #region Relation

        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; }

        [ForeignKey(nameof(AuxiliarAccessoryTypeId))]
        public AuxiliarAccessoryType AuxiliarAccessoryType { get; set; }

        #endregion
    }
}
