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

        public async Task<IActionResult> Index(string sortOrder, string currentSearchNameFilter, string searchNameString, int? page)
        {
            IQueryable<Doctor> doctors = _doctorService.GetAllQueryable(new string[] { "PatientInformationDoctors" });
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchNameString != null)
            {
                page = 1;
            }
            else
            {
                searchNameString = currentSearchNameFilter;
            }

            ViewData["CurrentSearchNameFilter"] = searchNameString;

            if (!string.IsNullOrEmpty(searchNameString))
            {
                doctors = doctors.Where(s => s.Name.Contains(searchNameString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    doctors = doctors.OrderByDescending(s => s.Name);
                    break;
                default:
                    doctors = doctors.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 4;

            PaginatedList<Doctor> paginateList = await PaginatedList<Doctor>.CreateAsync(doctors.AsNoTracking(), page ?? 1, pageSize);
            return View(paginateList);
        }

        public IActionResult AddDoctor()
        {
            return PartialView("_AddDoctor", new DoctorViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDoctor(DoctorViewModel model)
        {
            if (ModelState.IsValid)
            {
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
            }

            return PartialView("_AddDoctor", model);
        }


        public async Task<IActionResult> EditDoctor(string id)
        {
            DoctorViewModel doctorViewModel = new DoctorViewModel();

            if (!string.IsNullOrEmpty(id))
            {
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
            }

            return PartialView("_EditDoctor", doctorViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDoctor(string id, DoctorViewModel model)
        {
            if (ModelState.IsValid)
            {
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
            }

            return PartialView("_EditDoctor", model);
        }


        public async Task<IActionResult> DeleteDoctor(string id)
        {
            string name = string.Empty;

            if (!string.IsNullOrEmpty(id))
            {
                Doctor doctor = await _doctorService.FindByIdAsync(id);
                if (doctor != null)
                {
                    name = doctor.Name;
                }
            }

            return PartialView("_DeleteDoctor", name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDoctor(string id, IFormCollection form)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Doctor doctor = await _doctorService.FindByIdAsync(id);
                if (doctor != null)
                {
                    TaskResult result = await _doctorService.DeleteAsync(doctor);

                    if (result.Succeeded)
                    {
                        return StatusCode(200, "200");
                    }
                    ModelState.AddErrors(result);
                    return PartialView("_DeleteDoctor", doctor.Name);
                }
            }
            return RedirectToAction("Index");
        }

        #region Custom Methods

        public JsonResult IsCRMExist(string Crm, int DoctorId)
        {
            Doctor doctor = ((DoctorStore)_doctorService).FindByCRMAsync(Crm, DoctorId).Result;

            if (doctor != null)
            {
                return Json(false);
            }
            else
            {
                return Json(true);
            }
        }

        #endregion

    }
}
