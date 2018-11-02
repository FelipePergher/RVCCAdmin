using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Data.Requests;
using LigaCancer.Data.Store;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LigaCancer.Controllers.Api
{
    [Authorize(Roles = "Admin"), Route("api/[action]")]
    public class DoctorApiController : Controller
    {
        private readonly IDataTable<Doctor> _doctorDataTable;

        public DoctorApiController(IDataTable<Doctor> doctorDataTable)
        {
            _doctorDataTable = doctorDataTable;
        }

        [HttpPost]
        public async Task<IActionResult> GetDoctorDataTableResponseAsync(DataTableOptions options)
        {
            try
            {
                BaseSpecification<Doctor> specification = new BaseSpecification<Doctor>(x => x.PatientInformationDoctors);
                return Ok(await _doctorDataTable.GetOptionResponseWithSpec(options, specification));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
