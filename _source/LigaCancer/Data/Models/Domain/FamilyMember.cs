// <copyright file="FamilyMember.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Business;
using RVCC.Data.Interfaces;
using RVCC.Data.Models.Audit;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RVCC.Data.Models.Domain
{
    public class FamilyMember : RegisterData, IFamilyMember
    {
        [Key]
        public int FamilyMemberId { get; set; }

        public string Name { get; set; }

        public string Kinship { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public Enums.Sex Sex { get; set; }

        public double MonthlyIncome { get; set; }

        [Obsolete("Use MonthlyIncome instead")]
        public double MonthlyIncomeMinSalary { get; set; }

        public int PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]
        public virtual Patient Patient { get; set; }
    }
}