using System.ComponentModel.DataAnnotations;
using LigaCancer.Code;

namespace LigaCancer.Models.FormViewModel
{
    public class PhoneViewModel
    {
        public string PatientId { get; set; }

        [Display(Name = "Número"), Required(ErrorMessage = "Este campo é obrigatório")]
        public string Number { get; set; }

        [Display(Name = "Tipo de telefone")]
        public Globals.PhoneType PhoneType { get; set; }

        [Display(Name = "Observações")]
        public string ObservationNote { get; set; }
    }
}
