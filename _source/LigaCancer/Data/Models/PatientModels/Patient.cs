using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static LigaCancer.Code.Globals;

namespace LigaCancer.Data.Models.PatientModels
{
    public class Patient : RegisterData
    {
        public Patient()
        {
            Phones = new HashSet<Phone>();
            Addresses = new HashSet<Address>();
            FileAttachments = new HashSet<FileAttachment>();
            PatientInformation = new PatientInformation();
            Naturality = new Naturality();
        }

        [Key]
        public int PatientId { get; set; }

        [Display(Name = "Nome")]
        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string RG { get; set; }

        public string CPF { get; set; }

        public int? Age { get; set; }

        public bool FamiliarityGroup { get; set; }

        public Sex Sex { get; set; }

        public CivilState CivilState { get; set; }

        public DateTime DateOfBirth { get; set; }

        public virtual Naturality Naturality { get; set; }

        public string Profession { get; set; }

        public Family Family { get; set; }

        public PatientInformation PatientInformation { get; set; }

        public virtual ICollection<Phone> Phones { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }

        public ICollection<FileAttachment> FileAttachments { get; set; }

    }
}
