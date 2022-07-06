// <copyright file="AuditPatient.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Business;
using RVCC.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Audit
{
    public class AuditPatient : IAudit, IPatient
    {
        [Key]
        public int AuditPatientId { get; set; }

        #region IPatient

        public int PatientId { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string RG { get; set; }

        public string CPF { get; set; }

        public bool FamiliarityGroup { get; set; }

        public double MonthlyIncome { get; set; }

        [Obsolete("Use MonthlyIncome instead")]
        public double MonthlyIncomeMinSalary { get; set; }

        public Enums.Sex Sex { get; set; }

        public Enums.CivilState? CivilState { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime JoinDate { get; set; }

        public string Profession { get; set; }

        public string SocialObservation { get; set; }

        #endregion

        #region IAudit

        public DateTime AuditDate { get; set; }

        public string UserName { get; set; }

        public string AuditAction { get; set; }

        #endregion
    }
}