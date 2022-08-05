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
    public class VisitorAttendanceTypeController : Controller
    {
        private readonly IDataRepository<VisitorAttendanceType> _visitorAttendanceTypeTypeService;
        private readonly IDataRepository<Visitor> _visitorService;
        private readonly IDataRepository<AttendanceType> _attendanceTypeService;
        private readonly ILogger<VisitorAttendanceTypeController> _logger;

        public VisitorAttendanceTypeController(IDataRepository<VisitorAttendanceType> visitorAttendanceTypeTypeService, IDataRepository<Visitor> visitorService, IDataRepository<AttendanceType> attendanceTypeService, ILogger<VisitorAttendanceTypeController> logger)
        {
            _visitorAttendanceTypeTypeService = visitorAttendanceTypeTypeService;
            _visitorService = visitorService;
            _attendanceTypeService = attendanceTypeService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new VisitorAttendanceTypeSearchModel());
        }

        [HttpGet]
        public async Task<IActionResult> AddVisitorAttendanceType()
        {
            List<Visitor> visitors = await _visitorService.GetAllAsync();
            List<AttendanceType> attendanceTypes = await _attendanceTypeService.GetAllAsync();
            var visitorAttendanceTypeForm = new VisitorAttendanceTypeFormModel
            {
                Visitors = visitors.Select(x => new SelectListItem($"{x.Name} ", x.VisitorId.ToString())).ToList(),
                AttendanceTypes = attendanceTypes.Select(x => new SelectListItem(x.Name, x.AttendanceTypeId.ToString())).ToList()
            };

            return PartialView("Partials/_AddVisitorAttendanceType", visitorAttendanceTypeForm);
        }

        [HttpPost]
        public async Task<IActionResult> AddVisitorAttendanceType(VisitorAttendanceTypeFormModel visitorAttendanceTypeForm)
        {
            if (ModelState.IsValid)
            {
                Visitor visitor = await _visitorService.FindByIdAsync(visitorAttendanceTypeForm.VisitorId);
                var dateTime = DateTime.Parse(visitorAttendanceTypeForm.Date);
                var visitorAttendanceType = new VisitorAttendanceType
                {
                    AttendanceDate = dateTime,
                    VisitorId = visitor.VisitorId,
                    AttendanceTypeId = int.Parse(visitorAttendanceTypeForm.Attendance),
                    Observation = visitorAttendanceTypeForm.Observation
                };

                TaskResult result = await _visitorAttendanceTypeTypeService.CreateAsync(visitorAttendanceType);

                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.InsertItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            List<Visitor> visitors = await _visitorService.GetAllAsync();
            List<AttendanceType> attendanceTypes = await _attendanceTypeService.GetAllAsync();
            visitorAttendanceTypeForm.Visitors = visitors.Select(x => new SelectListItem($"{x.Name}", x.VisitorId.ToString())).ToList();
            visitorAttendanceTypeForm.AttendanceTypes = attendanceTypes.Select(x => new SelectListItem(x.Name, x.AttendanceTypeId.ToString())).ToList();

            return PartialView("Partials/_AddVisitorAttendanceType", visitorAttendanceTypeForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditVisitorAttendanceType(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            VisitorAttendanceType visitorAttendanceType = await _visitorAttendanceTypeTypeService.FindByIdAsync(id, new[] { nameof(VisitorAttendanceType.Visitor), nameof(VisitorAttendanceType.AttendanceType) });

            if (visitorAttendanceType == null)
            {
                return NotFound();
            }

            var visitorAttendanceTypeForm = new VisitorAttendanceTypeFormModel
            {
                VisitorIdHidden = visitorAttendanceType.VisitorId,
                AttendanceTypeIdHidden = visitorAttendanceType.AttendanceTypeId,
                VisitorId = $"{visitorAttendanceType.Visitor.Name}",
                Date = visitorAttendanceType.AttendanceDate.ToString("dd/MM/yyyy"),
                Attendance = visitorAttendanceType.AttendanceType.Name,
            };

            return PartialView("Partials/_EditVisitorAttendanceType", visitorAttendanceTypeForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditVisitorAttendanceType(string id, VisitorAttendanceTypeFormModel visitorAttendanceTypeForm)
        {
            if (ModelState.IsValid)
            {
                VisitorAttendanceType visitorAttendanceType = await _visitorAttendanceTypeTypeService.FindByIdAsync(id);
                var dateTime = DateTime.Parse(visitorAttendanceTypeForm.Date);
                visitorAttendanceType.AttendanceDate = dateTime;

                TaskResult result = await _visitorAttendanceTypeTypeService.UpdateAsync(visitorAttendanceType);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.UpdateItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            List<AttendanceType> attendanceTypes = await _attendanceTypeService.GetAllAsync();
            visitorAttendanceTypeForm.AttendanceTypes = attendanceTypes.Select(x => new SelectListItem(x.Name, x.AttendanceTypeId.ToString())).ToList();

            return PartialView("Partials/_EditVisitorAttendanceType", visitorAttendanceTypeForm);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePatientBenefit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            VisitorAttendanceType visitorAttendanceType = await _visitorAttendanceTypeTypeService.FindByIdAsync(id);

            if (visitorAttendanceType == null)
            {
                return NotFound();
            }

            TaskResult result = await _visitorAttendanceTypeTypeService.DeleteAsync(visitorAttendanceType);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(LogEvents.DeleteItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
            return BadRequest();
        }
    }
}