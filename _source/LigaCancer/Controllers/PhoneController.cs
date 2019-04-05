using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.FormModel;
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
            PhoneFormModel phoneForm = new PhoneFormModel
            {
                PatientId = id
            };
            return PartialView("_AddPhone", phoneForm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPhone(PhoneFormModel phoneForm)
        {
            if (!ModelState.IsValid) return PartialView("_AddPhone", phoneForm);

            ApplicationUser user = await _userManager.GetUserAsync(User);

            TaskResult result = await ((PatientStore)_patientService).AddPhone(
                new Phone
                {
                    Number = phoneForm.Number,
                    PhoneType = phoneForm.PhoneType,
                    ObservationNote = phoneForm.ObservationNote,
                    UserCreated = user
                }, phoneForm.PatientId);

            if (result.Succeeded)
            {
                return StatusCode(200, "phone");
            }
            ModelState.AddErrors(result);

            return PartialView("_AddPhone", phoneForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditPhone(string id)
        {
            PhoneFormModel phoneForm = new PhoneFormModel();

            if (string.IsNullOrEmpty(id)) return PartialView("_EditPhone", phoneForm);

            Phone phone = await _phoneService.FindByIdAsync(id);
            if (phone != null)
            {
                phoneForm = new PhoneFormModel
                {
                    Number = phone.Number,
                    PhoneType = phone.PhoneType,
                    ObservationNote = phone.ObservationNote
                };
            }

            return PartialView("_EditPhone", phoneForm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPhone(string id, PhoneFormModel phoneForm)
        {
            if (!ModelState.IsValid) return PartialView("_EditPhone", phoneForm);

            Phone phone = await _phoneService.FindByIdAsync(id);
            ApplicationUser user = await _userManager.GetUserAsync(User);

            phone.Number = phoneForm.Number;
            phone.PhoneType = phoneForm.PhoneType;
            phone.ObservationNote = phoneForm.ObservationNote;
            phone.UpdatedDate = DateTime.Now;
            phone.UserUpdated = user;

            TaskResult result = await _phoneService.UpdateAsync(phone);
            if (result.Succeeded)
            {
                return StatusCode(200, "phone");
            }
            ModelState.AddErrors(result);

            return PartialView("_EditPhone", phoneForm);
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
