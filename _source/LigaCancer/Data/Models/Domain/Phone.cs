// <copyright file="Phone.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Business;
using RVCC.Data.Interfaces;
using RVCC.Data.Models.Audit;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RVCC.Data.Models.Domain
{
    public class Phone : RegisterData, IPhone
    {
        public Phone()
        {
        }

        public Phone(string patientId, string number, Enums.PhoneType? phoneType, string observationNote)
        {
            Number = number;
            PhoneType = phoneType;
            ObservationNote = observationNote;
            PatientId = int.Parse(patientId);
        }

        [Key]
        public int PhoneId { get; set; }

        public string Number { get; set; }

        public Enums.PhoneType? PhoneType { get; set; }

        public string ObservationNote { get; set; }

        public int PatientId { get; set; }

        #region Relations

        [ForeignKey(nameof(PatientId))]
        public virtual Patient Patient { get; set; }

        #endregion
    }
}