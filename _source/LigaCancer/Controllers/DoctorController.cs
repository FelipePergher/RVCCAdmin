using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    public class DoctorController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataRepository<Doctor> _doctorService;
        private readonly ILogger<DoctorController> _logger;

        public DoctorController(
            IDataRepository<Doctor> doctorService,
            ILogger<DoctorController> logger,
            UserManager<ApplicationUser> userManager)
        {
            _doctorService = doctorService;
            _userManager = userManager;
            _logger = logger;
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
                var doctor = new Doctor(doctorForm.Name, doctorForm.CRM, await _userManager.GetUserAsync(User));

                TaskResult result = await _doctorService.CreateAsync(doctor);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
                return BadRequest();
            }

            return PartialView("Partials/_AddDoctor", doctorForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditDoctor(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Doctor doctor = await _doctorService.FindByIdAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            return PartialView("Partials/_EditDoctor", new DoctorFormModel(doctor.Name, doctor.CRM, doctor.DoctorId));
        }

        [HttpPost]
        public async Task<IActionResult> EditDoctor(string id, DoctorFormModel doctorForm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);
                Doctor doctor = await _doctorService.FindByIdAsync(id);

                doctor.Name = doctorForm.Name;
                doctor.CRM = doctorForm.CRM;
                doctor.UpdatedTime = DateTime.Now;
                doctor.UpdatedBy = user.Name;

                TaskResult result = await _doctorService.UpdateAsync(doctor);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
                return BadRequest();
            }

            return PartialView("Partials/_EditDoctor", doctorForm);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> DeleteDoctor(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Doctor doctor = await _doctorService.FindByIdAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            TaskResult result = await _doctorService.DeleteAsync(doctor);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
            return BadRequest();
        }

    }
}
