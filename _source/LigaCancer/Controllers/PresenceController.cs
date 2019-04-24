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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Controllers
{
    [Authorize(Roles = "Admin"), AutoValidateAntiforgeryToken]
    public class PresenceController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataStore<Presence> _presenceService;
        private readonly IDataStore<Patient> _patientService;

        public PresenceController(IDataStore<Presence> presenceService, IDataStore<Patient> patientService, UserManager<ApplicationUser> userManager)
        {
            _presenceService = presenceService;
            _patientService = patientService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(new PresenceSearchModel());
        }

        [HttpGet]
        public async Task<IActionResult> AddPresence()
        {
            List<Patient> patients = await _patientService.GetAllAsync();
            PresenceFormModel presenceForm = new PresenceFormModel
            {
                Patients = patients.Select(x => new SelectListItem
                {
                    Text = x.FirstName + " " + x.Surname,
                    Value = x.PatientId.ToString()
                }).ToList()
            };

            return PartialView("_AddPresence", presenceForm);
        }

        [HttpPost]
        public async Task<IActionResult> AddPresence(PresenceFormModel presenceForm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);

                Patient patient = await _patientService.FindByIdAsync(presenceForm.PatientId);

                Presence presence = new Presence
                {
                    PresenceDateTime = new DateTime(presenceForm.Date.Year, presenceForm.Date.Month, presenceForm.Date.Day, presenceForm.Time.Hours, presenceForm.Time.Minutes, 0),
                    Name = $"{patient.FirstName} {patient.Surname}",
                    UserCreated = user
                };

                TaskResult result = await _presenceService.CreateAsync(presence);

                if (result.Succeeded) return Ok();

                ModelState.AddErrors(result);
            }

            List<Patient> patients = await _patientService.GetAllAsync();
            presenceForm.Patients = patients.Select(x => new SelectListItem
            {
                Text = x.FirstName + " " + x.Surname,
                Value = x.PatientId.ToString()
            }).ToList();

            return PartialView("_AddPresence", presenceForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditPresence(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            Presence presence = await _presenceService.FindByIdAsync(id);

            if (presence == null) return NotFound();

            List<Patient> patients = await _patientService.GetAllAsync();
            PresenceFormModel presenceform = new PresenceFormModel
            {
                PresenceId = presence.PresenceId,
                PatientId = presence.PatientId.ToString(),
                Date = presence.PresenceDateTime,
                Time = new TimeSpan(presence.PresenceDateTime.Hour, presence.PresenceDateTime.Minute, 0),
                Patients = patients.Select(x => new SelectListItem
                {
                    Text = x.FirstName + " " + x.Surname,
                    Value = x.PatientId.ToString()
                }).ToList()
            };

            return PartialView("_EditPresence", presenceform);
        }

        [HttpPost]
        public async Task<IActionResult> EditPresence(string id, PresenceFormModel presenceForm)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(presenceForm.PatientId)) return BadRequest();
            
            if (ModelState.IsValid)
            {
                Presence presence = await _presenceService.FindByIdAsync(id);
                
                if (presence == null) return NotFound();

                Patient patient = await _patientService.FindByIdAsync(presenceForm.PatientId);

                presence.Name = $"{patient.FirstName} {patient.Surname}";
                presence.PresenceDateTime = new DateTime(presenceForm.Date.Year, presenceForm.Date.Month, presenceForm.Date.Day, presenceForm.Time.Hours, presenceForm.Time.Minutes, 0);
                presence.UserUpdated = await _userManager.GetUserAsync(User);

                TaskResult result = await _presenceService.UpdateAsync(presence);
                if (result.Succeeded) return Ok();

                ModelState.AddErrors(result);
            }

            List<Patient> patients = await _patientService.GetAllAsync();
            presenceForm.Patients = patients.Select(x => new SelectListItem
            {
                Text = x.FirstName + " " + x.Surname,
                Value = x.PatientId.ToString()
            }).ToList();

            return PartialView("_EditPresence", presenceForm);
        }

        [HttpGet]
        public async Task<IActionResult> DeletePresence(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            Presence presence = await _presenceService.FindByIdAsync(id);

            if (presence == null) return NotFound();

            TaskResult result = await _presenceService.DeleteAsync(presence);

            if (result.Succeeded) return Ok();

            return BadRequest(result);
        }

    }
}