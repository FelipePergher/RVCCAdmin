﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business.Interface;
using RVCC.Data.Models.PatientModels;
using RVCC.Models.SearchModel;
using RVCC.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RVCC.Business;

namespace RVCC.Controllers.Api
{
    [Authorize(Roles = Roles.AdminAndUserAuthorize)]
    [ApiController]
    public class FileAttachmentApiController : Controller
    {
        private readonly IDataRepository<FileAttachment> _fileAttachmentService;
        private readonly ILogger<FileAttachmentApiController> _logger;

        public FileAttachmentApiController(IDataRepository<FileAttachment> fileAttachmentService, ILogger<FileAttachmentApiController> logger)
        {
            _fileAttachmentService = fileAttachmentService;
            _logger = logger;
        }

        [HttpPost("~/api/FileAttachment/search")]
        public async Task<IActionResult> FileAttachmentSearch([FromForm] SearchModel searchModel, [FromForm] FileAttachmentSearchModel fileAttachmentSearch)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                IEnumerable<FileAttachment> fileAttachments = await _fileAttachmentService.GetAllAsync(null, sortColumn, sortDirection, fileAttachmentSearch);
                IEnumerable<FileAttachmentViewModel> data = fileAttachments.Select(x => new FileAttachmentViewModel
                {
                    FileAttachmentId = x.FileAttachmentId.ToString(),
                    Name = x.FileName,
                    Extension = x.FileExtension,
                    Size = x.FileSize.ToString("N"),
                    FilePath = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToString()}/{x.FilePath}",
                    Actions = GetActionsHtml(x)
                });

                int recordsTotal = fileAttachments.Count();

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered = recordsTotal});
            }
            catch (Exception e)
            {
                _logger.LogError(e, "File Attachment Search Error", null);
                return BadRequest();
            }
        }

        #region Private Methods

        private string GetActionsHtml(FileAttachment fileAttachment)
        {
            string deleteFileAttachment = $"<a href='javascript:void(0);' data-url='/FileAttachment/DeleteFileAttachment' data-id='{fileAttachment.FileAttachmentId}' class='dropdown-item deleteFileAttachmentButton'>" +
                $"<i class='fas fa-trash-alt'></i> Excluir </a>";

            string actionsHtml =
                $"<div class='dropdown'>" +
                $"  <button type='button' class='btn btn-info dropdown-toggle' data-toggle='dropdown'>Ações</button>" +
                $"  <div class='dropdown-menu'>" +
                $"      {deleteFileAttachment}" +
                $"  </div>" +
                $"</div>";

            return actionsHtml;
        }

        #endregion

    }
}
