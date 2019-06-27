using LigaCancer.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace LigaCancer.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResendConfirmEmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ResendConfirmEmailModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public ResetConfirmEmailFormModel ResetConfirmEmail { get; set; }

        public class ResetConfirmEmailFormModel
        {
            [Required(ErrorMessage = "Este campo é obrigatório!")]
            [EmailAddress(ErrorMessage = "Insira um endreço de e-mail válido")]
            public string Email { get; set; }
        }

        public void OnGet(string email)
        {
            ResetConfirmEmail = new ResetConfirmEmailFormModel
            {
                Email = email
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(ResetConfirmEmail.Email);
            if (user != null && !await _userManager.IsEmailConfirmedAsync(user))
            {
                //Resend email just when user exists and not is confirmed yet
                string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                string callbackUrl = Url.Page(
                   "/Account/ConfirmEmail",
                   pageHandler: null,
                   values: new { userId = user.Id, code },
                   protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(user.Email, "Confirme seu email",
                    $"Por favor confirme sua conta <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicando aqui</a>.");
            }

            return RedirectToPage("./ResendConfirmEmailConfirmation");
        }

    }
}
