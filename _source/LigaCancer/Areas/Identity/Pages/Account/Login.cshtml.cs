// <copyright file="Login.cshtml.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RVCC.Data.Models.Domain;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace RVCC.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public LoginFormModel Login { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ReturnUrl = returnUrl;

            Login = new LoginFormModel();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(Login.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    Login.ShowResend = user != null;
                    return Page();
                }

                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(Login.Email, Login.Password, Login.RememberMe, true);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }

                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, Login.RememberMe });
                }

                if (result.IsLockedOut)
                {
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Tentativa de login inválida.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        #region Classes

        public class LoginFormModel
        {
            [Required(ErrorMessage = "Este campo é obrigatório!")]
            [EmailAddress(ErrorMessage = "Insira um endreço de e-mail válido")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Este campo é obrigatório!")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Lembrar Login?")]
            public bool RememberMe { get; set; }

            public bool ShowResend { get; set; }
        }

        #endregion
    }
}
