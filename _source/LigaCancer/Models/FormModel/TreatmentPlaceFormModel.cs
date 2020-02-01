using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class TreatmentPlaceFormModel
    {
        public TreatmentPlaceFormModel() { }

        public TreatmentPlaceFormModel(string city, int treatmentPlaceId)
        {
            City = city;
            TreatmentPlaceId = treatmentPlaceId;
        }

        [HiddenInput]
        public int TreatmentPlaceId { get; set; }

        [Display(Name = "Cidade")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [Remote("IsCityExist", "TreatmentPlaceApi", AdditionalFields = "TreatmentPlaceId", ErrorMessage = "Cidade já registrada!")]
        public string City { get; set; }
    }
}
