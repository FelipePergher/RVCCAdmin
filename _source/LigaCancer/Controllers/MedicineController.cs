using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.MedicalViewModels;
using LigaCancer.Code;
using LigaCancer.Data.Store;
using LigaCancer.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using LigaCancer.Code.Interface;

namespace LigaCancer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MedicineController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataStore<Medicine> _medicineService;

        public MedicineController(IDataStore<Medicine> medicineService, UserManager<ApplicationUser> userManager)
        {
            _medicineService = medicineService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string sortOrder, string currentSearchNameFilter, string searchNameString, int? page)
        {
            IQueryable<Medicine> medicines = _medicineService.GetAllQueryable(new string[] { "PatientInformationMedicines" });
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
                medicines = medicines.Where(s => s.Name.Contains(searchNameString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    medicines = medicines.OrderByDescending(s => s.Name);
                    break;
                default:
                    medicines = medicines.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 4;

            PaginatedList<Medicine> paginateList = await PaginatedList<Medicine>.CreateAsync(medicines.AsNoTracking(), page ?? 1, pageSize);
            return View(paginateList);
        }

        public IActionResult AddMedicine()
        {
            return PartialView("_AddMedicine", new MedicineViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMedicine(MedicineViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(this.User);
                Medicine medicine = new Medicine
                {
                    Name = model.Name,
                    UserCreated = user
                };

                TaskResult result = await _medicineService.CreateAsync(medicine);
                if (result.Succeeded)
                {
                    return StatusCode(200, "200");
                }
                ModelState.AddErrors(result);
            }

            return PartialView("_AddMedicine", model);
        }


        public async Task<IActionResult> EditMedicine(string id)
        {
            MedicineViewModel medicineViewModel = new MedicineViewModel();

            if (!string.IsNullOrEmpty(id))
            {
                Medicine medicine = await _medicineService.FindByIdAsync(id);
                if(medicine != null)
                {
                    medicineViewModel = new MedicineViewModel
                    {
                        MedicineId = medicine.MedicineId,
                        Name = medicine.Name
                    };
                }
            }

            return PartialView("_EditMedicine", medicineViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMedicine(string id, MedicineViewModel model)
        {
            if (ModelState.IsValid)
            {
                Medicine medicine = await _medicineService.FindByIdAsync(id);
                ApplicationUser user = await _userManager.GetUserAsync(this.User);

                medicine.Name = model.Name;
                medicine.LastUpdatedDate = DateTime.Now;
                medicine.LastUserUpdate = user;

                TaskResult result = await _medicineService.UpdateAsync(medicine);
                if (result.Succeeded)
                {
                    return StatusCode(200, "200");
                }
                ModelState.AddErrors(result);
            }

            return PartialView("_EditMedicine", model);
        }


        public async Task<IActionResult> DeleteMedicine(string id)
        {
            string name = string.Empty;

            if (!string.IsNullOrEmpty(id))
            {
                Medicine medicine = await _medicineService.FindByIdAsync(id);
                if (medicine != null)
                {
                    name = medicine.Name;
                }
            }

            return PartialView("_DeleteMedicine", name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMedicine(string id, IFormCollection form)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Medicine medicine = await _medicineService.FindByIdAsync(id);
                if (medicine != null)
                {
                    TaskResult result = await _medicineService.DeleteAsync(medicine);

                    if (result.Succeeded)
                    {
                        return StatusCode(200, "200");
                    }
                    ModelState.AddErrors(result);
                    return PartialView("_DeleteMedicine", medicine.Name);
                }
            }
            return RedirectToAction("Index");
        }

        #region Custom Methods

        public JsonResult IsNameExist(string Name, int MedicineId)
        {
            Medicine medicine = ((MedicineStore)_medicineService).FindByNameAsync(Name, MedicineId).Result;

            if (medicine != null)
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
