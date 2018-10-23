using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static LigaCancer.Code.Globals;

namespace LigaCancer.Models.MedicalViewModels
{
    public class PhoneViewModel
    {
        public string PatientId { get; set; }

        [Display(Name = "Número"), Required(ErrorMessage = "Este campo é obrigatório")]
        public string Number { get; set; }

        [Display(Name = "Tipo de telefone")]
        public PhoneType PhoneType { get; set; }

        [Display(Name = "Observações")]
        public string ObservationNote { get; set; }
    }
}
