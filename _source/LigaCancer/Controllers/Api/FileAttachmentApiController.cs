using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LigaCancer.Controllers.Api
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class FileAttachmentApiController : Controller 
    {
        public readonly IDataStore<Patient> _patientService;

        public FileAttachmentApiController(IDataStore<Patient> patientService)
        {
            _patientService = patientService;
        }

    }
}
