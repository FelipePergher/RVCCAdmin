// <copyright file="BirthdayController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RVCC.Business;
using RVCC.Models.SearchModel;

namespace RVCC.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class BirthdayController : Controller
    {
        [Authorize(Roles = Roles.AdminUserSocialAssistanceAuthorize)]
        [HttpGet]
        public IActionResult Index()
        {
            return View(new BirthdaySearchModel());
        }
    }
}