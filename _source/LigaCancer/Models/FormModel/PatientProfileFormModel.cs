using Microsoft.AspNetCore.Mvc;
using RVCC.Business;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class PatientProfileFormModel
    {
        public PatientProfileFormModel() { }

        public PatientProfileFormModel(int patientId)
        {
            PatientId = patientId;
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
        [Remote("IsRgExist", "PatientApi", AdditionalFields = "PatientId", ErrorMessage = "RG já registrado!")]
        public string RG { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [Remote("IsCpfExist", "PatientApi", AdditionalFields = "PatientId", ErrorMessage = "CPF já registrado!")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "CPF inválido")]
        public string CPF { get; set; }

        [Display(Name = "Grupo de Convivência")]
        public bool FamiliarityGroup { get; set; }

        [Display(Name = "Gênero")]
        public Globals.Sex Sex { get; set; }

        [Display(Name = "Estado Civil")]
        public Globals.CivilState? CivilState { get; set; }

        [Display(Name = "Data de Nascimento")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        [DateRange]
        [RegularExpression(@"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$", ErrorMessage = "Insira uma data válida")]
        public string DateOfBirth { get; set; }

        [Display(Name = "Profissão")]
        public string Profession { get; set; }

        [Display(Name = "Renda mensal")]
        public string MonthlyIncome { get; set; }

    }
}
