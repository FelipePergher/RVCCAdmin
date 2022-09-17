// <copyright file="AttendantApiController.cs" company="Doffs">
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
using System.Threading.Tasks;

namespace RVCC.Controllers.Api
{
    [Authorize(Roles = Roles.AdminSecretarySocialAssistanceAuthorize)]
    [ApiController]
    public class AttendantApiController : Controller
    {
        private readonly IDataRepository<Attendant> _attendantService;
        private readonly ILogger<AttendantApiController> _logger;

        public AttendantApiController(IDataRepository<Attendant> attendantService, ILogger<AttendantApiController> logger)
        {
            _attendantService = attendantService;
            _logger = logger;
        }

        [HttpPost("~/api/attendant/search")]
        public async Task<IActionResult> AttendantSearch([FromForm] SearchModel searchModel, [FromForm] AttendantSearchModel attendantSearch)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                IEnumerable<Attendant> attendants = await _attendantService.GetAllAsync(new[] { nameof(Attendant.VisitorAttendanceTypes) }, sortColumn, sortDirection, attendantSearch);
                IEnumerable<AttendantViewModel> data = attendants.Select(x => new AttendantViewModel
                {
                    Name = x.Name,
                    CPF = x.CPF,
                    Phone = x.Phone,
                    Note = x.Note,
                    Quantity = x.VisitorAttendanceTypes.Count(),
                    Actions = GetActionsHtml(x)
                }).Skip(skip).Take(take);

                int recordsTotal = _attendantService.Count();
                int recordsFiltered = attendants.Count();

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered });
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.ListItems, e, "Attendant Search Error");
                return BadRequest();
            }
        }

        #region Private Methods

        private static string GetActionsHtml(Attendant attendant)
        {
            string options = $"<a href='/Attendant/EditAttendant/{attendant.AttendantId}' data-toggle='modal' data-target='#modal-action' " +
                             "data-title='Editar Atendente' class='dropdown-item editAttendantButton'><span class='fas fa-edit'></span> Editar </a>";

            options += $@"<a href='javascript:void(0);' data-url='/Attendant/DeleteAttendant' data-id='{attendant.AttendantId}' 
                        data-relation='{attendant.VisitorAttendanceTypes.Count > 0}' class='dropdown-item deleteAttendantButton'>
                        <span class='fas fa-trash-alt'></span> Excluir </a>";

            string actionsHtml =
                "<div class='dropdown'>" +
                "  <button type='button' class='btn btn-info dropdown-toggle' data-toggle='dropdown'>Ações</button>" +
                "  <div class='dropdown-menu'>" +
                $"      {options}" +
                "  </div>" +
                "</div>";

            return actionsHtml;
        }

        #endregion
    }
}
