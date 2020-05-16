﻿// <copyright file="Phone.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using RVCC.Business;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RVCC.Data.Models
{
    public class Phone : RegisterData
    {
        public Phone()
        {
        }

        public Phone(string patientId, string number, Globals.PhoneType? phoneType, string observationNote, ApplicationUser user)
        {
            Number = number;
            PhoneType = phoneType;
            ObservationNote = observationNote;
            PatientId = int.Parse(patientId);
            CreatedBy = user.Name;
        }

        [Key]
        public int PhoneId { get; set; }

        public string Number { get; set; }

        public Globals.PhoneType? PhoneType { get; set; }

        public string ObservationNote { get; set; }

        public int PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]
        public virtual Patient Patient { get; set; }
    }
}