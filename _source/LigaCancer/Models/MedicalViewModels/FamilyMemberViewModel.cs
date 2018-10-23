using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static LigaCancer.Code.Globals;

namespace LigaCancer.Models.MedicalViewModels
{
    public class FamilyMemberViewModel
    {
        public string PatientId { get; set; }

        [Display(Name = "Nome"), Required(ErrorMessage = "Este campo é obrigátorio!")]
        public string Name { get; set; }

        [Display(Name = "Parentesco"), Required(ErrorMessage = "Este campo é obrigátorio!")]
        public string Kinship { get; set; }

        [Display(Name = "Idade"), Required(ErrorMessage = "Este campo é obrigátorio!"), Range(0, 120, ErrorMessage = "Insira um valor de 0 até 120")]
        public int Age { get; set; }

        [Display(Name = "Gênero")]
        public Sex Sex { get; set; }

        [Display(Name = "Renda mensal"), Range(0, 1000000.00, ErrorMessage = "Insira um valor de 0.00 até 1,000,000"), RegularExpression(@"[0-9]{0,5}\.?[0-9]{1,2}", ErrorMessage = "Insira um valor válido!")]
        public decimal MonthlyIncome { get; set; }
    }
}
