﻿using RVCC.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.PatientModels
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

        public virtual ActivePatient ActivePatient { get; set; }

        public virtual PatientInformation PatientInformation { get; set; }

        public virtual Naturality Naturality { get; set; }

        public virtual ICollection<Phone> Phones { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }

        public virtual ICollection<FamilyMember> FamilyMembers { get; set; }

        public virtual ICollection<FileAttachment> FileAttachments { get; set; }

    }
}
