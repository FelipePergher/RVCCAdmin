// <copyright file="AttendanceTypeController.cs" company="Doffs">
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
    public class AttendanceTypeController : Controller
    {
        private readonly IDataRepository<AttendanceType> _attendanceTypeService;
        private readonly ILogger<AttendanceTypeController> _logger;

        public AttendanceTypeController(IDataRepository<AttendanceType> attendanceTypeService, ILogger<AttendanceTypeController> logger)
        {
            _attendanceTypeService = attendanceTypeService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddAttendanceType()
        {
            return PartialView("Partials/_AddAttendanceType", new AttendanceTypeFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddAttendanceType(AttendanceTypeFormModel attendanceTypeForm)
        {
            if (ModelState.IsValid)
            {
                var attendanceType = new AttendanceType(attendanceTypeForm.Name);

                TaskResult result = await _attendanceTypeService.CreateAsync(attendanceType);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.InsertItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_AddAttendanceType", attendanceTypeForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditAttendanceType(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            AttendanceType attendanceType = await _attendanceTypeService.FindByIdAsync(id);

            if (attendanceType == null)
            {
                return NotFound();
            }

            var attendanceTypeForm = new AttendanceTypeFormModel(attendanceType.Name, attendanceType.AttendanceTypeId);

            return PartialView("Partials/_EditAttendanceType", attendanceTypeForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditAttendanceType(string id, AttendanceTypeFormModel attendanceTypeForm)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                AttendanceType attendanceType = await _attendanceTypeService.FindByIdAsync(id);

                if (attendanceType == null)
                {
                    return NotFound();
                }

                attendanceType.Name = attendanceTypeForm.Name;

                TaskResult result = await _attendanceTypeService.UpdateAsync(attendanceType);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.UpdateItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_EditAttendanceType", attendanceTypeForm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAttendanceType([FromForm] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            AttendanceType attendanceType = await _attendanceTypeService.FindByIdAsync(id);

            if (attendanceType == null)
            {
                return NotFound();
            }

            TaskResult result = await _attendanceTypeService.DeleteAsync(attendanceType);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(LogEvents.DeleteItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
            return BadRequest(result);
        }
    }
}
