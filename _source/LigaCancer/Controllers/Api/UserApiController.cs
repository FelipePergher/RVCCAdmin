using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Data.Requests;
using LigaCancer.Data.Store;
using LigaCancer.Models.UserViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
