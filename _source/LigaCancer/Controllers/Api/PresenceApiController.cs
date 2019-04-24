using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.SearchModel;
using LigaCancer.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IActionResult> PresenceSearch([FromForm] SearchModel searchModel, [FromForm] PresenceSearchModel presenceSearchModel )
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                IEnumerable<Presence> presences = await _presenceService.GetAllAsync(new string[] { "Patient" }, sortColumn, sortDirection, presenceSearchModel);
                IEnumerable<PresenceViewModel> data = presences.Select(x => new PresenceViewModel
                {
                    //Patient = x.Patient.FirstName + " " + x.Patient.Surname,
                    Date = x.PresenceDateTime.ToString("dd/MM/yyyy"),
                    Hour = x.PresenceDateTime.ToString("HH:mm"),
                    Actions = GetActionsHtml(x)
                }).Skip(skip).Take(take);

                int recordsTotal = _presenceService.Count();
                int recordsFiltered = presences.Count();

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered });
            }
            catch
            {
                return BadRequest();
            }
        }

        #region Private Methods

        private string GetActionsHtml(Presence presence)
        {
            string actionsHtml = $"<a href='/Presence/EditPresence/{presence.PresenceId}' data-toggle='modal' data-target='#modal-action' class='btn btn-secondary editPresenceButton'><i class='fas fa-edit'></i> Editar </a>";

            actionsHtml += $"<a href='javascript:void(0);' data-url='/Presence/DeletePresence/{presence.PresenceId}' class='btn btn-danger ml-1 deletePresenceButton'><i class='fas fa-trash-alt'></i> Excluir </a>";

            return actionsHtml;
        }

        #endregion
    }
}
