using LigaCancer.Code;
using LigaCancer.Data.Models;
using LigaCancer.Models.UserViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Controllers
{
    [Authorize(Roles = "Admin"), AutoValidateAntiforgeryToken]
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
            return PartialView("_AddUser", new UserFormModel());
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
                    CreatedBy = User.Identity.Name
                };

                IdentityResult result = await _userManager.CreateAsync(user, userForm.Password);

                if (result.Succeeded)
                {
                    IdentityRole applicationRole = await _roleManager.FindByIdAsync(userForm.RoleId);
                    if (applicationRole != null) await _userManager.AddToRoleAsync(user, applicationRole.Name);
                    return Ok();
                }
            }

            return PartialView("_AddUser", userForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();
            
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            
            if (user == null) return NotFound();


            EditUserFormModel editUserForm = new EditUserFormModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            string userRole = _userManager.GetRolesAsync(user).Result?.FirstOrDefault();
            if (userRole != null) editUserForm.Role = _roleManager.FindByNameAsync(userRole).Result.Id;

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
