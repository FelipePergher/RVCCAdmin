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
    //[Authorize]
    public class PatientController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataStore<Patient> _patientService;

        public PatientController(IDataStore<Patient> patientService, UserManager<ApplicationUser> userManager)
        {
            _patientService = patientService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string sortOrder, string currentSearchNameFilter, string searchNameString, int? page)
        {
            IQueryable<Patient> patients = _patientService.GetAllQueryable();
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchNameString != null)
            {
                page = 1;
            }
            else
            {
                searchNameString = currentSearchNameFilter;
            }

            ViewData["CurrentSearchNameFilter"] = searchNameString;

            if (!string.IsNullOrEmpty(searchNameString))
            {
                patients = patients.Where(s => s.FirstName.Contains(searchNameString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    patients = patients.OrderByDescending(s => s.FirstName);
                    break;
                default:
                    patients = patients.OrderBy(s => s.FirstName);
                    break;
            }

            int pageSize = 4;

            PaginatedList<Patient> paginateList = await PaginatedList<Patient>.CreateAsync(patients.AsNoTracking(), page ?? 1, pageSize);
            return View(paginateList);
        }

        public IActionResult AddPatient()
        {
            PatientViewModel patientViewModel = new PatientViewModel
            {
                SelectProfessions = ((PatientStore)_patientService).GetAllProfessions().Result.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.ProfessionId.ToString()
                }).ToList()
            };




            return PartialView("_AddPatient", patientViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPatient(PatientViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(this.User);
                Patient patient = new Patient
                {
                    FirstName = model.FirstName,
                    UserCreated = user
                };

                TaskResult result = await _patientService.CreateAsync(patient);
                if (result.Succeeded)
                {
                    return StatusCode(200, "200");
                }
                ModelState.AddErrors(result);
            }

            return PartialView("_AddPatient", model);
        }


        public async Task<IActionResult> EditPatient(string id)
        {
            PatientViewModel patientViewModel = new PatientViewModel();

            if (!string.IsNullOrEmpty(id))
            {
                Patient patient = await _patientService.FindByIdAsync(id);
                if(patient != null)
                {
                    patientViewModel = new PatientViewModel
                    {
                        PatientId = patient.PatientId,
                        FirstName = patient.FirstName
                    };
                }
            }

            return PartialView("_EditPatient", patientViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPatient(string id, PatientViewModel model)
        {
            if (ModelState.IsValid)
            {
                Patient patient = await _patientService.FindByIdAsync(id);
                ApplicationUser user = await _userManager.GetUserAsync(this.User);

                patient.FirstName = model.FirstName;
                patient.LastUpdatedDate = DateTime.Now;
                patient.LastUserUpdate = user;

                TaskResult result = await _patientService.UpdateAsync(patient);
                if (result.Succeeded)
                {
                    return StatusCode(200, "200");
                }
                ModelState.AddErrors(result);
            }

            return PartialView("_EditPatient", model);
        }


        public async Task<IActionResult> DeletePatient(string id)
        {
            string name = string.Empty;

            if (!string.IsNullOrEmpty(id))
            {
                Patient patient = await _patientService.FindByIdAsync(id);
                if (patient != null)
                {
                    name = patient.FirstName;
                }
            }

            return PartialView("_DeletePatient", name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePatient(string id, IFormCollection form)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Patient patient = await _patientService.FindByIdAsync(id);
                if (patient != null)
                {
                    TaskResult result = await _patientService.DeleteAsync(patient);

                    if (result.Succeeded)
                    {
                        return StatusCode(200, "200");
                    }
                    ModelState.AddErrors(result);
                    return PartialView("_DeletePatient", patient.FirstName);
                }
            }
            return RedirectToAction("Index");
        }

        //Todo: Delete this view
        public async Task<IActionResult> DetailsPatient(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var patient = await _patientService.FindByIdAsync(id.ToString());
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        #region Custom Methods

        public JsonResult IsNameExist(string Name, int PatientId)
        {
            Patient patient = ((PatientStore)_patientService).FindByNameAsync(Name, PatientId).Result;

            if (patient != null)
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
