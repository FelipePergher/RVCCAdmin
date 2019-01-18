using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LigaCancer.Models.SearchViewModels;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Code.Interface;
using Microsoft.AspNetCore.Identity;
using LigaCancer.Data.Models;
using LigaCancer.Code;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Http;

namespace LigaCancer.Controllers
{
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
            PresenceViewModel presence = new PresenceViewModel();
            List<Patient> patients = _patientService.GetAllAsync().Result;

            foreach (Patient patient in patients)
            {
                SelectListItem selectListItem = new SelectListItem
                {
                    Text = patient.FirstName + " " + patient.Surname,
                    Value = patient.PatientId.ToString()
                };
                presence.Patients.Add(selectListItem);
            }

            return View(new PresenceSearchViewModel
            {
                Presences = _presenceService.GetAllAsync().Result,
                Patients = presence.Patients
            });
        }

        [HttpGet]
        public IActionResult AddPresence()
        {
            PresenceViewModel presence = new PresenceViewModel();
            List<Patient> patients = _patientService.GetAllAsync().Result;


            //presence.Patients = patients.Select(x => new SelectListItem
            //{
            //    Text = x.FirstName + " " + x.Surname,
            //    Value = x.PatientId.ToString()
            //}).ToList();

            

            foreach (Patient patient in patients)
            {
                SelectListItem selectListItem = new SelectListItem
                {
                    Text = patient.FirstName + " " + patient.Surname,
                    Value = patient.PatientId.ToString()
                };
                presence.Patients.Add(selectListItem);
            }  

            return PartialView("_AddPresence", presence);
        }

        [HttpPost]
        public async Task<IActionResult> AddPresence(PresenceViewModel model)
        {

            ApplicationUser user = await _userManager.GetUserAsync(this.User);


            Presence presence = new Presence
            {
                PresenceDateTime = new DateTime(model.Date.Year, model.Date.Month, model.Date.Day, model.Time.Hours, model.Time.Minutes, 0),
                UserCreated = user,
                Patient = await _patientService.FindByIdAsync(model.Patient)
            };

            TaskResult result = await _presenceService.CreateAsync(presence);
            if (result.Succeeded)
            {
                return StatusCode(200, "200");
            }
            ModelState.AddErrors(result);
            return Ok("200");
            }

        [HttpGet]
        public async Task<IActionResult> EditPresence(string id)
        {
            PresenceViewModel presenceViewModel = new PresenceViewModel();
            List<Patient> patients = _patientService.GetAllAsync().Result;

            if (string.IsNullOrEmpty(id)) return PartialView("_EditPresence", presenceViewModel);
            Presence presence = await _presenceService.FindByIdAsync(id);
            if (presence != null)
            {
                presenceViewModel = new PresenceViewModel
                {
                    PresenceId = presence.PresenceId,
                    Patient = presence.Patient.PatientId.ToString(),
                    Date = presence.PresenceDateTime,
                    Time = new TimeSpan(presence.PresenceDateTime.Hour, presence.PresenceDateTime.Minute, 0),
                };
                
            }

            foreach (Patient patient in patients)
            {
                SelectListItem selectListItem = new SelectListItem
                {
                    Text = patient.FirstName + " " + patient.Surname,
                    Value = patient.PatientId.ToString(),
                    Selected = patient.PatientId == presence.Patient.PatientId ? true : false,
                };
                presenceViewModel.Patients.Add(selectListItem);
            }

            return PartialView("_EditPresence", presenceViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditPresence(string id, PresenceViewModel model)
        {
            if (!ModelState.IsValid) return PartialView("_EditPresence", model);

            Presence presence = await _presenceService.FindByIdAsync(id);
            ApplicationUser user = await _userManager.GetUserAsync(this.User);

            presence.Patient = await _patientService.FindByIdAsync(model.Patient);
            presence.PresenceDateTime = new DateTime(model.Date.Year, model.Date.Month, model.Date.Day, model.Time.Hours, model.Time.Minutes, 0);
            presence.LastUserUpdate = user;

            TaskResult result = await _presenceService.UpdateAsync(presence);
            if (result.Succeeded)
            {
                return StatusCode(200, "200");
            }
            ModelState.AddErrors(result);

            return PartialView("_EditPresence", model);
        }

        public async Task<IActionResult> DeletePresence(string id)
        {
            DeletePresenceViewModel deletePresenceViewModel = new DeletePresenceViewModel();

            if (string.IsNullOrEmpty(id)) return PartialView("_DeletePresence", deletePresenceViewModel);

            Presence presence = await _presenceService.FindByIdAsync(id);
            if (presence != null)
            {
                deletePresenceViewModel.Name = presence.Patient.FirstName;
                deletePresenceViewModel.Date = presence.PresenceDateTime;
            }

            return PartialView("_DeletePresence", deletePresenceViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePresence(string id, IFormCollection form)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index");

            Presence presence = await _presenceService.FindByIdAsync(id);
            if (presence == null) return RedirectToAction("Index");

            TaskResult result = await _presenceService.DeleteAsync(presence);

            if (result.Succeeded)
            {
                return StatusCode(200, "200");
            }
            ModelState.AddErrors(result);
            return PartialView("_DeletePresence", presence);
        }


    }
}