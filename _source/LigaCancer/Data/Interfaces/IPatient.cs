// <copyright file="IPatient.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Business;
using System;

namespace RVCC.Data.Interfaces
{
    public interface IPatient
    {
        public int PatientId { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string RG { get; set; }

        public string CPF { get; set; }

        public bool FamiliarityGroup { get; set; }

        public double MonthlyIncome { get; set; }

        public double MonthlyIncomeMinSalary { get; set; }

        public Enums.Sex Sex { get; set; }

        public Enums.CivilState? CivilState { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime JoinDate { get; set; }

        public string Profession { get; set; }

        public string SocialObservation { get; set; }
    }
}