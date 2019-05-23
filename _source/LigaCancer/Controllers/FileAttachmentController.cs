using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.SearchModel;
using LigaCancer.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Controllers
{
    [Authorize(Roles = "Admin"), AutoValidateAntiforgeryToken]
    public class FileAttachmentController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataStore<FileAttachment> _fileAttachmentService;
        private readonly IDataStore<Patient> _patientService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public FileAttachmentController(
            IDataStore<FileAttachment> fileAttachmentService,
            IDataStore<Patient> patientService,
            UserManager<ApplicationUser> userManager,
            IHostingEnvironment hostingEnvironment)
        {
            _fileAttachmentService = fileAttachmentService;
            _userManager = userManager;
            _patientService = patientService;
            _hostingEnvironment = hostingEnvironment;
        }


        [HttpGet]
        public IActionResult FileUpload(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();
            return PartialView("Partials/_FileUpload", new FileAttachmentSearchModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> FileUpload(string id, IFormFile file)
        {
            if (string.IsNullOrEmpty(id) || file == null || file.Length == 0) return BadRequest();

            Patient patient = await _patientService.FindByIdAsync(id);

            if(patient == null) return NotFound();

            FileAttachment fileAttachment = new FileAttachment
            {
                PatientId = patient.PatientId,
                FileName = Path.GetFileNameWithoutExtension(file.FileName),
                FileExtension = Path.GetExtension(file.FileName),
                FileSize = (double) file.Length / 1024 / 1024,
                UserCreated = await _userManager.GetUserAsync(User)
            };

            string path = $"uploads\\files\\{patient.PatientId}";
            string uploads = Path.Combine(_hostingEnvironment.WebRootPath, path);
            try
            {
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                using (FileStream fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                string imageUrl = Path.Combine(path + "\\" + fileName);
                fileAttachment.FilePath = imageUrl;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            TaskResult result = await _fileAttachmentService.CreateAsync(fileAttachment);
            
            if (result.Succeeded) return Ok();
            return BadRequest(result.Errors.FirstOrDefault().Description);
        }

        [HttpPost, IgnoreAntiforgeryToken]
        public async Task<IActionResult> DeleteFileAttachment(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            FileAttachment fileAttachment = await _fileAttachmentService.FindByIdAsync(id);

            if (fileAttachment == null) return NotFound();

            TaskResult result = await _fileAttachmentService.DeleteAsync(fileAttachment);

            if (result.Succeeded) return Ok();
            return BadRequest();
        }

        [HttpPost, IgnoreAntiforgeryToken]
        public async Task<IActionResult> UpdateNameFile(FileAttachmentViewModel fileAttachmentModel)
        {
            if (string.IsNullOrEmpty(fileAttachmentModel.FileAttachmentId)) return BadRequest();

            FileAttachment fileAttachment = await _fileAttachmentService.FindByIdAsync(fileAttachmentModel.FileAttachmentId);

            if (fileAttachment == null) return NotFound();

            fileAttachment.FileName = fileAttachmentModel.Name;

            TaskResult result = await _fileAttachmentService.UpdateAsync(fileAttachment);

            if (result.Succeeded) return Ok();
            return BadRequest();
        }
    }
}
