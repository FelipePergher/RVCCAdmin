// <copyright file="ServiceTypeController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models.Domain;
using RVCC.Models.FormModel;
using RVCC.Models.SearchModel;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Controllers
{
    [Authorize(Roles = Roles.AdminSecretarySocialAssistanceAuthorize)]
    [AutoValidateAntiforgeryToken]
    public class ServiceTypeController : Controller
    {
        private readonly IDataRepository<ServiceType> _serviceTypeService;
        private readonly ILogger<ServiceTypeController> _logger;

        public ServiceTypeController(IDataRepository<ServiceType> serviceTypeService, ILogger<ServiceTypeController> logger)
        {
            _serviceTypeService = serviceTypeService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(new ServiceTypeSearchModel());
        }

        [HttpGet]
        public IActionResult AddServiceType()
        {
            return PartialView("Partials/_AddServiceType", new ServiceTypeFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddServiceType(ServiceTypeFormModel serviceTypeForm)
        {
            if (ModelState.IsValid)
            {
                var serviceType = new ServiceType(serviceTypeForm.Name);

                TaskResult result = await _serviceTypeService.CreateAsync(serviceType);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.InsertItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_AddServiceType", serviceTypeForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditServiceType(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            ServiceType serviceType = await _serviceTypeService.FindByIdAsync(id);

            if (serviceType == null)
            {
                return NotFound();
            }

            var serviceTypeForm = new ServiceTypeFormModel(serviceType.Name, serviceType.ServiceTypeId);

            return PartialView("Partials/_EditServiceType", serviceTypeForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditServiceType(string id, ServiceTypeFormModel serviceTypeForm)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                ServiceType serviceType = await _serviceTypeService.FindByIdAsync(id);

                if (serviceType == null)
                {
                    return NotFound();
                }

                serviceType.Name = serviceTypeForm.Name;

                TaskResult result = await _serviceTypeService.UpdateAsync(serviceType);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.UpdateItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_EditServiceType", serviceTypeForm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteServiceType([FromForm] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            ServiceType serviceType = await _serviceTypeService.FindByIdAsync(id);

            if (serviceType == null)
            {
                return NotFound();
            }

            TaskResult result = await _serviceTypeService.DeleteAsync(serviceType);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(LogEvents.DeleteItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
            return BadRequest(result);
        }
    }
}
