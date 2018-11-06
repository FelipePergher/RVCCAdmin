using System.Threading.Tasks;
using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LigaCancer.Controllers.Api
{
    [Authorize(Roles = "Admin"), Route("api/[action]/{id?}")]
    public class AddressApiController : Controller
    {
        private readonly IDataStore<Patient> _patientService;

        public AddressApiController(IDataStore<Patient> patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAddressDataTableResponseAsync(string id)
        {
            try
            {
                BaseSpecification<Patient> specification = new BaseSpecification<Patient>(x => x.Addresses);
                Patient patient = await _patientService.FindByIdAsync(id, specification);
                return Ok(new { data = patient.Addresses});
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
