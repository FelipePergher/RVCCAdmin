// <copyright file="AuditPatientInformationMedicine.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Audit
{
    public class AuditPatientInformationMedicine : IAudit, IPatientInformationMedicine
    {
        [Key]
        public int AuditPatientInformationMedicineId { get; set; }

        #region IPatientInformationMedicine

        public int PatientInformationId { get; set; }

        public int MedicineId { get; set; }

        #endregion

        #region IAudit

        public DateTime AuditDate { get; set; }

        public string UserName { get; set; }

        public string AuditAction { get; set; }

        #endregion
    }
}