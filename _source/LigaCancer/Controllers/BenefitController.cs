// <copyright file="BenefitController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models;
using RVCC.Models.FormModel;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Controllers
{
    [Authorize(Roles = Roles.AdminUserAuthorize)]
    [AutoValidateAntiforgeryToken]
    public class BenefitController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataRepository<Benefit> _benefitService;
        private readonly ILogger<BenefitController> _logger;

        public BenefitController(
            IDataRepository<Benefit> benefitService,
            ILogger<BenefitController> logger,
            UserManager<ApplicationUser> userManager)
        {
            _benefitService = benefitService;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddBenefit()
        {
            return PartialView("Partials/_AddBenefit", new BenefitFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddBenefit(BenefitFormModel benefitForm)
        {
            if (ModelState.IsValid)
            {
                var benefit = new Benefit(benefitForm.Name, await _userManager.GetUserAsync(User))
                {
                    Note = benefitForm.Note
                };

                TaskResult result = await _benefitService.CreateAsync(benefit);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
                return BadRequest();
            }

            return PartialView("Partials/_AddBenefit", benefitForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditBenefit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Benefit benefit = await _benefitService.FindByIdAsync(id);

            if (benefit == null)
            {
                return NotFound();
            }

            var benefitForm = new BenefitFormModel(benefit.Name, benefit.BenefitId)
            {
                Note = benefit.Note
            };

            return PartialView("Partials/_EditBenefit", benefitForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditBenefit(string id, BenefitFormModel benefitForm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);
                Benefit benefit = await _benefitService.FindByIdAsync(id);

                benefit.Name = benefitForm.Name;
                benefit.Note = benefitForm.Note;
                benefit.UpdatedBy = user.Name;

                TaskResult result = await _benefitService.UpdateAsync(benefit);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
                return BadRequest();
            }

            return PartialView("Partials/_EditBenefit", benefitForm);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> DeleteBenefit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Benefit benefit = await _benefitService.FindByIdAsync(id);

            if (benefit == null)
            {
                return NotFound();
            }

            TaskResult result = await _benefitService.DeleteAsync(benefit);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
            return BadRequest(result);
        }
    }
}
