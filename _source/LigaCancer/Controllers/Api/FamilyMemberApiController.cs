using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LigaCancer.Controllers.Api
{
    [Authorize(Roles = "Admin")]
    public class FamilyMemberApiController : Controller
    {
        private readonly IDataStore<Patient> _patientService;

        public FamilyMemberApiController(IDataStore<Patient> patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFamilyMemberDataTableResponseAsync(string id)
        {
            try
            {
                BaseSpecification<Patient> specification = new BaseSpecification<Patient>(x => x.Family, x => x.Family.FamilyMembers);
                Patient patient = await _patientService.FindByIdAsync(id, specification);
                return Ok(new { data = patient.Family.FamilyMembers});
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
