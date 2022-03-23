// <copyright file="PhoneApiController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models.Domain;
using RVCC.Models.SearchModel;
using RVCC.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RVCC.Controllers.Api
{
    [Authorize(Roles = Roles.AdminSecretarySocialAssistanceAuthorize)]
    [ApiController]
    public class PhoneApiController : Controller
    {
        private readonly IDataRepository<Phone> _phoneService;
        private readonly ILogger<PhoneApiController> _logger;

        public PhoneApiController(IDataRepository<Phone> phoneService, ILogger<PhoneApiController> logger)
        {
            _phoneService = phoneService;
            _logger = logger;
        }

        [HttpPost("~/api/phone/search")]
        public async Task<IActionResult> PhoneSearch([FromForm] SearchModel searchModel, [FromForm] PhoneSearchModel phoneSearch)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                List<Phone> phones = await _phoneService.GetAllAsync(null, sortColumn, sortDirection, phoneSearch);
                IEnumerable<PhoneViewModel> data = phones.Select(x => new PhoneViewModel
                {
                    Number = x.Number,
                    PhoneType = Enums.GetDisplayName(x.PhoneType),
                    ObservationNote = x.ObservationNote,
                    Actions = GetActionsHtml(x, User)
                }).Skip(skip).Take(take);

                int recordsTotal = phones.Count;

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered = recordsTotal });
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.ListItems, e, "Phone Search Error");
                return BadRequest();
            }
        }

        #region Private Methods

        private static string GetActionsHtml(Phone phone, ClaimsPrincipal user)
        {
            string options = $"<a href='/Phone/EditPhone/{phone.PhoneId}' data-toggle='modal' " +
                             "data-target='#modal-action' data-title='Editar Telefone' class='dropdown-item editPhoneButton'><span class='fas fa-edit'></span> Editar </a>";

            if (user.IsInRole(Roles.Admin) || user.IsInRole(Roles.Secretary))
            {
                options += $"<a href='javascript:void(0);' data-url='/Phone/DeletePhone' data-id='{phone.PhoneId}' class='dropdown-item deletePhoneButton'>" +
                           "<span class='fas fa-trash-alt'></span> Excluir </a>";
            }

            string actionsHtml =
                $@"<div class='dropdown'>
                  <button type='button' class='btn btn-info dropdown-toggle' data-toggle='dropdown'>Ações</button>
                  <div class='dropdown-menu'>
                      {options}
                  </div>
                </div>";

            return actionsHtml;
        }

        #endregion

    }
}
