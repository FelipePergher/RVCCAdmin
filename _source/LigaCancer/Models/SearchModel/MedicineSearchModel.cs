using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.SearchModel
{
    public class MedicineSearchModel
    {
        [Display(Name = "Nome")]
        public string Name { get; set; }
    }
}
