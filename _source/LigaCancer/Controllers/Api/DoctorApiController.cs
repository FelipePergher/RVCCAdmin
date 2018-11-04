using System.Threading.Tasks;
using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Data.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
