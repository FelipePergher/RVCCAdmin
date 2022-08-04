// <copyright file="VisitorController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models.Domain;
using RVCC.Models.FormModel;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Controllers
{
    [Authorize(Roles = Roles.AdminSecretarySocialAssistanceAuthorize)]
    [AutoValidateAntiforgeryToken]
    public class VisitorController : Controller
    {
        private readonly IDataRepository<Visitor> _visitorService;
        private readonly ILogger<VisitorController> _logger;

        public VisitorController(IDataRepository<Visitor> visitorService, ILogger<VisitorController> logger)
        {
            _visitorService = visitorService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddVisitor()
        {
            return PartialView("Partials/_AddVisitor", new VisitorFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddVisitor(VisitorFormModel visitorForm)
        {
            if (ModelState.IsValid)
            {
                var visitor = new Visitor(visitorForm.Name, visitorForm.CPF, visitorForm.Phone);

                TaskResult result = await _visitorService.CreateAsync(visitor);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.InsertItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_AddVisitor", visitorForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditVisitor(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Visitor visitor = await _visitorService.FindByIdAsync(id);

            if (visitor == null)
            {
                return NotFound();
            }

            return PartialView("Partials/_EditVisitor", new VisitorFormModel(visitor.Name, visitor.CPF, visitor.Phone, visitor.VisitorId));
        }

        [HttpPost]
        public async Task<IActionResult> EditVisitor(string id, VisitorFormModel visitorForm)
        {
            if (ModelState.IsValid)
            {
                Visitor visitor = await _visitorService.FindByIdAsync(id);

                visitor.Name = visitorForm.Name;
                visitor.CPF = visitorForm.CPF;
                visitor.Phone = visitorForm.Phone;

                TaskResult result = await _visitorService.UpdateAsync(visitor);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.UpdateItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_EditVisitor", visitorForm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteVisitor(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Visitor visitor = await _visitorService.FindByIdAsync(id);

            if (visitor == null)
            {
                return NotFound();
            }

            TaskResult result = await _visitorService.DeleteAsync(visitor);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(LogEvents.DeleteItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
            return BadRequest();
        }
    }
}
