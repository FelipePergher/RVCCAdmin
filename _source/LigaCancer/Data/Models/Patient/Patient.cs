﻿using LigaCancer.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Data.Models.Patient
{
    public class Patient : RegisterData
    {
        public Patient()
        {
            Phones = new HashSet<Phone>();
            Addresses = new HashSet<Address>();
            Attachments = new HashSet<Attachments>();
        }

        [Key]
        public int PatientId { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string RG { get; set; }

        public string CPF { get; set; }

        public bool FamiliarityGroup { get; set; }

        public Sex Sex { get; set; }

        public CivilState CivilState { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Naturality Naturality { get; set; }

        public Profession Profession { get; set; }

        public Family Family { get; set; }

        public PatientInformation PatientInformation { get; set; }

        public ICollection<Phone> Phones { get; set; }

        public ICollection<Address> Addresses { get; set; }

        public ICollection<Attachments> Attachments { get; set; }
    }
}
