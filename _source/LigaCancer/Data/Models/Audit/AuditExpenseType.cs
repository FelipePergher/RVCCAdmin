// <copyright file="AuditExpenseType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using static RVCC.Business.Enums;

namespace RVCC.Data.Models.Audit
{
    public class AuditExpenseType : IAudit, IExpenseType
    {
        [Key]
        public int AuditExpenseTypeId { get; set; }

        #region IExpenseType

        public int ExpenseTypeId { get; set; }

        public string Name { get; set; }

        public ExpenseTypeFrequency ExpenseTypeFrequency { get; set; }

        #endregion

        #region Audit

        public DateTime AuditDate { get; set; }

        public string UserName { get; set; }

        public string AuditAction { get; set; }

        #endregion
    }
}