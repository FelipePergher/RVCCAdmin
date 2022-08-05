// <copyright file="IFamilyMember.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Business;
using System;

namespace RVCC.Data.Interfaces
{
    public interface IFamilyMember
    {
        public int FamilyMemberId { get; set; }

        public string Name { get; set; }

        public string Kinship { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public Enums.Sex Sex { get; set; }

        public double MonthlyIncome { get; set; }

        public int PatientId { get; set; }

        public bool Responsible { get; set; }

        public bool IgnoreOnIncome { get; set; }

        #region Obsolete

        [Obsolete("Use MonthlyIncome instead")]
        public double MonthlyIncomeMinSalary { get; set; }

        #endregion
    }
}