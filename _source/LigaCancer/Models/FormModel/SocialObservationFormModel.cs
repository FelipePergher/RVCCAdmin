using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class SocialObservationFormModel
    {
        [Display(Name = "Observações")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        public string Observations { get; set; }
    }
}
