using LigaCancer.Code;
using System;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.FormModel
{
    public class FamilyMemberFormModel
    {
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Este campo é obrigátorio!")]
        public string Name { get; set; }

        [Display(Name = "Parentesco")]
        [Required(ErrorMessage = "Este campo é obrigátorio!")]
        public string Kinship { get; set; }

        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        [DateRange]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Gênero")]
        public Globals.Sex Sex { get; set; }

        [Display(Name = "Renda mensal")]
        public string MonthlyIncome { get; set; }
    }
}
