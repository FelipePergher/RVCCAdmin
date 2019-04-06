using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.FormModel;
using LigaCancer.Code;
using LigaCancer.Data.Store;
using LigaCancer.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using LigaCancer.Code.Interface;

namespace LigaCancer.Controllers
{
    [Authorize(Roles = "Admin"), AutoValidateAntiforgeryToken]
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

        [HttpGet]
        public IActionResult AddMedicine()
        {
            return PartialView("Partials/_AddMedicine", new MedicineFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddMedicine(MedicineFormModel medicineForm)
        {
            if (ModelState.IsValid) {
                Medicine medicine = new Medicine
                {
                    Name = medicineForm.Name,
                    UserCreated = await _userManager.GetUserAsync(User)
                };

                TaskResult result = await _medicineService.CreateAsync(medicine);
                if (result.Succeeded) return Ok();
                ModelState.AddErrors(result);
            }

            return PartialView("Partials/_AddMedicine", medicineForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditMedicine(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            BaseSpecification<Medicine> baseSpecification = new BaseSpecification<Medicine>(x => x.PatientInformationMedicines);
            baseSpecification.IncludeStrings.Add("PatientInformationMedicines.PatientInformation");

            Medicine medicine = await _medicineService.FindByIdAsync(id, baseSpecification);

            if (medicine == null) return NotFound();

            MedicineFormModel medicineForm = new MedicineFormModel
            {
                MedicineId = medicine.MedicineId,
                Name = medicine.Name
            };

            return PartialView("Partials/_EditMedicine", medicineForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditMedicine(string id, MedicineFormModel medicineForm)
        {
            if (ModelState.IsValid)
            {
                Medicine medicine = await _medicineService.FindByIdAsync(id);

                medicine.Name = medicineForm.Name;
                medicine.UpdatedDate = DateTime.Now;
                medicine.UserUpdated = await _userManager.GetUserAsync(User);

                TaskResult result = await _medicineService.UpdateAsync(medicine);
                if (result.Succeeded) return Ok();
                ModelState.AddErrors(result);
            }

            return PartialView("Partials/_EditMedicine", medicineForm);
        }

        [HttpPost, IgnoreAntiforgeryToken]
        public async Task<IActionResult> DeleteMedicine(string id, IFormCollection form)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            Medicine medicine = await _medicineService.FindByIdAsync(id);

            if (medicine == null) return NotFound();

            TaskResult result = await _medicineService.DeleteAsync(medicine);

            if (result.Succeeded) return Ok();

            return BadRequest(result);
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
