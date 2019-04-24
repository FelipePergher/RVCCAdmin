using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Data.Store;
using LigaCancer.Models.FormModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LigaCancer.Controllers
{
    [Authorize(Roles = "Admin"), AutoValidateAntiforgeryToken]
    public class AddressController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataStore<Address> _addressService;
        private readonly IDataStore<Patient> _patientService;

        public AddressController(IDataStore<Address> addressService, UserManager<ApplicationUser> userManager, IDataStore<Patient> patientService)
        {
            _addressService = addressService;
            _userManager = userManager;
            _patientService = patientService;
        }

        [HttpGet]
        public IActionResult AddAddress(string id)
        {
            AddressFormModel addressForm = new AddressFormModel
            {
                PatientId = id
            };
            return PartialView("_AddAddress", addressForm);
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress(AddressFormModel addressForm)
        {
            if (!ModelState.IsValid) return PartialView("_AddAddress", addressForm);

            ApplicationUser user = await _userManager.GetUserAsync(User);

            TaskResult result = await ((PatientStore)_patientService).AddAddress(
                new Address
                {
                    City = addressForm.City,
                    Complement = addressForm.Complement,
                    HouseNumber = addressForm.HouseNumber,
                    Neighborhood = addressForm.Neighborhood,
                    ObservationAddress = addressForm.ObservationAddress,
                    Street = addressForm.Street,
                    ResidenceType = addressForm.ResidenceType,
                    MonthlyAmmountResidence = addressForm.ResidenceType != null ? addressForm.MonthlyAmmountResidence : 0
                }, addressForm.PatientId);

            if (result.Succeeded)
            {
                return StatusCode(200, "address");
            }
            ModelState.AddErrors(result);

            return PartialView("_AddAddress", addressForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditAddress(string id)
        {
            AddressFormModel addressForm = new AddressFormModel();

            if (string.IsNullOrEmpty(id)) return PartialView("_EditAddress", addressForm);

            Address address = await _addressService.FindByIdAsync(id);
            if (address != null)
            {
                addressForm = new AddressFormModel
                {
                    City = address.City,
                    Complement = address.Complement,
                    HouseNumber = address.HouseNumber,
                    Neighborhood = address.Neighborhood,
                    ObservationAddress = address.ObservationAddress,
                    Street = address.Street,
                    ResidenceType = address.ResidenceType,
                    MonthlyAmmountResidence = address.MonthlyAmmountResidence
                };
            }

            return PartialView("_EditAddress", addressForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditAddress(string id, AddressFormModel addressForm)
        {
            if (!ModelState.IsValid) return PartialView("_EditAddress", addressForm);

            Address address = await _addressService.FindByIdAsync(id);
            ApplicationUser user = await _userManager.GetUserAsync(User);

            address.Complement = addressForm.Complement;
            address.City = addressForm.City;
            address.HouseNumber = addressForm.HouseNumber;
            address.Neighborhood = addressForm.Neighborhood;
            address.ObservationAddress = addressForm.ObservationAddress;
            address.ResidenceType = addressForm.ResidenceType;
            address.MonthlyAmmountResidence = addressForm.ResidenceType != null ? addressForm.MonthlyAmmountResidence : 0;
            address.Street = addressForm.Street;
            address.UpdatedDate = DateTime.Now;
            address.UserUpdated = user;

            TaskResult result = await _addressService.UpdateAsync(address);
            if (result.Succeeded)
            {
                return StatusCode(200, "address");
            }
            ModelState.AddErrors(result);

            return PartialView("_EditAddress", addressForm);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAddress(string id)
        {
            string addressText = string.Empty;

            if (string.IsNullOrEmpty(id)) return PartialView("_DeleteAddress", addressText);

            Address address = await _addressService.FindByIdAsync(id);
            if (address != null)
            {
                addressText = $"{address.Street} {address.Neighborhood} nº {address.HouseNumber}";
            }

            return PartialView("_DeleteAddress", addressText);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAddress(string id, IFormCollection form)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Address address = await _addressService.FindByIdAsync(id);
                if (address == null) return RedirectToAction("Index");

                TaskResult result = await _addressService.DeleteAsync(address);

                if (result.Succeeded)
                {
                    return StatusCode(200, "address");
                }
                ModelState.AddErrors(result);
                return PartialView("_DeleteAddress", $"{address.Street} {address.Neighborhood} nº {address.HouseNumber}");
            }
            return RedirectToAction("Index");
        }

    }
}
