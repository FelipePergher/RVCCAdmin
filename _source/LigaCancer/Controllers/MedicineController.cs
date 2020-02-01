using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models;
using RVCC.Data.Models.PatientModels;
using RVCC.Models.FormModel;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Controllers
{
    [Authorize(Roles = Roles.AdminAndUserAuthorize)]
    [AutoValidateAntiforgeryToken]
    public class MedicineController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataRepository<Medicine> _medicineService;
        private readonly ILogger<MedicineController> _logger;

        public MedicineController(
            IDataRepository<Medicine> medicineService,
            ILogger<MedicineController> logger,
            UserManager<ApplicationUser> userManager)
        {
            _medicineService = medicineService;
            _userManager = userManager;
            _logger = logger;
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
            if (ModelState.IsValid)
            {
                var medicine = new Medicine(medicineForm.Name, await _userManager.GetUserAsync(User));

                TaskResult result = await _medicineService.CreateAsync(medicine);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
                return BadRequest();
            }

            return PartialView("Partials/_AddMedicine", medicineForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditMedicine(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Medicine medicine = await _medicineService.FindByIdAsync(id);

            if (medicine == null)
            {
                return NotFound();
            }

            var medicineForm = new MedicineFormModel(medicine.Name, medicine.MedicineId);

            return PartialView("Partials/_EditMedicine", medicineForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditMedicine(string id, MedicineFormModel medicineForm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);
                Medicine medicine = await _medicineService.FindByIdAsync(id);

                medicine.Name = medicineForm.Name;
                medicine.UpdatedTime = DateTime.Now;
                medicine.UpdatedBy = user.Name;

                TaskResult result = await _medicineService.UpdateAsync(medicine);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
                return BadRequest();
            }

            return PartialView("Partials/_EditMedicine", medicineForm);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> DeleteMedicine(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Medicine medicine = await _medicineService.FindByIdAsync(id);

            if (medicine == null)
            {
                return NotFound();
            }

            TaskResult result = await _medicineService.DeleteAsync(medicine);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
            return BadRequest(result);
        }

    }
}
