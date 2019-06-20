using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.SearchModel
{
    public class DoctorSearchModel
    {
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Display(Name = "CRM")]
        public string CRM { get; set; }
    }
}
