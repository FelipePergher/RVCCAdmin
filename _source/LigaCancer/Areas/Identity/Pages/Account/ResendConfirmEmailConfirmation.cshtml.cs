using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LigaCancer.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResendConfirmEmailConfirmationModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}
