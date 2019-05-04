using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Data.Store;
using LigaCancer.Models.FormModel;
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
            return PartialView("_AddFileAttachment", new FileAttachmentFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddFileAttachment(string id, FileAttachmentFormModel fileAttachmentForm)
        {
            if (!ModelState.IsValid) return StatusCode(500, "Invalid");

            ApplicationUser user = await _userManager.GetUserAsync(User);
            Patient patient = await _patientService.FindByIdAsync(id);
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

            TaskResult result = await ((PatientStore)_patientService).AddFileAttachment(fileAttachment, id);
            if (result.Succeeded)
            {
                return StatusCode(200, "attachmentFile");
            }
            ModelState.AddErrors(result);
            return StatusCode(500, result.Errors.First().Description);
        }

        [HttpPost]
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
