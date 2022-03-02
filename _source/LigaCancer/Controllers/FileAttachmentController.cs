// <copyright file="FileAttachmentController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models;
using RVCC.Models.SearchModel;
using RVCC.Models.ViewModel;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Controllers
{
    [Authorize(Roles = Roles.AdminSecretarySocialAssistanceAuthorize)]
    [AutoValidateAntiforgeryToken]
    public class FileAttachmentController : Controller
    {
        private readonly IDataRepository<FileAttachment> _fileAttachmentService;
        private readonly IDataRepository<Patient> _patientService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<FileAttachmentController> _logger;

        public FileAttachmentController(IDataRepository<FileAttachment> fileAttachmentService, IDataRepository<Patient> patientService, IWebHostEnvironment webHostEnvironment, ILogger<FileAttachmentController> logger)
        {
            _fileAttachmentService = fileAttachmentService;
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

            var fileAttachment = new FileAttachment
            {
                PatientId = patient.PatientId,
                FileName = Path.GetFileNameWithoutExtension(file.FileName),
                FileExtension = Path.GetExtension(file.FileName),
                FileSize = (double)file.Length / 1024 / 1024,
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
                _logger.LogError(LogEvents.InsertItem, e, "File Upload Error");
                return BadRequest();
            }

            TaskResult result = await _fileAttachmentService.CreateAsync(fileAttachment);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(LogEvents.InsertItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
            return BadRequest();
        }

        [HttpPost]
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
                _logger.LogError(LogEvents.DeleteItem, ioExp, "Upload File");
            }

            TaskResult result = await _fileAttachmentService.DeleteAsync(fileAttachment);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(LogEvents.DeleteItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
            return BadRequest();
        }

        [HttpPost]
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

            _logger.LogError(LogEvents.UpdateItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
            return BadRequest();
        }
    }
}
