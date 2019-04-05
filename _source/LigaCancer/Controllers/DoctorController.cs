using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.FormModel;
using LigaCancer.Code;
using LigaCancer.Data.Store;
using LigaCancer.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using LigaCancer.Code.Interface;

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
                ApplicationUser user = await _userManager.GetUserAsync(User);
                Doctor doctor = new Doctor
                {
                    CRM = doctorForm.CRM,
                    Name = doctorForm.Name,
                    UserCreated = user
                };

                TaskResult result = await _doctorService.CreateAsync(doctor);
                if (result.Succeeded) return Ok();

                ModelState.AddErrors(result);
            }
                
            return PartialView("Partials/_AddDoctor", doctorForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditDoctor(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            Doctor doctor = await _doctorService.FindByIdAsync(id);
            if (doctor == null) return NotFound();

            return PartialView("Partials/_EditDoctor", new DoctorFormModel
            {
                DoctorId = doctor.DoctorId,
                CRM = doctor.CRM,
                Name = doctor.Name
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditDoctor(string id, DoctorFormModel doctorForm)
        {
            if (ModelState.IsValid)
            {
                Doctor doctor = await _doctorService.FindByIdAsync(id);
                ApplicationUser user = await _userManager.GetUserAsync(User);

                doctor.Name = doctorForm.Name;
                doctor.CRM = doctorForm.CRM;
                doctor.UpdatedDate = DateTime.Now;
                doctor.UserUpdated = user;

                TaskResult result = await _doctorService.UpdateAsync(doctor);
                if (result.Succeeded) return Ok();

                ModelState.AddErrors(result);
            }
            
            return PartialView("Partials/_EditDoctor", doctorForm);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteDoctor(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            Doctor doctor = await _doctorService.FindByIdAsync(id);

            if (doctor == null) return NotFound();

            TaskResult result = await _doctorService.DeleteAsync(doctor);

            if (result.Succeeded) return Ok();

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> IsCrmExist(string crm, int doctorId)
        {
            Doctor doctor = await ((DoctorStore)_doctorService).FindByCrmAsync(crm, doctorId);
            return Json(doctor == null);
        }

    }
}
