// <copyright file="UserApiController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Data.Models;
using RVCC.Models.SearchModel;
using RVCC.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Controllers.Api
{
    [Authorize(Roles = Roles.Admin)]
    [ApiController]
    public class UserApiController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserApiController> _logger;
        private readonly IConfiguration _configuration;

        public UserApiController(ILogger<UserApiController> logger, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
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

                string adminEmail = _configuration["Admin:Email"];

                var users = _userManager.Users.Where(x => x.Email.ToLower() != adminEmail.ToLower()).ToList();

                // Remove admin user
                int adminUserCount = _userManager.Users.Count(x => x.Email.ToLower() == adminEmail.ToLower());
                if (adminUserCount > 0)
                {
                    users = users.Where(x => x.Email.ToLower() != adminEmail.ToLower()).ToList();
                }

                // Filter
                if (!string.IsNullOrEmpty(name))
                {
                    users = users.Where(x => x.Name.ToLower().Contains(name)).ToList();
                }

                IEnumerable<UserViewModel> data = users.Select(x => new UserViewModel
                {
                    UserId = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    ConfirmedEmail = _userManager.IsEmailConfirmedAsync(x).Result ? "<span class='fa fa-check'></span>" : string.Empty,
                    Lockout = x.LockoutEnd != null ? "<span class='fa fa-check'></span>" : string.Empty,
                    Role = Roles.GetRoleName(_userManager.GetRolesAsync(x).Result.FirstOrDefault()),
                    Actions = GetActionsHtml(x)
                }).ToList();

                // Sort
                if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
                {
                    data = GetOrdinationUser(data, sortColumn, sortDirection);
                }

                int recordsTotal = _userManager.Users.Count() - adminUserCount;
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
        public async Task<IActionResult> IsEmailUsed(string email, string userId)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            return user != null ? Ok(user.Id == userId) : Ok(true);
        }

        #region Private Methods

        private string GetActionsHtml(ApplicationUser user)
        {
            string editUser = $"<a href='/User/EditUser/{user.Id}' data-toggle='modal' " +
                $"data-target='#modal-action' data-title='Editar Usuário' class='dropdown-item editUserButton'><span class='fas fa-edit'></span> Editar </a>";

            string deleteUser = $"<a href='javascript:void(0);' data-url='/User/DeleteUser' data-id='{user.Id}' " +
                $" class='dropdown-item deleteUserButton'><span class='fas fa-trash-alt'></span> Excluir </a>";

            string unlockAccount = string.Empty;
            if (user.LockoutEnd != null)
            {
                unlockAccount = $"<a href='javascript:void(0);' data-url='/User/UnlockUser' data-id='{user.Id}' " +
                $" class='dropdown-item unlockUserButton'><span class='fas fa-unlock'></span> Desbloquear </a>";
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

        private IEnumerable<UserViewModel> GetOrdinationUser(IEnumerable<UserViewModel> query, string sortColumn, string sortDirection)
        {
            return sortColumn switch
            {
                "Name" => sortDirection == "asc"
                    ? query.OrderBy(x => x.Name).ToList()
                    : query.OrderByDescending(x => x.Name).ToList(),
                "Email" => sortDirection == "asc"
                    ? query.OrderBy(x => x.Email).ToList()
                    : query.OrderByDescending(x => x.Email).ToList(),
                "ConfirmedEmail" => sortDirection == "asc"
                    ? query.OrderBy(x => x.ConfirmedEmail).ToList()
                    : query.OrderByDescending(x => x.ConfirmedEmail).ToList(),
                "Lockout" => sortDirection == "asc"
                    ? query.OrderBy(x => x.Lockout).ToList()
                    : query.OrderByDescending(x => x.Lockout).ToList(),
                "Role" => sortDirection == "asc"
                    ? query.OrderBy(x => x.Role).ToList()
                    : query.OrderByDescending(x => x.Role).ToList(),
                _ => sortDirection == "asc"
                    ? query.OrderBy(x => x.Name).ToList()
                    : query.OrderByDescending(x => x.Name).ToList()
            };
        }

        #endregion
    }
}
