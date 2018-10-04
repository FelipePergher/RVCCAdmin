using LigaCancer.Data.Models.PatientModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static LigaCancer.Code.Globals;

namespace LigaCancer.Models.MedicalViewModels
{
    public class PatientViewModel
    {
        public PatientViewModel()
        {
            DateOfBirth = DateTime.Now;
        }

        public int PatientId { get; set; }

        [Display(Name = "Nome"), Required]
        public string FirstName { get; set; }

        [Display(Name = "Surname"), Required]
        public string Surname { get; set; }

        [Required]
        public string RG { get; set; }

        [Required]
        public string CPF { get; set; }

        [Display(Name = "Grupo de Convivência")]
        public bool FamiliarityGroup { get; set; }

        [Display(Name = "Sexo")]
        public Sex Sex { get; set; }

        [Display(Name = "Estado Civil")]
        public CivilState CivilState { get; set; }

        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public NaturalityViewModel Naturality { get; set; }

        public FamilyViewModel Family { get; set; }

        public PatientInformationViewModel PatientInformation { get; set; }

        public ICollection<PhoneViewModel> Phones { get; set; }

        public ICollection<AddressViewModel> Addresses { get; set; }

        public ICollection<AttachmentsViewModel> Attachments { get; set; }

    }
}
