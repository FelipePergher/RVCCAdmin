﻿// <copyright file="UserController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Data.Models.Domain;
using RVCC.Models.FormModel;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace RVCC.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    [AutoValidateAntiforgeryToken]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UserController> _logger;
        private readonly IEmailSender _emailSender;

        public UserController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<UserController> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            return PartialView("Partials/_AddUser", new UserFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserFormModel userForm)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Name = userForm.Name,
                    UserName = userForm.Email,
                    Email = userForm.Email,
                    CreatedBy = User.Identity.Name
                };

                IdentityResult result = await _userManager.CreateAsync(user, userForm.Password);

                if (result.Succeeded)
                {
                    IdentityRole applicationRole = await _roleManager.FindByNameAsync(userForm.Role);
                    if (applicationRole != null)
                    {
                        await _userManager.AddToRoleAsync(user, applicationRole.Name);
                    }

                    try
                    {
                        string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        string callbackUrl = Url.Page(
                           "/Account/ConfirmEmail",
                           pageHandler: null,
                           values: new { area = "Identity", userId = user.Id, code },
                           protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(user.Email, "Confirme seu email", $"Por favor confirme sua conta <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicando aqui</a>.");
                    }
                    catch (System.Exception e)
                    {
                    }

                    return Ok();
                }

                _logger.LogError(LogEvents.InsertItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_AddUser", userForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            ApplicationUser user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var userForm = new EditUserFormModel(user.Id, user.Name);

            IList<string> roles = await _userManager.GetRolesAsync(user);
            string userRole = roles.Any() ? roles.FirstOrDefault() : string.Empty;
            if (!string.IsNullOrEmpty(userRole))
            {
                userForm.Role = userRole;
            }

            return PartialView("Partials/_EditUser", userForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(string id, EditUserFormModel userForm)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            ApplicationUser user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                user.Name = userForm.Name;

                IdentityResult result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    IList<string> userRoles = await _userManager.GetRolesAsync(user);
                    string userRole = userRoles.FirstOrDefault();
                    if (userRole != null && userRole != userForm.Role)
                    {
                        await _userManager.RemoveFromRoleAsync(user, userRole);
                    }

                    IdentityRole role = await _roleManager.FindByNameAsync(userForm.Role);
                    if (role != null && role.Name != userRole)
                    {
                        await _userManager.AddToRoleAsync(user, role.Name);
                    }

                    return Ok();
                }

                _logger.LogError(LogEvents.UpdateItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
            }

            return PartialView("Partials/_EditUser", userForm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            ApplicationUser applicationUser = await _userManager.FindByIdAsync(id);

            if (applicationUser == null)
            {
                return NotFound();
            }

            IdentityResult result = await _userManager.DeleteAsync(applicationUser);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(LogEvents.DeleteItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> UnlockUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            ApplicationUser applicationUser = await _userManager.FindByIdAsync(id);

            if (applicationUser == null)
            {
                return NotFound();
            }

            IdentityResult result = await _userManager.SetLockoutEndDateAsync(applicationUser, null);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(LogEvents.UpdateItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
            return BadRequest();
        }
    }
}
