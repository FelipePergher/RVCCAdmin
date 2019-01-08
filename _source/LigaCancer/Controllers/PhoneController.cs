using System;
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
    public class PhoneController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataStore<Phone> _phoneService;
        private readonly IDataStore<Patient> _patientService;

        public PhoneController(IDataStore<Phone> phoneService, UserManager<ApplicationUser> userManager, IDataStore<Patient> patientService)
        {
            _phoneService = phoneService;
            _userManager = userManager;
            _patientService = patientService;
        }


        [HttpGet]
        public IActionResult AddPhone(string id)
        {
            PhoneViewModel phoneViewModel = new PhoneViewModel
            {
                PatientId = id
            };
            return PartialView("_AddPhone", phoneViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPhone(PhoneViewModel model)
        {
            if (!ModelState.IsValid) return PartialView("_AddPhone", model);

            ApplicationUser user = await _userManager.GetUserAsync(this.User);

            TaskResult result = await ((PatientStore)_patientService).AddPhone(
                new Phone
                {
                    Number = model.Number,
                    PhoneType = model.PhoneType,
                    ObservationNote = model.ObservationNote,
                    UserCreated = user
                }, model.PatientId);

            if (result.Succeeded)
            {
                return StatusCode(200, "phone");
            }
            ModelState.AddErrors(result);

            return PartialView("_AddPhone", model);
        }

        [HttpGet]
        public async Task<IActionResult> EditPhone(string id)
        {
            PhoneViewModel phoneViewModel = new PhoneViewModel();

            if (string.IsNullOrEmpty(id)) return PartialView("_EditPhone", phoneViewModel);

            Phone phone = await _phoneService.FindByIdAsync(id);
            if (phone != null)
            {
                phoneViewModel = new PhoneViewModel
                {
                    Number = phone.Number,
                    PhoneType = phone.PhoneType,
                    ObservationNote = phone.ObservationNote
                };
            }

            return PartialView("_EditPhone", phoneViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPhone(string id, PhoneViewModel model)
        {
            if (!ModelState.IsValid) return PartialView("_EditPhone", model);

            Phone phone = await _phoneService.FindByIdAsync(id);
            ApplicationUser user = await _userManager.GetUserAsync(this.User);

            phone.Number = model.Number;
            phone.PhoneType = model.PhoneType;
            phone.ObservationNote = model.ObservationNote;
            phone.LastUpdatedDate = DateTime.Now;
            phone.LastUserUpdate = user;

            TaskResult result = await _phoneService.UpdateAsync(phone);
            if (result.Succeeded)
            {
                return StatusCode(200, "phone");
            }
            ModelState.AddErrors(result);

            return PartialView("_EditPhone", model);
        }


        [HttpGet]
        public async Task<IActionResult> DeletePhone(string id)
        {
            string number = string.Empty;

            if (string.IsNullOrEmpty(id)) return PartialView("_DeletePhone", number);

            Phone phone = await _phoneService.FindByIdAsync(id);
            if (phone != null)
            {
                number = phone.Number;
            }

            return PartialView("_DeletePhone", number);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePhone(string id, IFormCollection form)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index");

            Phone phone = await _phoneService.FindByIdAsync(id);
            if (phone == null) return RedirectToAction("Index");

            TaskResult result = await _phoneService.DeleteAsync(phone);

            if (result.Succeeded)
            {
                return StatusCode(200, "phone");
            }
            ModelState.AddErrors(result);
            return PartialView("_DeletePhone", phone.Number);
        }

    }
}
