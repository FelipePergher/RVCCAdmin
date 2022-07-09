// <copyright file="Patient.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Business;
using RVCC.Data.Interfaces;
using RVCC.Data.Models.Audit;
using RVCC.Data.Models.RelationModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Domain
{
    public class Patient : RegisterData, IPatient
    {
        public Patient()
        {
            Phones = new HashSet<Phone>();
            Addresses = new HashSet<Address>();
            FileAttachments = new HashSet<FileAttachment>();
            FamilyMembers = new HashSet<FamilyMember>();
            PatientInformation = new PatientInformation();
            Naturality = new Naturality();
            ActivePatient = new ActivePatient();
            PatientBenefits = new HashSet<PatientBenefit>();
            Stays = new HashSet<Stay>();
        }

        [Key]
        public int PatientId { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string RG { get; set; }

        public string CPF { get; set; }

        public bool FamiliarityGroup { get; set; }

        public bool ForwardedToSupportHouse { get; set; }

        public double MonthlyIncome { get; set; }

        [Obsolete("Use MonthlyIncome instead")]
        public double MonthlyIncomeMinSalary { get; set; }

        public Enums.Sex Sex { get; set; }

        public Enums.CivilState? CivilState { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime JoinDate { get; set; }

        public string Profession { get; set; }

        public string SocialObservation { get; set; }

        public string ImmediateNecessities { get; set; }

        public DateTime ImmediateNecessitiesDateUpdated { get; set; }

        #region Relations

        public ActivePatient ActivePatient { get; set; }

        public PatientInformation PatientInformation { get; set; }

        public Naturality Naturality { get; set; }

        public ICollection<Phone> Phones { get; set; }

        public ICollection<Address> Addresses { get; set; }

        public ICollection<FamilyMember> FamilyMembers { get; set; }

        public ICollection<FileAttachment> FileAttachments { get; set; }

        public ICollection<PatientBenefit> PatientBenefits { get; set; }

        public ICollection<Stay> Stays { get; set; }

        #endregion
    }
}
