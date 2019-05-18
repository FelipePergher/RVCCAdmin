﻿using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Data.Store;
using LigaCancer.Models.FormModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LigaCancer.Controllers
{
    [Authorize(Roles = "Admin"), AutoValidateAntiforgeryToken]
    public class DoctorController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataStore<Doctor> _doctorService;

        public DoctorController(IDataStore<Doctor> doctorService, UserManager<ApplicationUser> userManager)
        {
            _doctorService = doctorService;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddDoctor()
        {
            return PartialView("Partials/_AddDoctor", new DoctorFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddDoctor(DoctorFormModel doctorForm)
        {
            if (ModelState.IsValid)
            {
                Doctor doctor = new Doctor(doctorForm.Name, doctorForm.CRM, await _userManager.GetUserAsync(User));

                TaskResult result = await _doctorService.CreateAsync(doctor);
                if (result.Succeeded) return Ok();
                return BadRequest(result.Errors);
            }
                
            return PartialView("Partials/_AddDoctor", doctorForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditDoctor(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            Doctor doctor = await _doctorService.FindByIdAsync(id);
            
            if (doctor == null) return NotFound();

            return PartialView("Partials/_EditDoctor", new DoctorFormModel(doctor.Name, doctor.CRM, doctor.DoctorId));
        }

        [HttpPost]
        public async Task<IActionResult> EditDoctor(string id, DoctorFormModel doctorForm)
        {
            if (ModelState.IsValid)
            {
                Doctor doctor = await _doctorService.FindByIdAsync(id);

                doctor.Name = doctorForm.Name;
                doctor.CRM = doctorForm.CRM;
                doctor.UpdatedDate = DateTime.Now;
                doctor.UserUpdated = await _userManager.GetUserAsync(User);

                TaskResult result = await _doctorService.UpdateAsync(doctor);
                if (result.Succeeded) return Ok();
                return BadRequest(result.Errors);
            }
            
            return PartialView("Partials/_EditDoctor", doctorForm);
        }

        [HttpPost, IgnoreAntiforgeryToken]
        public async Task<IActionResult> DeleteDoctor(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            Doctor doctor = await _doctorService.FindByIdAsync(id);

            if (doctor == null) return NotFound();

            TaskResult result = await _doctorService.DeleteAsync(doctor);

            if (result.Succeeded) return Ok();
            return BadRequest(result.Errors);
        }

    }
}
