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
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Gênero")]
        public Globals.Sex Sex { get; set; }

        [Display(Name = "Renda mensal")]
        [Range(0, 1000000.00, ErrorMessage = "Insira um valor de 0.00 até 1,000,000")]
        [RegularExpression(@"[0-9]{0,5}\.?[0-9]{1,2}", ErrorMessage = "Insira um valor válido!")]
        public double? MonthlyIncome { get; set; }
    }
}
