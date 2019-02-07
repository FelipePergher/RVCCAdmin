using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.SearchModel;
using LigaCancer.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LigaCancer.Controllers.Api
{
    [Authorize(Roles = "Admin"), ApiController]
    public class CancerTypeApiController : Controller
    {
        private readonly IDataStore<CancerType> _cancerService;

        public CancerTypeApiController(IDataStore<CancerType> cancerService)
        {
            _cancerService = cancerService;
        }

        [HttpPost("~/api/CancerType/search")]
        public async Task<IActionResult> CancerTypeSearch([FromForm] SearchModel searchModel)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                IEnumerable<CancerType> cancerTypes = await _cancerService.GetAllAsync(new string[] { "PatientInformationCancerTypes" }, sortColumn, sortDirection);
                IEnumerable<CancerTypeViewModel> data = cancerTypes.Select(x => new CancerTypeViewModel
                {
                    Name = x.Name,
                    Actions = GetActionsHtml(x)
                }).Skip(skip).Take(take);

                int recordsTotal = _cancerService.Count();
                int recordsFiltered = cancerTypes.Count();

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered });
            }
            catch
            {
                return BadRequest();
            }
        }

        #region Private Methods

        private string GetActionsHtml(CancerType cancerType)
        {
            string actionsHtml = "";
            //string actionsHtml = $"<a href='/TreatmentPlace/EditTreatmentPlace/{treatmentPlace.TreatmentPlaceId}' data-toggle='modal' data-target='#modal-action' class='btn btn-secondary editTreatmentPlaceButton'><i class='fas fa-edit'></i> Editar </a>";

            //actionsHtml += $"<a href='javascript:void(0);' data-url='/TreatmentPlace/DeleteTreatmentPlace/{treatmentPlace.TreatmentPlaceId}' class='btn btn-danger ml-1 deleteTreatmentPlaceButton'><i class='fas fa-trash-alt'></i> Excluir </a>";

            //if (row.patientInformationCancerTypes.length === 0)
            //{
            //    render = render.concat(
            //        '<a href="/CancerType/DeleteCancerType/' + row.cancerTypeId + '" data-toggle="modal" data-target="#modal-action"' +
            //        ' class="btn btn-danger ml-1"><i class="fas fa-trash-alt"></i> Excluir </a>'
            //    );
            //}
            //else
            //{
            //    render = render.concat(
            //        '<a class="btn btn-danger ml-1 disabled"><i class="fas fa-trash-alt"></i> Excluir </a>'
            //    );
            //}

            return actionsHtml;
        }

        #endregion
    }
}
