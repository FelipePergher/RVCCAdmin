using LigaCancer.Code;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.FormModel
{
    public class PhoneFormModel
    {
        [Display(Name = "Número")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Number { get; set; }

        [Display(Name = "Tipo de telefone")]
        public Globals.PhoneType PhoneType { get; set; }

        [Display(Name = "Observações")]
        public string ObservationNote { get; set; }
    }
}
