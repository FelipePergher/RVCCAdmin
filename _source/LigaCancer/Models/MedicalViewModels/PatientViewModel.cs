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

        [Display(Name = "Nome"), Required(ErrorMessage = "Este campo é obrigatório!")]
        public string FirstName { get; set; }

        [Display(Name = "Sobrenome"), Required(ErrorMessage = "Este campo é obrigatório!")]
        public string Surname { get; set; }
        
        [Required(ErrorMessage = "Este campo é obrigatório!"), Remote("IsRgExist", "Patient", AdditionalFields = "PatientId", ErrorMessage = "RG já registrado!")]
        public string RG { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório!"), Remote("IsCpfExist", "Patient", AdditionalFields = "PatientId", ErrorMessage = "CPF já registrado!")]
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

        [Display(Name = "Profissão")]
        public string Profession { get; set; }

        public NaturalityViewModel Naturality { get; set; }

        public PatientInformationViewModel PatientInformation { get; set; }

        #region Selects

        public List<SelectListItem> SelectProfessions { get; set; }
        public List<SelectListItem> SelectDoctors { get; set; }
        public List<SelectListItem> SelectTreatmentPlaces { get; set; }
        public List<SelectListItem> SelectMedicines { get; set; }
        public List<SelectListItem> SelectCancerTypes { get; set; }

        #endregion
    }
}
