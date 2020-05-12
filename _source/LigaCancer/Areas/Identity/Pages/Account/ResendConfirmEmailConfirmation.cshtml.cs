// <copyright file="ResendConfirmEmailConfirmation.cshtml.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

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
