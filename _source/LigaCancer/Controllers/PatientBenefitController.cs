// <copyright file="PatientBenefitController.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models;
using RVCC.Data.Models.RelationModels;
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
    public class PatientBenefitController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataRepository<PatientBenefit> _patientBenefitService;
        private readonly IDataRepository<Patient> _patientService;
        private readonly IDataRepository<Benefit> _benefitService;
        private readonly ILogger<PatientBenefitController> _logger;

        public PatientBenefitController(
            IDataRepository<PatientBenefit> patientBenefitService,
            IDataRepository<Patient> patientService,
            IDataRepository<Benefit> benefitService,
            ILogger<PatientBenefitController> logger,
            UserManager<ApplicationUser> userManager)
        {
            _patientBenefitService = patientBenefitService;
            _patientService = patientService;
            _benefitService = benefitService;
            _userManager = userManager;
            _logger = logger;
        }

        [Authorize(Roles = Roles.AdminUserSocialAssistanceAuthorize)]
        [HttpGet]
        public IActionResult Index()
        {
            return View(new PatientBenefitSearchModel());
        }

        [Authorize(Roles = Roles.AdminUserAuthorize)]
        [HttpGet]
        public async Task<IActionResult> AddPatientBenefit()
        {
            List<Patient> patients = await _patientService.GetAllAsync();
            List<Benefit> benefits = await _benefitService.GetAllAsync();
            var patientBenefitForm = new PatientBenefitFormModel
            {
                Patients = patients.Select(x => new SelectListItem($"{x.FirstName} {x.Surname}", x.PatientId.ToString())).ToList(),
                BenefitsList = benefits.Select(x => new SelectListItem(x.Name, x.BenefitId.ToString())).ToList()
            };

            return PartialView("Partials/_AddPatientBenefit", patientBenefitForm);
        }

        [Authorize(Roles = Roles.AdminUserAuthorize)]
        [HttpPost]
        public async Task<IActionResult> AddPatientBenefit(PatientBenefitFormModel patientBenefitForm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);
                Patient patient = await _patientService.FindByIdAsync(patientBenefitForm.PatientId);
                var dateTime = DateTime.Parse(patientBenefitForm.Date);
                var patientBenefit = new PatientBenefit
                {
                    BenefitDate = dateTime,
                    PatientId = patient.PatientId,
                    Quantity = patientBenefitForm.Quantity,
                    BenefitId = int.Parse(patientBenefitForm.Benefit),
                    CreatedBy = user.Name
                };

                TaskResult result = await _patientBenefitService.CreateAsync(patientBenefit);

                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
                return BadRequest();
            }

            List<Patient> patients = await _patientService.GetAllAsync();
            List<Benefit> benefits = await _benefitService.GetAllAsync();
            patientBenefitForm.Patients = patients.Select(x => new SelectListItem($"{x.FirstName} {x.Surname}", x.PatientId.ToString())).ToList();
            patientBenefitForm.BenefitsList = benefits.Select(x => new SelectListItem(x.Name, x.BenefitId.ToString())).ToList();

            return PartialView("Partials/_AddPatientBenefit", patientBenefitForm);
        }

        [Authorize(Roles = Roles.AdminUserAuthorize)]
        [HttpGet("PatientBenefit/EditPatientBenefit/{patientId}/{benefitId}")]
        public async Task<IActionResult> EditPatientBenefit(int patientId, int benefitId)
        {
            if (patientId == 0 || benefitId == 0)
            {
                return BadRequest();
            }

            PatientBenefit patientBenefit = await ((PatientBenefitRepository)_patientBenefitService).FindByIdsAsync(patientId, benefitId, new[] { "Patient", "Benefit" });

            if (patientBenefit == null)
            {
                return NotFound();
            }

            var patientBenefitForm = new PatientBenefitFormModel
            {
                PatientIdHidden = patientBenefit.PatientId,
                BenefitIdHidden = patientBenefit.BenefitId,
                PatientId = $"{patientBenefit.Patient.FirstName} {patientBenefit.Patient.Surname}",
                Date = patientBenefit.BenefitDate.ToString("dd/MM/yyyy"),
                Benefit = patientBenefit.Benefit.Name,
                Quantity = patientBenefit.Quantity,
            };

            return PartialView("Partials/_EditPatientBenefit", patientBenefitForm);
        }

        [Authorize(Roles = Roles.AdminUserAuthorize)]
        [HttpPost("PatientBenefit/EditPatientBenefit/{patientId}/{benefitId}")]
        public async Task<IActionResult> EditPatientBenefit(PatientBenefitFormModel patientBenefitForm)
        {
            if (patientBenefitForm.PatientIdHidden == 0 || patientBenefitForm.BenefitIdHidden == 0)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);
                PatientBenefit patientBenefit = await ((PatientBenefitRepository)_patientBenefitService).FindByIdsAsync(patientBenefitForm.PatientIdHidden, patientBenefitForm.BenefitIdHidden);
                var dateTime = DateTime.Parse(patientBenefitForm.Date);
                patientBenefit.BenefitDate = dateTime;
                patientBenefit.Quantity = patientBenefitForm.Quantity;
                patientBenefit.UpdatedBy = user.Name;

                TaskResult result = await _patientBenefitService.UpdateAsync(patientBenefit);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
                return BadRequest();
            }

            List<Benefit> benefits = await _benefitService.GetAllAsync();
            patientBenefitForm.BenefitsList = benefits.Select(x => new SelectListItem(x.Name, x.BenefitId.ToString())).ToList();

            return PartialView("Partials/_EditPatientBenefit", patientBenefitForm);
        }

        [Authorize(Roles = Roles.AdminUserAuthorize)]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> DeletePatientBenefit(int patientId, int benefitId)
        {
            if (patientId == 0 || benefitId == 0)
            {
                return BadRequest();
            }

            PatientBenefit patientBenefit = await ((PatientBenefitRepository)_patientBenefitService).FindByIdsAsync(patientId, benefitId);

            if (patientBenefit == null)
            {
                return NotFound();
            }

            TaskResult result = await _patientBenefitService.DeleteAsync(patientBenefit);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
            return BadRequest();
        }
    }
}