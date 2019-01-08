﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.MedicalViewModels;
using LigaCancer.Code;
using LigaCancer.Data.Store;
using LigaCancer.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using LigaCancer.Code.Interface;

namespace LigaCancer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DoctorController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataStore<Doctor> _doctorService;

        public DoctorController(IDataStore<Doctor> doctorService, UserManager<ApplicationUser> userManager)
        {
            _doctorService = doctorService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddDoctor()
        {
            return PartialView("_AddDoctor", new DoctorViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDoctor(DoctorViewModel model)
        {
            if (!ModelState.IsValid) return PartialView("_AddDoctor", model);

            ApplicationUser user = await _userManager.GetUserAsync(this.User);
            Doctor doctor = new Doctor
            {
                CRM = model.CRM,
                Name = model.Name,
                UserCreated = user
            };

            TaskResult result = await _doctorService.CreateAsync(doctor);
            if (result.Succeeded)
            {
                return StatusCode(200, "200");
            }
            ModelState.AddErrors(result);

            return PartialView("_AddDoctor", model);
        }

        public async Task<IActionResult> EditDoctor(string id)
        {
            DoctorViewModel doctorViewModel = new DoctorViewModel();

            if (string.IsNullOrEmpty(id)) return PartialView("_EditDoctor", doctorViewModel);

            Doctor doctor = await _doctorService.FindByIdAsync(id);
            if(doctor != null)
            {
                doctorViewModel = new DoctorViewModel
                {
                    DoctorId = doctor.DoctorId,
                    CRM = doctor.CRM,
                    Name = doctor.Name
                };
            }

            return PartialView("_EditDoctor", doctorViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDoctor(string id, DoctorViewModel model)
        {
            if (!ModelState.IsValid) return PartialView("_EditDoctor", model);

            Doctor doctor = await _doctorService.FindByIdAsync(id);
            ApplicationUser user = await _userManager.GetUserAsync(this.User);

            doctor.Name = model.Name;
            doctor.CRM = model.CRM;
            doctor.LastUpdatedDate = DateTime.Now;
            doctor.LastUserUpdate = user;

            TaskResult result = await _doctorService.UpdateAsync(doctor);
            if (result.Succeeded)
            {
                return StatusCode(200, "200");
            }
            ModelState.AddErrors(result);

            return PartialView("_EditDoctor", model);
        }


        public async Task<IActionResult> DeleteDoctor(string id)
        {
            string name = string.Empty;

            if (string.IsNullOrEmpty(id)) return PartialView("_DeleteDoctor", name);

            Doctor doctor = await _doctorService.FindByIdAsync(id);
            if (doctor != null)
            {
                name = doctor.Name;
            }

            return PartialView("_DeleteDoctor", name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDoctor(string id, IFormCollection form)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index");

            Doctor doctor = await _doctorService.FindByIdAsync(id);
            if (doctor == null) return RedirectToAction("Index");

            TaskResult result = await _doctorService.DeleteAsync(doctor);

            if (result.Succeeded)
            {
                return StatusCode(200, "200");
            }
            ModelState.AddErrors(result);
            return PartialView("_DeleteDoctor", doctor.Name);
        }

        #region Custom Methods

        public JsonResult IsCrmExist(string crm, int doctorId)
        {
            Doctor doctor = ((DoctorStore)_doctorService).FindByCrmAsync(crm, doctorId).Result;

            return Json(doctor == null);
        }

        #endregion

    }
}
