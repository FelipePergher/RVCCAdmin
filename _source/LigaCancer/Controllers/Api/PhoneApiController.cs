using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LigaCancer.Controllers.Api
{
    [Authorize(Roles = "Admin")]
    public class PhoneApiController : Controller
    {
        private readonly IDataStore<Patient> _patientService;

        public PhoneApiController(IDataStore<Patient> patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPhoneDataTableResponseAsync(string id)
        {
            try
            {
                BaseSpecification<Patient> specification = new BaseSpecification<Patient>(x => x.Phones);
                Patient patient = await _patientService.FindByIdAsync(id, specification);
                return Ok(new { data = patient.Phones});
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
