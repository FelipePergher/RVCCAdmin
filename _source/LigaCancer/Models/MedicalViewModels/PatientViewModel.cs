using LigaCancer.Data.Models.PatientModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            Naturality = new NaturalityViewModel();
            PatientInformation = new PatientInformationViewModel();
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

        public List<SelectListItem>  SelectProfessions { get; set; }

        [Display(Name = "Profissão")]
        public string Profession { get; set; }

        public NaturalityViewModel Naturality { get; set; }

        public PatientInformationViewModel PatientInformation { get; set; }

        //Todo:
        public FamilyViewModel Family { get; set; }

        public ICollection<PhoneViewModel> Phones { get; set; }

        public ICollection<AddressViewModel> Addresses { get; set; }

        public ICollection<AttachmentsViewModel> Attachments { get; set; }

    }
}
