using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LigaCancer.Models.SearchViewModel;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Code.Interface;
using Microsoft.AspNetCore.Identity;
using LigaCancer.Data.Models;
using LigaCancer.Code;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using LigaCancer.Models.FormViewModel;
using System.Linq;

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
            return View(new PresenceSearchViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> AddPresence()
        {
            List<Patient> patients = await _patientService.GetAllAsync();

            PresenceViewModel presenceViewModel = new PresenceViewModel
            {
                Patients = patients.Select(x => new SelectListItem
                {
                    Text = x.FirstName + " " + x.Surname,
                    Value = x.PatientId.ToString()

                }).ToList()
            };

            return PartialView("_AddPresence", presenceViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddPresence(PresenceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(this.User);

                Presence presence = new Presence
                {
                    PresenceDateTime = new DateTime(model.Date.Year, model.Date.Month, model.Date.Day, model.Time.Hours, model.Time.Minutes, 0),
                    UserCreated = user,
                    Patient = await _patientService.FindByIdAsync(model.Patient)
                };

                TaskResult result = await _presenceService.CreateAsync(presence);

                if (result.Succeeded) return Ok();

                ModelState.AddErrors(result);
            }

            List<Patient> patients = await _patientService.GetAllAsync();

            model.Patients = patients.Select(x => new SelectListItem
            {
                Text = x.FirstName + " " + x.Surname,
                Value = x.PatientId.ToString()

            }).ToList();

            return PartialView("_AddPresence", model);
        }

        [HttpGet]
        public async Task<IActionResult> EditPresence(string id)
        {
            
            if (string.IsNullOrEmpty(id)) return BadRequest();

            

            BaseSpecification<Presence> specification = new BaseSpecification<Presence>(x => x.Patient);
            Presence presence = await _presenceService.FindByIdAsync(id, specification);

            if (presence == null) return NotFound();

            List<Patient> patients = await _patientService.GetAllAsync();

            PresenceViewModel presenceViewModel = new PresenceViewModel
            {
                PresenceId = presence.PresenceId,
                Patient = presence.Patient.PatientId.ToString(),
                Date = presence.PresenceDateTime,
                Time = new TimeSpan(presence.PresenceDateTime.Hour, presence.PresenceDateTime.Minute, 0),

                Patients = patients.Select(x => new SelectListItem
                {
                    Text = x.FirstName + " " + x.Surname,
                    Value = x.PatientId.ToString()

                }).ToList()
            };

            return PartialView("_EditPresence", presenceViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditPresence(string id, PresenceViewModel model)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(model.Patient)) return BadRequest();
            if (ModelState.IsValid)
            {
                BaseSpecification<Presence> specification = new BaseSpecification<Presence>(x => x.Patient);
                Presence presence = await _presenceService.FindByIdAsync(id, specification);
                if (presence == null) return NotFound();
                ApplicationUser user = await _userManager.GetUserAsync(this.User);

                presence.Patient = await _patientService.FindByIdAsync(model.Patient);
                presence.PresenceDateTime = new DateTime(model.Date.Year, model.Date.Month, model.Date.Day, model.Time.Hours, model.Time.Minutes, 0);
                presence.UserUpdated = user;

                TaskResult result = await _presenceService.UpdateAsync(presence);
                if (result.Succeeded) return Ok();

                ModelState.AddErrors(result);

            }

            List<Patient> patients = await _patientService.GetAllAsync();

            model.Patients = patients.Select(x => new SelectListItem
            {
                Text = x.FirstName + " " + x.Surname,
                Value = x.PatientId.ToString()

            }).ToList();

            return PartialView("_EditPresence", model);
        }

        public async Task<IActionResult> DeletePresence(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            DeletePresenceViewModel deletePresenceViewModel = new DeletePresenceViewModel();

            BaseSpecification<Presence> specification = new BaseSpecification<Presence>(x => x.Patient);
            Presence presence = await _presenceService.FindByIdAsync(id, specification);
            if (presence == null) return NotFound();

            deletePresenceViewModel.Name = presence.Patient.FirstName;
            deletePresenceViewModel.Date = presence.PresenceDateTime;

            return PartialView("_DeletePresence", deletePresenceViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePresence(string id, IFormCollection form)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            BaseSpecification<Presence> specification = new BaseSpecification<Presence>(x => x.Patient);
            Presence presence = await _presenceService.FindByIdAsync(id, specification);
            if (presence == null) return NotFound();

            TaskResult result = await _presenceService.DeleteAsync(presence);

            if (result.Succeeded)
            {
                return Ok();
            }
            ModelState.AddErrors(result);
            return PartialView("_DeletePresence", presence);
        }


    }
}