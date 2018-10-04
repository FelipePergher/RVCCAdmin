using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Models.MedicalViewModels
{
    public class TreatmentPlaceViewModel
    {

        public int TreatmentPlaceId { get; set; }

        [Display(Name = "Cidade"), Required(ErrorMessage = "Este campo é obrigatório!"),
            Remote("IsCityExist", "TreatmentPlace", AdditionalFields = "TreatmentPlaceId", ErrorMessage = "Cidade já registrada!")]
        public string City { get; set; }
    }
}
