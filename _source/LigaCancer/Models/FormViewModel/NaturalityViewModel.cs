using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.FormViewModel
{
    public class NaturalityViewModel
    {
        [Display(Name = "Cidade")]
        public string City { get; set; }

        [Display(Name = "Estado")]
        public string State { get; set; }

        [Display(Name = "País")]
        public string Country { get; set; }
    }
}
