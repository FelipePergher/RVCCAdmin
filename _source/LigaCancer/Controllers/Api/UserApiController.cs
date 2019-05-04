using LigaCancer.Data.Models;
using LigaCancer.Models.SearchModel;
using LigaCancer.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace LigaCancer.Controllers.Api
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class UserApiController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserApiController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("~/api/user/search")]
        public IActionResult UserSearch([FromForm] SearchModel searchModel)
        {
            List<ApplicationUser> users = _userManager.Users.Where(x => !x.IsDeleted).ToList();

            IEnumerable<UserViewModel> data = users.Select(x => new UserViewModel
            {
                UserId = x.Id,
                Name = $"{x.FirstName} {x.LastName}",
                Email = x.Email,
                Role = _userManager.GetRolesAsync(x).Result.FirstOrDefault() == "User" ? "Usuário" : "Administrador"
            });


            return Ok(new { data });
        }
    }
}
