using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.FormViewModel;
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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddMedicine()
        {
            return PartialView("_AddMedicine", new MedicineViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMedicine(MedicineViewModel model)
        {
            if (!ModelState.IsValid) return PartialView("_AddMedicine", model);

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

            return PartialView("_AddMedicine", model);
        }


        public async Task<IActionResult> EditMedicine(string id)
        {
            MedicineViewModel medicineViewModel = new MedicineViewModel();

            if (string.IsNullOrEmpty(id)) return PartialView("_EditMedicine", medicineViewModel);

            Medicine medicine = await _medicineService.FindByIdAsync(id);
            if(medicine != null)
            {
                medicineViewModel = new MedicineViewModel
                {
                    MedicineId = medicine.MedicineId,
                    Name = medicine.Name
                };
            }

            return PartialView("_EditMedicine", medicineViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMedicine(string id, MedicineViewModel model)
        {
            if (!ModelState.IsValid) return PartialView("_EditMedicine", model);

            Medicine medicine = await _medicineService.FindByIdAsync(id);
            ApplicationUser user = await _userManager.GetUserAsync(this.User);

            medicine.Name = model.Name;
            medicine.UpdatedDate = DateTime.Now;
            medicine.UserUpdated = user;

            TaskResult result = await _medicineService.UpdateAsync(medicine);
            if (result.Succeeded)
            {
                return StatusCode(200, "200");
            }
            ModelState.AddErrors(result);

            return PartialView("_EditMedicine", model);
        }


        public async Task<IActionResult> DeleteMedicine(string id)
        {
            string name = string.Empty;

            if (string.IsNullOrEmpty(id)) return PartialView("_DeleteMedicine", name);

            Medicine medicine = await _medicineService.FindByIdAsync(id);
            if (medicine != null)
            {
                name = medicine.Name;
            }

            return PartialView("_DeleteMedicine", name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMedicine(string id, IFormCollection form)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index");

            Medicine medicine = await _medicineService.FindByIdAsync(id);
            if (medicine == null) return RedirectToAction("Index");

            TaskResult result = await _medicineService.DeleteAsync(medicine);

            if (result.Succeeded)
            {
                return StatusCode(200, "200");
            }
            ModelState.AddErrors(result);
            return PartialView("_DeleteMedicine", medicine.Name);
        }

        #region Custom Methods

        public JsonResult IsNameExist(string name, int medicineId)
        {
            Medicine medicine = ((MedicineStore)_medicineService).FindByNameAsync(name, medicineId).Result;

            return Json(medicine == null);
        }

        #endregion

    }
}
