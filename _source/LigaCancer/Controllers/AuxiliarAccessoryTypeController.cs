// <copyright file="AuxiliarAccessoryTypeController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models.Domain;
using RVCC.Models.FormModel;
using RVCC.Models.SearchModel;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Controllers
{
    [Authorize(Roles = Roles.AdminSecretarySocialAssistanceAuthorize)]
    [AutoValidateAntiforgeryToken]
    public class AuxiliarAccessoryTypeController : Controller
    {
        private readonly IDataRepository<AuxiliarAccessoryType> _auxiliarAccessoryTypeService;
        private readonly ILogger<AuxiliarAccessoryTypeController> _logger;

        public AuxiliarAccessoryTypeController(IDataRepository<AuxiliarAccessoryType> auxiliarAccessoryTypeService, ILogger<AuxiliarAccessoryTypeController> logger)
        {
            _auxiliarAccessoryTypeService = auxiliarAccessoryTypeService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(new AuxiliarAccessoryTypeSearchModel());
        }

        [HttpGet]
        public IActionResult AddAuxiliarAccessoryType()
        {
            return PartialView("Partials/_AddAuxiliarAccessoryType", new AuxiliarAccessoryTypeFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddAuxiliarAccessoryType(AuxiliarAccessoryTypeFormModel auxiliarAccessoryTypeForm)
        {
            if (ModelState.IsValid)
            {
                var auxiliarAccessoryType = new AuxiliarAccessoryType(auxiliarAccessoryTypeForm.Name);

                TaskResult result = await _auxiliarAccessoryTypeService.CreateAsync(auxiliarAccessoryType);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.InsertItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_AddAuxiliarAccessoryType", auxiliarAccessoryTypeForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditAuxiliarAccessoryType(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            AuxiliarAccessoryType auxiliarAccessoryType = await _auxiliarAccessoryTypeService.FindByIdAsync(id);

            if (auxiliarAccessoryType == null)
            {
                return NotFound();
            }

            var auxiliarAccessoryTypeForm = new AuxiliarAccessoryTypeFormModel(auxiliarAccessoryType.Name, auxiliarAccessoryType.AuxiliarAccessoryTypeId);

            return PartialView("Partials/_EditAuxiliarAccessoryType", auxiliarAccessoryTypeForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditAuxiliarAccessoryType(string id, AuxiliarAccessoryTypeFormModel auxiliarAccessoryTypeForm)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                AuxiliarAccessoryType auxiliarAccessoryType = await _auxiliarAccessoryTypeService.FindByIdAsync(id);

                if (auxiliarAccessoryType == null)
                {
                    return NotFound();
                }

                auxiliarAccessoryType.Name = auxiliarAccessoryTypeForm.Name;

                TaskResult result = await _auxiliarAccessoryTypeService.UpdateAsync(auxiliarAccessoryType);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.UpdateItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_EditAuxiliarAccessoryType", auxiliarAccessoryTypeForm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAuxiliarAccessoryType([FromForm] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            AuxiliarAccessoryType auxiliarAccessoryType = await _auxiliarAccessoryTypeService.FindByIdAsync(id);

            if (auxiliarAccessoryType == null)
            {
                return NotFound();
            }

            TaskResult result = await _auxiliarAccessoryTypeService.DeleteAsync(auxiliarAccessoryType);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(LogEvents.DeleteItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
            return BadRequest(result);
        }
    }
}
