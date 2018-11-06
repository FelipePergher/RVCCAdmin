using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LigaCancer.Controllers.Api
{
    [Authorize(Roles = "Admin"), Route("api/[action]/{id?}")]
    public class FamilyApiController : Controller 
    {
        public readonly IDataStore<Patient> _patientService;

        public FamilyApiController(IDataStore<Patient> patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFamilyIncomeAsync(string id)
        {
            try
            {
                BaseSpecification<Patient> specification = new BaseSpecification<Patient>(x => x.Family);
                Patient patient = await _patientService.FindByIdAsync(id, specification);
                return Ok(new { patient.Family.FamilyIncome, patient.Family.PerCapitaIncome });
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
