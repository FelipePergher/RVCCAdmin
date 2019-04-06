using System.Threading.Tasks;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LigaCancer.Models.SearchModel;
using System.Collections.Generic;
using System.Linq;
using LigaCancer.Models.ViewModel;

namespace LigaCancer.Controllers.Api
{
    [Authorize(Roles = "Admin"), ApiController]
    public class MedicineApiController : Controller
    {
        private readonly IDataStore<Medicine> _medicineService;

        public MedicineApiController(IDataStore<Medicine> medicineService)
        {
            _medicineService = medicineService;
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
            catch
            {
                return BadRequest();
            }
        }

        #region Private Methods

        private string GetActionsHtml(Medicine medicine)
        {
            string editMedicine = $"<a href='/Medicine/EditMedicine/{medicine.MedicineId}' data-toggle='modal' " +
                $"data-target='#modal-action' class='dropdown-item editMedicineButton'><i class='fas fa-edit'></i> Editar </a>";

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
