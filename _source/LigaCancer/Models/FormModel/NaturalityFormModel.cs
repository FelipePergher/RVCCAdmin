using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class NaturalityFormModel
    {
        [Display(Name = "Cidade")]
        [StringLength(200, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string City { get; set; }

        [Display(Name = "Estado")]
        [StringLength(200, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string State { get; set; }

        [Display(Name = "País")]
        [StringLength(200, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string Country { get; set; }
    }
}
