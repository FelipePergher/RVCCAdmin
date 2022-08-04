// <copyright file="VisitorAttendanceApiController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models.RelationModels;
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
    public class VisitorAttendanceApiController : Controller
    {
        private readonly IDataRepository<VisitorAttendance> _visitorAttendanceService;
        private readonly ILogger<VisitorAttendanceApiController> _logger;

        public VisitorAttendanceApiController(IDataRepository<VisitorAttendance> visitorAttendanceService, ILogger<VisitorAttendanceApiController> logger)
        {
            _visitorAttendanceService = visitorAttendanceService;
            _logger = logger;
        }

        [HttpPost("~/api/visitorAttendance/search")]
        public async Task<IActionResult> VisitorAttendanceSearch([FromForm] SearchModel searchModel, [FromForm] VisitorAttendanceSearchModel visitorAttendanceSearchModel)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                IEnumerable<VisitorAttendance> visitorAttendances = await _visitorAttendanceService.GetAllAsync(new[] { nameof(VisitorAttendance.Visitor), nameof(VisitorAttendance.AttendanceType) }, sortColumn, sortDirection, visitorAttendanceSearchModel);
                IEnumerable<VisitorAttendanceViewModel> data = visitorAttendances.Select(x => new VisitorAttendanceViewModel
                {
                    Visitor = $"{x.Visitor.Name}",
                    Date = x.AttendanceDate.ToString("dd/MM/yyyy"),
                    AttendanceType = x.AttendanceType.Name,
                    Observation = x.Observation,
                    Actions = GetActionsHtml(x, User)
                }).Skip(skip).Take(take);

                int recordsTotal = string.IsNullOrEmpty(visitorAttendanceSearchModel.VisitorId)
                    ? _visitorAttendanceService.Count()
                    : ((VisitorAttendanceRepository)_visitorAttendanceService).CountByVisitor(int.Parse(visitorAttendanceSearchModel.VisitorId));
                int recordsFiltered = visitorAttendances.Count();

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered });
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.ListItems, e, "PatientBenefit Search Error");
                return BadRequest();
            }
        }

        #region Private Methods

        private static string GetActionsHtml(VisitorAttendance visitorAttendance, ClaimsPrincipal user)
        {
            string options = $"<a href='/VisitorAttendance/EditVisitorAttendance/{visitorAttendance.VisitorAttendanceTypeId}' data-toggle='modal' data-target='#modal-action' " +
                                                  "data-title='Editar Atendimento visitante ' class='dropdown-item editVisitorAttendanceButton'><span class='fas fa-edit'></span> Editar </a>";

            if (user.IsInRole(Roles.Admin) || user.IsInRole(Roles.Secretary))
            {
                options += $@"<a href='javascript:void(0);' data-url='/VisitorAttendance/DeleteVisitorAttendance' data-visitorAttendanceId='{visitorAttendance.VisitorAttendanceTypeId}' 
                    class='deleteVisitorAttendanceButton dropdown-item'><span class='fas fa-trash-alt'></span> Excluir </a>";
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
