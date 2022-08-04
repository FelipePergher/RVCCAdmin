// <copyright file="VisitorAttendanceController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models.Domain;
using RVCC.Data.Models.RelationModels;
using RVCC.Models.FormModel;
using RVCC.Models.SearchModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Controllers
{
    [Authorize(Roles = Roles.AdminSecretarySocialAssistanceAuthorize)]
    [AutoValidateAntiforgeryToken]
    public class VisitorAttendanceController : Controller
    {
        private readonly IDataRepository<VisitorAttendance> _visitorAttendanceService;
        private readonly IDataRepository<Visitor> _visitorService;
        private readonly IDataRepository<AttendanceType> _attendanceTypeService;
        private readonly ILogger<VisitorAttendanceController> _logger;

        public VisitorAttendanceController(IDataRepository<VisitorAttendance> visitorAttendanceService, IDataRepository<Visitor> visitorService, IDataRepository<AttendanceType> attendanceTypeService, ILogger<VisitorAttendanceController> logger)
        {
            _visitorAttendanceService = visitorAttendanceService;
            _visitorService = visitorService;
            _attendanceTypeService = attendanceTypeService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new VisitorAttendanceSearchModel());
        }

        [HttpGet]
        public async Task<IActionResult> AddVisitorAttendance()
        {
            List<Visitor> visitors = await _visitorService.GetAllAsync();
            List<AttendanceType> attendanceTypes = await _attendanceTypeService.GetAllAsync();
            var visitorAttendanceForm = new VisitorAttendanceFormModel
            {
                Visitors = visitors.Select(x => new SelectListItem($"{x.Name} ", x.VisitorId.ToString())).ToList(),
                AttendanceTypes = attendanceTypes.Select(x => new SelectListItem(x.Name, x.AttendanceTypeId.ToString())).ToList()
            };

            return PartialView("Partials/_AddVisitorAttendance", visitorAttendanceForm);
        }

        [HttpPost]
        public async Task<IActionResult> AddVisitorAttendance(VisitorAttendanceFormModel visitorAttendanceForm)
        {
            if (ModelState.IsValid)
            {
                Visitor visitor = await _visitorService.FindByIdAsync(visitorAttendanceForm.VisitorId);
                var dateTime = DateTime.Parse(visitorAttendanceForm.Date);
                var visitorAttendance = new VisitorAttendance
                {
                    AttendanceDate = dateTime,
                    VisitorId = visitor.VisitorId,
                    AttendanceTypeId = int.Parse(visitorAttendanceForm.Attendance),
                    Observation = visitorAttendanceForm.Observation
                };

                TaskResult result = await _visitorAttendanceService.CreateAsync(visitorAttendance);

                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.InsertItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            List<Visitor> visitors = await _visitorService.GetAllAsync();
            List<AttendanceType> attendanceTypes = await _attendanceTypeService.GetAllAsync();
            visitorAttendanceForm.Visitors = visitors.Select(x => new SelectListItem($"{x.Name}", x.VisitorId.ToString())).ToList();
            visitorAttendanceForm.AttendanceTypes = attendanceTypes.Select(x => new SelectListItem(x.Name, x.AttendanceTypeId.ToString())).ToList();

            return PartialView("Partials/_AddVisitorAttendance", visitorAttendanceForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditVisitorAttendance(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            VisitorAttendance visitorAttendance = await _visitorAttendanceService.FindByIdAsync(id, new[] { nameof(VisitorAttendance.Visitor), nameof(VisitorAttendance.AttendanceType) });

            if (visitorAttendance == null)
            {
                return NotFound();
            }

            var visitorAttendanceForm = new VisitorAttendanceFormModel
            {
                VisitorIdHidden = visitorAttendance.VisitorId,
                AttendanceTypeIdHidden = visitorAttendance.AttendanceTypeId,
                VisitorId = $"{visitorAttendance.Visitor.Name}",
                Date = visitorAttendance.AttendanceDate.ToString("dd/MM/yyyy"),
                Attendance = visitorAttendance.AttendanceType.Name,
            };

            return PartialView("Partials/_EditVisitorAttendance", visitorAttendanceForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditVisitorAttendance(string id, VisitorAttendanceFormModel visitorAttendanceForm)
        {
            if (ModelState.IsValid)
            {
                VisitorAttendance visitorAttendance = await _visitorAttendanceService.FindByIdAsync(id);
                var dateTime = DateTime.Parse(visitorAttendanceForm.Date);
                visitorAttendance.AttendanceDate = dateTime;

                TaskResult result = await _visitorAttendanceService.UpdateAsync(visitorAttendance);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.UpdateItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            List<AttendanceType> attendanceTypes = await _attendanceTypeService.GetAllAsync();
            visitorAttendanceForm.AttendanceTypes = attendanceTypes.Select(x => new SelectListItem(x.Name, x.AttendanceTypeId.ToString())).ToList();

            return PartialView("Partials/_EditVisitorAttendance", visitorAttendanceForm);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePatientBenefit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            VisitorAttendance visitorAttendance = await _visitorAttendanceService.FindByIdAsync(id);

            if (visitorAttendance == null)
            {
                return NotFound();
            }

            TaskResult result = await _visitorAttendanceService.DeleteAsync(visitorAttendance);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(LogEvents.DeleteItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
            return BadRequest();
        }
    }
}