using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class BenefitFormModel
    {
        public BenefitFormModel() { }

        public BenefitFormModel(string name, int benefitId)
        {
            Name = name;
            BenefitId = benefitId;
        }

        [HiddenInput]
        public int BenefitId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [Remote("IsNameExist", "BenefitApi", AdditionalFields = "BenefitId", ErrorMessage = "Benefício já registrado!")]
        [StringLength(200, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string Name { get; set; }

        [Display(Name = "Nota")]
        [StringLength(200, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string Note { get; set; }
    }
}
