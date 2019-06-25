﻿using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.FormModel;
using LigaCancer.Models.SearchModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, User")]
    [AutoValidateAntiforgeryToken]
    [Area("Admin")]
    public class PhoneController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataStore<Phone> _phoneService;
        private readonly IDataStore<Patient> _patientService;
        private readonly ILogger<PhoneController> _logger;

        public PhoneController(
            IDataStore<Phone> phoneService,
            IDataStore<Patient> patientService,
            ILogger<PhoneController> logger, 
            UserManager<ApplicationUser> userManager)
        {
            _phoneService = phoneService;
            _userManager = userManager;
            _patientService = patientService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();
            return PartialView("Partials/_Index", new PhoneSearchModel(id));
        }

        [HttpGet]
        public IActionResult AddPhone(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();
            return PartialView("Partials/_AddPhone", new PhoneFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddPhone(string id, PhoneFormModel phoneForm)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            if (ModelState.IsValid)
            {
                if (await _patientService.FindByIdAsync(id) == null) return NotFound();

                Phone phone = new Phone(id, phoneForm.Number, phoneForm.PhoneType, phoneForm.ObservationNote, await _userManager.GetUserAsync(User));

                TaskResult result = await _phoneService.CreateAsync(phone);

                if (result.Succeeded) return Ok();
                _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
                return BadRequest();
            }

            return PartialView("Partials/_AddPhone", phoneForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditPhone(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            Phone phone = await _phoneService.FindByIdAsync(id);

            if (phone == null) return NotFound();

            return PartialView("Partials/_EditPhone", new PhoneFormModel
            {
                Number = phone.Number,
                PhoneType = phone.PhoneType,
                ObservationNote = phone.ObservationNote
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditPhone(string id, PhoneFormModel phoneForm)
        {
            if (ModelState.IsValid)
            {
                Phone phone = await _phoneService.FindByIdAsync(id);

                phone.Number = phoneForm.Number;
                phone.PhoneType = phoneForm.PhoneType;
                phone.ObservationNote = phoneForm.ObservationNote;
                phone.UpdatedDate = DateTime.Now;
                phone.UserUpdated = await _userManager.GetUserAsync(User);

                TaskResult result = await _phoneService.UpdateAsync(phone);
                if (result.Succeeded) return Ok();
                _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
                return BadRequest();
            }

            return PartialView("Partials/_EditPhone", phoneForm);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> DeletePhone(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            Phone phone = await _phoneService.FindByIdAsync(id);

            if (phone == null) return NotFound();

            TaskResult result = await _phoneService.DeleteAsync(phone);

            if (result.Succeeded) return Ok();
            _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
            return BadRequest();
        }

    }
}