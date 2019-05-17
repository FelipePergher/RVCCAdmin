using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.SearchModel;
using LigaCancer.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Controllers.Api
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class FileAttachmentApiController : Controller 
    {
        private readonly IDataStore<FileAttachment> _fileAttachmentService;

        public FileAttachmentApiController(IDataStore<FileAttachment> fileAttachmentService)
        {
            _fileAttachmentService = fileAttachmentService;
        }

        [HttpPost("~/api/FileAttachment/search")]
        public async Task<IActionResult> FileAttachmentSearch([FromForm] FileAttachmentSearchModel fileAttachmentSearch)
        {
            try
            {
                IEnumerable<FileAttachment> fileAttachments = await _fileAttachmentService.GetAllAsync(filter: fileAttachmentSearch);
                IEnumerable<FileAttachmentViewModel> data = fileAttachments.Select(x => new FileAttachmentViewModel
                {
                    FileAttachmentId = x.FileAttachmentId.ToString(),
                    Name = x.FileName,
                    Extension = x.FileExtension,
                    Size = x.FileSize.ToString("N"),
                    FilePath = x.FilePath,
                    Actions = GetActionsHtml(x)
                });

                int recordsTotal = fileAttachments.Count();

                return Ok(new { data });
            }
            catch
            {
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
