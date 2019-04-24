using LigaCancer.Code;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.FormModel
{
    public class PatientFormModel
    {
        public PatientFormModel()
        {
            DateOfBirth = DateTime.Now;
        }

        [HiddenInput]
        public int PatientId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        public string FirstName { get; set; }

        [Display(Name = "Sobrenome")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        public string Surname { get; set; }
        
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [Remote("IsRgExist", "Patient", AdditionalFields = "PatientId", ErrorMessage = "RG já registrado!")]
        public string RG { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [Remote("IsCpfExist", "Patient", AdditionalFields = "PatientId", ErrorMessage = "CPF já registrado!")]
        public string CPF { get; set; }

        [Display(Name = "Idade")]
        public int? Age { get; set; }

        [Display(Name = "Grupo de Convivência")]
        public bool FamiliarityGroup { get; set; }

        [Display(Name = "Gênero")]
        public Globals.Sex Sex { get; set; }

        [Display(Name = "Estado Civil")]
        public Globals.CivilState CivilState { get; set; }

        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Profissão")]
        public string Profession { get; set; }

        [Display(Name = "Renda Mensal")]
        public double? MonthlyIncome { get; set; }

        public NaturalityFormModel Naturality { get; set; }

        public PatientInformationFormModel PatientInformation { get; set; }

        #region Selects

        public List<SelectListItem> SelectDoctors { get; set; }

        public List<SelectListItem> SelectTreatmentPlaces { get; set; }

        public List<SelectListItem> SelectMedicines { get; set; }

        public List<SelectListItem> SelectCancerTypes { get; set; }

        #endregion
    }
}
