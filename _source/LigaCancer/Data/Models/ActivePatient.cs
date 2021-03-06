// <copyright file="ActivePatient.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RVCC.Data.Models
{
    public class ActivePatient : RegisterData
    {
        [Key]
        public int ActivePatientId { get; set; }

        public bool Death { get; set; }

        public bool Discharge { get; set; }

        public bool ResidenceChange { get; set; }

        public DateTime DeathDate { get; set; }

        public DateTime DischargeDate { get; set; }

        public DateTime ResidenceChangeDate { get; set; }

        public int PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]
        public virtual Patient Patient { get; set; }
    }
}