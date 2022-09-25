// <copyright file="AuditPatientTreatmentType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Audit
{
    public class AuditPatientTreatmentType : IAudit, IPatientTreatmentType
    {
        [Key]
        public int AuditTreatmentTypeId { get; set; }

        #region IPatientTreatmentType

        public int PatientTreatmentTypeId { get; set; }

        public int PatientId { get; set; }

        public int TreatmentTypeId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Note { get; set; }

        #endregion

        #region IAudit

        public DateTime AuditDate { get; set; }

        public string UserName { get; set; }

        public string AuditAction { get; set; }

        #endregion
    }
}