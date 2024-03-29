﻿// <copyright file="ResetPassword.cshtml.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

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
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ResetPasswordModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public ResetPasswordFormModel ResetPassword { get; set; }

        public IActionResult OnGet(string code = null)
        {
            if (code == null)
            {
                return BadRequest("É necessário um código para a redefinição da senha.");
            }
            else
            {
                ResetPassword = new ResetPasswordFormModel
                {
                    Code = code
                };
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ApplicationUser user = await _userManager.FindByEmailAsync(ResetPassword.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            IdentityResult result = await _userManager.ResetPasswordAsync(user, ResetPassword.Code, ResetPassword.Password);
            if (result.Succeeded)
            {
                await _userManager.SetLockoutEndDateAsync(user, null);
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }

        #region Classes

        public class ResetPasswordFormModel
        {
            [Required(ErrorMessage = "Este campo é obrigatório!")]
            [EmailAddress(ErrorMessage = "Insira um endreço de e-mail válido")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Este campo é obrigatório!")]
            [RegularExpression(@"^(?=.*[a-z])(?=.*\d).{8,}$", ErrorMessage = "A senha deve conter letras, numeros e minimo de 8 caracteres")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "A senha e a confirmação de senha são diferentes.")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
        }

        #endregion
    }
}
