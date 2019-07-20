using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.FormModel;
using LigaCancer.Models.SearchModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, User")]
    [AutoValidateAntiforgeryToken]
    [Area("Admin")]
    public class PresenceController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataStore<Presence> _presenceService;
        private readonly IDataStore<Patient> _patientService;
        private readonly ILogger<PresenceController> _logger;

        public PresenceController(
            IDataStore<Presence> presenceService,
            IDataStore<Patient> patientService,
            ILogger<PresenceController> logger,
            UserManager<ApplicationUser> userManager)
        {
            _presenceService = presenceService;
            _patientService = patientService;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new PresenceSearchModel());
        }

        [HttpGet]
        public async Task<IActionResult> AddPresence()
        {
            List<Patient> patients = await _patientService.GetAllAsync();
            var presenceForm = new PresenceFormModel
            {
                Patients = patients.Select(x => new SelectListItem($"{x.FirstName} {x.Surname}", x.PatientId.ToString())).ToList()
            };

            return PartialView("Partials/_AddPresence", presenceForm);
        }

        [HttpPost]
        public async Task<IActionResult> AddPresence(PresenceFormModel presenceForm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);
                Patient patient = await _patientService.FindByIdAsync(presenceForm.PatientId);
                var dateTime = DateTime.Parse(presenceForm.Date);
                var presence = new Presence
                {
                    PresenceDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, presenceForm.Time.Hours, presenceForm.Time.Minutes, 0),
                    PatientId = patient.PatientId,
                    Name = $"{patient.FirstName} {patient.Surname}",
                    CreatedBy = user.Name
                };

                TaskResult result = await _presenceService.CreateAsync(presence);

                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
                return BadRequest();
            }

            List<Patient> patients = await _patientService.GetAllAsync();
            presenceForm.Patients = patients.Select(x => new SelectListItem($"{x.FirstName} {x.Surname}", x.PatientId.ToString())).ToList();

            return PartialView("Partials/_AddPresence", presenceForm);
        }

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

            var presenceform = new PresenceFormModel
            {
                PatientId = presence.Name,
                Date = presence.PresenceDateTime.ToString("dd/M/yyyy"),
                Time = new TimeSpan(presence.PresenceDateTime.Hour, presence.PresenceDateTime.Minute, 0)
            };

            return PartialView("Partials/_EditPresence", presenceform);
        }

        [HttpPost]
        public async Task<IActionResult> EditPresence(string id, PresenceFormModel presenceForm)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);
                Presence presence = await _presenceService.FindByIdAsync(id);
                var dateTime = DateTime.Parse(presenceForm.Date);
                presence.PresenceDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, presenceForm.Time.Hours, presenceForm.Time.Minutes, 0);
                presence.UpdatedBy = user.Name;

                TaskResult result = await _presenceService.UpdateAsync(presence);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
                return BadRequest();
            }

            return PartialView("Partials/_EditPresence", presenceForm);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
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

            _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
            return BadRequest();
        }

    }
}