using System.Collections.Generic;
using System.Linq;
using LigaCancer.Data.Models;
using LigaCancer.Models.UserViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LigaCancer.Controllers.Api
{
    [Authorize(Roles = "Admin"), Route("api/[action]")]
    public class UserApiController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserApiController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult GetUsersDataTableResponseAsync()
        {
            try
            {
                var users = _userManager.Users.Where(x => !x.IsDeleted).ToList();

                List<UserListViewModel> data = users.Select(x => new UserListViewModel
                {
                    UserId = x.Id,
                    FirstName = x.FirstName ?? "",
                    LastName = x.LastName ?? "",
                    Email = x.Email ?? "",
                    Role = _userManager.GetRolesAsync(x).Result.FirstOrDefault() == "User" ? "Usuário" : "Administrador"
                }).ToList();

                return Ok(new { data });
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
