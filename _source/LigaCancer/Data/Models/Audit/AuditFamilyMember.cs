// <copyright file="AuditFamilyMember.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Business;
using RVCC.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Audit
{
    public class AuditFamilyMember : IAudit, IFamilyMember
    {
        [Key]
        public int AuditFamilyMemberId { get; set; }

        #region IFamilyMember

        public int FamilyMemberId { get; set; }

        public string Name { get; set; }

        public string Kinship { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public Enums.Sex Sex { get; set; }

        public double MonthlyIncome { get; set; }

        public double MonthlyIncomeMinSalary { get; set; }

        public int PatientId { get; set; }

        #endregion

        #region IAudit

        public DateTime AuditDate { get; set; }

        public string UserName { get; set; }

        public string AuditAction { get; set; }

        #endregion
    }
}