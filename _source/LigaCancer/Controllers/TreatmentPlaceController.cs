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
    [Authorize(Roles = "Admin")]
    public class TreatmentPlaceController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataStore<TreatmentPlace> _treatmentPlaceService;

        public TreatmentPlaceController(IDataStore<TreatmentPlace> treatmentPlaceService, UserManager<ApplicationUser> userManager)
        {
            _treatmentPlaceService = treatmentPlaceService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddTreatmentPlace()
        {
            return PartialView("_AddTreatmentPlace", new TreatmentPlaceViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTreatmentPlace(TreatmentPlaceViewModel model)
        {
            if (!ModelState.IsValid) return PartialView("_AddTreatmentPlace", model);

            ApplicationUser user = await _userManager.GetUserAsync(this.User);
            TreatmentPlace treatmentPlace = new TreatmentPlace
            {
                City = model.City,
                UserCreated = user
            };

            TaskResult result = await _treatmentPlaceService.CreateAsync(treatmentPlace);
            if (result.Succeeded)
            {
                return StatusCode(200, "200");
            }
            ModelState.AddErrors(result);

            return PartialView("_AddTreatmentPlace", model);
        }


        public async Task<IActionResult> EditTreatmentPlace(string id)
        {
            TreatmentPlaceViewModel treatmentPlaceViewModel = new TreatmentPlaceViewModel();

            if (string.IsNullOrEmpty(id)) return PartialView("_EditTreatmentPlace", treatmentPlaceViewModel);

            TreatmentPlace treatmentPlace = await _treatmentPlaceService.FindByIdAsync(id);
            if (treatmentPlace != null)
            {
                treatmentPlaceViewModel = new TreatmentPlaceViewModel
                {
                    TreatmentPlaceId = treatmentPlace.TreatmentPlaceId,
                    City = treatmentPlace.City
                };
            }

            return PartialView("_EditTreatmentPlace", treatmentPlaceViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTreatmentPlace(string id, TreatmentPlaceViewModel model)
        {
            if (!ModelState.IsValid) return PartialView("_EditTreatmentPlace", model);

            TreatmentPlace treatmentPlace = await _treatmentPlaceService.FindByIdAsync(id);
            ApplicationUser user = await _userManager.GetUserAsync(this.User);

            treatmentPlace.City = model.City;
            treatmentPlace.UpdatedDate = DateTime.Now;
            treatmentPlace.UserUpdated = user;

            TaskResult result = await _treatmentPlaceService.UpdateAsync(treatmentPlace);
            if (result.Succeeded)
            {
                return StatusCode(200, "200");
            }
            ModelState.AddErrors(result);

            return PartialView("_EditTreatmentPlace", model);
        }


        public async Task<IActionResult> DeleteTreatmentPlace(string id)
        {
            string city = string.Empty;

            if (string.IsNullOrEmpty(id)) return PartialView("_DeleteTreatmentPlace", city);

            TreatmentPlace treatmentPlace = await _treatmentPlaceService.FindByIdAsync(id);
            if (treatmentPlace != null)
            {
                city = treatmentPlace.City;
            }

            return PartialView("_DeleteTreatmentPlace", city);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTreatmentPlace(string id, IFormCollection form)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index");

            TreatmentPlace treatmentPlace = await _treatmentPlaceService.FindByIdAsync(id);
            if (treatmentPlace == null) return RedirectToAction("Index");

            TaskResult result = await _treatmentPlaceService.DeleteAsync(treatmentPlace);

            if (result.Succeeded)
            {
                return StatusCode(200, "200");
            }
            ModelState.AddErrors(result);
            return PartialView("_DeleteTreatmentPlace", treatmentPlace.City);
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
