using LigaCancer.Data.Models;
using LigaCancer.Models.FormModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
                    Name = userForm.Name,
                    UserName = userForm.Email,
                    Email = userForm.Email,
                    CreatedBy = User.Identity.Name
                };

                IdentityResult result = await _userManager.CreateAsync(user, userForm.Password);

                if (result.Succeeded)
                {
                    IdentityRole applicationRole = await _roleManager.FindByNameAsync(userForm.Role);
                    if (applicationRole != null) await _userManager.AddToRoleAsync(user, applicationRole.Name);
                    return Ok();
                }
                return BadRequest(result.Errors);
            }

            return PartialView("Partials/_AddUser", userForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();
            
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            
            if (user == null) return NotFound();

            EditUserFormModel userForm = new EditUserFormModel(user.Id, user.Name);

            //Todo change to use await
            IList<string> roles = await _userManager.GetRolesAsync(user);
            string userRole = roles.Any() ? roles.FirstOrDefault() : string.Empty;
            if (!string.IsNullOrEmpty(userRole)) userForm.Role = userRole;

            return PartialView("Partials/_EditUser", userForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(string id, EditUserFormModel userForm)
        {
            if(string.IsNullOrEmpty(id)) return BadRequest();
            
            ApplicationUser user = await _userManager.FindByIdAsync(id);

            if (user == null) return NotFound();

            if (ModelState.IsValid)
            {
                user.Name = userForm.Name;

                IdentityResult result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    IList<string> userRoles = await _userManager.GetRolesAsync(user);
                    string userRole = userRoles.FirstOrDefault();
                    if (userRole != null && userRole != userForm.Role) { 
                        await _userManager.RemoveFromRoleAsync(user, userRole);
                    }

                    IdentityRole role = await _roleManager.FindByNameAsync(userForm.Role);
                    if (role != null && role.Name != userRole) await _userManager.AddToRoleAsync(user, role.Name);

                    return Ok();
                }
            }

            return PartialView("Partials/_EditUser", userForm);
        }

        [HttpPost, IgnoreAntiforgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            ApplicationUser applicationUser = await _userManager.FindByIdAsync(id);

            if (applicationUser == null) return NotFound();

            IdentityResult result = await _userManager.DeleteAsync(applicationUser);

            if (result.Succeeded) return Ok();
            return BadRequest(result.Errors);
        }
    }
}
