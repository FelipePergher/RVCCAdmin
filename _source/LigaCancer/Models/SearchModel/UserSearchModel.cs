using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.SearchModel
{
    public class UserSearchModel
    {
        [Display(Name = "Nome")]
        public string Name { get; set; }
    }
}
