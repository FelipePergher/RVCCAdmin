// <copyright file="PatientBenefit.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using System;

namespace RVCC.Data.Models.RelationModels
{
    public class PatientBenefit : RegisterData
    {
        public PatientBenefit()
        {
        }

        public PatientBenefit(Benefit benefit)
        {
            Benefit = benefit;
        }

        public int PatientId { get; set; }

        public Patient Patient { get; set; }

        public int BenefitId { get; set; }

        public Benefit Benefit { get; set; }

        public DateTime BenefitDate { get; set; }

        public int Quantity { get; set; }
    }
}
