// <copyright file="IPatientInformation.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System;

namespace RVCC.Data.Interfaces
{
    public interface IPatientInformation
    {
        public int PatientInformationId { get; set; }

        public DateTime TreatmentBeginDate { get; set; }

        public int PatientId { get; set; }
    }
}