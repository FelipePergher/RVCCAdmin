// <copyright file="ResendConfirmEmailConfirmation.cshtml.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
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
