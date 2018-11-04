using System.Threading.Tasks;
using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Code.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LigaCancer.Controllers.Api
{
    [Authorize(Roles = "Admin"), Route("api/[action]")]
    public class PatientApiController : Controller
    {
        private readonly IDataTable<Patient> _patientDataTable;

        public PatientApiController(IDataTable<Patient> patientDataTable)
        {
            _patientDataTable = patientDataTable;
        }

        [HttpPost]
        public async Task<IActionResult> GetPatientDataTableResponseAsync(DataTableOptions options)
        {
            try
            {
                BaseSpecification<Patient> specification = new BaseSpecification<Patient>();
                return Ok(await _patientDataTable.GetOptionResponseWithSpec(options, specification));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
