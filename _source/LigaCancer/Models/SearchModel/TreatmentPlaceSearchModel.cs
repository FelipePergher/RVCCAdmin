using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.SearchModel
{
    public class TreatmentPlaceSearchModel
    {
        [Display(Name = "Cidade")]
        public string City { get; set; }
    }
}
