// <copyright file="VisitorAttendanceTypeApiController.cs" company="Doffs">
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
using System.Threading.Tasks;

namespace RVCC.Controllers.Api
{
    [Authorize(Roles = Roles.AdminSecretarySocialAssistanceAuthorize)]
    [ApiController]
    public class VisitorAttendanceTypeApiController : Controller
    {
        private readonly IDataRepository<VisitorAttendanceType> _visitorAttendanceTypeService;
        private readonly ILogger<VisitorAttendanceTypeApiController> _logger;

        public VisitorAttendanceTypeApiController(IDataRepository<VisitorAttendanceType> visitorAttendanceTypeService, ILogger<VisitorAttendanceTypeApiController> logger)
        {
            _visitorAttendanceTypeService = visitorAttendanceTypeService;
            _logger = logger;
        }

        [HttpPost("~/api/visitorAttendanceType/search")]
        public async Task<IActionResult> VisitorAttendanceTypeSearch([FromForm] SearchModel searchModel, [FromForm] VisitorAttendanceTypeSearchModel visitorAttendanceTypeSearchModel)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                IEnumerable<VisitorAttendanceType> visitorAttendances = await _visitorAttendanceTypeService.GetAllAsync(new[] { nameof(VisitorAttendanceType.Attendant), nameof(VisitorAttendanceType.Visitor), nameof(VisitorAttendanceType.AttendanceType) }, sortColumn, sortDirection, visitorAttendanceTypeSearchModel);
                IEnumerable<VisitorAttendanceViewModel> data = visitorAttendances.Select(x => new VisitorAttendanceViewModel
                {
                    Visitor = x.Visitor.Name,
                    Attendant = x.Attendant.Name,
                    Date = x.AttendanceDate.ToDateString(),
                    AttendanceType = x.AttendanceType.Name,
                    Observation = x.Observation,
                    Actions = GetActionsHtml(x)
                }).Skip(skip).Take(take);

                int recordsTotal = string.IsNullOrEmpty(visitorAttendanceTypeSearchModel.VisitorId)
                    ? _visitorAttendanceTypeService.Count()
                    : ((VisitorAttendanceTypeRepository)_visitorAttendanceTypeService).CountByVisitor(int.Parse(visitorAttendanceTypeSearchModel.VisitorId));
                int recordsFiltered = visitorAttendances.Count();

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered });
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.ListItems, e, "PatientBenefit Search Error");
                return BadRequest();
            }
        }

        [HttpPost("~/api/visitorAttendanceType/getChartData")]
        public IActionResult GetChartData([FromForm] HomeViewModel home)
        {
            var dateTime = DateTime.Parse(home.ChartDate);
            List<int> monthChartDate = ((VisitorAttendanceTypeRepository)_visitorAttendanceTypeService).GetMonthChartData(dateTime);
            List<int> yearChartDate = ((VisitorAttendanceTypeRepository)_visitorAttendanceTypeService).GetYearChartData(dateTime);

            int daysInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);

            return Ok(new { monthChartDate, yearChartDate, daysInMonth });
        }

        #region Private Methods

        private static string GetActionsHtml(VisitorAttendanceType visitorAttendanceType)
        {
            string options = $"<a href='/VisitorAttendanceType/EditVisitorAttendanceType/{visitorAttendanceType.VisitorAttendanceTypeId}' data-toggle='modal' data-target='#modal-action' " +
                                                  "data-title='Editar Atendimento visitante ' class='dropdown-item editVisitorAttendanceTypeButton'><span class='fas fa-edit'></span> Editar </a>";

            options += $@"<a href='javascript:void(0);' data-url='/VisitorAttendanceType/DeleteVisitorAttendanceType' data-visitorAttendanceTypeId='{visitorAttendanceType.VisitorAttendanceTypeId}' 
                    class='deleteVisitorAttendanceTypeButton dropdown-item'><span class='fas fa-trash-alt'></span> Excluir </a>";

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
