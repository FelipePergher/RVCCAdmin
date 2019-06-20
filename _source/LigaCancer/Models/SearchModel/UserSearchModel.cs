using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.SearchModel
{
    public class UserSearchModel
    {
        [Display(Name = "Nome")]
        public string Name { get; set; }
    }
}
