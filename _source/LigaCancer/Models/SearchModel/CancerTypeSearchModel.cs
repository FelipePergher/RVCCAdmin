using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.SearchModel
{
    public class CancerTypeSearchModel
    {
        [Display(Name = "Nome")]
        public string Name { get; set; }
    }
}
