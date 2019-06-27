using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.SearchModel;
using LigaCancer.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Controllers.Api
{
    [Authorize(Roles = "Admin, User")]
    [ApiController]
    public class PresenceApiController : Controller
    {
        private readonly IDataStore<Presence> _presenceService;
        private readonly ILogger<PresenceApiController> _logger;

        public PresenceApiController(IDataStore<Presence> presenceService, ILogger<PresenceApiController> logger)
        {
            _presenceService = presenceService;
            _logger = logger;
        }

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

                int recordsTotal = _presenceService.Count();
                int recordsFiltered = presences.Count();

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Presence Search Error", null);
                return BadRequest();
            }
        }

        #region Private Methods

        private string GetActionsHtml(Presence presence)
        {
            string editPresence = $"<a href='/Admin/Presence/EditPresence/{presence.PresenceId}' data-toggle='modal' data-target='#modal-action' " +
               $"data-title='Editar Presença' class='dropdown-item editPresenceButton'><i class='fas fa-edit'></i> Editar </a>";
            string deletePresence = $"<a href='javascript:void(0);' data-url='/Admin/Presence/DeletePresence' data-id='{presence.PresenceId}' " +
                $"class='deletePresenceButton dropdown-item'><i class='fas fa-trash-alt'></i> Excluir </a>";

            TimeSpan diff = DateTime.Now.Subtract(presence.RegisterDate);
            if (diff.Days > 0) editPresence = string.Empty;

            string actionsHtml =
                $"<div class='dropdown'>" +
                $"  <button type='button' class='btn btn-info dropdown-toggle' data-toggle='dropdown'>Ações</button>" +
                $"  <div class='dropdown-menu'>" +
                $"      {editPresence}" +
                $"      {deletePresence}" +
                $"  </div>" +
                $"</div>";

            return actionsHtml;
        }

        #endregion
    }
}
