﻿using System;
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
using LigaCancer.Models.SearchModel;

namespace LigaCancer.Controllers
{
    [Authorize(Roles = "Admin"), AutoValidateAntiforgeryToken]
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
            return View(new CancerTypeSearchModel());
        }

        [HttpGet]
        public IActionResult AddCancerType()
        {
            return PartialView("Partials/_AddCancerType", new CancerTypeFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddCancerType(CancerTypeFormModel cancerTypeForm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);
                CancerType cancerType = new CancerType
                {
                    Name = cancerTypeForm.Name,
                    UserCreated = user
                };

                TaskResult result = await _cancerTypeService.CreateAsync(cancerType);
                if (result.Succeeded) return Ok();
                ModelState.AddErrors(result);
            }

            return PartialView("Partials/_AddCancerType", cancerTypeForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditCancerType(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            CancerType cancerType = await _cancerTypeService.FindByIdAsync(id);

            if (cancerType == null) return NotFound();

            CancerTypeFormModel cancerTypeForm = new CancerTypeFormModel
            {
                CancerTypeId = cancerType.CancerTypeId,
                Name = cancerType.Name
            };

            return PartialView("Partials/_EditCancerType", cancerTypeForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditCancerType(string id, CancerTypeFormModel cancerTypeForm)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            if (ModelState.IsValid)
            {
                CancerType cancerType = await _cancerTypeService.FindByIdAsync(id);

                if (cancerType == null) return NotFound();

                cancerType.Name = cancerTypeForm.Name;
                cancerType.UpdatedDate = DateTime.Now;
                cancerType.UserUpdated = await _userManager.GetUserAsync(User);

                TaskResult result = await _cancerTypeService.UpdateAsync(cancerType);
                if (result.Succeeded) return Ok();

                ModelState.AddErrors(result);
            }

            return PartialView("Partials/_EditCancerType", cancerTypeForm);
        }
    
        [HttpPost, IgnoreAntiforgeryToken]
        public async Task<IActionResult> DeleteCancerType([FromForm] string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            CancerType cancerType = await _cancerTypeService.FindByIdAsync(id);

            if (cancerType == null) return NotFound();

            TaskResult result = await _cancerTypeService.DeleteAsync(cancerType);

            if (result.Succeeded) return Ok();

            return BadRequest(result);
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
