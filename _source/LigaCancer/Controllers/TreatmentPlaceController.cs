﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LigaCancer.Data;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.MedicalViewModels;
using LigaCancer.Data.Models;
using LigaCancer.Code;
using LigaCancer.Data.Store;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace LigaCancer.Controllers
{
    public class TreatmentPlaceController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataStore<TreatmentPlace> _treatmentPlaceService;

        public TreatmentPlaceController(IDataStore<TreatmentPlace> treatmentPlaceService, UserManager<ApplicationUser> userManager)
        {
            _treatmentPlaceService = treatmentPlaceService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string sortOrder, string currentSearchCityFilter, string searchCityString, int? page)
        {
            IQueryable<TreatmentPlace> treatmentPlaces = _treatmentPlaceService.GetAllQueryable();
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CitySortParm"] = string.IsNullOrEmpty(sortOrder) ? "city_desc" : "";

            if (searchCityString != null)
            {
                page = 1;
            }
            else
            {
                searchCityString = currentSearchCityFilter;
            }

            ViewData["CurrentSearchCityFilter"] = searchCityString;

            if (!string.IsNullOrEmpty(searchCityString))
            {
                treatmentPlaces = treatmentPlaces.Where(s => s.City.Contains(searchCityString));
            }

            switch (sortOrder)
            {
                case "city_desc":
                    treatmentPlaces = treatmentPlaces.OrderByDescending(s => s.City);
                    break;
                default:
                    treatmentPlaces = treatmentPlaces.OrderBy(s => s.City);
                    break;
            }

            int pageSize = 4;

            PaginatedList<TreatmentPlace> paginateList = await PaginatedList<TreatmentPlace>.CreateAsync(treatmentPlaces.AsNoTracking(), page ?? 1, pageSize);
            return View(paginateList);
        }

        public IActionResult AddTreatmentPlace()
        {
            return PartialView("_AddTreatmentPlace", new TreatmentPlaceViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTreatmentPlace(TreatmentPlaceViewModel model)
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
                if (result.Succeeded)
                {
                    return StatusCode(200, "200");
                }
                ModelState.AddErrors(result);
            }

            return PartialView("_AddTreatmentPlace", model);
        }


        public async Task<IActionResult> EditTreatmentPlace(string id)
        {
            TreatmentPlaceViewModel treatmentPlaceViewModel = new TreatmentPlaceViewModel();

            if (!string.IsNullOrEmpty(id))
            {
                TreatmentPlace treatmentPlace = await _treatmentPlaceService.FindByIdAsync(id);
                if (treatmentPlace != null)
                {
                    treatmentPlaceViewModel = new TreatmentPlaceViewModel
                    {
                        TreatmentPlaceId = treatmentPlace.TreatmentPlaceId,
                        City = treatmentPlace.City
                    };
                }
            }

            return PartialView("_EditTreatmentPlace", treatmentPlaceViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTreatmentPlace(string id, TreatmentPlaceViewModel model)
        {
            if (ModelState.IsValid)
            {
                TreatmentPlace treatmentPlace = await _treatmentPlaceService.FindByIdAsync(id);
                ApplicationUser user = await _userManager.GetUserAsync(this.User);

                treatmentPlace.City = model.City;
                treatmentPlace.LastUpdatedDate = DateTime.Now;
                treatmentPlace.LastUserUpdate = user;

                TaskResult result = await _treatmentPlaceService.UpdateAsync(treatmentPlace);
                if (result.Succeeded)
                {
                    return StatusCode(200, "200");
                }
                ModelState.AddErrors(result);
            }

            return PartialView("_EditTreatmentPlace", model);
        }


        public async Task<IActionResult> DeleteTreatmentPlace(string id)
        {
            string city = string.Empty;

            if (!string.IsNullOrEmpty(id))
            {
                TreatmentPlace treatmentPlace = await _treatmentPlaceService.FindByIdAsync(id);
                if (treatmentPlace != null)
                {
                    city = treatmentPlace.City;
                }
            }

            return PartialView("_DeleteTreatmentPlace", city);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTreatmentPlace(string id, IFormCollection form)
        {
            if (!string.IsNullOrEmpty(id))
            {
                TreatmentPlace treatmentPlace = await _treatmentPlaceService.FindByIdAsync(id);
                if (treatmentPlace != null)
                {
                    TaskResult result = await _treatmentPlaceService.DeleteAsync(treatmentPlace);

                    if (result.Succeeded)
                    {
                        return StatusCode(200, "200");
                    }
                    ModelState.AddErrors(result);
                    return PartialView("_DeleteTreatmentPlace", treatmentPlace.City);
                }
            }
            return RedirectToAction("Index");
        }

        #region Custom Methods

        public JsonResult IsCityExist(string City, int TreatmentPlaceId)
        {
            TreatmentPlace treatmentPlace = ((TreatmentPlaceStore)_treatmentPlaceService).FindByCityAsync(City, TreatmentPlaceId).Result;

            if (treatmentPlace != null)
            {
                return Json(false);
            }
            else
            {
                return Json(true);
            }
        }

        #endregion

    }
}
