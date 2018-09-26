using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LigaCancer.Data;
using LigaCancer.Data.Models.Patient;
using LigaCancer.Models.MedicalViewModels;
using LigaCancer.Code;
using LigaCancer.Data.Store;
using LigaCancer.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace LigaCancer.Controllers
{
    public class DoctorController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataStore<Doctor> _doctorService;

        public DoctorController(IDataStore<Doctor> doctorService, UserManager<ApplicationUser> userManager)
        {
            _doctorService = doctorService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _doctorService.GetAllAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _doctorService.FindByIdAsync(id.ToString());
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        public IActionResult AddDoctor()
        {
            return PartialView("_AddDoctor", new DoctorViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDoctor(DoctorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Doctor doctor = new Doctor
                {
                    CRM = model.CRM,
                    Name = model.Name
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

    }
}
