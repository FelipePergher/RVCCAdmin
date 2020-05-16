// <copyright file="Patient.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using RVCC.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RVCC.Data.Models.RelationModels;

namespace RVCC.Data.Models
{
    public class Patient : RegisterData
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

        public double MonthlyIncome { get; set; }

        public Globals.Sex Sex { get; set; }

        public Globals.CivilState? CivilState { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime JoinDate { get; set; }

        public string Profession { get; set; }

        public string SocialObservation { get; set; }

        public ActivePatient ActivePatient { get; set; }

        public PatientInformation PatientInformation { get; set; }

        public Naturality Naturality { get; set; }

        public ICollection<Phone> Phones { get; set; }

        public ICollection<Address> Addresses { get; set; }

        public ICollection<FamilyMember> FamilyMembers { get; set; }

        public ICollection<FileAttachment> FileAttachments { get; set; }

        public ICollection<PatientBenefit> PatientBenefits { get; set; }

        public ICollection<Stay> Stays { get; set; }
    }
}
