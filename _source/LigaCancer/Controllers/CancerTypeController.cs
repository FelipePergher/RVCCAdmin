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
    [Authorize(Roles = "Admin")]
    public class CancerTypeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataStore<CancerType> _cancerTypeService;

        public CancerTypeController(IDataStore<CancerType> cancerTypeService, UserManager<ApplicationUser> userManager)
        {
            _cancerTypeService = cancerTypeService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddCancerType()
        {
            return PartialView("_AddCancerType", new CancerTypeFormModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCancerType(CancerTypeFormModel cancerTypeForm)
        {
            if (!ModelState.IsValid) return PartialView("_AddCancerType", cancerTypeForm);
            ApplicationUser user = await _userManager.GetUserAsync(this.User);
            CancerType cancerType = new CancerType
            {
                Name = cancerTypeForm.Name,
                UserCreated = user
            };

            TaskResult result = await _cancerTypeService.CreateAsync(cancerType);
            if (result.Succeeded)
            {
                return StatusCode(200, "200");
            }
            ModelState.AddErrors(result);

            return PartialView("_AddCancerType", cancerTypeForm);
        }


        public async Task<IActionResult> EditCancerType(string id)
        {
            CancerTypeFormModel cancerTypeForm = new CancerTypeFormModel();

            if (string.IsNullOrEmpty(id)) return PartialView("_EditCancerType", cancerTypeForm);

            CancerType cancerType = await _cancerTypeService.FindByIdAsync(id);
            if(cancerType != null)
            {
                cancerTypeForm = new CancerTypeFormModel
                {
                    CancerTypeId = cancerType.CancerTypeId,
                    Name = cancerType.Name
                };
            }

            return PartialView("_EditCancerType", cancerTypeForm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCancerType(string id, CancerTypeFormModel cancerTypeForm)
        {
            if (!ModelState.IsValid) return PartialView("_EditCancerType", cancerTypeForm);

            CancerType cancerType = await _cancerTypeService.FindByIdAsync(id);
            ApplicationUser user = await _userManager.GetUserAsync(this.User);

            cancerType.Name = cancerTypeForm.Name;
            cancerType.UpdatedDate = DateTime.Now;
            cancerType.UserUpdated = user;

            TaskResult result = await _cancerTypeService.UpdateAsync(cancerType);
            if (result.Succeeded)
            {
                return StatusCode(200, "200");
            }
            ModelState.AddErrors(result);

            return PartialView("_EditCancerType", cancerTypeForm);
        }

        public async Task<IActionResult> DeleteCancerType(string id)
        {
            string name = string.Empty;

            if (string.IsNullOrEmpty(id)) return PartialView("_DeleteCancerType", name);

            CancerType cancerType = await _cancerTypeService.FindByIdAsync(id);
            if (cancerType != null)
            {
                name = cancerType.Name;
            }

            return PartialView("_DeleteCancerType", name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCancerType(string id, IFormCollection form)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index");

            CancerType cancerType = await _cancerTypeService.FindByIdAsync(id);
            if (cancerType == null) return RedirectToAction("Index");

            TaskResult result = await _cancerTypeService.DeleteAsync(cancerType);

            if (result.Succeeded)
            {
                return StatusCode(200, "200");
            }
            ModelState.AddErrors(result);
            return PartialView("_DeleteCancerType", cancerType.Name);
        }

        #region Custom Methods

        public JsonResult IsNameExist(string name, int cancerTypeId)
        {
            CancerType cancerType = ((CancerTypeStore)_cancerTypeService).FindByNameAsync(name, cancerTypeId).Result;

            return Json(cancerType == null);
        }

        #endregion

    }
}
