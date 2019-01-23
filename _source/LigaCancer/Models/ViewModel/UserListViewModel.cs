using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.ViewModel
{
    public class UserListViewModel
    {
        public string UserId { get; set; }

        [Display(Name = "Nome")]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        [Display(Name = "Regra")]
        public string Role { get; set; }
    }
}
