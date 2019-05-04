using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.FormModel;
using LigaCancer.Models.SearchModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LigaCancer.Controllers
{
    [Authorize(Roles = "Admin"), AutoValidateAntiforgeryToken]
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
        public IActionResult Index(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();
            return PartialView("Partials/_Index", new PhoneSearchModel(id));
        }

        [HttpGet]
        public IActionResult AddPhone(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();
            return PartialView("Partials/_AddPhone", new PhoneFormModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddPhone(PhoneFormModel phoneForm)
        {
            if (string.IsNullOrEmpty(phoneForm.PatientId)) return BadRequest();

            if (ModelState.IsValid)
            {
                if (await _patientService.FindByIdAsync(phoneForm.PatientId) == null) return NotFound();

                Phone phone = new Phone(phoneForm.PatientId, phoneForm.Number, phoneForm.PhoneType, phoneForm.ObservationNote, await _userManager.GetUserAsync(User));

                TaskResult result = await _phoneService.CreateAsync(phone);

                if (result.Succeeded) return Ok();
                return BadRequest(result.Errors);
            }

            return PartialView("Partials/_AddPhone", phoneForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditPhone(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            Phone phone = await _phoneService.FindByIdAsync(id);

            if (phone == null) return NotFound();

            return PartialView("Partials/_EditPhone", new PhoneFormModel(int.Parse(id))
            {
                Number = phone.Number,
                PhoneType = phone.PhoneType,
                ObservationNote = phone.ObservationNote
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditPhone(string id, PhoneFormModel phoneForm)
        {
            if (ModelState.IsValid)
            {
                Phone phone = await _phoneService.FindByIdAsync(id);

                phone.Number = phoneForm.Number;
                phone.PhoneType = phoneForm.PhoneType;
                phone.ObservationNote = phoneForm.ObservationNote;
                phone.UpdatedDate = DateTime.Now;
                phone.UserUpdated = await _userManager.GetUserAsync(User);

                TaskResult result = await _phoneService.UpdateAsync(phone);
                if (result.Succeeded) return Ok();
                return BadRequest(result.Errors);
            }

            return PartialView("Partials/_EditPhone", phoneForm);
        }

        [HttpPost, IgnoreAntiforgeryToken]
        public async Task<IActionResult> DeletePhone(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            Phone phone = await _phoneService.FindByIdAsync(id);

            if (phone == null) return NotFound();

            TaskResult result = await _phoneService.DeleteAsync(phone);

            if (result.Succeeded) return Ok();
            return BadRequest(result.Errors);
        }

    }
}
