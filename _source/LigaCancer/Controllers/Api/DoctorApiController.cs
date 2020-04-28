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
    public class DoctorApiController : Controller
    {
        private readonly IDataRepository<Doctor> _doctorService;
        private readonly ILogger<DoctorApiController> _logger;

        public DoctorApiController(IDataRepository<Doctor> doctorService, ILogger<DoctorApiController> logger)
        {
            _doctorService = doctorService;
            _logger = logger;
        }

        [HttpPost("~/api/doctor/search")]
        public async Task<IActionResult> DoctorSearch([FromForm] SearchModel searchModel, [FromForm] DoctorSearchModel doctorSearch)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                IEnumerable<Doctor> doctors = await _doctorService.GetAllAsync(new string[] { "PatientInformationDoctors" }, sortColumn, sortDirection, doctorSearch);
                IEnumerable<DoctorViewModel> data = doctors.Select(x => new DoctorViewModel
                {
                    Name = x.Name,
                    CRM = x.CRM,
                    Actions = GetActionsHtml(x)
                }).Skip(skip).Take(take);

                int recordsTotal = _doctorService.Count();
                int recordsFiltered = _doctorService.Count();

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Doctor Search Error", null);
                return BadRequest();
            }
        }

        [HttpGet("~/api/doctor/select2Get")]
        public async Task<IActionResult> Select2GetDoctors(string term)
        {
            IEnumerable<Doctor> doctors = await _doctorService.GetAllAsync(null, "Name", "asc", new DoctorSearchModel { Name = term });
            var select2PagedResult = new Select2PagedResult
            {
                Results = doctors.Select(x => new Result
                {
                    Id = x.DoctorId.ToString(),
                    Text = x.Name
                }).ToList(),
            };

            return Ok(select2PagedResult);
        }

        [HttpGet("~/api/doctor/IsCrmExist")]
        public async Task<IActionResult> IsCrmExist(string crm, int doctorId)
        {
            Doctor doctor = await ((DoctorRepository)_doctorService).FindByCrmAsync(crm, doctorId);
            return Ok(doctor == null);
        }

        #region Private Methods

        private string GetActionsHtml(Doctor doctor)
        {
            string editDoctor = $"<a href='/Doctor/EditDoctor/{doctor.DoctorId}' data-toggle='modal' data-target='#modal-action' " +
                $"data-title='Editar Médico' class='dropdown-item editDoctorButton'><i class='fas fa-edit'></i> Editar </a>";
            string deleteDoctor = $"<a href='javascript:void(0);' data-url='/Doctor/DeleteDoctor' data-id='{doctor.DoctorId}' " +
                $"data-relation='{doctor.PatientInformationDoctors.Count > 0}' class='deleteDoctorButton dropdown-item'><i class='fas fa-trash-alt'></i> Excluir </a>";

            string actionsHtml =
                $"<div class='dropdown'>" +
                $"  <button type='button' class='btn btn-info dropdown-toggle' data-toggle='dropdown'>Ações</button>" +
                $"  <div class='dropdown-menu'>" +
                $"      {editDoctor}" +
                $"      {deleteDoctor}" +
                $"  </div>" +
                $"</div>";

            return actionsHtml;
        }

        #endregion
    }
}
