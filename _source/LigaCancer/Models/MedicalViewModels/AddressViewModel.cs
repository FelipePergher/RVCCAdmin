using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Models.MedicalViewModels
{
    public class AddressViewModel
    {
        public string PatientId { get; set; }

        [Display(Name = "Rua")]
        public string Street { get; set; }

        [Display(Name = "Bairro")]
        public string Neighborhood { get; set; }

        [Display(Name = "Cidade")]
        public string City { get; set; }

        [Display(Name = "Número")]
        public string HouseNumber { get; set; }

        [Display(Name = "Complemento")]
        public string Complement { get; set; }

        [Display(Name = "Observação")]
        public string ObservationAddress { get; set; }
    }
}
