// <copyright file="PatientBenefit.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Key]
        public int PatientBenefitId { get; set; }

        public int PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; }

        public int BenefitId { get; set; }

        [ForeignKey(nameof(BenefitId))]
        public Benefit Benefit { get; set; }

        public DateTime BenefitDate { get; set; }

        public int Quantity { get; set; }
    }
}
