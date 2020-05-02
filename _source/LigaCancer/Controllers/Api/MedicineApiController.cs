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
    [Authorize(Roles = Roles.AdminUserAuthorize)]
    [ApiController]
    public class MedicineApiController : Controller
    {
        private readonly IDataRepository<Medicine> _medicineService;
        private readonly ILogger<MedicineApiController> _logger;

        public MedicineApiController(IDataRepository<Medicine> medicineService, ILogger<MedicineApiController> logger)
        {
            _medicineService = medicineService;
            _logger = logger;
        }

        [HttpPost("~/api/Medicine/search")]
        public async Task<IActionResult> MedicineSearch([FromForm] SearchModel searchModel, [FromForm] MedicineSearchModel medicineSearch)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                IEnumerable<Medicine> medicines = await _medicineService.GetAllAsync(new string[] { "PatientInformationMedicines" }, sortColumn, sortDirection, medicineSearch);
                IEnumerable<MedicineViewModel> data = medicines.Select(x => new MedicineViewModel
                {
                    Name = x.Name,
                    Actions = GetActionsHtml(x)
                }).Skip(skip).Take(take);

                int recordsTotal = _medicineService.Count();
                int recordsFiltered = medicines.Count();

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Medicine Search Error", null);
                return BadRequest();
            }
        }

        [HttpGet("~/api/Medicine/select2Get")]
        public async Task<IActionResult> Select2GetMedicines(string term)
        {
            IEnumerable<Medicine> medicines = await _medicineService.GetAllAsync(null, "Name", "asc", new MedicineSearchModel { Name = term });
            var select2PagedResult = new Select2PagedResult
            {
                Results = medicines.Select(x => new Result
                {
                    Id = x.MedicineId.ToString(),
                    Text = x.Name
                }).ToList(),
            };

            return Ok(select2PagedResult);
        }

        [HttpGet("~/api/Medicine/IsNameExist")]
        public async Task<IActionResult> IsNameExist(string name, int medicineId)
        {
            Medicine medicine = await ((MedicineRepository)_medicineService).FindByNameAsync(name, medicineId);

            return Ok(medicine == null);
        }

        #region Private Methods

        private string GetActionsHtml(Medicine medicine)
        {
            string editMedicine = $"<a href='/Medicine/EditMedicine/{medicine.MedicineId}' data-toggle='modal' " +
                $"data-target='#modal-action' data-title='Editar Remédio' class='dropdown-item editMedicineButton'><i class='fas fa-edit'></i> Editar </a>";

            string deleteMedicine = $"<a href='javascript:void(0);' data-url='/Medicine/DeleteMedicine' data-id='{medicine.MedicineId}' " +
                $"data-relation='{medicine.PatientInformationMedicines.Count > 0}' class='dropdown-item deleteMedicineButton'>" +
                $"<i class='fas fa-trash-alt'></i> Excluir </a>";

            string actionsHtml =
                $"<div class='dropdown'>" +
                $"  <button type='button' class='btn btn-info dropdown-toggle' data-toggle='dropdown'>Ações</button>" +
                $"  <div class='dropdown-menu'>" +
                $"      {editMedicine}" +
                $"      {deleteMedicine}" +
                $"  </div>" +
                $"</div>";

            return actionsHtml;
        }

        #endregion
    }
}
