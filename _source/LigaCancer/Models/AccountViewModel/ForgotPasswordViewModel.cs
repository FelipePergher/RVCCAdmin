using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.AccountViewModel
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
