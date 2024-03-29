﻿// <copyright file="PhoneController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models.Domain;
using RVCC.Models.FormModel;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Controllers
{
    [Authorize(Roles = Roles.AdminSecretarySocialAssistanceAuthorize)]
    [AutoValidateAntiforgeryToken]
    public class PhoneController : Controller
    {
        private readonly IDataRepository<Phone> _phoneService;
        private readonly IDataRepository<Patient> _patientService;
        private readonly ILogger<PhoneController> _logger;

        public PhoneController(IDataRepository<Phone> phoneService, IDataRepository<Patient> patientService, ILogger<PhoneController> logger)
        {
            _phoneService = phoneService;
            _patientService = patientService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult AddPhone(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            return PartialView("Partials/_AddPhone", new PhoneFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddPhone(string id, PhoneFormModel phoneForm)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                if (await _patientService.FindByIdAsync(id) == null)
                {
                    return NotFound();
                }

                var phone = new Phone(id, phoneForm.Number, phoneForm.PhoneType, phoneForm.ObservationNote);

                TaskResult result = await _phoneService.CreateAsync(phone);

                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.InsertItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_AddPhone", phoneForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditPhone(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Phone phone = await _phoneService.FindByIdAsync(id);

            if (phone == null)
            {
                return NotFound();
            }

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

                if (phone == null)
                {
                    return NotFound();
                }

                phone.Number = phoneForm.Number;
                phone.PhoneType = phoneForm.PhoneType;
                phone.ObservationNote = phoneForm.ObservationNote;

                TaskResult result = await _phoneService.UpdateAsync(phone);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.UpdateItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_EditPhone", phoneForm);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePhone(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Phone phone = await _phoneService.FindByIdAsync(id);

            if (phone == null)
            {
                return NotFound();
            }

            TaskResult result = await _phoneService.DeleteAsync(phone);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(LogEvents.DeleteItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
            return BadRequest();
        }
    }
}
