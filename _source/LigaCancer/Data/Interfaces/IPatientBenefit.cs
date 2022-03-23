// <copyright file="IPatientBenefit.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System;

namespace RVCC.Data.Interfaces
{
    public interface IPatientBenefit
    {
        public int PatientBenefitId { get; set; }

        public int PatientId { get; set; }

        public int BenefitId { get; set; }

        public DateTime BenefitDate { get; set; }

        public int Quantity { get; set; }
    }
}