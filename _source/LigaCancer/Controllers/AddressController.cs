using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LigaCancer.Data;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.MedicalViewModels;
using LigaCancer.Code;
using LigaCancer.Data.Store;
using LigaCancer.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace LigaCancer.Controllers
{
    [Authorize(Roles = "Admin")]
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


        public IActionResult AddAddress(string id)
        {
            AddressViewModel addressViewModel = new AddressViewModel
            {
                PatientId = id
            };
            return PartialView("_AddAddress", addressViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAddress(AddressViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(this.User);

                TaskResult result = await ((PatientStore)_patientService).AddAddress(
                    new Address
                    {
                        City = model.City,
                        Complement = model.Complement,
                        HouseNumber = model.HouseNumber,
                        Neighborhood = model.Neighborhood,
                        ObservationAddress = model.ObservationAddress,
                        Street = model.Street
                    }, model.PatientId);

                if (result.Succeeded)
                {
                    return StatusCode(200, "200");
                }
                ModelState.AddErrors(result);
            }

            return PartialView("_AddAddress", model);
        }


        public async Task<IActionResult> EditAddress(string id)
        {
            AddressViewModel addressViewModel = new AddressViewModel();

            if (!string.IsNullOrEmpty(id))
            {
                Address address = await _addressService.FindByIdAsync(id);
                if (address != null)
                {
                    addressViewModel = new AddressViewModel
                    {
                        City = address.City,
                        Complement = address.Complement,
                        HouseNumber = address.HouseNumber,
                        Neighborhood = address.Neighborhood,
                        ObservationAddress = address.ObservationAddress,
                        Street = address.Street
                    };
                }
            }

            return PartialView("_EditAddress", addressViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAddress(string id, AddressViewModel model)
        {
            if (ModelState.IsValid)
            {
                Address address = await _addressService.FindByIdAsync(id);
                ApplicationUser user = await _userManager.GetUserAsync(this.User);

                address.Complement = model.Complement;
                address.City = model.City;
                address.HouseNumber = model.HouseNumber;
                address.Neighborhood = model.Neighborhood;
                address.ObservationAddress = model.ObservationAddress;
                address.Street = model.Street;
                address.LastUpdatedDate = DateTime.Now;
                address.LastUserUpdate = user;

                TaskResult result = await _addressService.UpdateAsync(address);
                if (result.Succeeded)
                {
                    return StatusCode(200, "200");
                }
                ModelState.AddErrors(result);
            }

            return PartialView("_EditAddress", model);
        }


        public async Task<IActionResult> DeleteAddress(string id)
        {
            string addressText = string.Empty;

            if (!string.IsNullOrEmpty(id))
            {
                Address address = await _addressService.FindByIdAsync(id);
                if (address != null)
                {
                    addressText = $"{address.Street} {address.Neighborhood} nº {address.HouseNumber}";
                }
            }

            return PartialView("_DeleteAddress", addressText);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAddress(string id, IFormCollection form)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Address address = await _addressService.FindByIdAsync(id);
                if (address != null)
                {
                    TaskResult result = await _addressService.DeleteAsync(address);

                    if (result.Succeeded)
                    {
                        return StatusCode(200, "200");
                    }
                    ModelState.AddErrors(result);
                    return PartialView("_DeleteAddress", $"{address.Street} {address.Neighborhood} nº {address.HouseNumber}");
                }
            }
            return RedirectToAction("Index");
        }

    }
}
