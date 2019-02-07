using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.FormModel;
using LigaCancer.Code;
using LigaCancer.Data.Store;
using LigaCancer.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using LigaCancer.Code.Interface;

namespace LigaCancer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FileAttachmentController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataStore<FileAttachment> _fileAttachmentService;
        private readonly IDataStore<Patient> _patientService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public FileAttachmentController(IDataStore<FileAttachment> fileAttachmentService,
            IDataStore<Patient> patientService,
            UserManager<ApplicationUser> userManager,
            IHostingEnvironment hostingEnvironment
            )
        {
            _fileAttachmentService = fileAttachmentService;
            _userManager = userManager;
            _patientService = patientService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult AddFileAttachment(string id)
        {
            FileAttachmentFormModel fileAttachmentForm = new FileAttachmentFormModel
            {
                PatientId = id
            };
            return PartialView("_AddFileAttachment", fileAttachmentForm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFileAttachment(FileAttachmentFormModel fileAttachmentForm)
        {
            if (!ModelState.IsValid) return StatusCode(500, "Invalid");

            ApplicationUser user = await _userManager.GetUserAsync(this.User);
            Patient patient = await _patientService.FindByIdAsync(fileAttachmentForm.PatientId);
            FileAttachment fileAttachment = new FileAttachment
            {
                ArchiveCategorie = fileAttachmentForm.FileCategory,
                FileName = $"{fileAttachmentForm.FileName}{Path.GetExtension(fileAttachmentForm.File.FileName)}",
                UserCreated = user
            };

            if (fileAttachmentForm.File != null && fileAttachmentForm.File.Length > 0)
            {
                if (patient != null)
                {
                    string path = $"uploads\\files\\{patient.PatientId}";
                    string uploads = Path.Combine(_hostingEnvironment.WebRootPath, path);
                    try
                    {
                        if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                        string fileName = $"{Guid.NewGuid()}.{Path.GetExtension(fileAttachmentForm.File.FileName)}";
                        using (FileStream fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                        {
                            await fileAttachmentForm.File.CopyToAsync(fileStream);
                        }
                        string imageUrl = Path.Combine(path + "\\" + fileName);
                        fileAttachment.FilePath = imageUrl;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return StatusCode(500, e.Message);
                    }

                }
            }

            TaskResult result = await ((PatientStore)_patientService).AddFileAttachment(fileAttachment, fileAttachmentForm.PatientId);
            if (result.Succeeded)
            {
                return StatusCode(200, "attachmentFile");
            }
            ModelState.AddErrors(result);
            return StatusCode(500, result.Errors.First().Description);
        }

        public async Task<IActionResult> DeleteFileAttachment(string id)
        {
            string name = string.Empty;

            if (string.IsNullOrEmpty(id)) return PartialView("_DeleteFileAttachment", name);

            FileAttachment fileAttachment = await _fileAttachmentService.FindByIdAsync(id);
            if (fileAttachment != null)
            {
                name = fileAttachment.FileName;
            }

            return PartialView("_DeleteFileAttachment", name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFileAttachment(string id, IFormCollection form)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index");

            FileAttachment fileAttachment = await _fileAttachmentService.FindByIdAsync(id);
            if (fileAttachment == null) return RedirectToAction("Index");

            TaskResult result = await _fileAttachmentService.DeleteAsync(fileAttachment);

            if (result.Succeeded)
            {
                return StatusCode(200, "attachmentFile");
            }
            ModelState.AddErrors(result);
            return PartialView("_DeleteFileAttachment", fileAttachment.FileName);
        }

    }
}
