using LigaCancer.Code;
using LigaCancer.Data.Models;
using LigaCancer.Models.UserViewModels;
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
            UserFormModel userForm = new UserFormModel();

            IdentityRole adminRoleOption = _roleManager.FindByNameAsync(Globals.Roles.Admin.ToString()).Result;
            IdentityRole userRoleOption = _roleManager.FindByNameAsync(Globals.Roles.User.ToString()).Result;
           
            if(userRoleOption != null)
            {
                userForm.ApplicationRoles.Add(new SelectListItem
                {
                    Text = Globals.GetDisplayName(Globals.Roles.User),
                    Value = userRoleOption.Id
                });
                userForm.RoleId = userRoleOption.Id;
            }
            if (adminRoleOption != null)
            {
                userForm.ApplicationRoles.Add(new SelectListItem
                {
                    Text = Globals.GetDisplayName(Globals.Roles.Admin),
                    Value = adminRoleOption.Id
                });
            }

            return PartialView("_AddUser", userForm);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserFormModel userForm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    FirstName = userForm.FirstName,
                    LastName = userForm.LastName,
                    UserName = userForm.Email,
                    Email = userForm.Email,
                    CreatedBy = User.Identity.Name,
                };
                IdentityResult result = await _userManager.CreateAsync(user, userForm.Password);
                if (result.Succeeded)
                {
                    IdentityRole applicationRole = await _roleManager.FindByIdAsync(userForm.RoleId);
                    if (applicationRole != null)
                    {
                        await _userManager.AddToRoleAsync(user, applicationRole.Name);
                    }
                    return StatusCode(200, "200");

                }
                ModelState.AddErrors(result);
            }

            userForm.ApplicationRoles = _roleManager.Roles.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id
            }).ToList();

            return PartialView("_AddUser", userForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            EditUserFormModel editUserForm = new EditUserFormModel();

            if (string.IsNullOrEmpty(id)) return PartialView("_EditUser", editUserForm);

            IdentityRole adminRoleOption = _roleManager.FindByNameAsync(Globals.Roles.Admin.ToString()).Result;
            IdentityRole userRoleOption = _roleManager.FindByNameAsync(Globals.Roles.User.ToString()).Result;

            if (userRoleOption != null)
            {
                editUserForm.ApplicationRoles.Add(new SelectListItem
                {
                    Text = Globals.GetDisplayName(Globals.Roles.User),
                    Value = userRoleOption.Id
                });
            }
            if (adminRoleOption != null)
            {
                editUserForm.ApplicationRoles.Add(new SelectListItem
                {
                    Text = Globals.GetDisplayName(Globals.Roles.Admin),
                    Value = adminRoleOption.Id
                });
            }

            ApplicationUser user = await _userManager.FindByIdAsync(id);
            string userRole = _userManager.GetRolesAsync(user).Result?.FirstOrDefault();
            if (userRole != null)
            {
                editUserForm.Role = _roleManager.FindByNameAsync(userRole).Result.Id;
            }

            if (user == null) return PartialView("_EditUser", editUserForm);

            editUserForm.UserId = user.Id;
            editUserForm.FirstName = user.FirstName;
            editUserForm.LastName = user.LastName;
            editUserForm.Email = user.Email;
            return PartialView("_EditUser", editUserForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(string id, EditUserFormModel editUserForm)
        {
            if (!ModelState.IsValid) return PartialView("_EditUser", editUserForm);

            ApplicationUser user = await _userManager.FindByIdAsync(id);

            if (user == null) return PartialView("_EditUser", editUserForm);

            user.FirstName = editUserForm.FirstName;
            user.LastName = editUserForm.LastName;
            user.Email = editUserForm.Email;

            IdentityResult result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                string userRole = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
                if(userRole != null)
                {
                    await _userManager.RemoveFromRoleAsync(user, userRole);
                }

                IdentityRole role = _roleManager.Roles.FirstOrDefault(x => x.Id == editUserForm.Role);
                if(role != null)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }

                return StatusCode(200, "200");
            }
            ModelState.AddErrors(result);

            return PartialView("_EditUser", editUserForm);
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
