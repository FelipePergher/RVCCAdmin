﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models;
using RVCC.Data.Models.PatientModels;
using RVCC.Models.FormModel;
using RVCC.Models.SearchModel;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Controllers
{
    [Authorize(Roles = Roles.AdminAndUserAuthorize)]
    [AutoValidateAntiforgeryToken]
    public class FamilyMemberController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataRepository<FamilyMember> _familyMemberService;
        private readonly IDataRepository<Patient> _patientService;
        private readonly ILogger<FamilyMemberController> _logger;

        public FamilyMemberController(
            IDataRepository<FamilyMember> familyMemberService,
            IDataRepository<Patient> patientService,
            ILogger<FamilyMemberController> logger,
            UserManager<ApplicationUser> userManager)
        {
            _familyMemberService = familyMemberService;
            _userManager = userManager;
            _patientService = patientService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            return PartialView("Partials/_Index", new FamilyMemberSearchModel(id));
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

                ApplicationUser user = await _userManager.GetUserAsync(User);
                var familyMember = new FamilyMember
                {
                    PatientId = int.Parse(id),
                    DateOfBirth = string.IsNullOrEmpty(familyMemberForm.DateOfBirth) ? (DateTime?)null : DateTime.Parse(familyMemberForm.DateOfBirth),
                    Kinship = familyMemberForm.Kinship,
                    MonthlyIncome =
                        (double)(decimal.TryParse(familyMemberForm.MonthlyIncome, out decimal monthlyIncome) ? monthlyIncome : 0),
                    Name = familyMemberForm.Name,
                    Sex = familyMemberForm.Sex,
                    CreatedBy = user.Name
                };

                TaskResult result = await _familyMemberService.CreateAsync(familyMember);

                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
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
                DateOfBirth = familyMember.DateOfBirth.HasValue ? familyMember.DateOfBirth.Value.ToString("dd/MM/yyyy") : "",
                Kinship = familyMember.Kinship,
                MonthlyIncome = familyMember.MonthlyIncome.ToString("C2"),
                Name = familyMember.Name,
                Sex = familyMember.Sex
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditFamilyMember(string id, FamilyMemberFormModel familyMemberForm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);
                FamilyMember familyMember = await _familyMemberService.FindByIdAsync(id);

                familyMember.DateOfBirth = string.IsNullOrEmpty(familyMemberForm.DateOfBirth) ? (DateTime?)null : DateTime.Parse(familyMemberForm.DateOfBirth);
                familyMember.Kinship = familyMemberForm.Kinship;
                familyMember.MonthlyIncome =
                    (double)(decimal.TryParse(familyMemberForm.MonthlyIncome, out decimal monthlyIncome) ? monthlyIncome : 0);
                familyMember.Name = familyMemberForm.Name;
                familyMember.Sex = familyMemberForm.Sex;
                familyMember.UpdatedTime = DateTime.Now;
                familyMember.UpdatedBy = user.Name;

                TaskResult result = await _familyMemberService.UpdateAsync(familyMember);

                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
                return BadRequest();
            }
            return PartialView("Partials/_EditFamilyMember", familyMemberForm);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
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

            _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
            return BadRequest();
        }

    }
}