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
    public class DoctorApiController : Controller
    {
        private readonly IDataStore<Doctor> _doctorService;

        public DoctorApiController(IDataStore<Doctor> doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpPost("~/api/doctor/search")]
        public async Task<IActionResult> DoctorSearch([FromForm] SearchViewModel searchModel)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                IEnumerable<Doctor> doctors = await _doctorService.GetAllAsync(new string[] { "PatientInformationDoctors" }, sortColumn, sortDirection);
                IEnumerable<DoctorViewModel> data = doctors.Select(x => new DoctorViewModel {
                    Name = x.Name,
                    CRM = x.CRM,
                    Actions = GetActionsHtml(x)
                });

                int recordsTotal = _doctorService.Count();
                int recordsFiltered = _doctorService.Count();

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered });
            }
            catch
            {
                return BadRequest();
            }
        }

        #region Private Methods

        private string GetActionsHtml(Doctor doctor)
        {
            string actionsHtml = $"<a href='/Doctor/EditDoctor/{doctor.DoctorId}' data-toggle='modal' data-target='#modal-action' class='btn btn-secondary'><i class='fas fa-edit'></i> Editar </a>";

            if (doctor.PatientInformationDoctors.Count == 0) {
                actionsHtml += $"<a href='/Doctor/DeleteDoctor/{doctor.DoctorId}' data-toggle='modal' data-target='#modal-action' class='btn btn-danger ml-1'><i class='fas fa-trash-alt'></i> Excluir </a>";
            } 
            else {
                actionsHtml +=  $"<a class='btn btn-danger ml-1 disabled'><i class='fas fa-trash-alt'></i> Excluir </a>";
            }

            return actionsHtml;
        }

        #endregion
    }
}
