using System.Threading.Tasks;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using LigaCancer.Models.SearchViewModel;
using LigaCancer.Models.ViewModel;
using System.Linq;

namespace LigaCancer.Controllers.Api
{
    [Authorize(Roles = "Admin"), ApiController]
    public class PresenceApiController : Controller
    {
        private readonly IDataStore<Presence> _presenceService;

        public PresenceApiController(IDataStore<Presence> presenceService)
        {
            _presenceService = presenceService;
        }

        [HttpPost("~/api/presence/search")]
        public async Task<IActionResult> PresenceSearch([FromForm] SearchViewModel model)
        {
            try
            {
                string sortColumn = model.Columns[model.Order[0].Column].Name;

                // Sort Column Direction ( asc ,desc)  
                string sortColumnDirection = model.Order[0].Dir;
                //Paging Size (10,20,50,100)  
                int take = model.Length != null ? int.Parse(model.Length) : 0;
                int skip = model.Start != null ? int.Parse(model.Start) : 0;

                IEnumerable<Presence> presences = await _presenceService.GetAllAsync(new string[] { "Patient" }, take, skip);
                IEnumerable<PresenceViewModel> data = presences.Select(x => new PresenceViewModel
                {
                    Patient = x.Patient.FirstName,
                    Date = x.PresenceDateTime.ToString("dd/mm/yyyy"),
                    Hour = x.PresenceDateTime.ToString("HH:mm"),
                    Actions = GetActionsHtml(x)
                });

                int recordsTotal = _presenceService.Count();
                int recordsFiltered = _presenceService.Count();

                return Ok(new { model.Draw, data, recordsTotal, recordsFiltered });
            }
            catch
            {
                return BadRequest();
            }
        }

        #region Private Methods

        private string GetActionsHtml(Presence presence)
        {
            string actionsHtml = $"<a href='/Presence/EditPresence/{presence.PresenceId}' data-toggle='modal' data-target='#modal-action' class='btn btn-secondary'><i class='fas fa-edit'></i> Editar </a>";

            actionsHtml += $"<a href='/Presence/DeletePresence/{presence.PresenceId}' data-toggle='modal' data-target='#modal-action' class='btn btn-danger ml-1'><i class='fas fa-trash-alt'></i> Excluir </a>";

            return actionsHtml;
        }

        #endregion
    }
}
