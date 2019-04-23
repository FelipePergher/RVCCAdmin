using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using LigaCancer.Code;

namespace LigaCancer.Models.FormModel
{
    public class PatientProfileFormModel
    {
        [HiddenInput]
        public string PatientId { get; set; }

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

        [Display(Name = "Grupo de Convivência")]
        public bool FamiliarityGroup { get; set; }

        [Display(Name = "Gênero")]
        public Globals.Sex Sex { get; set; }

        [Display(Name = "Estado Civil")]
        public Globals.CivilState CivilState { get; set; }

        [Display(Name = "Data de Nascimento")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Profissão")]
        public string Profession { get; set; }

        [Display(Name = "Renda Mensal")]
        public double? MonthlyIncome { get; set; }

    }
}
