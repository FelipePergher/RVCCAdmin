// <copyright file="AttendantController.cs" company="Doffs">
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
    public class AttendantController : Controller
    {
        private readonly IDataRepository<Attendant> _attendantService;
        private readonly ILogger<AttendantController> _logger;

        public AttendantController(IDataRepository<Attendant> attendantService, ILogger<AttendantController> logger)
        {
            _attendantService = attendantService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddAttendant()
        {
            return PartialView("Partials/_AddAttendant", new AttendantFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddAttendant(AttendantFormModel attendantForm)
        {
            if (ModelState.IsValid)
            {
                var attendant = new Attendant(attendantForm.Name, attendantForm.CPF, attendantForm.Phone, attendantForm.Note);

                TaskResult result = await _attendantService.CreateAsync(attendant);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.InsertItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_AddAttendant", attendantForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditAttendant(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Attendant attendant = await _attendantService.FindByIdAsync(id);

            if (attendant == null)
            {
                return NotFound();
            }

            return PartialView("Partials/_EditAttendant", new AttendantFormModel(attendant.Name, attendant.CPF, attendant.Phone, attendant.AttendantId, attendant.Note));
        }

        [HttpPost]
        public async Task<IActionResult> EditAttendant(string id, AttendantFormModel attendantForm)
        {
            if (ModelState.IsValid)
            {
                Attendant attendant = await _attendantService.FindByIdAsync(id);

                attendant.Name = attendantForm.Name;
                attendant.CPF = attendantForm.CPF;
                attendant.Phone = attendantForm.Phone;
                attendant.Note = attendantForm.Note;

                TaskResult result = await _attendantService.UpdateAsync(attendant);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.UpdateItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_EditAttendant", attendantForm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAttendant(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Attendant attendant = await _attendantService.FindByIdAsync(id);

            if (attendant == null)
            {
                return NotFound();
            }

            TaskResult result = await _attendantService.DeleteAsync(attendant);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(LogEvents.DeleteItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
            return BadRequest();
        }
    }
}
