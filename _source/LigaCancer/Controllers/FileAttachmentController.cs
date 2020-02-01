using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models;
using RVCC.Data.Models.PatientModels;
using RVCC.Models.SearchModel;
using RVCC.Models.ViewModel;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Controllers
{
    [Authorize(Roles = Roles.AdminAndUserAuthorize)]
    [AutoValidateAntiforgeryToken]
    public class FileAttachmentController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataRepository<FileAttachment> _fileAttachmentService;
        private readonly IDataRepository<Patient> _patientService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<FileAttachmentController> _logger;

        public FileAttachmentController(
            IDataRepository<FileAttachment> fileAttachmentService,
            IDataRepository<Patient> patientService,
            ILogger<FileAttachmentController> logger,
            IWebHostEnvironment webHostEnvironment,
            UserManager<ApplicationUser> userManager)
        {
            _fileAttachmentService = fileAttachmentService;
            _userManager = userManager;
            _patientService = patientService;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }


        [HttpGet]
        public IActionResult FileUpload(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            return PartialView("Partials/_FileUpload", new FileAttachmentSearchModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> FileUpload(string id, IFormFile file)
        {
            if (string.IsNullOrEmpty(id) || file == null || file.Length == 0)
            {
                return BadRequest();
            }

            Patient patient = await _patientService.FindByIdAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            ApplicationUser user = await _userManager.GetUserAsync(User);
            var fileAttachment = new FileAttachment
            {
                PatientId = patient.PatientId,
                FileName = Path.GetFileNameWithoutExtension(file.FileName),
                FileExtension = Path.GetExtension(file.FileName),
                FileSize = (double)file.Length / 1024 / 1024,
                CreatedBy = user.Name
            };

            string path = $"uploads\\files\\{patient.PatientId}";
            string uploads = Path.Combine(_webHostEnvironment.WebRootPath, path);
            try
            {
                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }

                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                string imageUrl = Path.Combine(path + "\\" + fileName);
                fileAttachment.FilePath = imageUrl;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "File Upload Error", null);
                return BadRequest();
            }

            TaskResult result = await _fileAttachmentService.CreateAsync(fileAttachment);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
            return BadRequest();
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> DeleteFileAttachment(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            FileAttachment fileAttachment = await _fileAttachmentService.FindByIdAsync(id);

            if (fileAttachment == null)
            {
                return NotFound();
            }

            try
            {
                string uploadFile = Path.Combine(_webHostEnvironment.WebRootPath, fileAttachment.FilePath);
                if (System.IO.File.Exists(uploadFile))
                {
                    System.IO.File.Delete(uploadFile);
                }
            }
            catch (IOException ioExp)
            {
                _logger.LogError(ioExp, "Upload File", null);
            }

            TaskResult result = await _fileAttachmentService.DeleteAsync(fileAttachment);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
            return BadRequest();
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> UpdateNameFile(FileAttachmentViewModel fileAttachmentModel)
        {
            if (string.IsNullOrEmpty(fileAttachmentModel.FileAttachmentId))
            {
                return BadRequest();
            }

            FileAttachment fileAttachment = await _fileAttachmentService.FindByIdAsync(fileAttachmentModel.FileAttachmentId);

            if (fileAttachment == null)
            {
                return NotFound();
            }

            fileAttachment.FileName = fileAttachmentModel.Name;

            TaskResult result = await _fileAttachmentService.UpdateAsync(fileAttachment);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
            return BadRequest();
        }
    }
}
