using LigaCancer.Data.Models;
using LigaCancer.Models.SearchModel;
using LigaCancer.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Controllers.Api
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class UserApiController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserApiController> _logger;

        public UserApiController(ILogger<UserApiController> logger, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPost("~/api/user/search")]
        public IActionResult UserSearch([FromForm] SearchModel searchModel, [FromForm] string name)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                List<ApplicationUser> users = _userManager.Users.ToList();
                //Filter
                if (!string.IsNullOrEmpty(name)) users = users.Where(x => x.Name.ToLower().Contains(name)).ToList();

                IEnumerable<UserViewModel> data = users.Select(x => new UserViewModel
                {
                    UserId = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    ConfirmedEmail = _userManager.IsEmailConfirmedAsync(x).Result ? "<span class='fa fa-check'></span>" : "",
                    Lockout = x.LockoutEnd != null ? "<span class='fa fa-check'></span>" : "",
                    Role = _userManager.GetRolesAsync(x).Result.FirstOrDefault() == "Admin" ? "Administrador" : "Usuário",
                    Actions = GetActionsHtml(x)
                });

                //Sort
                if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection)) data = GetOrdenationUser(data, sortColumn, sortDirection);

                int recordsTotal = _userManager.Users.Count();
                int recordsFiltered = data.Count();

                return Ok(new { searchModel.Draw, data = data.Skip(skip).Take(take), recordsTotal, recordsFiltered });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "User Search Error", null);
                return BadRequest();
            }
        }

        [HttpGet("~/api/user/IsEmailUsed")]
        public async Task<IActionResult> IsEmailUsed(string Email, string UserId)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(Email);
            return user != null ? Ok(user.Id == UserId) : Ok(true);
        }

        #region Private Methods

        private string GetActionsHtml(ApplicationUser user)
        {
            if (user.NormalizedEmail == "FELIPEPERGHER_10@HOTMAIL.COM") return string.Empty;

            string editUser = $"<a href='/Admin/User/EditUser/{user.Id}' data-toggle='modal' " +
                $"data-target='#modal-action' data-title='Editar Usuário' class='dropdown-item editUserButton'><i class='fas fa-edit'></i> Editar </a>";

            string deleteUser = $"<a href='javascript:void(0);' data-url='/Admin/User/DeleteUser' data-id='{user.Id}' " +
                $" class='dropdown-item deleteUserButton'><i class='fas fa-trash-alt'></i> Excluir </a>";

            string unlockAccount = string.Empty;
            if (user.LockoutEnd != null)
            {
                unlockAccount = $"<a href='javascript:void(0);' data-url='/User/UnlockUser' data-id='{user.Id}' " +
                $" class='dropdown-item unlockUserButton'><i class='fas fa-unlock'></i> Desbloquear </a>";
            }

            string actionsHtml =
                $"<div class='dropdown'>" +
                $"  <button type='button' class='btn btn-info dropdown-toggle' data-toggle='dropdown'>Ações</button>" +
                $"  <div class='dropdown-menu'>" +
                $"      {editUser}" +
                $"      {unlockAccount}" +
                $"      {deleteUser}" +
                $"  </div>" +
                $"</div>";

            return actionsHtml;
        }

        private IEnumerable<UserViewModel> GetOrdenationUser(IEnumerable<UserViewModel> query, string sortColumn, string sortDirection)
        {
            switch (sortColumn)
            {
                case "Name":
                    return sortDirection == "asc" ? query.OrderBy(x => x.Name).ToList() : query.OrderByDescending(x => x.Name).ToList();
                case "Email":
                    return sortDirection == "asc" ? query.OrderBy(x => x.Email).ToList() : query.OrderByDescending(x => x.Email).ToList();
                case "ConfirmedEmail":
                    return sortDirection == "asc" ? query.OrderBy(x => x.ConfirmedEmail).ToList() : query.OrderByDescending(x => x.ConfirmedEmail).ToList();
                case "Lockout":
                    return sortDirection == "asc" ? query.OrderBy(x => x.Lockout).ToList() : query.OrderByDescending(x => x.Lockout).ToList();
                case "Role":
                    return sortDirection == "asc" ? query.OrderBy(x => x.Role).ToList() : query.OrderByDescending(x => x.Role).ToList();
                default:
                    return sortDirection == "asc" ? query.OrderBy(x => x.Name).ToList() : query.OrderByDescending(x => x.Name).ToList();
            }
        }

        #endregion
    }
}
