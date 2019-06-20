using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.SearchModel
{
    public class CancerTypeSearchModel
    {
        [Display(Name = "Nome")]
        public string Name { get; set; }
    }
}
