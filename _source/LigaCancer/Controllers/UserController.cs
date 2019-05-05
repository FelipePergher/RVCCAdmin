using LigaCancer.Data.Models;
using LigaCancer.Models.UserViewModels;
using Microsoft.AspNetCore.Authorization;
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
            return PartialView("Partials/_AddUser", new UserFormModel());
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

            return PartialView("Partials/_AddUser", userForm);
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

            return PartialView("Partials/_EditUser", editUserForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(string id, EditUserFormModel editUserForm)
        {
            if(string.IsNullOrEmpty(id)) return BadRequest();
            
            ApplicationUser user = await _userManager.FindByIdAsync(id);

            if (user == null) return NotFound();

            if (ModelState.IsValid)
            {
                user.FirstName = editUserForm.FirstName;
                user.LastName = editUserForm.LastName;
                user.Email = editUserForm.Email;

                IdentityResult result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    string userRole = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
                    if (userRole != null) await _userManager.RemoveFromRoleAsync(user, userRole);

                    IdentityRole role = _roleManager.Roles.FirstOrDefault(x => x.Id == editUserForm.Role);
                    if (role != null) await _userManager.AddToRoleAsync(user, role.Name);

                    return Ok();
                }
            }

            return PartialView("Partials/_EditUser", editUserForm);
        }

        [HttpPost, IgnoreAntiforgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            ApplicationUser applicationUser = await _userManager.FindByIdAsync(id);

            if (applicationUser == null) return NotFound();

            //Todo see how use user identity to disable user
            applicationUser.DeletedDate = DateTime.Now;
            applicationUser.IsDeleted = true;

            IdentityResult result = await _userManager.UpdateAsync(applicationUser);
            if (result.Succeeded) return Ok();
            return BadRequest(result.Errors);
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
