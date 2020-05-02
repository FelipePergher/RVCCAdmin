using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.SearchModel
{
    public class BenefitSearchModel
    {
        [Display(Name = "Nome")]
        public string Name { get; set; }
    }
}
