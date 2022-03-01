// <copyright file="AddressController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models;
using RVCC.Models.FormModel;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Controllers
{
    [Authorize(Roles = Roles.AdminSecretaryAuthorize)]
    [AutoValidateAntiforgeryToken]
    public class AddressController : Controller
    {
        private readonly IDataRepository<Address> _addressService;
        private readonly ILogger<AddressController> _logger;

        public AddressController(IDataRepository<Address> addressService, ILogger<AddressController> logger)
        {
            _addressService = addressService;
            _logger = logger;
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
                };

                if (addressForm.ResidenceType != null && double.TryParse(addressForm.MonthlyAmmountResidence, out double monthlyIncome))
                {
                    address.MonthlyAmountResidence = monthlyIncome;
                }

                TaskResult result = await _addressService.CreateAsync(address);

                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.InsertItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
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
                MonthlyAmmountResidence = address.MonthlyAmountResidence.ToString("C2")
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
                address.Street = addressForm.Street;

                if (addressForm.ResidenceType != null && double.TryParse(addressForm.MonthlyAmmountResidence, out double monthlyIncome))
                {
                    address.MonthlyAmountResidence = monthlyIncome;
                }

                TaskResult result = await _addressService.UpdateAsync(address);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.UpdateItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_EditAddress", addressForm);
        }

        [HttpPost]
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

            _logger.LogError(LogEvents.DeleteItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
            return BadRequest();
        }
    }
}
