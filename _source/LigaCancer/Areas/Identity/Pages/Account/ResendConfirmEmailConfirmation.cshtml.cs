using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RVCC.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResendConfirmEmailConfirmationModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}
