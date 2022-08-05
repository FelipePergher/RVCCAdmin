// <copyright file="AuditPatientExpenseType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Audit
{
    public class AuditPatientExpenseType : IAudit, IPatientExpenseType
    {
        [Key]
        public int AuditExpenseTypeId { get; set; }

        #region IPatientExpenseType

        public int PatientExpenseTypeId { get; set; }

        public int PatientId { get; set; }

        public int ExpenseTypeId { get; set; }

        public double Value { get; set; }

        #endregion

        #region IAudit

        public DateTime AuditDate { get; set; }

        public string UserName { get; set; }

        public string AuditAction { get; set; }

        #endregion
    }
}