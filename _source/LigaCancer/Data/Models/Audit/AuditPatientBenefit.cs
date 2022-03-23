// <copyright file="AuditPatientBenefit.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Audit
{
    public class AuditPatientBenefit : IAudit, IPatientBenefit
    {
        [Key]
        public int AuditPatientBenefitId { get; set; }

        #region IPatientBenefit

        public int PatientBenefitId { get; set; }

        public int PatientId { get; set; }

        public int BenefitId { get; set; }

        public DateTime BenefitDate { get; set; }

        public int Quantity { get; set; }

        #endregion

        #region Extra Fields

        /// <summary>
        /// Saving benefit name in case the benefit name is updated in future
        /// </summary>
        public string BenefitName { get; set; }

        /// <summary>
        /// Saving Patient name in case the patient name is updated in future
        /// </summary>
        public string PatientName { get; set; }

        #endregion

        #region IAudit

        public DateTime AuditDate { get; set; }

        public string UserName { get; set; }

        public string AuditAction { get; set; }

        #endregion
    }
}