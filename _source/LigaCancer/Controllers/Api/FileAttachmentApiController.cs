using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LigaCancer.Controllers.Api
{
    [Authorize(Roles = "Admin"), Route("api/[action]/{id?}")]
    public class FileAttachmentApiController : Controller 
    {
        public readonly IDataStore<Patient> _patientService;

        public FileAttachmentApiController(IDataStore<Patient> patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFileAttachmentsAsync(string id)
        {
            try
            {
                BaseSpecification<Patient> specification = new BaseSpecification<Patient>(x => x.FileAttachments);
                Patient patient = await _patientService.FindByIdAsync(id, specification);
                return Ok(new { data = patient.FileAttachments });
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
