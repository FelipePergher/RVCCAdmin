using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Data.Store;
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
        public IActionResult Index(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();
            return PartialView("Partials/_Index", new AddressSearchModel(id));
        }

        [HttpGet]
        public IActionResult AddAddress(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();
            return PartialView("Partials/_AddAddress", new AddressFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress(int id, AddressFormModel addressForm)
        {
            if (ModelState.IsValid)
            {
                Address address = new Address
                {
                    PatientId = id,
                    City = addressForm.City,
                    Complement = addressForm.Complement,
                    HouseNumber = addressForm.HouseNumber,
                    Neighborhood = addressForm.Neighborhood,
                    ObservationAddress = addressForm.ObservationAddress,
                    Street = addressForm.Street,
                    ResidenceType = addressForm.ResidenceType,
                    MonthlyAmmountResidence = addressForm.ResidenceType != null ? addressForm.MonthlyAmmountResidence : 0,
                    UserCreated = await _userManager.GetUserAsync(User)
                };

                TaskResult result = await _addressService.CreateAsync(address);

                if (result.Succeeded) return Ok();
                return BadRequest(result.Errors);
            }

            return PartialView("Partials/_AddAddress", addressForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditAddress(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            Address address = await _addressService.FindByIdAsync(id);

            if (address == null) return NotFound();

            AddressFormModel addressForm = new AddressFormModel
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

            return PartialView("Partials/_EditAddress", addressForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditAddress(string id, AddressFormModel addressForm)
        {
            if (ModelState.IsValid)
            {
                Address address = await _addressService.FindByIdAsync(id);

                address.Complement = addressForm.Complement;
                address.City = addressForm.City;
                address.HouseNumber = addressForm.HouseNumber;
                address.Neighborhood = addressForm.Neighborhood;
                address.ObservationAddress = addressForm.ObservationAddress;
                address.ResidenceType = addressForm.ResidenceType;
                address.MonthlyAmmountResidence = addressForm.ResidenceType != null ? addressForm.MonthlyAmmountResidence : 0;
                address.Street = addressForm.Street;
                address.UpdatedDate = DateTime.Now;
                address.UserUpdated = await _userManager.GetUserAsync(User);

                TaskResult result = await _addressService.UpdateAsync(address);
                if (result.Succeeded) return Ok();
                return BadRequest(result.Errors);
            }

            return PartialView("Partials/_EditAddress", addressForm);
        }

        [HttpPost, IgnoreAntiforgeryToken]
        public async Task<IActionResult> DeleteAddress(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            Address address = await _addressService.FindByIdAsync(id);

            if (address == null) return NotFound();

            TaskResult result = await _addressService.DeleteAsync(address);

            if (result.Succeeded) return Ok();
            return BadRequest(result.Errors);
        }

    }
}
