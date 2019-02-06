using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.FormViewModel;
using LigaCancer.Data.Models;
using LigaCancer.Code;
using LigaCancer.Data.Store;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> AddTreatmentPlace(TreatmentPlaceFormModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(this.User);
                TreatmentPlace treatmentPlace = new TreatmentPlace
                {
                    City = model.City,
                    UserCreated = user
                };

                TaskResult result = await _treatmentPlaceService.CreateAsync(treatmentPlace);
                if (result.Succeeded) return Ok();
             
                ModelState.AddErrors(result);
            }

            return PartialView("_AddTreatmentPlace", model);
        }

        [HttpGet]
        public async Task<IActionResult> EditTreatmentPlace(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            TreatmentPlace treatmentPlace = await _treatmentPlaceService.FindByIdAsync(id);
            
            if (treatmentPlace == null) return NotFound();

            TreatmentPlaceFormModel treatmentPlaceViewModel = new TreatmentPlaceFormModel
            {
                TreatmentPlaceId = treatmentPlace.TreatmentPlaceId,
                City = treatmentPlace.City
            };

            return PartialView("_EditTreatmentPlace", treatmentPlaceViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditTreatmentPlace(string id, TreatmentPlaceFormModel model)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            if (ModelState.IsValid)
            {
                TreatmentPlace treatmentPlace = await _treatmentPlaceService.FindByIdAsync(id);
                
                if (treatmentPlace == null) return NotFound();

                treatmentPlace.City = model.City;
                treatmentPlace.UpdatedDate = DateTime.Now;
                treatmentPlace.UserUpdated = await _userManager.GetUserAsync(this.User);

                TaskResult result = await _treatmentPlaceService.UpdateAsync(treatmentPlace);
                if (result.Succeeded) return Ok();

                ModelState.AddErrors(result);
            }

            return PartialView("_EditTreatmentPlace", model);
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
