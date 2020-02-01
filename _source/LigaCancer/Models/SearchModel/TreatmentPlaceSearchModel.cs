using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.SearchModel
{
    public class TreatmentPlaceSearchModel
    {
        [Display(Name = "Cidade")]
        public string City { get; set; }
    }
}
