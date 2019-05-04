using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Data.Store;
using LigaCancer.Models.FormModel;
using LigaCancer.Models.SearchModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LigaCancer.Controllers
{
    [Authorize(Roles = "Admin"), AutoValidateAntiforgeryToken]
    public class TreatmentPlaceController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataStore<TreatmentPlace> _treatmentPlaceService;

        public TreatmentPlaceController(IDataStore<TreatmentPlace> treatmentPlaceService, UserManager<ApplicationUser> userManager)
        {
            _treatmentPlaceService = treatmentPlaceService;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new TreatmentPlaceSearchModel());
        }

        [HttpGet]
        public IActionResult AddTreatmentPlace()
        {
            return PartialView("Partials/_AddTreatmentPlace", new TreatmentPlaceFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddTreatmentPlace(TreatmentPlaceFormModel treatmentPlaceForm)
        {
            if (ModelState.IsValid)
            {
                TreatmentPlace treatmentPlace = new TreatmentPlace(treatmentPlaceForm.City, await _userManager.GetUserAsync(User));

                TaskResult result = await _treatmentPlaceService.CreateAsync(treatmentPlace);
                if (result.Succeeded) return Ok();
             
                ModelState.AddErrors(result);
            }

            return PartialView("Partials/_AddTreatmentPlace", treatmentPlaceForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditTreatmentPlace(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            TreatmentPlace treatmentPlace = await _treatmentPlaceService.FindByIdAsync(id);
            
            if (treatmentPlace == null) return NotFound();

            TreatmentPlaceFormModel treatmentPlaceForm = new TreatmentPlaceFormModel
            {
                City = treatmentPlace.City
            };

            return PartialView("Partials/_EditTreatmentPlace", treatmentPlaceForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditTreatmentPlace(string id, TreatmentPlaceFormModel treatmentPlaceForm)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            if (ModelState.IsValid)
            {
                TreatmentPlace treatmentPlace = await _treatmentPlaceService.FindByIdAsync(id);
                
                if (treatmentPlace == null) return NotFound();

                treatmentPlace.City = treatmentPlaceForm.City;
                treatmentPlace.UpdatedDate = DateTime.Now;
                treatmentPlace.UserUpdated = await _userManager.GetUserAsync(User);

                TaskResult result = await _treatmentPlaceService.UpdateAsync(treatmentPlace);
                if (result.Succeeded) return Ok();

                ModelState.AddErrors(result);
            }

            return PartialView("Partials/_EditTreatmentPlace", treatmentPlaceForm);
        }

        [HttpPost, IgnoreAntiforgeryToken]
        public async Task<IActionResult> DeleteTreatmentPlace(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            TreatmentPlace treatmentPlace = await _treatmentPlaceService.FindByIdAsync(id);

            if (treatmentPlace == null) return NotFound();

            TaskResult result = await _treatmentPlaceService.DeleteAsync(treatmentPlace);

            if (result.Succeeded) return Ok();

            return BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> IsCityExist(string city, int treatmentPlaceId)
        {
            TreatmentPlace treatmentPlace = await ((TreatmentPlaceStore)_treatmentPlaceService).FindByCityAsync(city, treatmentPlaceId);
            return Json(treatmentPlace == null);
        }
    }
}
