﻿using System;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Data.Models.PatientModels
{
    public class ActivePatient : RegisterData
    {
        [Key]
        public int ActivePatientId { get; set; }

        public bool Death { get; set; }

        public bool Discharge { get; set; }

        public DateTime DeathDate { get; set; }

        public DateTime DischargeDate { get; set; }
    }
}