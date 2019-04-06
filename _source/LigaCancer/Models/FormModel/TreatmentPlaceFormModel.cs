using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.FormModel
{
    public class TreatmentPlaceFormModel
    {
        [HiddenInput]
        public int TreatmentPlaceId { get; set; }

        [Display(Name = "Cidade"), Required(ErrorMessage = "Este campo é obrigatório!"),
            Remote("IsCityExist", "TreatmentPlace", AdditionalFields = "TreatmentPlaceId", ErrorMessage = "Cidade já registrada!", HttpMethod = "GET")]
        public string City { get; set; }
    }
}
