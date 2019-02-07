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
        public async Task<IActionResult> MedicineSearch([FromForm] SearchModel searchModel)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                IEnumerable<Medicine> medicines = await _medicineService.GetAllAsync(new string[] { "PatientInformationMedicines" }, sortColumn, sortDirection);
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
            string actionsHtml = "";

            //string actionsHtml = $"<a href='/TreatmentPlace/EditTreatmentPlace/{treatmentPlace.TreatmentPlaceId}' data-toggle='modal' data-target='#modal-action' class='btn btn-secondary editTreatmentPlaceButton'><i class='fas fa-edit'></i> Editar </a>";

            //actionsHtml += $"<a href='javascript:void(0);' data-url='/TreatmentPlace/DeleteTreatmentPlace/{treatmentPlace.TreatmentPlaceId}' class='btn btn-danger ml-1 deleteTreatmentPlaceButton'><i class='fas fa-trash-alt'></i> Excluir </a>";

             //let render = '<a href="/Medicine/EditMedicine/' + row.medicineId + '" data-toggle="modal" data-target="#modal-action"' +
             //           ' class="btn btn-secondary"><i class="fas fa-edit"></i> Editar </a>';

             //       if (row.patientInformationMedicines.length === 0) {
             //           render = render.concat(
             //               '<a href="/Medicine/DeleteMedicine/' + row.medicineId + '" data-toggle="modal" data-target="#modal-action"' +
             //               ' class="btn btn-danger ml-1"><i class="fas fa-trash-alt"></i> Excluir </a>'
             //           );
             //       } else {
             //           render = render.concat(
             //               '<a class="btn btn-danger ml-1 disabled"><i class="fas fa-trash-alt"></i>  Excluir </a>'
             //           );
             //       }

            return actionsHtml;
        }

        #endregion
    }
}
