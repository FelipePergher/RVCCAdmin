using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Data.Store;
using LigaCancer.Models.SearchModel;
using LigaCancer.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Controllers.Api
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class CancerTypeApiController : Controller
    {
        private readonly IDataStore<CancerType> _cancerTypeService;

        public CancerTypeApiController(IDataStore<CancerType> cancerTypeService)
        {
            _cancerTypeService = cancerTypeService;
        }

        [HttpPost("~/api/CancerType/search")]
        public async Task<IActionResult> CancerTypeSearch([FromForm] SearchModel searchModel, [FromForm] CancerTypeSearchModel cancerTypeSearch)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                IEnumerable<CancerType> cancerTypes = await _cancerTypeService.GetAllAsync(new string[] { "PatientInformationCancerTypes" }, sortColumn, sortDirection, cancerTypeSearch);
                IEnumerable<CancerTypeViewModel> data = cancerTypes.Select(x => new CancerTypeViewModel
                {
                    Name = x.Name,
                    Actions = GetActionsHtml(x)
                }).Skip(skip).Take(take);

                int recordsTotal = _cancerTypeService.Count();
                int recordsFiltered = cancerTypes.Count();

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("~/api/CancerType/select2Get")]
        public async Task<IActionResult> Select2GetCancerTypes(string term)
        {
            IEnumerable<CancerType> cancerTypes = await _cancerTypeService.GetAllAsync(null, "FirstName", "asc", new CancerTypeSearchModel{ Name = term });
            Select2PagedResult select2PagedResult = new Select2PagedResult
            {
                Results = cancerTypes.Select(x => new Result
                {
                    Id = x.CancerTypeId.ToString(),
                    Text = x.Name
                }).ToList(),
            };

            return Ok(select2PagedResult);
        }

        [HttpGet("~/api/CancerType/IsNameExist")]
        public async Task<IActionResult> IsNameExist(string name, int cancerTypeId)
        {
            CancerType cancerType = await ((CancerTypeStore)_cancerTypeService).FindByNameAsync(name, cancerTypeId);
            return Ok(cancerType == null);
        }

        #region Private Methods

        private string GetActionsHtml(CancerType cancerType)
        {
            string editCancerType = $"<a href='/CancerType/EditCancerType/{cancerType.CancerTypeId}' data-toggle='modal' data-target='#modal-action' " +
                $"data-title='Editar Tipo de Câncer' class='dropdown-item editCancerTypeButton'><i class='fas fa-edit'></i> Editar </a>";

            string deleteCancerType = $"<a href='javascript:void(0);' data-url='/CancerType/DeleteCancerType' data-id='{cancerType.CancerTypeId}' " +
                $"data-relation='{cancerType.PatientInformationCancerTypes.Count > 0}' class='dropdown-item deleteCancerTypeButton'>" +
                $"<i class='fas fa-trash-alt'></i> Excluir </a>";

            string actionsHtml =
                $"<div class='dropdown'>" +
                $"  <button type='button' class='btn btn-info dropdown-toggle' data-toggle='dropdown'>Ações</button>" +
                $"  <div class='dropdown-menu'>" +
                $"      {editCancerType}" +
                $"      {deleteCancerType}" +
                $"  </div>" +
                $"</div>";

            return actionsHtml;
        }

        #endregion
    }
}
