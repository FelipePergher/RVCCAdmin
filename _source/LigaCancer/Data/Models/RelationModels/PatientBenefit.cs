// <copyright file="PatientBenefit.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using RVCC.Data.Models.Audit;
using RVCC.Data.Models.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RVCC.Data.Models.RelationModels
{
    public class PatientBenefit : RegisterData, IPatientBenefit
    {
        public PatientBenefit()
        {
        }

        public PatientBenefit(Benefit benefit)
        {
            Benefit = benefit;
        }

        #region IPatientBenefit

        [Key]
        public int PatientBenefitId { get; set; }

        public int PatientId { get; set; }

        public int BenefitId { get; set; }

        public DateTime BenefitDate { get; set; }

        public int Quantity { get; set; }

        #endregion

        #region Relations

        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; }

        [ForeignKey(nameof(BenefitId))]
        public Benefit Benefit { get; set; }

        #endregion
    }
}
