using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.FormModel;
using LigaCancer.Models.SearchModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, User")]
    [AutoValidateAntiforgeryToken]
    [Area("Admin")]
    public class AddressController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataStore<Address> _addressService;
        private readonly ILogger<AddressController> _logger;

        public AddressController(
            IDataStore<Address> addressService,
            ILogger<AddressController> logger,
            UserManager<ApplicationUser> userManager)
        {
            _addressService = addressService;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            return PartialView("Partials/_Index", new AddressSearchModel(id));
        }

        [HttpGet]
        public IActionResult AddAddress(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            return PartialView("Partials/_AddAddress", new AddressFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress(int id, AddressFormModel addressForm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);
                var address = new Address
                {
                    PatientId = id,
                    City = addressForm.City,
                    Complement = addressForm.Complement,
                    HouseNumber = addressForm.HouseNumber,
                    Neighborhood = addressForm.Neighborhood,
                    ObservationAddress = addressForm.ObservationAddress,
                    Street = addressForm.Street,
                    ResidenceType = addressForm.ResidenceType,
                    MonthlyAmmountResidence = addressForm.ResidenceType != null
                        ? (double)(decimal.TryParse(addressForm.MonthlyAmmountResidence, out decimal monthlyIncome) ? monthlyIncome : 0) : 0,

                    CreatedBy = user.Name
                };

                TaskResult result = await _addressService.CreateAsync(address);

                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
                return BadRequest();
            }

            return PartialView("Partials/_AddAddress", addressForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditAddress(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Address address = await _addressService.FindByIdAsync(id);

            if (address == null)
            {
                return NotFound();
            }

            var addressForm = new AddressFormModel
            {
                City = address.City,
                Complement = address.Complement,
                HouseNumber = address.HouseNumber,
                Neighborhood = address.Neighborhood,
                ObservationAddress = address.ObservationAddress,
                Street = address.Street,
                ResidenceType = address.ResidenceType,
                MonthlyAmmountResidence = address.MonthlyAmmountResidence.ToString("C2")
            };

            return PartialView("Partials/_EditAddress", addressForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditAddress(string id, AddressFormModel addressForm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);
                Address address = await _addressService.FindByIdAsync(id);

                address.Complement = addressForm.Complement;
                address.City = addressForm.City;
                address.HouseNumber = addressForm.HouseNumber;
                address.Neighborhood = addressForm.Neighborhood;
                address.ObservationAddress = addressForm.ObservationAddress;
                address.ResidenceType = addressForm.ResidenceType;
                address.MonthlyAmmountResidence = addressForm.ResidenceType != null ?
                    (double)(decimal.TryParse(addressForm.MonthlyAmmountResidence, out decimal monthlyIncome) ? monthlyIncome : 0) : 0;
                address.Street = addressForm.Street;
                address.UpdatedTime = DateTime.Now;
                address.UpdatedBy = user.Name;

                TaskResult result = await _addressService.UpdateAsync(address);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
                return BadRequest();
            }

            return PartialView("Partials/_EditAddress", addressForm);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> DeleteAddress(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Address address = await _addressService.FindByIdAsync(id);

            if (address == null)
            {
                return NotFound();
            }

            TaskResult result = await _addressService.DeleteAsync(address);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
            return BadRequest();
        }

    }
}
