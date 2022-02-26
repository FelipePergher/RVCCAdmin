// <copyright file="FamilyMemberController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models;
using RVCC.Models.FormModel;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Controllers
{
    [Authorize(Roles = Roles.AdminUserAuthorize)]
    [AutoValidateAntiforgeryToken]
    public class FamilyMemberController : Controller
    {
        private readonly IDataRepository<FamilyMember> _familyMemberService;
        private readonly IDataRepository<Patient> _patientService;
        private readonly ILogger<FamilyMemberController> _logger;

        public FamilyMemberController(IDataRepository<FamilyMember> familyMemberService, IDataRepository<Patient> patientService, ILogger<FamilyMemberController> logger)
        {
            _familyMemberService = familyMemberService;
            _patientService = patientService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult AddFamilyMember(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            return PartialView("Partials/_AddFamilyMember", new FamilyMemberFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddFamilyMember(string id, FamilyMemberFormModel familyMemberForm)
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

                var familyMember = new FamilyMember
                {
                    PatientId = int.Parse(id),
                    DateOfBirth = string.IsNullOrEmpty(familyMemberForm.DateOfBirth) ? (DateTime?)null : DateTime.Parse(familyMemberForm.DateOfBirth),
                    Kinship = familyMemberForm.Kinship,
                    MonthlyIncomeMinSalary = (double)(decimal.TryParse(familyMemberForm.MonthlyIncomeMinSalary, out decimal monthlyIncomeMinSalary) ? monthlyIncomeMinSalary : 0),
                    Name = familyMemberForm.Name,
                    Sex = familyMemberForm.Sex,
                };

                TaskResult result = await _familyMemberService.CreateAsync(familyMember);

                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.InsertItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_AddFamilyMember", familyMemberForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditFamilyMember(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            FamilyMember familyMember = await _familyMemberService.FindByIdAsync(id);

            if (familyMember == null)
            {
                return NotFound();
            }

            return PartialView("Partials/_EditFamilyMember", new FamilyMemberFormModel
            {
                DateOfBirth = familyMember.DateOfBirth.HasValue ? familyMember.DateOfBirth.Value.ToString("dd/MM/yyyy") : string.Empty,
                Kinship = familyMember.Kinship,
                MonthlyIncome = familyMember.MonthlyIncome.ToString("C2"),
                MonthlyIncomeMinSalary = familyMember.MonthlyIncomeMinSalary.ToString("N2"),
                Name = familyMember.Name,
                Sex = familyMember.Sex
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditFamilyMember(string id, FamilyMemberFormModel familyMemberForm)
        {
            if (ModelState.IsValid)
            {
                FamilyMember familyMember = await _familyMemberService.FindByIdAsync(id);

                familyMember.DateOfBirth = string.IsNullOrEmpty(familyMemberForm.DateOfBirth) ? (DateTime?)null : DateTime.Parse(familyMemberForm.DateOfBirth);
                familyMember.Kinship = familyMemberForm.Kinship;
                familyMember.MonthlyIncomeMinSalary = (double)(decimal.TryParse(familyMemberForm.MonthlyIncomeMinSalary, out decimal monthlyIncomeMinSalary) ? monthlyIncomeMinSalary : 0);
                familyMember.Name = familyMemberForm.Name;
                familyMember.Sex = familyMemberForm.Sex;

                TaskResult result = await _familyMemberService.UpdateAsync(familyMember);

                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.UpdateItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_EditFamilyMember", familyMemberForm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFamilyMember(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            FamilyMember familyMember = await _familyMemberService.FindByIdAsync(id);

            if (familyMember == null)
            {
                return NotFound();
            }

            TaskResult result = await _familyMemberService.DeleteAsync(familyMember);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(LogEvents.DeleteItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
            return BadRequest();
        }
    }
}
