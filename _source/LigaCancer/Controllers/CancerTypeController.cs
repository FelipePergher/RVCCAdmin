using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            return PartialView("_AddCancerType", new CancerTypeViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCancerType(CancerTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(this.User);
                CancerType cancerType = new CancerType
                {
                    Name = model.Name,
                    UserCreated = user
                };

                TaskResult result = await _cancerTypeService.CreateAsync(cancerType);
                if (result.Succeeded)
                {
                    return StatusCode(200, "200");
                }
                ModelState.AddErrors(result);
            }

            return PartialView("_AddCancerType", model);
        }


        public async Task<IActionResult> EditCancerType(string id)
        {
            CancerTypeViewModel cancerTypeViewModel = new CancerTypeViewModel();

            if (!string.IsNullOrEmpty(id))
            {
                CancerType cancerType = await _cancerTypeService.FindByIdAsync(id);
                if(cancerType != null)
                {
                    cancerTypeViewModel = new CancerTypeViewModel
                    {
                        CancerTypeId = cancerType.CancerTypeId,
                        Name = cancerType.Name
                    };
                }
            }

            return PartialView("_EditCancerType", cancerTypeViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCancerType(string id, CancerTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                CancerType cancerType = await _cancerTypeService.FindByIdAsync(id);
                ApplicationUser user = await _userManager.GetUserAsync(this.User);

                cancerType.Name = model.Name;
                cancerType.LastUpdatedDate = DateTime.Now;
                cancerType.LastUserUpdate = user;

                TaskResult result = await _cancerTypeService.UpdateAsync(cancerType);
                if (result.Succeeded)
                {
                    return StatusCode(200, "200");
                }
                ModelState.AddErrors(result);
            }

            return PartialView("_EditCancerType", model);
        }


        public async Task<IActionResult> DeleteCancerType(string id)
        {
            string name = string.Empty;

            if (!string.IsNullOrEmpty(id))
            {
                CancerType cancerType = await _cancerTypeService.FindByIdAsync(id);
                if (cancerType != null)
                {
                    name = cancerType.Name;
                }
            }

            return PartialView("_DeleteCancerType", name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCancerType(string id, IFormCollection form)
        {
            if (!string.IsNullOrEmpty(id))
            {
                CancerType cancerType = await _cancerTypeService.FindByIdAsync(id);
                if (cancerType != null)
                {
                    TaskResult result = await _cancerTypeService.DeleteAsync(cancerType);

                    if (result.Succeeded)
                    {
                        return StatusCode(200, "200");
                    }
                    ModelState.AddErrors(result);
                    return PartialView("_DeleteCancerType", cancerType.Name);
                }
            }
            return RedirectToAction("Index");
        }

        #region Custom Methods

        public JsonResult IsNameExist(string Name, int CancerTypeId)
        {
            CancerType cancerType = ((CancerTypeStore)_cancerTypeService).FindByNameAsync(Name, CancerTypeId).Result;

            if (cancerType != null)
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
