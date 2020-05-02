using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    public class StayController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataRepository<Stay> _stayService;
        private readonly IDataRepository<Patient> _patientService;
        private readonly ILogger<StayController> _logger;

        public StayController(
            IDataRepository<Stay> stayService,
            IDataRepository<Patient> patientService,
            ILogger<StayController> logger,
            UserManager<ApplicationUser> userManager)
        {
            _stayService = stayService;
            _patientService = patientService;
            _userManager = userManager;
            _logger = logger;
        }

        [Authorize(Roles = Roles.AdminUserSocialAssistanceAuthorize)]
        [HttpGet]
        public IActionResult Index()
        {
            return View(new StaySearchModel());
        }

        [Authorize(Roles = Roles.AdminUserAuthorize)]
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

        [Authorize(Roles = Roles.AdminUserAuthorize)]
        [HttpPost]
        public async Task<IActionResult> AddStay(StayFormModel stayForm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);
                Patient patient = await _patientService.FindByIdAsync(stayForm.PatientId);
                var dateTime = DateTime.Parse(stayForm.Date);
                var stay = new Stay
                {
                    StayDateTime = dateTime,
                    PatientId = patient.PatientId,
                    PatientName = $"{patient.FirstName} {patient.Surname}",
                    City = stayForm.City,
                    Note = stayForm.Note,
                    CreatedBy = user.Name
                };

                TaskResult result = await _stayService.CreateAsync(stay);

                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
                return BadRequest();
            }

            List<Patient> patients = await _patientService.GetAllAsync();
            stayForm.Patients = patients.Select(x => new SelectListItem($"{x.FirstName} {x.Surname}", x.PatientId.ToString())).ToList();

            return PartialView("Partials/_AddStay", stayForm);
        }

        [Authorize(Roles = Roles.AdminUserAuthorize)]
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
                Date = stay.StayDateTime.ToString("dd/MM/yyyy"),
                City = stay.City,
                Note = stay.Note
            };

            return PartialView("Partials/_EditStay", stayForm);
        }

        [Authorize(Roles = Roles.AdminUserAuthorize)]
        [HttpPost]
        public async Task<IActionResult> EditStay(string id, StayFormModel stayForm)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);
                Stay stay = await _stayService.FindByIdAsync(id);
                var dateTime = DateTime.Parse(stayForm.Date);
                stay.StayDateTime = dateTime;
                stay.Note = stayForm.Note;
                stay.City = stayForm.City;
                stay.UpdatedBy = user.Name;

                TaskResult result = await _stayService.UpdateAsync(stay);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
                return BadRequest();
            }

            return PartialView("Partials/_EditStay", stayForm);
        }

        [Authorize(Roles = Roles.AdminUserAuthorize)]
        [HttpPost]
        [IgnoreAntiforgeryToken]
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

            _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
            return BadRequest();
        }

    }
}