// <copyright file="AttendanceTypeApiController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models.Domain;
using RVCC.Data.Repositories;
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
    public class AttendanceTypeApiController : Controller
    {
        private readonly IDataRepository<AttendanceType> _attendanceTypeService;
        private readonly ILogger<AttendanceTypeApiController> _logger;

        public AttendanceTypeApiController(IDataRepository<AttendanceType> attendanceTypeService, ILogger<AttendanceTypeApiController> logger)
        {
            _attendanceTypeService = attendanceTypeService;
            _logger = logger;
        }

        [HttpPost("~/api/attendancetype/search")]
        public async Task<IActionResult> AttendanceTypeSearch([FromForm] SearchModel searchModel, [FromForm] AttendanceTypeSearchModel attendanceTypeSearch)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                IEnumerable<AttendanceType> attendanceTypes = await _attendanceTypeService.GetAllAsync(new[] { nameof(AttendanceType.VisitorAttendanceTypes) }, sortColumn, sortDirection, attendanceTypeSearch);
                IEnumerable<AttendanceTypeViewModel> data = attendanceTypes.Select(x => new AttendanceTypeViewModel
                {
                    Name = x.Name,
                    Quantity = x.VisitorAttendanceTypes.Count(),
                    Actions = GetActionsHtml(x, User)
                }).Skip(skip).Take(take);

                int recordsTotal = _attendanceTypeService.Count();
                int recordsFiltered = attendanceTypes.Count();

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered });
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.ListItems, e, "Attendance Type Search Error");
                return BadRequest();
            }
        }

        [HttpGet("~/api/AttendanceType/select2Get")]
        public async Task<IActionResult> Select2GetAttendanceTypes(string term)
        {
            IEnumerable<AttendanceType> attendanceTypes = await _attendanceTypeService.GetAllAsync(null, "Name", "asc", new AttendanceTypeSearchModel { Name = term });
            var select2PagedResult = new Select2PagedResult
            {
                Results = attendanceTypes.Select(x => new Select2Result
                {
                    Id = x.AttendanceTypeId.ToString(),
                    Text = x.Name
                }).ToList(),
            };

            return Ok(select2PagedResult);
        }

        [HttpGet("~/api/AttendanceType/IsNameExist")]
        public async Task<IActionResult> IsNameExist(string name, int attendanceTypeId)
        {
            AttendanceType attendanceType = await ((AttendanceTypeRepository)_attendanceTypeService).FindByNameAsync(name, attendanceTypeId);
            return Ok(attendanceType == null);
        }

        #region Private Methods

        private static string GetActionsHtml(AttendanceType attendanceType, ClaimsPrincipal user)
        {
            string options = $"<a href='/AttendanceType/EditAttendanceType/{attendanceType.AttendanceTypeId}' data-toggle='modal' data-target='#modal-action' " +
                "data-title='Editar Tipo de Atendimento' class='dropdown-item editAttendanceTypeButton'><span class='fas fa-edit'></span> Editar </a>";

            options += $"<a href='javascript:void(0);' data-url='/AttendanceType/DeleteAttendanceType' data-id='{attendanceType.AttendanceTypeId}' class='dropdown-item deleteAttendanceTypeButton'>" +
                         "<span class='fas fa-trash-alt'></span> Excluir </a>";

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
