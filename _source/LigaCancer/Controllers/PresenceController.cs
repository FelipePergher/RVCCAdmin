// <copyright file="PresenceController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models;
using RVCC.Data.Repositories;
using RVCC.Models.FormModel;
using RVCC.Models.SearchModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class PresenceController : Controller
    {
        private readonly IDataRepository<Presence> _presenceService;
        private readonly IDataRepository<Patient> _patientService;
        private readonly ILogger<PresenceController> _logger;

        public PresenceController(IDataRepository<Presence> presenceService, IDataRepository<Patient> patientService, ILogger<PresenceController> logger)
        {
            _presenceService = presenceService;
            _patientService = patientService;
            _logger = logger;
        }

        [Authorize(Roles = Roles.AdminUserSocialAssistanceAuthorize)]
        [HttpGet]
        public IActionResult Index()
        {
            return View(new PresenceSearchModel());
        }

        [Authorize(Roles = Roles.AdminUserAuthorize)]
        [HttpGet]
        public async Task<IActionResult> AddPresence()
        {
            List<Patient> patients = await ((PatientRepository)_patientService).GetAllAsync();
            var presenceForm = new PresenceFormModel
            {
                Patients = patients.Select(x => new SelectListItem($"{x.FirstName} {x.Surname}", x.PatientId.ToString())).ToList()
            };

            return PartialView("Partials/_AddPresence", presenceForm);
        }

        [Authorize(Roles = Roles.AdminUserAuthorize)]
        [HttpPost]
        public async Task<IActionResult> AddPresence(PresenceFormModel presenceForm)
        {
            if (ModelState.IsValid)
            {
                Patient patient = await _patientService.FindByIdAsync(presenceForm.PatientId);
                var dateTime = DateTime.Parse(presenceForm.Date);
                var presence = new Presence
                {
                    PresenceDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, presenceForm.Time.Hours, presenceForm.Time.Minutes, 0),
                    PatientId = patient.PatientId,
                    Name = $"{patient.FirstName} {patient.Surname}",
                };

                TaskResult result = await _presenceService.CreateAsync(presence);

                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.InsertItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            List<Patient> patients = await _patientService.GetAllAsync();
            presenceForm.Patients = patients.Select(x => new SelectListItem($"{x.FirstName} {x.Surname}", x.PatientId.ToString())).ToList();

            return PartialView("Partials/_AddPresence", presenceForm);
        }

        [Authorize(Roles = Roles.AdminUserAuthorize)]
        [HttpGet]
        public async Task<IActionResult> EditPresence(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Presence presence = await _presenceService.FindByIdAsync(id);

            if (presence == null)
            {
                return NotFound();
            }

            var presenceForm = new PresenceFormModel
            {
                PatientId = presence.Name,
                Date = presence.PresenceDateTime.ToString("dd/MM/yyyy"),
                Time = new TimeSpan(presence.PresenceDateTime.Hour, presence.PresenceDateTime.Minute, 0)
            };

            return PartialView("Partials/_EditPresence", presenceForm);
        }

        [Authorize(Roles = Roles.AdminUserAuthorize)]
        [HttpPost]
        public async Task<IActionResult> EditPresence(string id, PresenceFormModel presenceForm)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                Presence presence = await _presenceService.FindByIdAsync(id);
                var dateTime = DateTime.Parse(presenceForm.Date);
                presence.PresenceDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, presenceForm.Time.Hours, presenceForm.Time.Minutes, 0);

                TaskResult result = await _presenceService.UpdateAsync(presence);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.UpdateItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_EditPresence", presenceForm);
        }

        [Authorize(Roles = Roles.AdminUserAuthorize)]
        [HttpPost]
        public async Task<IActionResult> DeletePresence(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Presence presence = await _presenceService.FindByIdAsync(id);

            if (presence == null)
            {
                return NotFound();
            }

            TaskResult result = await _presenceService.DeleteAsync(presence);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(LogEvents.DeleteItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
            return BadRequest();
        }
    }
}