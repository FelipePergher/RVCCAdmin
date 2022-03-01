// <copyright file="CancerTypeController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models;
using RVCC.Models.FormModel;
using RVCC.Models.SearchModel;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Controllers
{
    [Authorize(Roles = Roles.AdminSecretarySocialAssistanceAuthorize)]
    [AutoValidateAntiforgeryToken]
    public class CancerTypeController : Controller
    {
        private readonly IDataRepository<CancerType> _cancerTypeService;
        private readonly ILogger<CancerTypeController> _logger;

        public CancerTypeController(IDataRepository<CancerType> cancerTypeService, ILogger<CancerTypeController> logger)
        {
            _cancerTypeService = cancerTypeService;
            _logger = logger;
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
                var cancerType = new CancerType(cancerTypeForm.Name);

                TaskResult result = await _cancerTypeService.CreateAsync(cancerType);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.InsertItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_AddCancerType", cancerTypeForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditCancerType(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            CancerType cancerType = await _cancerTypeService.FindByIdAsync(id);

            if (cancerType == null)
            {
                return NotFound();
            }

            var cancerTypeForm = new CancerTypeFormModel(cancerType.Name, cancerType.CancerTypeId);

            return PartialView("Partials/_EditCancerType", cancerTypeForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditCancerType(string id, CancerTypeFormModel cancerTypeForm)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                CancerType cancerType = await _cancerTypeService.FindByIdAsync(id);

                if (cancerType == null)
                {
                    return NotFound();
                }

                cancerType.Name = cancerTypeForm.Name;

                TaskResult result = await _cancerTypeService.UpdateAsync(cancerType);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.UpdateItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_EditCancerType", cancerTypeForm);
        }

        [Authorize(Roles = Roles.AdminSecretaryAuthorize)]
        [HttpPost]
        public async Task<IActionResult> DeleteCancerType([FromForm] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            CancerType cancerType = await _cancerTypeService.FindByIdAsync(id);

            if (cancerType == null)
            {
                return NotFound();
            }

            TaskResult result = await _cancerTypeService.DeleteAsync(cancerType);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(LogEvents.DeleteItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
            return BadRequest(result);
        }
    }
}
