// <copyright file="TreatmentPlaceController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models;
using RVCC.Models.FormModel;
using RVCC.Models.SearchModel;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Controllers
{
    [Authorize(Roles = Roles.AdminSecretarySocialAssistanceAuthorize)]
    [AutoValidateAntiforgeryToken]
    public class TreatmentPlaceController : Controller
    {
        private readonly IDataRepository<TreatmentPlace> _treatmentPlaceService;
        private readonly ILogger<TreatmentPlaceController> _logger;

        public TreatmentPlaceController(IDataRepository<TreatmentPlace> treatmentPlaceService, ILogger<TreatmentPlaceController> logger)
        {
            _treatmentPlaceService = treatmentPlaceService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new TreatmentPlaceSearchModel());
        }

        [HttpGet]
        public IActionResult AddTreatmentPlace()
        {
            return PartialView("Partials/_AddTreatmentPlace", new TreatmentPlaceFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddTreatmentPlace(TreatmentPlaceFormModel treatmentPlaceForm)
        {
            if (ModelState.IsValid)
            {
                var treatmentPlace = new TreatmentPlace(treatmentPlaceForm.City);

                TaskResult result = await _treatmentPlaceService.CreateAsync(treatmentPlace);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.InsertItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_AddTreatmentPlace", treatmentPlaceForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditTreatmentPlace(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            TreatmentPlace treatmentPlace = await _treatmentPlaceService.FindByIdAsync(id);

            if (treatmentPlace == null)
            {
                return NotFound();
            }

            var treatmentPlaceForm = new TreatmentPlaceFormModel(treatmentPlace.City, treatmentPlace.TreatmentPlaceId);

            return PartialView("Partials/_EditTreatmentPlace", treatmentPlaceForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditTreatmentPlace(string id, TreatmentPlaceFormModel treatmentPlaceForm)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                TreatmentPlace treatmentPlace = await _treatmentPlaceService.FindByIdAsync(id);

                if (treatmentPlace == null)
                {
                    return NotFound();
                }

                treatmentPlace.City = treatmentPlaceForm.City;

                TaskResult result = await _treatmentPlaceService.UpdateAsync(treatmentPlace);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.UpdateItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_EditTreatmentPlace", treatmentPlaceForm);
        }

        [Authorize(Roles = Roles.AdminSecretaryAuthorize)]
        [HttpPost]
        public async Task<IActionResult> DeleteTreatmentPlace(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            TreatmentPlace treatmentPlace = await _treatmentPlaceService.FindByIdAsync(id);

            if (treatmentPlace == null)
            {
                return NotFound();
            }

            TaskResult result = await _treatmentPlaceService.DeleteAsync(treatmentPlace);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(LogEvents.DeleteItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
            return BadRequest(result);
        }
    }
}
