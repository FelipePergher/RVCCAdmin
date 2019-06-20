using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.SearchModel
{
    public class MedicineSearchModel
    {
        [Display(Name = "Nome")]
        public string Name { get; set; }
    }
}
