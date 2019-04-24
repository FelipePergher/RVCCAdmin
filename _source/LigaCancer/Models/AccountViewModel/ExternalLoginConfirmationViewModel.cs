using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.AccountViewModel
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
