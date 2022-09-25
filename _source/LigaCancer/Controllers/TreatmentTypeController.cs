// <copyright file="TreatmentTypeController.cs" company="Doffs">
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
    public class TreatmentTypeController : Controller
    {
        private readonly IDataRepository<TreatmentType> _treatmentTypeService;
        private readonly ILogger<TreatmentTypeController> _logger;

        public TreatmentTypeController(IDataRepository<TreatmentType> treatmentTypeService, ILogger<TreatmentTypeController> logger)
        {
            _treatmentTypeService = treatmentTypeService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(new TreatmentTypeSearchModel());
        }

        [HttpGet]
        public IActionResult AddTreatmentType()
        {
            return PartialView("Partials/_AddTreatmentType", new TreatmentTypeFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddTreatmentType(TreatmentTypeFormModel treatmentTypeForm)
        {
            if (ModelState.IsValid)
            {
                var treatmentType = new TreatmentType(treatmentTypeForm.Name, treatmentTypeForm.Note);

                TaskResult result = await _treatmentTypeService.CreateAsync(treatmentType);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.InsertItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_AddTreatmentType", treatmentTypeForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditTreatmentType(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            TreatmentType treatmentType = await _treatmentTypeService.FindByIdAsync(id);

            if (treatmentType == null)
            {
                return NotFound();
            }

            var treatmentTypeForm = new TreatmentTypeFormModel(treatmentType.Name, treatmentType.Note, treatmentType.TreatmentTypeId);

            return PartialView("Partials/_EditTreatmentType", treatmentTypeForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditTreatmentType(string id, TreatmentTypeFormModel treatmentTypeForm)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                TreatmentType treatmentType = await _treatmentTypeService.FindByIdAsync(id);

                if (treatmentType == null)
                {
                    return NotFound();
                }

                treatmentType.Name = treatmentTypeForm.Name;
                treatmentType.Note = treatmentTypeForm.Note;

                TaskResult result = await _treatmentTypeService.UpdateAsync(treatmentType);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.UpdateItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_EditTreatmentType", treatmentTypeForm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTreatmentType([FromForm] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            TreatmentType treatmentType = await _treatmentTypeService.FindByIdAsync(id);

            if (treatmentType == null)
            {
                return NotFound();
            }

            TaskResult result = await _treatmentTypeService.DeleteAsync(treatmentType);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(LogEvents.DeleteItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
            return BadRequest(result);
        }
    }
}
