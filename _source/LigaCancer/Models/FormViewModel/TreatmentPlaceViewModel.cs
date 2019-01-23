using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.FormViewModel
{
    public class TreatmentPlaceViewModel
    {

        public int TreatmentPlaceId { get; set; }

        [Display(Name = "Cidade"), Required(ErrorMessage = "Este campo é obrigatório!"),
            Remote("IsCityExist", "TreatmentPlace", AdditionalFields = "TreatmentPlaceId", ErrorMessage = "Cidade já registrada!")]
        public string City { get; set; }
    }
}
