// <copyright file="IPatientTreatmentType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System;

namespace RVCC.Data.Interfaces
{
    public interface IPatientTreatmentType
    {
        public int PatientTreatmentTypeId { get; set; }

        public int PatientId { get; set; }

        public int TreatmentTypeId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Note { get; set; }
    }
}