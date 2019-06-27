﻿using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.FormModel;
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
    public class MedicineController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataStore<Medicine> _medicineService;
        private readonly ILogger<MedicineController> _logger;

        public MedicineController(
            IDataStore<Medicine> medicineService,
            ILogger<MedicineController> logger,
            UserManager<ApplicationUser> userManager)
        {
            _medicineService = medicineService;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddMedicine()
        {
            return PartialView("Partials/_AddMedicine", new MedicineFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddMedicine(MedicineFormModel medicineForm)
        {
            if (ModelState.IsValid)
            {
                var medicine = new Medicine(medicineForm.Name, await _userManager.GetUserAsync(User));

                TaskResult result = await _medicineService.CreateAsync(medicine);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
                return BadRequest();
            }

            return PartialView("Partials/_AddMedicine", medicineForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditMedicine(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Medicine medicine = await _medicineService.FindByIdAsync(id);

            if (medicine == null)
            {
                return NotFound();
            }

            var medicineForm = new MedicineFormModel(medicine.Name, medicine.MedicineId);

            return PartialView("Partials/_EditMedicine", medicineForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditMedicine(string id, MedicineFormModel medicineForm)
        {
            if (ModelState.IsValid)
            {
                Medicine medicine = await _medicineService.FindByIdAsync(id);

                medicine.Name = medicineForm.Name;
                medicine.UpdatedDate = DateTime.Now;
                medicine.UserUpdated = await _userManager.GetUserAsync(User);

                TaskResult result = await _medicineService.UpdateAsync(medicine);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
                return BadRequest();
            }

            return PartialView("Partials/_EditMedicine", medicineForm);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> DeleteMedicine(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Medicine medicine = await _medicineService.FindByIdAsync(id);

            if (medicine == null)
            {
                return NotFound();
            }

            TaskResult result = await _medicineService.DeleteAsync(medicine);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
            return BadRequest(result);
        }

    }
}
