using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Controllers
{
    using Code;
    using Data.Models;
    using Models.UserViewModels;

    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            UserViewModel model = new UserViewModel();

            IdentityRole adminRoleOption = _roleManager.FindByNameAsync(Globals.Roles.Admin.ToString()).Result;
            IdentityRole userRoleOption = _roleManager.FindByNameAsync(Globals.Roles.User.ToString()).Result;
           
            if(userRoleOption != null)
            {
                model.ApplicationRoles.Add(new SelectListItem
                {
                    Text = Globals.GetDisplayName(Globals.Roles.User),
                    Value = userRoleOption.Id
                });
                model.RoleId = userRoleOption.Id;
            }
            if (adminRoleOption != null)
            {
                model.ApplicationRoles.Add(new SelectListItem
                {
                    Text = Globals.GetDisplayName(Globals.Roles.Admin),
                    Value = adminRoleOption.Id
                });
            }

            return PartialView("_AddUser", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.Email,
                    Email = model.Email,
                    CreatedBy = User.Identity.Name,
                };
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    IdentityRole applicationRole = await _roleManager.FindByIdAsync(model.RoleId);
                    if (applicationRole != null)
                    {
                        await _userManager.AddToRoleAsync(user, applicationRole.Name);
                    }
                    return StatusCode(200, "200");

                }
                ModelState.AddErrors(result);
            }

            model.ApplicationRoles = _roleManager.Roles.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id
            }).ToList();

            return PartialView("_AddUser", model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            EditUserViewModel model = new EditUserViewModel();

            if (string.IsNullOrEmpty(id)) return PartialView("_EditUser", model);

            IdentityRole adminRoleOption = _roleManager.FindByNameAsync(Globals.Roles.Admin.ToString()).Result;
            IdentityRole userRoleOption = _roleManager.FindByNameAsync(Globals.Roles.User.ToString()).Result;

            if (userRoleOption != null)
            {
                model.ApplicationRoles.Add(new SelectListItem
                {
                    Text = Globals.GetDisplayName(Globals.Roles.User),
                    Value = userRoleOption.Id
                });
            }
            if (adminRoleOption != null)
            {
                model.ApplicationRoles.Add(new SelectListItem
                {
                    Text = Globals.GetDisplayName(Globals.Roles.Admin),
                    Value = adminRoleOption.Id
                });
            }

            ApplicationUser user = await _userManager.FindByIdAsync(id);
            string userRole = _userManager.GetRolesAsync(user).Result?.FirstOrDefault();
            if (userRole != null)
            {
                model.Role = _roleManager.FindByNameAsync(userRole).Result.Id;
            }

            if (user == null) return PartialView("_EditUser", model);

            model.UserId = user.Id;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.Email = user.Email;
            return PartialView("_EditUser", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(string id, EditUserViewModel model)
        {
            if (!ModelState.IsValid) return PartialView("_EditUser", model);

            ApplicationUser user = await _userManager.FindByIdAsync(id);

            if (user == null) return PartialView("_EditUser", model);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;

            IdentityResult result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                string userRole = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
                if(userRole != null)
                {
                    await _userManager.RemoveFromRoleAsync(user, userRole);
                }

                IdentityRole role = _roleManager.Roles.FirstOrDefault(x => x.Id == model.Role);
                if(role != null)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }

                return StatusCode(200, "200");
            }
            ModelState.AddErrors(result);

            return PartialView("_EditUser", model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            string name = string.Empty;
            if (string.IsNullOrEmpty(id)) return PartialView("_DeleteUser", name);

            ApplicationUser applicationUser = await _userManager.FindByIdAsync(id);
            if (applicationUser != null)
            {
                name = $"{applicationUser.FirstName} {applicationUser.LastName}";
            }
            return PartialView("_DeleteUser", name);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id, IFormCollection form)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index");

            ApplicationUser applicationUser = await _userManager.FindByIdAsync(id);
            if (applicationUser == null) return RedirectToAction("Index");

            applicationUser.DeletedDate = DateTime.Now;
            applicationUser.IsDeleted = true;
            IdentityResult result = await _userManager.UpdateAsync(applicationUser);
            if (result.Succeeded)
            {
                return StatusCode(200, "200");
            }
            ModelState.AddErrors(result);
            return PartialView("_DeleteUser", $"{applicationUser.FirstName} {applicationUser.LastName}");
        }

        #region Custom Methods

        public JsonResult IsEmailUsed(string Email, string UserId)
        {
            ApplicationUser user = _userManager.FindByEmailAsync(Email).Result;

            return user != null ? Json(user.Id == UserId) : Json(true);
        }

        #endregion
    }
}
