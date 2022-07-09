// <copyright file="StayController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models.Domain;
using RVCC.Data.Repositories;
using RVCC.Models.FormModel;
using RVCC.Models.SearchModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Controllers
{
    [Authorize(Roles = Roles.AdminSecretarySocialAssistanceAuthorize)]
    [AutoValidateAntiforgeryToken]
    public class StayController : Controller
    {
        private readonly IDataRepository<Stay> _stayService;
        private readonly IDataRepository<Patient> _patientService;
        private readonly ILogger<StayController> _logger;

        public StayController(IDataRepository<Stay> stayService, IDataRepository<Patient> patientService, ILogger<StayController> logger)
        {
            _stayService = stayService;
            _patientService = patientService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new StaySearchModel());
        }

        [HttpGet]
        public async Task<IActionResult> AddStay()
        {
            List<Patient> patients = await ((PatientRepository)_patientService).GetAllAsync();
            var stayForm = new StayFormModel
            {
                Patients = patients.Select(x => new SelectListItem($"{x.FirstName} {x.Surname}", x.PatientId.ToString())).ToList()
            };

            return PartialView("Partials/_AddStay", stayForm);
        }

        [HttpPost]
        public async Task<IActionResult> AddStay(StayFormModel stayForm)
        {
            if (ModelState.IsValid)
            {
                Patient patient = await _patientService.FindByIdAsync(stayForm.PatientId);

                if (patient == null)
                {
                    return NotFound();
                }

                var dateTime = DateTime.Parse(stayForm.Date);
                var stay = new Stay
                {
                    StayDateTime = dateTime,
                    PatientId = patient.PatientId,
                    PatientName = $"{patient.FirstName} {patient.Surname}",
                    City = stayForm.City,
                    Note = stayForm.Note,
                };

                TaskResult result = await _stayService.CreateAsync(stay);

                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.InsertItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            List<Patient> patients = await _patientService.GetAllAsync();
            stayForm.Patients = patients.Select(x => new SelectListItem($"{x.FirstName} {x.Surname}", x.PatientId.ToString())).ToList();

            return PartialView("Partials/_AddStay", stayForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditStay(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Stay stay = await _stayService.FindByIdAsync(id);

            if (stay == null)
            {
                return NotFound();
            }

            var stayForm = new StayFormModel
            {
                PatientId = stay.PatientName,
                Date = stay.StayDateTime.ToDateString(),
                City = stay.City,
                Note = stay.Note
            };

            return PartialView("Partials/_EditStay", stayForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditStay(string id, StayFormModel stayForm)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                Stay stay = await _stayService.FindByIdAsync(id);

                if (stay == null)
                {
                    return NotFound();
                }

                var dateTime = DateTime.Parse(stayForm.Date);
                stay.StayDateTime = dateTime;
                stay.Note = stayForm.Note;
                stay.City = stayForm.City;

                TaskResult result = await _stayService.UpdateAsync(stay);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.UpdateItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_EditStay", stayForm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStay(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Stay stay = await _stayService.FindByIdAsync(id);

            if (stay == null)
            {
                return NotFound();
            }

            TaskResult result = await _stayService.DeleteAsync(stay);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(LogEvents.DeleteItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
            return BadRequest();
        }
    }
}