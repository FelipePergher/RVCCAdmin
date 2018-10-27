﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.MedicalViewModels;
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
            FileAttachmentViewModel fileAttachmentViewModel = new FileAttachmentViewModel
            {
                PatientId = id
            };
            return PartialView("_AddFileAttachment", fileAttachmentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFileAttachment(FileAttachmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(this.User);
                Patient patient = await _patientService.FindByIdAsync(model.PatientId);
                FileAttachment fileAttachment = new FileAttachment
                {
                    ArchiveCategorie = model.FileCategory,
                    FileName = $"{model.FileName}{Path.GetExtension(model.File.FileName)}",
                    UserCreated = user
                };

                if (model.File != null && model.File.Length > 0)
                {
                    if (patient != null)
                    {
                        string path = $"uploads\\files\\{patient.FirstName}-{patient.Surname}";
                        var uploads = Path.Combine(_hostingEnvironment.WebRootPath, path);
                        try
                        {
                            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                            string fileName = $"{Guid.NewGuid()}.{Path.GetExtension(model.File.FileName)}";
                            using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                            {
                                await model.File.CopyToAsync(fileStream);
                            }
                            var imageUrl = Path.Combine(path + "\\" + fileName);
                            fileAttachment.FilePath = imageUrl;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            return StatusCode(500, e.Message);
                        }

                    }
                }

                TaskResult result = await ((PatientStore)_patientService).AddFileAttachment(fileAttachment, model.PatientId);
                if (result.Succeeded)
                {
                    return StatusCode(200, true);
                }
                ModelState.AddErrors(result);
                return StatusCode(500, result.Errors.First().Description);
            }
            return StatusCode(500, "Invalid");
        }

        public async Task<IActionResult> DeleteFileAttachment(string id)
        {
            string name = string.Empty;

            if (!string.IsNullOrEmpty(id))
            {
                FileAttachment fileAttachment = await _fileAttachmentService.FindByIdAsync(id);
                if (fileAttachment != null)
                {
                    name = fileAttachment.FileName;
                }
            }

            return PartialView("_DeleteFileAttachment", name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFileAttachment(string id, IFormCollection form)
        {
            if (!string.IsNullOrEmpty(id))
            {
                FileAttachment fileAttachment = await _fileAttachmentService.FindByIdAsync(id);
                if (fileAttachment != null)
                {
                    TaskResult result = await _fileAttachmentService.DeleteAsync(fileAttachment);

                    if (result.Succeeded)
                    {
                        return StatusCode(200, "200");
                    }
                    ModelState.AddErrors(result);
                    return PartialView("_DeleteFileAttachment", fileAttachment.FileName);
                }
            }
            return RedirectToAction("Index");
        }

    }
}
