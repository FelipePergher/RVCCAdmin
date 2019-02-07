using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.FormModel;
using LigaCancer.Data.Models;
using LigaCancer.Code;
using LigaCancer.Data.Store;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using LigaCancer.Code.Interface;

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
            return View();
        }

        [HttpGet]
        public IActionResult AddTreatmentPlace()
        {
            return PartialView("_AddTreatmentPlace", new TreatmentPlaceFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddTreatmentPlace(TreatmentPlaceFormModel treatmentPlaceForm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(this.User);
                TreatmentPlace treatmentPlace = new TreatmentPlace
                {
                    City = treatmentPlaceForm.City,
                    UserCreated = user
                };

                TaskResult result = await _treatmentPlaceService.CreateAsync(treatmentPlace);
                if (result.Succeeded) return Ok();
             
                ModelState.AddErrors(result);
            }

            return PartialView("_AddTreatmentPlace", treatmentPlaceForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditTreatmentPlace(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            TreatmentPlace treatmentPlace = await _treatmentPlaceService.FindByIdAsync(id);
            
            if (treatmentPlace == null) return NotFound();

            TreatmentPlaceFormModel treatmentPlaceForm = new TreatmentPlaceFormModel
            {
                TreatmentPlaceId = treatmentPlace.TreatmentPlaceId,
                City = treatmentPlace.City
            };

            return PartialView("_EditTreatmentPlace", treatmentPlaceForm);
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
                treatmentPlace.UserUpdated = await _userManager.GetUserAsync(this.User);

                TaskResult result = await _treatmentPlaceService.UpdateAsync(treatmentPlace);
                if (result.Succeeded) return Ok();

                ModelState.AddErrors(result);
            }

            return PartialView("_EditTreatmentPlace", treatmentPlaceForm);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteTreatmentPlace(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            TreatmentPlace treatmentPlace = await _treatmentPlaceService.FindByIdAsync(id);

            if (treatmentPlace == null) return NotFound();

            TaskResult result = await _treatmentPlaceService.DeleteAsync(treatmentPlace);

            if (result.Succeeded) return Ok();

            return BadRequest(result);
        }

        #region Custom Methods

        public JsonResult IsCityExist(string city, int treatmentPlaceId)
        {
            TreatmentPlace treatmentPlace = ((TreatmentPlaceStore)_treatmentPlaceService).FindByCityAsync(city, treatmentPlaceId).Result;

            return Json(treatmentPlace == null);
        }

        #endregion

    }
}
