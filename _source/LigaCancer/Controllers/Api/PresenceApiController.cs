using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models;
using RVCC.Data.Repositories;
using RVCC.Models.SearchModel;
using RVCC.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Controllers.Api
{
    [ApiController]
    public class PresenceApiController : Controller
    {
        private readonly IDataRepository<Presence> _presenceService;
        private readonly ILogger<PresenceApiController> _logger;

        public PresenceApiController(IDataRepository<Presence> presenceService, ILogger<PresenceApiController> logger)
        {
            _presenceService = presenceService;
            _logger = logger;
        }

        [Authorize(Roles = Roles.AdminUserSocialAssistanceAuthorize)]
        [HttpPost("~/api/presence/search")]
        public async Task<IActionResult> PresenceSearch([FromForm] SearchModel searchModel, [FromForm] PresenceSearchModel presenceSearchModel)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                IEnumerable<Presence> presences = await _presenceService.GetAllAsync(null, sortColumn, sortDirection, presenceSearchModel);
                IEnumerable<PresenceViewModel> data = presences.Select(x => new PresenceViewModel
                {
                    Patient = x.Name,
                    Date = x.PresenceDateTime.ToString("dd/MM/yyyy"),
                    Hour = x.PresenceDateTime.ToString("HH:mm"),
                    Actions = GetActionsHtml(x)
                }).Skip(skip).Take(take);

                int recordsTotal = string.IsNullOrEmpty(presenceSearchModel.PatientId) 
                    ? _presenceService.Count() 
                    : ((PresenceRepository)_presenceService).CountByPatient(int.Parse(presenceSearchModel.PatientId));

                int recordsFiltered = presences.Count();

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Presence Search Error", null);
                return BadRequest();
            }
        }

        [Authorize(Roles = Roles.AdminUserSocialAssistanceAuthorize)]
        [HttpPost("~/api/presence/getChartData")]
        public IActionResult GetChartData([FromForm] HomeViewModel home)
        {
            var dateTime = DateTime.Parse(home.ChartDate);
            List<int> dayChartDate = ((PresenceRepository)_presenceService).GetDayChartData(dateTime);
            List<int> monthChartDate = ((PresenceRepository)_presenceService).GetMonthChartData(dateTime);
            List<int> yearChartDate = ((PresenceRepository)_presenceService).GetYearChartData(dateTime);

            int daysInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);

            return Ok(new { dayChartDate, monthChartDate, yearChartDate, daysInMonth });
        }

        #region Private Methods

        private string GetActionsHtml(Presence presence)
        {
            string editPresence = string.Empty;
            string deletePresence = string.Empty;
            if (!User.IsInRole(Roles.SocialAssistance))
            {
                editPresence = $"<a href='/Presence/EditPresence/{presence.PresenceId}' data-toggle='modal' data-target='#modal-action' " +
                   $"data-title='Editar Presença' class='dropdown-item editPresenceButton'><i class='fas fa-edit'></i> Editar </a>";
                deletePresence = $"<a href='javascript:void(0);' data-url='/Presence/DeletePresence' data-id='{presence.PresenceId}' " +
                    $"class='deletePresenceButton dropdown-item'><i class='fas fa-trash-alt'></i> Excluir </a>";

                TimeSpan diff = DateTime.Now.Subtract(presence.RegisterTime);
                if (diff.Days > 0)
                {
                    editPresence = string.Empty;
                }
            }

            string actionsHtml =
                $@"<div class='dropdown'>
                  <button type='button' class='btn btn-info dropdown-toggle' data-toggle='dropdown'>Ações</button>
                  <div class='dropdown-menu'>
                      {editPresence}
                      {deletePresence}
                  </div>
                </div>";

            return actionsHtml;
        }

        #endregion
    }
}
